// ------------------------------------------------------------------------
// Cast.NET - A .NET Library for reading and writing Cast files.
// Copyright(c) 2025 Philip/Scobalula
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
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace Cast.NET.Nodes
{
    /// <summary>
    /// A class to hold a <see cref="CastNode"/> that contains a Mesh.
    /// </summary>
    public class MeshNode : CastNode
    {
        /// <summary>
        /// Gets or Sets the name of the mesh.
        /// </summary>
        public string Name { get => GetStringValue("n", string.Empty); set => AddString("n", value); }

        /// <summary>
        /// Gets or Sets the hash of the <see cref="MaterialNode"/> assigned to this mesh.
        /// </summary>
        public ulong MaterialHash { get => GetFirstValue<ulong>("m", 0); set => AddValue("m", value); }

        /// <summary>
        /// Gets or Sets the <see cref="MaterialNode"/> assigned to this mesh.
        /// </summary>
        public MaterialNode? Material { get => Parent?.TryGetChild<MaterialNode>(MaterialHash, out var node) == true ? node : null; set { if (value is not null) MaterialHash = value.Hash; } }

        /// <summary>
        /// Gets or Sets the raw vertex positions buffer stored within this mesh.
        /// </summary>
        public CastArrayProperty<Vector3> VertexPositionBuffer { get => GetArrayProperty<Vector3>("vp"); set => Properties["vp"] = value; }

        /// <summary>
        /// Gets the raw vertex normal buffer stored within this mesh.
        /// </summary>
        public CastArrayProperty<Vector3>? VertexNormalBuffer { get => TryGetArrayProperty<Vector3>("vn", out var array) ? array : null; set { if (value is not null) Properties["vn"] = value; } }

        /// <summary>
        /// Gets or Sets the raw vertex tangent buffer stored within this mesh.
        /// </summary>
        public CastArrayProperty<Vector3>? VertexTangentBuffer { get => TryGetArrayProperty<Vector3>("vt", out var array) ? array : null; set { if (value is not null) Properties["vt"] = value; } }

        /// <summary>
        /// Gets or Sets the raw vertex color buffer stored within this mesh for legacy files.
        /// </summary>
        public CastProperty? VertexColorBuffer { get => GetPropertyOrNull("vc"); set { if (value is not null) Properties["vc"] = value; } }

        /// <summary>
        /// Gets or Sets the raw vertex weight bone buffer.
        /// </summary>
        public CastProperty? VertexWeightBoneBuffer { get => GetPropertyOrNull("wb"); set { if (value is not null) Properties["wb"] = value; } }

        /// <summary>
        /// Gets or Sets the raw vertex weight value buffer.
        /// </summary>
        public CastArrayProperty<float>? VertexWeightValueBuffer { get => TryGetArrayProperty<float>("wv", out var array) ? array : null; set { if (value is not null) Properties["wv"] = value; } }

        /// <summary>
        /// Gets or Sets the raw face value buffer.
        /// </summary>
        public CastProperty FaceBuffer { get => GetProperty("f"); set { if (value is not null) Properties["f"] = value; } }

        /// <summary>
        /// Gets or Sets the number of uv layers within this mesh.
        /// </summary>
        public int UVLayerCount { get => (int)GetFirstInteger("ul", 0, 32); set => AddValue("ul", (uint)value); }

        /// <summary>
        /// Gets or Sets the number of color layers within this mesh.
        /// </summary>
        public int ColorLayerCount { get => (int)GetFirstInteger("cl", 0, 32); set => AddValue("cl", (uint)value); }

        /// <summary>
        /// Gets or Sets the max number of weight influences within this mesh.
        /// </summary>
        public int MaximumWeightInfluence { get => (int)GetFirstInteger("mi", 0, 32); set => AddValue("mi", (uint)value); }

        /// <summary>
        /// Gets or Sets the skinning type the mesh uses.
        /// </summary>
        public string SkinningMethod { get => GetStringValue("sm", "linear"); set => AddString("sm", value); }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeshNode"/> class.
        /// </summary>
        public MeshNode() : base(CastNodeIdentifier.Mesh) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeshNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        public MeshNode(CastNodeIdentifier identifier) : base(identifier) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeshNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        public MeshNode(CastNodeIdentifier identifier, ulong hash) : base(identifier, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeshNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        public MeshNode(ulong hash) : base(CastNodeIdentifier.Mesh, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeshNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public MeshNode(ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(CastNodeIdentifier.Mesh, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public MeshNode(CastNodeIdentifier identifier, ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(identifier, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeshNode"/> class.
        /// </summary>
        /// <param name="source">Node to copy from. A shallow copy is performed and references to the source are stored.</param>
        public MeshNode(CastNode source) : base(source) { }

        /// <summary>
        /// Gets the layer with the given index.
        /// </summary>
        /// <param name="index">The index of the layer to obtain.</param>
        /// <returns>The layer if found, otherwise null.</returns>
        public CastArrayProperty<Vector2>? GetUVLayer(int index) => GetUVLayer($"u{index}");

        /// <summary>
        /// Gets the layer with the given key.
        /// </summary>
        /// <param name="key">The key of the layer to obtain.</param>
        /// <returns>The layer if found, otherwise null.</returns>
        public CastArrayProperty<Vector2>? GetUVLayer(string key) => GetPropertyOrNull(key) as CastArrayProperty<Vector2>;

        /// <summary>
        /// Gets the layer with the given index.
        /// </summary>
        /// <param name="index">The index of the layer to obtain.</param>
        /// <returns>The layer if found, otherwise null.</returns>
        public CastProperty? GetColorLayer(int index) => GetColorLayer($"c{index}");

        /// <summary>
        /// Gets the layer with the given key.
        /// </summary>
        /// <param name="key">The key of the layer to obtain.</param>
        /// <returns>The layer if found, otherwise null.</returns>
        public CastProperty? GetColorLayer(string key) => GetPropertyOrNull(key);

        /// <summary>
        /// Enumerates all weight bones and values.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> with the bone and weight value.</returns>
        public IEnumerable<(int, float)> EnumerateBoneWeights()
        {
            if (VertexWeightValueBuffer is not null)
            {
                if (VertexWeightBoneBuffer is CastArrayProperty<byte> byteArray)
                {
                    for (int i = 0; i < VertexWeightBoneBuffer.ValueCount; i++)
                    {
                        yield return (byteArray.Values[i], VertexWeightValueBuffer.Values[i]);
                    }
                }
                else if (VertexWeightBoneBuffer is CastArrayProperty<ushort> shortArray)
                {
                    for (int i = 0; i < VertexWeightBoneBuffer.ValueCount; i++)
                    {
                        yield return (shortArray.Values[i], VertexWeightValueBuffer.Values[i]);
                    }
                }
                else if (VertexWeightBoneBuffer is CastArrayProperty<uint> intArray)
                {
                    for (int i = 0; i < VertexWeightBoneBuffer.ValueCount; i++)
                    {
                        yield return ((int)intArray.Values[i], VertexWeightValueBuffer.Values[i]);
                    }
                }
            }
        }

        public IEnumerable<(int, int, int)> EnumerateFaceIndices()
        {
            var faceCount = FaceBuffer.ValueCount / 3;

            if (FaceBuffer is CastArrayProperty<byte> byteArray)
            {
                for(int i = 0; i < faceCount && i < byteArray.Values.Count; i++)
                {
                    yield return (byteArray.Values[i * 3 + 0], byteArray.Values[i * 3 + 1], byteArray.Values[i * 3 + 2]);
                }
            }
            else if (FaceBuffer is CastArrayProperty<ushort> ushortArray)
            {
                for (int i = 0; i < faceCount && i < ushortArray.Values.Count; i++)
                {
                    yield return (ushortArray.Values[i * 3 + 0], ushortArray.Values[i * 3 + 1], ushortArray.Values[i * 3 + 2]);
                }
            }
            else if (FaceBuffer is CastArrayProperty<uint> intArray)
            {
                for (int i = 0; i < faceCount && i < intArray.Values.Count; i++)
                {
                    yield return ((int)intArray.Values[i * 3 + 0], (int)intArray.Values[i * 3 + 1], (int)intArray.Values[i * 3 + 2]);
                }
            }
        }

        /// <summary>
        /// Adds a new uv layer to the mesh.
        /// </summary>
        /// <param name="uvLayer">The index of the uv layer, if a layer with the provided index already exists, this is overriden.</param>
        /// <returns>Resulting uv layer.</returns>
        public CastArrayProperty<Vector2> AddUVLayer(int uvLayer) => AddArray<Vector2>($"u{uvLayer}");

        /// <summary>
        /// Adds a new uv layer to the mesh.
        /// </summary>
        /// <param name="uvLayer">The index of the uv layer, if a layer with the provided index already exists, this is overriden.</param>
        /// <param name="property">Property to add.</param>
        public void AddUVLayer(int uvLayer, CastArrayProperty<Vector2> property) => Properties[$"u{uvLayer}"] = property;

        /// <summary>
        /// Adds a new color layer to the mesh.
        /// </summary>
        /// <param name="colorLayer">The index of the color layer, if a layer with the provided index already exists, this is overriden.</param>
        /// <returns>Resulting color layer.</returns>
        public CastArrayProperty<Vector4> AddColorLayer(int colorLayer) => AddArray<Vector4>($"c{colorLayer}");

        /// <summary>
        /// Adds a new color layer to the mesh.
        /// </summary>
        /// <param name="colorLayer">The index of the color layer, if a layer with the provided index already exists, this is overriden.</param>
        /// <param name="property">Property to add.</param>
        public void AddColorLayer(int colorLayer, CastArrayProperty<Vector4> property) => Properties[$"c{colorLayer}"] = property;

        /// <inheritdoc/>
        public override string ToString() => Name;
    }
}
