using System.Numerics;
using CastNet.Nodes;

namespace CastNet.Tests;

[TestClass]
public sealed class InteropTests
{
    [TestMethod]
    public void SceneCoveringTheSpecificationIsWrittenForInteropChecks()
    {
        var root = new RootNode();

        root.AddNode(new MetadataNode { Author = "Cast.NET ✓", Software = "Cast.NET Tests", UpAxis = "z", SceneRoot = new string('r', 300) });

        var model = root.AddNode(new ModelNode { Name = "モデル", Position = Vector3.One, Rotation = Quaternion.Identity, Scale = Vector3.One });
        var skeleton = model.AddNode<SkeletonNode>();
        var origin = skeleton.AddNode(new BoneNode { Name = "tag_origin", LocalPosition = Vector3.Zero, LocalRotation = Quaternion.Identity, WorldPosition = Vector3.Zero, WorldRotation = Quaternion.Identity, Scale = Vector3.One });
        var arm = skeleton.AddNode(new BoneNode { Name = "j_arm", ParentIndex = 0, SegmentScaleCompensate = false, LocalPosition = Vector3.UnitZ });
        var hand = skeleton.AddNode(new BoneNode { Name = "j_hand", ParentIndex = 1, LocalPosition = Vector3.UnitZ });

        skeleton.AddNode(new IKHandleNode { Name = "ik_arm", StartBone = origin, EndBone = hand, TargetBone = arm, PoleBone = arm, TargetOffset = Vector3.One, UseTargetRotation = true });
        skeleton.AddNode(new ConstraintNode { Name = "orient", ConstraintType = "or", ConstraintBone = hand, TargetBone = arm, MaintainOffset = true, CustomOffset = Vector4.UnitW, Weight = 0.5f, SkipY = true });
        skeleton.AddNode(new ConstraintNode { Name = "scale", ConstraintType = "sc", ConstraintBone = hand, TargetBone = arm, CustomOffset = new Vector4(2, 2, 2, 0) });

        var material = model.AddNode(new MaterialNode { Name = "material" });
        material.Albedo = new FileNode { Path = new string('p', 85) + ".dds" };
        material.Metalness = new ColorNode { Name = "metal", ColorSpace = "linear", Rgba = new Vector4(0.5f) };
        material.SetSlot("extra0", new FileNode { Path = new string('x', 5000) });

        var mesh = model.AddNode(new MeshNode { Name = "mesh", Material = material, SkinningMethod = "quaternion" });
        mesh.Positions = CastArrayProperty.Create<Vector3>([Vector3.Zero, Vector3.UnitX, Vector3.UnitY]);
        mesh.Normals = CastArrayProperty.Create<Vector3>([Vector3.UnitZ, Vector3.UnitZ, Vector3.UnitZ]);
        mesh.Tangents = CastArrayProperty.Create<Vector3>([Vector3.UnitX, Vector3.UnitX, Vector3.UnitX]);
        mesh.Faces = CastArrayProperty.CreateIndices<int>([0, 1, 2]);
        mesh.SetUVLayer(0, CastArrayProperty.Create<Vector2>([Vector2.Zero, Vector2.UnitX, Vector2.UnitY]));
        mesh.SetUVLayer(1, CastArrayProperty.Create<Vector2>([Vector2.One, Vector2.One, Vector2.One]));
        mesh.SetColorLayer(0, CastArrayProperty.Create<uint>([0xFF0000FF, 0xFF00FF00, 0xFFFF0000]));
        mesh.SetColorLayer(1, CastArrayProperty.Create<Vector4>([Vector4.One, Vector4.One, Vector4.One]));
        mesh.MaximumWeightInfluence = 2;
        mesh.WeightBones = CastArrayProperty.CreateIndices<int>([0, 1, 1, 2, 2, 0]);
        mesh.WeightValues = CastArrayProperty.Create<float>([0.5f, 0.5f, 1, 0, 0.25f, 0.75f]);

        model.AddNode(new BlendShapeNode { Name = "smile", BaseShape = mesh, VertexIndices = CastArrayProperty.CreateIndices<int>([1, 2]), VertexPositions = CastArrayProperty.Create<Vector3>([Vector3.One, Vector3.One]), TargetWeightScale = 0.75f });
        model.AddNode(new HairNode { Name = "hair", Material = material, Segments = CastArrayProperty.CreateIndices<int>([1]), Particles = CastArrayProperty.Create<Vector3>([Vector3.Zero, Vector3.One]) });

        var animation = root.AddNode(new AnimationNode { Name = "animation", Framerate = 30, Looping = true });
        animation.AddNode(new CurveNode { NodeName = "j_arm", KeyPropertyName = "rq", Mode = "absolute", KeyFrames = CastArrayProperty.CreateIndices<int>([0, 300]), KeyValues = CastArrayProperty.Create<Quaternion>([Quaternion.Identity, Quaternion.Identity]) });
        animation.AddNode(new CurveNode { NodeName = "j_arm", KeyPropertyName = "tx", Mode = "additive", AdditiveBlendWeight = 0.5f, KeyFrames = CastArrayProperty.CreateIndices<int>([0, 70000]), KeyValues = CastArrayProperty.Create<float>([0, 1]) });
        animation.AddNode(new CurveNode { NodeName = "j_hand", KeyPropertyName = "vb", Mode = "absolute", KeyFrames = CastArrayProperty.CreateIndices<int>([0]), KeyValues = CastArrayProperty.Create<byte>([1]) });
        animation.AddNode(new CurveModeOverrideNode { NodeName = "j_arm", Mode = "relative", OverrideTranslationCurves = true, OverrideRotationCurves = true });
        animation.AddNode(new NotificationTrackNode { Name = "fire", KeyFrames = CastArrayProperty.CreateIndices<int>([5, 10]) });

        var instance = root.AddNode(new InstanceNode { Name = "instance", Position = Vector3.One, Rotation = Quaternion.Identity, Scale = Vector3.One });
        instance.ReferenceFile = new FileNode { Path = "props/crate.cast" };

        var unknown = root.AddNode(new CastNode((CastNodeIdentifier)0x74736574));
        unknown.SetValue("d", 1.5);
        unknown.SetValue<long>("l", -1);
        unknown.SetValue<short>("h", -1);

        var directory = Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, "interop"));
        var path = Path.Combine(directory.FullName, "scene.cast");

        CastWriter.Save(path, root);

        using var resaved = new MemoryStream();
        CastWriter.Save(resaved, CastReader.Load(path));

        CollectionAssert.AreEqual(File.ReadAllBytes(path), resaved.ToArray());
    }
}
