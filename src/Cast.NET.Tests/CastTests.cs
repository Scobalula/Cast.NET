using System.Numerics;
using CastNet.Nodes;

namespace CastNet.Tests;

[TestClass]
public sealed class CastTests
{
    [TestMethod]
    public void UnicodeStringsRoundTrip()
    {
        var root = new RootNode();
        var model = root.AddNode(new ModelNode { Name = "モデル" });
        model.SetString("ñame", "Ünïcødé 🎉");

        var loaded = SaveAndLoad(root);
        var loadedModel = loaded.Roots[0].GetChild<ModelNode>()!;

        Assert.AreEqual("モデル", loadedModel.Name);
        Assert.AreEqual("Ünïcødé 🎉", loadedModel.GetString("ñame"));
    }

    [TestMethod]
    public void NodesLoadAsTheirTypedClasses()
    {
        var root = new RootNode();
        var model = root.AddNode<ModelNode>();
        model.AddNode<HairNode>();
        root.AddNode<AnimationNode>().AddNode<CurveModeOverrideNode>();
        root.AddNode(new CastNode((CastNodeIdentifier)0x12345678));

        var loaded = SaveAndLoad(root).Roots[0];

        Assert.IsInstanceOfType<RootNode>(loaded);
        Assert.IsNotNull(loaded.GetChild<ModelNode>()!.GetChild<HairNode>());
        Assert.IsNotNull(loaded.GetChild<AnimationNode>()!.GetChild<CurveModeOverrideNode>());
        Assert.AreEqual((CastNodeIdentifier)0x12345678, loaded.Children[2].Identifier);
    }

    [TestMethod]
    public void MeshDataRoundTrips()
    {
        var root = new RootNode();
        var mesh = root.AddNode<ModelNode>().AddNode(new MeshNode { Positions = CastArrayProperty.Create<Vector3>([Vector3.Zero, Vector3.UnitX, Vector3.UnitY]), Faces = CastArrayProperty.CreateIndices([0, 1, 2]) });

        var loaded = SaveAndLoad(root).Roots[0].GetChild<ModelNode>()!.GetChild<MeshNode>()!;

        Assert.AreEqual(mesh.Hash, loaded.Hash);
        CollectionAssert.AreEqual(new[] { 0, 1, 2 }, loaded.Faces!.ToArray<int>());
        Assert.AreEqual(Vector3.UnitY, loaded.Positions!.Get<Vector3>(2));
    }

    [TestMethod]
    public void InvalidMagicIsRejected()
    {
        Assert.ThrowsExactly<InvalidDataException>(() => CastReader.Load(new MemoryStream(new byte[16])));
    }

    [TestMethod]
    public void NonRootTopLevelNodeIsRejected()
    {
        using var stream = new MemoryStream();
        CastWriter.Save(stream, new RootNode());

        var bytes = stream.ToArray();
        BitConverter.GetBytes((uint)CastNodeIdentifier.Model).CopyTo(bytes, 16);

        Assert.ThrowsExactly<InvalidDataException>(() => CastReader.Load(new MemoryStream(bytes)));
    }

    [TestMethod]
    public void TruncatedFileIsRejected()
    {
        using var stream = new MemoryStream();
        CastWriter.Save(stream, new RootNode());

        Assert.ThrowsExactly<EndOfStreamException>(() => CastReader.Load(new MemoryStream(stream.ToArray()[..^4])));
    }

    private static Cast SaveAndLoad(RootNode root)
    {
        using var stream = new MemoryStream();
        CastWriter.Save(stream, root);
        stream.Position = 0;
        return CastReader.Load(stream);
    }
}
