﻿// ------------------------------------------------------------------------
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

namespace Cast.NET.Nodes
{
    /// <summary>
    /// A class to hold a <see cref="CastNode"/> that contains a Mesh.
    /// </summary>
    public class MeshNode : CastNode
    {
        /// <summary>
        /// Gets the hash of the <see cref="MaterialNode"/> assigned to this mesh.
        /// </summary>
        public ulong MaterialHash => GetFirstValue<ulong>("m", 0);

        /// <summary>
        /// Gets the <see cref="MaterialNode"/> assigned to this mesh.
        /// </summary>
        public MaterialNode? Material => Parent?.TryGetChild<MaterialNode>(MaterialHash, out var node) == true ? node : null;

        /// <summary>
        /// Gets the name of the mesh.
        /// </summary>
        public string Name => GetStringValue("n", string.Empty);

        /// <summary>
        /// Gets the raw vertex positions buffer stored within this mesh.
        /// </summary>
        public CastArrayProperty<Vector3> VertexPositionBuffer => GetArrayProperty<Vector3>("vp");

        /// <summary>
        /// Gets the raw vertex normal buffer stored within this mesh.
        /// </summary>
        public CastArrayProperty<Vector3>? VertexNormalBuffer => TryGetArrayProperty<Vector3>("vn", out var array) ? array : null;

        /// <summary>
        /// Gets the raw vertex tangent buffer stored within this mesh.
        /// </summary>
        public CastArrayProperty<Vector3>? VertexTangentBuffer => TryGetArrayProperty<Vector3>("vt", out var array) ? array : null;

        /// <summary>
        /// Gets the raw vertex color buffer stored within this mesh for legacy files.
        /// </summary>
        public CastProperty? VertexColorBuffer => GetPropertyOrNull("vc");

        /// <summary>
        /// Gets the raw vertex weight bone buffer.
        /// </summary>
        public CastProperty? VertexWeightBoneBuffer => GetPropertyOrNull("wb");

        /// <summary>
        /// Gets the raw vertex weight value buffer.
        /// </summary>
        public CastArrayProperty<float>? VertexWeightValueBuffer => TryGetArrayProperty<float>("wv", out var array) ? array : null;

        /// <summary>
        /// Gets the raw face value buffer.
        /// </summary>
        public CastProperty FaceBuffer => GetProperty("f");

        /// <summary>
        /// Gets the number of uv layers within this mesh.
        /// </summary>
        public int UVLayerCount => (int)GetFirstInteger("ul", 0, 32);

        /// <summary>
        /// Gets the number of color layers within this mesh.
        /// </summary>
        public int ColorLayerCount => (int)GetFirstInteger("cl", 0, 32);

        /// <summary>
        /// Gets the max number of weight influences within this mesh.
        /// </summary>
        public int MaximumWeightInfluence => (int)GetFirstInteger("mi", 0, 32);

        /// <summary>
        /// Gets the skinning type the mesh uses.
        /// </summary>
        public string SkinningMethod => GetStringValue("sm", "linear");

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
        public CastProperty? GetUVLayer(int index) => GetUVLayer($"u{index}");

        /// <summary>
        /// Gets the layer with the given key.
        /// </summary>
        /// <param name="key">The key of the layer to obtain.</param>
        /// <returns>The layer if found, otherwise null.</returns>
        public CastProperty? GetUVLayer(string key) => GetPropertyOrNull(key);

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
    }
}
