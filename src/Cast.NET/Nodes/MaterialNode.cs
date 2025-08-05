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

namespace Cast.NET.Nodes
{
    /// <summary>
    /// A class to hold a <see cref="CastNode"/> that contains a material.
    /// </summary>
    public class MaterialNode : CastNode
    {
        /// <summary>
        /// Gets or Sets the name of the material.
        /// </summary>
        public string Name { get => GetStringValue("n"); set => AddString("n", value); }

        /// <summary>
        /// Gets or Sets the material type.
        /// </summary>
        public string Type { get => GetStringValue("t"); set => AddString("t", value); }

        /// <summary>
        /// Gets or Sets the hash of the albedo <see cref="CastNode"/>.
        /// </summary>
        public ulong AlbedoHash { get => GetFirstValue<ulong>("albedo", 0); set => AddValue("albedo", value); }

        /// <summary>
        /// Gets or Sets the hash of the diffuse <see cref="CastNode"/>.
        /// </summary>
        public ulong DiffuseHash { get => GetFirstValue<ulong>("diffuse", 0); set => AddValue("diffuse", value); }

        /// <summary>
        /// Gets or Sets the hash of the normal <see cref="CastNode"/>.
        /// </summary>
        public ulong NormalHash { get => GetFirstValue<ulong>("normal", 0); set => AddValue("normal", value); }

        /// <summary>
        /// Gets or Sets the hash of the specular <see cref="CastNode"/>.
        /// </summary>
        public ulong SpecularHash { get => GetFirstValue<ulong>("specular", 0); set => AddValue("specular", value); }

        /// <summary>
        /// Gets or Sets the hash of the emissive <see cref="CastNode"/>.
        /// </summary>
        public ulong EmissiveHash { get => GetFirstValue<ulong>("emissive", 0); set => AddValue("emissive", value); }

        /// <summary>
        /// Gets or Sets the hash of the emissive mask <see cref="CastNode"/>.
        /// </summary>
        public ulong EmissiveMaskHash { get => GetFirstValue<ulong>("emask", 0); set => AddValue("emask", value); }

        /// <summary>
        /// Gets or Sets the hash of the gloss <see cref="CastNode"/>.
        /// </summary>
        public ulong GlossHash { get => GetFirstValue<ulong>("gloss", 0); set => AddValue("gloss", value); }

        /// <summary>
        /// Gets or Sets the hash of the roughness <see cref="CastNode"/>.
        /// </summary>
        public ulong RoughnessHash { get => GetFirstValue<ulong>("roughness", 0); set => AddValue("roughness", value); }

        /// <summary>
        /// Gets or Sets the hash of the ao <see cref="CastNode"/>.
        /// </summary>
        public ulong AmbientOcclusionHash { get => GetFirstValue<ulong>("ao", 0); set => AddValue("ao", value); }

        /// <summary>
        /// Gets or Sets the hash of the cavity <see cref="CastNode"/>.
        /// </summary>
        public ulong CavityHash { get => GetFirstValue<ulong>("cavity", 0); set => AddValue("cavity", value); }

        /// <summary>
        /// Gets or Sets the hash of the anisotropy <see cref="CastNode"/>.
        /// </summary>
        public ulong AnisotropyHash { get => GetFirstValue<ulong>("aniso", 0); set => AddValue("aniso", value); }

        /// <summary>
        /// Gets or Sets the albedo <see cref="CastNode"/>.
        /// </summary>
        public CastNode? Albedo { get => TryGetChild(AlbedoHash, out var node) == true ? node : null; set { if (value is not null) AlbedoHash = value.Hash; } }

        /// <summary>
        /// Gets or Sets the diffuse <see cref="CastNode"/>.
        /// </summary>
        public CastNode? Diffuse { get => TryGetChild(DiffuseHash, out var node) == true ? node : null; set { if (value is not null) DiffuseHash = value.Hash; } }

        /// <summary>
        /// Gets or Sets the normal <see cref="CastNode"/>.
        /// </summary>
        public CastNode? Normal { get => TryGetChild(NormalHash, out var node) == true ? node : null; set { if (value is not null) NormalHash = value.Hash; } }

        /// <summary>
        /// Gets or Sets the specular <see cref="CastNode"/>.
        /// </summary>
        public CastNode? Specular { get => TryGetChild(SpecularHash, out var node) == true ? node : null; set { if (value is not null) SpecularHash = value.Hash; } }

        /// <summary>
        /// Gets or Sets the emissive <see cref="CastNode"/>.
        /// </summary>
        public CastNode? Emissive { get => TryGetChild(EmissiveHash, out var node) == true ? node : null; set { if (value is not null) EmissiveHash = value.Hash; } }

        /// <summary>
        /// Gets or Sets the emissive mask <see cref="CastNode"/>.
        /// </summary>
        public CastNode? EmissiveMask { get => TryGetChild(EmissiveMaskHash, out var node) == true ? node : null; set { if (value is not null) EmissiveMaskHash = value.Hash; } }

        /// <summary>
        /// Gets or Sets the gloss <see cref="CastNode"/>.
        /// </summary>
        public CastNode? Gloss { get => TryGetChild(GlossHash, out var node) == true ? node : null; set { if (value is not null) GlossHash = value.Hash; } }

        /// <summary>
        /// Gets or Sets the roughness <see cref="CastNode"/>.
        /// </summary>
        public CastNode? Roughness { get => TryGetChild(RoughnessHash, out var node) == true ? node : null; set { if (value is not null) RoughnessHash = value.Hash; } }

        /// <summary>
        /// Gets or Sets the ao <see cref="CastNode"/>.
        /// </summary>
        public CastNode? AmbientOcclusion { get => TryGetChild(AmbientOcclusionHash, out var node) == true ? node : null; set { if (value is not null) AmbientOcclusionHash = value.Hash; } }

        /// <summary>
        /// Gets or Sets the cavity <see cref="CastNode"/>.
        /// </summary>
        public CastNode? Cavity { get => TryGetChild(CavityHash, out var node) == true ? node : null; set { if (value is not null) CavityHash = value.Hash; } }

        /// <summary>
        /// Gets or Sets the anisotropy <see cref="CastNode"/>.
        /// </summary>
        public CastNode? Anisotropy { get => TryGetChild(AnisotropyHash, out var node) == true ? node : null; set { if (value is not null) AnisotropyHash = value.Hash; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialNode"/> class.
        /// </summary>
        public MaterialNode() : base(CastNodeIdentifier.Material) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialNode"/> class.
        /// </summary>
        public MaterialNode(string name, string type) : base(CastNodeIdentifier.Material)
        {
            AddString("n", name);
            AddString("t", type);
            Hash = CastHasher.Compute(name);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        public MaterialNode(CastNodeIdentifier identifier) : base(identifier) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        public MaterialNode(CastNodeIdentifier identifier, ulong hash) : base(identifier, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        public MaterialNode(ulong hash) : base(CastNodeIdentifier.Material, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public MaterialNode(ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(CastNodeIdentifier.Material, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public MaterialNode(CastNodeIdentifier identifier, ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(identifier, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialNode"/> class.
        /// </summary>
        /// <param name="source">Node to copy from. A shallow copy is performed and references to the source are stored.</param>
        public MaterialNode(CastNode source) : base(source) { }

        /// <summary>
        /// Gets or Sets the hash of the extra <see cref="CastNode"/>.
        /// </summary>
        /// <param name="index">Index of the extra data.</param>
        /// <returns>Hash of the extra data if found.</returns>
        public ulong GetExtraHash(int index) => GetExtraHash($"extra{index}");

        /// <summary>
        /// Gets or Sets the hash of the extra <see cref="CastNode"/>.
        /// </summary>
        /// <param name="name">Name of the extra data.</param>
        /// <returns>Hash of the extra data if found.</returns>
        public ulong GetExtraHash(string name) => GetFirstValue<ulong>(name, 0);

        /// <summary>
        /// Gets or Sets the extra <see cref="CastNode"/>.
        /// </summary>
        /// <param name="index">Index of the extra data.</param>
        /// <returns>The extra data if found.</returns>
        public CastNode? GetExtraFile(int index) => GetExtraFile($"extra{index}");

        /// <summary>
        /// Gets or Sets the extra <see cref="CastNode"/>.
        /// </summary>
        /// <param name="name">Name of the extra data.</param>
        /// <returns>The extra data if found.</returns>
        public CastNode? GetExtraFile(string name) => TryGetChild<CastNode>(GetFirstValue<ulong>(name, 0), out var node) ? node : null;

        /// <inheritdoc/>
        public override string ToString() => Name;
    }
}
