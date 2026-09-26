using System.Numerics;
using CastNet.Nodes;

namespace CastNet.Tests;

[TestClass]
public sealed class NodeTests
{
    [TestMethod]
    public void SettingCollectionReplacesOnlyThatType()
    {
        var animation = new AnimationNode();
        var skeleton = animation.AddNode<SkeletonNode>();
        var oldCurve = animation.AddNode<CurveNode>();
        var newCurves = new[] { new CurveNode(), new CurveNode() };

        animation.Curves = newCurves;

        CollectionAssert.AreEqual(newCurves, animation.Curves);
        Assert.AreSame(skeleton, animation.Skeleton);
        Assert.IsNull(oldCurve.Parent);
        Assert.IsTrue(newCurves.All(curve => curve.Parent == animation));
    }

    [TestMethod]
    public void EnumerateHelpersMatchCollections()
    {
        var model = new ModelNode();
        model.AddNode<MeshNode>();
        model.AddNode<MaterialNode>();
        model.AddNode<MeshNode>();

        CollectionAssert.AreEqual(model.Meshes, model.EnumerateMeshes().ToArray());
        Assert.HasCount(1, model.Materials);
    }

    [TestMethod]
    public void MaterialSlotPropertiesUseSpecificationKeys()
    {
        var material = new MaterialNode { Albedo = new FileNode { Path = "a.png" }, Metalness = new ColorNode() };

        Assert.AreEqual("a.png", ((FileNode)material.Albedo!).Path);
        Assert.IsTrue(material.Properties.ContainsKey("albedo"));
        Assert.IsTrue(material.Properties.ContainsKey("metal"));
        Assert.IsNull(material.Normal);
    }

    [TestMethod]
    public void RootHelpersFindTopLevelNodes()
    {
        var root = new RootNode();
        var model = root.AddNode<ModelNode>();
        var metadata = root.AddNode(new MetadataNode { UpAxis = "z" });
        root.AddNode<AnimationNode>();

        CollectionAssert.AreEqual(new[] { model }, root.Models);
        Assert.AreEqual(1, root.EnumerateAnimations().Count());
        Assert.AreSame(metadata, root.Metadata);
    }

    [TestMethod]
    public void GetColorsUnpacksRgba8()
    {
        var mesh = new MeshNode();

        mesh.SetColorLayer(0, CastArrayProperty.Create<uint>([0xFF0000FF]));
        mesh.SetColorLayer(1, CastArrayProperty.Create(new Vector4(0.5f)));

        Assert.AreEqual(new Vector4(1, 0, 0, 1), mesh.GetColors(0)![0]);
        Assert.AreEqual(new Vector4(0.5f), mesh.GetColors(1)![0]);
        Assert.AreEqual(2, mesh.ColorLayerCount);
    }

    [TestMethod]
    public void SetUVLayerRaisesLayerCount()
    {
        var mesh = new MeshNode();

        mesh.SetUVLayer(1, CastArrayProperty.Create(Vector2.One));
        mesh.SetUVLayer(0, CastArrayProperty.Create(Vector2.Zero));

        Assert.AreEqual(2, mesh.UVLayerCount);
    }

    [TestMethod]
    public void FindBoneMatchesName()
    {
        var skeleton = new SkeletonNode();
        skeleton.AddNode(new BoneNode { Name = "tag_origin" });
        var torso = skeleton.AddNode(new BoneNode { Name = "j_spine4" });

        Assert.AreSame(torso, skeleton.FindBone("j_spine4"));
        Assert.IsNull(skeleton.FindBone("missing"));
    }

    [TestMethod]
    public void ToStringReturnsName()
    {
        Assert.AreEqual("tag_origin", new BoneNode { Name = "tag_origin" }.ToString());
        Assert.AreEqual("j_gun", new CurveNode { NodeName = "j_gun" }.ToString());
        Assert.AreEqual("Skeleton", new SkeletonNode().ToString());
    }

    [TestMethod]
    public void SharingASlotNodeBetweenMaterialsThrows()
    {
        var texture = new FileNode { Path = "shared.png" };
        new MaterialNode().Albedo = texture;

        Assert.ThrowsExactly<InvalidOperationException>(() => new MaterialNode().Albedo = texture);
    }

    [TestMethod]
    public void CustomOffsetRequiresConstraintType()
    {
        Assert.ThrowsExactly<InvalidOperationException>(() => new ConstraintNode { CustomOffset = Vector4.UnitW });
    }

    [TestMethod]
    public void WorldTransformsHandleParentsAfterChildren()
    {
        var skeleton = new SkeletonNode();
        var child = skeleton.AddNode(new BoneNode { ParentIndex = 1, LocalPosition = new Vector3(0, 0, 1) });
        skeleton.AddNode(new BoneNode { ParentIndex = -1, LocalPosition = new Vector3(5, 0, 0) });

        skeleton.CalculateWorldTransforms();

        Assert.AreEqual(new Vector3(5, 0, 1), child.WorldPosition);
    }

    [TestMethod]
    public void WorldTransformsRejectCycles()
    {
        var skeleton = new SkeletonNode();
        skeleton.AddNode(new BoneNode { ParentIndex = 1 });
        skeleton.AddNode(new BoneNode { ParentIndex = 0 });

        Assert.ThrowsExactly<InvalidDataException>(skeleton.CalculateWorldTransforms);
    }

    [TestMethod]
    public void BoneParentIndexAcceptsOtherWidths()
    {
        var bone = new BoneNode();
        bone.SetValue<ushort>("p", 3);

        Assert.AreEqual(3, bone.ParentIndex);
    }

    [TestMethod]
    public void SettingColorLayerOnLegacyMeshReplacesLegacyLayer()
    {
        var mesh = new MeshNode();
        mesh.SetArray("vc", CastArrayProperty.Create<uint>([0xFF0000FF]));
        var layer = CastArrayProperty.Create<uint>([0xFFFFFFFF]);

        mesh.SetColorLayer(0, layer);

        Assert.AreSame(layer, mesh.GetColorLayer(0));
        Assert.AreEqual(1, mesh.ColorLayerCount);
    }

    [TestMethod]
    public void RemovingLastUVLayerLowersLayerCount()
    {
        var mesh = new MeshNode();
        mesh.SetUVLayer(0, CastArrayProperty.Create(Vector2.Zero));
        mesh.SetUVLayer(1, CastArrayProperty.Create(Vector2.One));

        mesh.SetUVLayer(1, null);

        Assert.AreEqual(1, mesh.UVLayerCount);
    }

    [TestMethod]
    public void BoneDefaultsFollowSpecification()
    {
        var bone = new BoneNode();

        Assert.AreEqual(-1, bone.ParentIndex);
        Assert.IsTrue(bone.SegmentScaleCompensate);
    }

    [TestMethod]
    public void BoneParentIndexStoresTwosComplement()
    {
        var bone = new BoneNode { ParentIndex = -1 };

        Assert.AreEqual(uint.MaxValue, bone.GetValue<uint>("p"));
        Assert.AreEqual(-1, bone.ParentIndex);
    }

    [TestMethod]
    public void BoneSegmentScaleCompensateIsStoredAsByte()
    {
        var bone = new BoneNode { SegmentScaleCompensate = false };

        Assert.AreEqual(CastPropertyType.Byte, bone.Properties["ssc"].Type);
        Assert.IsFalse(bone.SegmentScaleCompensate);
    }

    [TestMethod]
    public void ConstraintSkipAxesUseSeparateKeys()
    {
        var constraint = new ConstraintNode { SkipY = true };

        Assert.IsFalse(constraint.SkipX);
        Assert.IsTrue(constraint.SkipY);
        Assert.IsFalse(constraint.SkipZ);
    }

    [TestMethod]
    [DataRow("pt", 0.0f, 0.0f)]
    [DataRow("or", 0.0f, 1.0f)]
    [DataRow("sc", 1.0f, 0.0f)]
    public void ConstraintCustomOffsetDefaultsByType(string type, float x, float w)
    {
        var constraint = new ConstraintNode { ConstraintType = type };

        Assert.AreEqual(x, constraint.CustomOffset.X);
        Assert.AreEqual(w, constraint.CustomOffset.W);
    }

    [TestMethod]
    public void ConstraintScaleOffsetRoundTrips()
    {
        var constraint = new ConstraintNode { ConstraintType = "sc", CustomOffset = new Vector4(2, 3, 4, 0) };

        Assert.AreEqual(CastPropertyType.Vector3, constraint.Properties["co"].Type);
        Assert.AreEqual(new Vector4(2, 3, 4, 0), constraint.CustomOffset);
    }

    [TestMethod]
    public void ConstraintWeightDefaultsToOne()
    {
        Assert.AreEqual(1.0f, new ConstraintNode().Weight);
    }

    [TestMethod]
    public void CurveModeOverrideFlagsUseSeparateKeys()
    {
        var node = new CurveModeOverrideNode { OverrideRotationCurves = true, OverrideScaleCurves = true };

        Assert.IsFalse(node.OverrideTranslationCurves);
        Assert.IsTrue(node.OverrideRotationCurves);
        Assert.IsTrue(node.OverrideScaleCurves);
    }

    [TestMethod]
    public void MeshCountsAreStoredNarrowest()
    {
        var mesh = new MeshNode { UVLayerCount = 2, MaximumWeightInfluence = 300 };

        Assert.AreEqual(CastPropertyType.Byte, mesh.Properties["ul"].Type);
        Assert.AreEqual(CastPropertyType.Short, mesh.Properties["mi"].Type);
        Assert.AreEqual(300, mesh.MaximumWeightInfluence);
    }

    [TestMethod]
    public void MeshFallsBackToLegacyColorLayer()
    {
        var mesh = new MeshNode();
        var legacy = CastArrayProperty.Create<uint>([0xFF0000FF]);
        mesh.SetArray("vc", legacy);

        Assert.AreEqual(1, mesh.ColorLayerCount);
        Assert.AreSame(legacy, mesh.GetColorLayer(0));
    }

    [TestMethod]
    public void MeshResolvesMaterialFromModel()
    {
        var model = new ModelNode();
        var material = model.AddNode(new MaterialNode { Name = "skin" });
        var mesh = model.AddNode(new MeshNode());

        mesh.Material = material;

        Assert.AreSame(material, mesh.Material);
    }

    [TestMethod]
    public void MaterialEnumeratesOpenSetOfSlots()
    {
        var material = new MaterialNode { Name = "metal" };
        var albedo = new FileNode { Path = "albedo.png" };
        var metal = new ColorNode { Rgba = Vector4.One };

        material.SetSlot("albedo", albedo);
        material.SetSlot("metal", metal);

        Assert.AreSame(albedo, material.GetSlot("albedo"));
        Assert.AreSame(material, metal.Parent);
        CollectionAssert.AreEquivalent(new[] { "albedo", "metal" }, material.EnumerateSlots().Select(slot => slot.Key).ToArray());
    }

    [TestMethod]
    public void InstanceResolvesReferenceFile()
    {
        var instance = new InstanceNode();
        var file = new FileNode { Path = "props/crate.cast" };

        instance.ReferenceFile = file;

        Assert.AreSame(file, instance.ReferenceFile);
        Assert.AreEqual(Vector3.One, instance.Scale);
        Assert.AreEqual(Quaternion.Identity, instance.Rotation);
    }

    [TestMethod]
    public void SkeletonTransformsRoundTrip()
    {
        var skeleton = new SkeletonNode();
        var parent = skeleton.AddNode(new BoneNode { WorldPosition = new Vector3(1, 2, 3), WorldRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, 0.5f) });
        var child = skeleton.AddNode(new BoneNode { ParentIndex = 0, WorldPosition = new Vector3(4, 5, 6), WorldRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, 0.25f) });
        var expectedPosition = child.WorldPosition!.Value;

        skeleton.CalculateLocalTransforms();
        child.WorldPosition = null;
        parent.WorldPosition = null;
        skeleton.CalculateWorldTransforms();

        Assert.IsLessThan(1e-5f, Vector3.Distance(expectedPosition, child.WorldPosition!.Value));
    }
}
