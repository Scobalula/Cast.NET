// ------------------------------------------------------------------------
// Cast.NET - A .NET Library for reading and writing Cast files.
// Copyright(c) 2026 Philip/Scobalula
// ------------------------------------------------------------------------
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// ------------------------------------------------------------------------
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// ------------------------------------------------------------------------
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ------------------------------------------------------------------------
using System.Numerics;

namespace CastNet.Nodes;

/// <summary>
/// A triangle mesh.
/// </summary>
/// <param name="hash">The node hash.</param>
public sealed class MeshNode(ulong hash) : CastNode(CastNodeIdentifier.Mesh, hash)
{
    /// <summary>
    /// Gets or sets the name of the mesh.
    /// </summary>
    public string? Name { get => GetString("n"); set => SetString("n", value); }

    /// <summary>
    /// Gets or sets the vertex positions.
    /// </summary>
    public CastArrayProperty? Positions { get => GetArray("vp"); set => SetArray("vp", value); }

    /// <summary>
    /// Gets or sets the vertex normals.
    /// </summary>
    public CastArrayProperty? Normals { get => GetArray("vn"); set => SetArray("vn", value); }

    /// <summary>
    /// Gets or sets the vertex tangents.
    /// </summary>
    public CastArrayProperty? Tangents { get => GetArray("vt"); set => SetArray("vt", value); }

    /// <summary>
    /// Gets or sets the counter-clockwise face indices, stored as any integer type.
    /// </summary>
    public CastArrayProperty? Faces { get => GetArray("f"); set => SetArray("f", value); }

    /// <summary>
    /// Gets or sets the weight bone indices, <see cref="MaximumWeightInfluence"/> per vertex, stored as any integer type.
    /// </summary>
    public CastArrayProperty? WeightBones { get => GetArray("wb"); set => SetArray("wb", value); }

    /// <summary>
    /// Gets or sets the weight values, <see cref="MaximumWeightInfluence"/> per vertex.
    /// </summary>
    public CastArrayProperty? WeightValues { get => GetArray("wv"); set => SetArray("wv", value); }

    /// <summary>
    /// Gets the number of vertices.
    /// </summary>
    public int VertexCount => Positions?.Count ?? 0;

    /// <summary>
    /// Gets the number of faces.
    /// </summary>
    public int FaceCount => (Faces?.Count ?? 0) / 3;

    /// <summary>
    /// Gets or sets the number of uv layers.
    /// </summary>
    public int UVLayerCount { get => GetScalar("ul", 0); set => SetArray("ul", CastArrayProperty.CreateIndices([value])); }

    /// <summary>
    /// Gets or sets the number of color layers.
    /// </summary>
    public int ColorLayerCount { get => GetScalar<int>("cl") ?? (Properties.ContainsKey("vc") ? 1 : 0); set => SetArray("cl", CastArrayProperty.CreateIndices([value])); }

    /// <summary>
    /// Gets or sets the maximum number of weights per vertex.
    /// </summary>
    public int MaximumWeightInfluence { get => GetScalar("mi", 0); set => SetArray("mi", CastArrayProperty.CreateIndices([value])); }

    /// <summary>
    /// Gets or sets the skinning method, either <c>linear</c> or <c>quaternion</c>.
    /// </summary>
    public string SkinningMethod { get => GetString("sm") ?? "linear"; set => SetString("sm", value); }

    /// <summary>
    /// Gets or sets the material assigned to this mesh, resolved from the parent model.
    /// </summary>
    public MaterialNode? Material { get => FindSibling<MaterialNode>("m"); set => SetValue("m", value?.Hash); }

    /// <summary>
    /// Initializes a new instance of the <see cref="MeshNode"/> class with a unique hash.
    /// </summary>
    public MeshNode() : this(CastHash.Next())
    {
    }

    /// <summary>
    /// Gets the uv layer with the given index.
    /// </summary>
    /// <param name="index">The index of the layer.</param>
    /// <returns>The layer, or <see langword="null"/> if it does not exist.</returns>
    public CastArrayProperty? GetUVLayer(int index) => GetArray($"u{index}");

    /// <summary>
    /// Sets the uv layer with the given index, removing it if <paramref name="layer"/> is <see langword="null"/>. <see cref="UVLayerCount"/> is updated to match.
    /// </summary>
    /// <param name="index">The index of the layer.</param>
    /// <param name="layer">The layer.</param>
    public void SetUVLayer(int index, CastArrayProperty? layer)
    {
        SetArray($"u{index}", layer);

        if (layer is not null && index >= UVLayerCount)
            UVLayerCount = index + 1;
        else if (layer is null && index == UVLayerCount - 1)
            UVLayerCount = index;
    }

    /// <summary>
    /// Gets the color layer with the given index, stored as packed RGBA8 integers or <see cref="CastPropertyType.Vector4"/>.
    /// </summary>
    /// <param name="index">The index of the layer.</param>
    /// <returns>The layer, or <see langword="null"/> if it does not exist.</returns>
    public CastArrayProperty? GetColorLayer(int index) => index == 0 && !Properties.ContainsKey("cl") ? GetArray("vc") ?? GetArray("c0") : GetArray($"c{index}");

    /// <summary>
    /// Gets the colors of the layer with the given index, unpacking RGBA8 layers.
    /// </summary>
    /// <param name="index">The index of the layer.</param>
    /// <returns>The colors, or <see langword="null"/> if the layer does not exist.</returns>
    public Vector4[]? GetColors(int index)
    {
        if (GetColorLayer(index) is not CastArrayProperty layer)
            return null;

        if (layer.Type == CastPropertyType.Vector4)
            return layer.AsSpan<Vector4>().ToArray();

        var packed = layer.AsSpan<uint>();
        var colors = new Vector4[packed.Length];

        for (var i = 0; i < packed.Length; i++)
            colors[i] = new Vector4((byte)packed[i], (byte)(packed[i] >> 8), (byte)(packed[i] >> 16), (byte)(packed[i] >> 24)) / byte.MaxValue;

        return colors;
    }

    /// <summary>
    /// Sets the color layer with the given index, removing it if <paramref name="layer"/> is <see langword="null"/>. <see cref="ColorLayerCount"/> is updated to match.
    /// </summary>
    /// <param name="index">The index of the layer.</param>
    /// <param name="layer">The layer.</param>
    public void SetColorLayer(int index, CastArrayProperty? layer)
    {
        var count = GetScalar<int>("cl") ?? 0;

        SetArray($"c{index}", layer);

        if (layer is not null && index >= count)
            ColorLayerCount = index + 1;
        else if (layer is null && index == count - 1)
            ColorLayerCount = index;
    }
}
