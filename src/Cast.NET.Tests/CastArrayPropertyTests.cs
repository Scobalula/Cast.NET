using System.Numerics;

namespace CastNet.Tests;

[TestClass]
public sealed class CastArrayPropertyTests
{
    [TestMethod]
    public void CreateInfersTypeFromValues()
    {
        Assert.AreEqual(CastPropertyType.Vector3, CastArrayProperty.Create<Vector3>([Vector3.One]).Type);
        Assert.AreEqual(CastPropertyType.Vector4, CastArrayProperty.Create(Quaternion.Identity).Type);
        Assert.AreEqual(CastPropertyType.Integer32, CastArrayProperty.Create(-1).Type);
        Assert.AreEqual(CastPropertyType.Integer64, CastArrayProperty.Create(1UL).Type);
    }

    [TestMethod]
    [DataRow(255, CastPropertyType.Byte)]
    [DataRow(256, CastPropertyType.Short)]
    [DataRow(65535, CastPropertyType.Short)]
    [DataRow(65536, CastPropertyType.Integer32)]
    public void CreateIndicesPicksNarrowestType(int maximum, CastPropertyType expected)
    {
        var property = CastArrayProperty.CreateIndices([0, maximum, 1]);

        Assert.AreEqual(expected, property.Type);
        Assert.AreSequenceEqual([0, maximum, 1], property.ToArray<int>());
    }

    [TestMethod]
    public void CreateIndicesAcceptsAnyIntegerType()
    {
        Assert.AreEqual(CastPropertyType.Byte, CastArrayProperty.CreateIndices<ushort>([1, 2, 200]).Type);
        Assert.AreEqual(CastPropertyType.Short, CastArrayProperty.CreateIndices<uint>([1, 60000]).Type);
        Assert.AreEqual(CastPropertyType.Integer64, CastArrayProperty.CreateIndices<ulong>([ulong.MaxValue]).Type);
        CollectionAssert.AreEqual(new ulong[] { 3, 70000 }, CastArrayProperty.CreateIndices<long>([3, 70000]).ToArray<ulong>());
    }

    [TestMethod]
    public void AddScalarConvertsToStoredType()
    {
        var property = new CastArrayProperty(CastPropertyType.Short);

        property.AddScalar(7);
        property.AddScalarRange<long>([8, 9]);
        property.AddScalar(10.0);

        CollectionAssert.AreEqual(new ushort[] { 7, 8, 9, 10 }, property.AsSpan<ushort>().ToArray());
    }

    [TestMethod]
    public void AddScalarRangeAddsNothingWhenAValueDoesNotFit()
    {
        var property = CastArrayProperty.Create<byte>([1]);

        Assert.ThrowsExactly<OverflowException>(() => property.AddScalarRange<int>([2, 300]));
        Assert.AreEqual(1, property.Count);
    }

    [TestMethod]
    public void AddScalarThrowsForVectors()
    {
        Assert.ThrowsExactly<InvalidCastException>(() => new CastArrayProperty(CastPropertyType.Vector3).AddScalar(1.0f));
    }

    [TestMethod]
    public void CreateIndicesRejectsNegativeValues()
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => CastArrayProperty.CreateIndices([1, -1]));
    }

    [TestMethod]
    public void GetScalarWidensAnyNumericType()
    {
        Assert.AreEqual(200.0, CastArrayProperty.Create<byte>(200).GetScalar<double>(0));
        Assert.AreEqual(60000, CastArrayProperty.Create<ushort>(60000).GetScalar<int>(0));
        Assert.AreEqual(2, CastArrayProperty.Create(2.5f).GetScalar<int>(0));
    }

    [TestMethod]
    public void GetScalarThrowsWhenValueDoesNotFit()
    {
        Assert.ThrowsExactly<OverflowException>(() => CastArrayProperty.Create(uint.MaxValue).GetScalar<short>(0));
    }

    [TestMethod]
    public void GetScalarThrowsForVectors()
    {
        Assert.ThrowsExactly<InvalidCastException>(() => CastArrayProperty.Create(Vector3.One).GetScalar<float>(0));
    }

    [TestMethod]
    public void AsSpanThrowsOnTypeMismatch()
    {
        var property = CastArrayProperty.Create<ushort>([1, 2, 3]);

        Assert.ThrowsExactly<InvalidCastException>(() => property.AsSpan<uint>());
    }

    [TestMethod]
    public void AsSpanAllowsSignedAliasesAndQuaternions()
    {
        Assert.AreEqual(-1, CastArrayProperty.Create(uint.MaxValue).Get<int>(0));
        Assert.AreEqual(Quaternion.Identity, CastArrayProperty.Create(Vector4.UnitW).Get<Quaternion>(0));
    }

    [TestMethod]
    public void AddGrowsStorage()
    {
        var property = new CastArrayProperty(CastPropertyType.Float);

        for (var i = 0; i < 100; i++)
            property.Add((float)i);

        Assert.AreEqual(100, property.Count);
        Assert.AreEqual(400, property.AsBytes().Length);
        Assert.AreEqual(99.0f, property.Get<float>(99));
    }

    [TestMethod]
    public void ResizeZeroesValuesAfterShrinking()
    {
        var property = CastArrayProperty.Create<float>([1, 2, 3]);

        property.Resize(1);
        property.Resize(3);

        CollectionAssert.AreEqual(new[] { 1.0f, 0.0f, 0.0f }, property.AsSpan<float>().ToArray());
    }

    [TestMethod]
    public void StringTypeIsRejected()
    {
        Assert.ThrowsExactly<ArgumentException>(() => new CastArrayProperty(CastPropertyType.String));
    }
}
