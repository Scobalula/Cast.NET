using System.Numerics;
using CastNet.Nodes;

namespace CastNet.Tests;

[TestClass]
public sealed class CastNodeTests
{
    [TestMethod]
    public void NewNodesHaveUniqueHashes()
    {
        Assert.AreNotEqual(new BoneNode().Hash, new BoneNode().Hash);
    }

    [TestMethod]
    public void AddNodeMovesNodeBetweenParents()
    {
        var first = new SkeletonNode();
        var second = new SkeletonNode();
        var bone = first.AddNode<BoneNode>();

        second.AddNode(bone);

        Assert.AreSame(second, bone.Parent);
        Assert.IsEmpty(first.Children);
        Assert.HasCount(1, second.Children);
    }

    [TestMethod]
    public void AddingAnAncestorThrows()
    {
        var model = new ModelNode();
        var skeleton = model.AddNode<SkeletonNode>();

        Assert.ThrowsExactly<InvalidOperationException>(() => skeleton.AddNode(model));
        Assert.ThrowsExactly<InvalidOperationException>(() => model.AddNode(model));
    }

    [TestMethod]
    public void NewHashesDoNotCollideWithTheReferenceSequence()
    {
        Assert.IsFalse(new BoneNode().Hash is > 0x534E495752545250 and < 0x534E495752545250 + 1_000_000);
    }

    [TestMethod]
    public void RemoveNodeClearsParent()
    {
        var skeleton = new SkeletonNode();
        var bone = skeleton.AddNode<BoneNode>();

        Assert.IsTrue(skeleton.RemoveNode(bone));
        Assert.IsNull(bone.Parent);
        Assert.IsFalse(skeleton.RemoveNode(bone));
    }

    [TestMethod]
    public void GetChildSkipsOtherTypes()
    {
        var animation = new AnimationNode();
        animation.AddNode<CurveNode>();
        var skeleton = animation.AddNode<SkeletonNode>();

        Assert.AreSame(skeleton, animation.Skeleton);
    }

    [TestMethod]
    public void EnumerateChildrenIncludesDerivedTypes()
    {
        var root = new RootNode();
        root.AddNode<ModelNode>();
        root.AddNode<AnimationNode>();

        Assert.AreEqual(2, root.EnumerateChildren<CastNode>().Count());
        Assert.AreEqual(1, root.EnumerateChildren<ModelNode>().Count());
    }

    [TestMethod]
    public void SettingNullRemovesProperty()
    {
        var bone = new BoneNode { LocalPosition = Vector3.One, Name = "tag_origin" };

        bone.LocalPosition = null;

        Assert.IsNull(bone.LocalPosition);
        Assert.IsFalse(bone.Properties.ContainsKey("lp"));
    }

    [TestMethod]
    public void NullableValuesRoundTrip()
    {
        var bone = new BoneNode { LocalRotation = Quaternion.CreateFromYawPitchRoll(1, 2, 3) };

        Assert.AreEqual(CastPropertyType.Vector4, bone.Properties["lr"].Type);
        Assert.AreEqual(Quaternion.CreateFromYawPitchRoll(1, 2, 3), bone.LocalRotation);
    }

    [TestMethod]
    public void BooleansAcceptAnyNonZeroValue()
    {
        var node = new CastNode(CastNodeIdentifier.Root);
        node.SetValue<ushort>("flag", 2);

        Assert.IsTrue(node.GetBoolean("flag", false));
        Assert.IsTrue(node.GetBoolean("missing", true));
    }
}
