// ------------------------------------------------------------------------
// Cast.NET - A .NET Library for reading and writing Cast files.
// Copyright(c) 2024 Philip/Scobalula
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
        /// Gets the name of the material.
        /// </summary>
        public string Name => GetStringValueOrDefault("n", string.Empty);

        /// <summary>
        /// Gets the material type.
        /// </summary>
        public string Type => GetStringValueOrDefault("t", string.Empty);

        /// <summary>
        /// Gets the hash of the albedo <see cref="FileNode"/>.
        /// </summary>
        public ulong AlbedoFileHash => GetFirstValueOrDefault<ulong>("albedo", 0);

        /// <summary>
        /// Gets the hash of the diffuse <see cref="FileNode"/>.
        /// </summary>
        public ulong DiffuseFileHash => GetFirstValueOrDefault<ulong>("diffuse", 0);

        /// <summary>
        /// Gets the hash of the normal <see cref="FileNode"/>.
        /// </summary>
        public ulong NormalFileHash => GetFirstValueOrDefault<ulong>("normal", 0);

        /// <summary>
        /// Gets the hash of the specular <see cref="FileNode"/>.
        /// </summary>
        public ulong SpecularFileHash => GetFirstValueOrDefault<ulong>("specular", 0);

        /// <summary>
        /// Gets the hash of the emissive <see cref="FileNode"/>.
        /// </summary>
        public ulong EmissiveFileHash => GetFirstValueOrDefault<ulong>("emissive", 0);

        /// <summary>
        /// Gets the hash of the gloss <see cref="FileNode"/>.
        /// </summary>
        public ulong GlossFileHash => GetFirstValueOrDefault<ulong>("gloss", 0);

        /// <summary>
        /// Gets the hash of the roughness <see cref="FileNode"/>.
        /// </summary>
        public ulong RoughnessFileHash => GetFirstValueOrDefault<ulong>("roughness", 0);

        /// <summary>
        /// Gets the hash of the ao <see cref="FileNode"/>.
        /// </summary>
        public ulong AmbientOcclusionFileHash => GetFirstValueOrDefault<ulong>("ao", 0);

        /// <summary>
        /// Gets the hash of the cavity <see cref="FileNode"/>.
        /// </summary>
        public ulong CavityFileHash => GetFirstValueOrDefault<ulong>("cavity", 0);

        /// <summary>
        /// Gets the albedo <see cref="FileNode"/>.
        /// </summary>
        public FileNode? AlbedoFile => GetChildByHashOrNull<FileNode>(AlbedoFileHash);

        /// <summary>
        /// Gets the diffuse <see cref="FileNode"/>.
        /// </summary>
        public FileNode? DiffuseFile => GetChildByHashOrNull<FileNode>(DiffuseFileHash);

        /// <summary>
        /// Gets the normal <see cref="FileNode"/>.
        /// </summary>
        public FileNode? NormalFile => GetChildByHashOrNull<FileNode>(NormalFileHash);

        /// <summary>
        /// Gets the specular <see cref="FileNode"/>.
        /// </summary>
        public FileNode? SpecularFile => GetChildByHashOrNull<FileNode>(SpecularFileHash);

        /// <summary>
        /// Gets the emissive <see cref="FileNode"/>.
        /// </summary>
        public FileNode? EmissiveFile => GetChildByHashOrNull<FileNode>(EmissiveFileHash);

        /// <summary>
        /// Gets the gloss <see cref="FileNode"/>.
        /// </summary>
        public FileNode? GlossFile => GetChildByHashOrNull<FileNode>(GlossFileHash);

        /// <summary>
        /// Gets the roughness <see cref="FileNode"/>.
        /// </summary>
        public FileNode? RoughnessFile => GetChildByHashOrNull<FileNode>(RoughnessFileHash);

        /// <summary>
        /// Gets the ao <see cref="FileNode"/>.
        /// </summary>
        public FileNode? AmbientOcclusionFile => GetChildByHashOrNull<FileNode>(AmbientOcclusionFileHash);

        /// <summary>
        /// Gets the cavity <see cref="FileNode"/>.
        /// </summary>
        public FileNode? CavityFile => GetChildByHashOrNull<FileNode>(CavityFileHash);

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
        /// Gets the hash of the extra <see cref="FileNode"/>.
        /// </summary>
        /// <param name="index">Index of the extra file.</param>
        /// <returns>Hash of the extra file if found.</returns>
        public ulong GetExtraFileHash(int index) => GetExtraFileHash($"extra{index}");

        /// <summary>
        /// Gets the hash of the extra <see cref="FileNode"/>.
        /// </summary>
        /// <param name="name">Name of the extra file.</param>
        /// <returns>Hash of the extra file if found.</returns>
        public ulong GetExtraFileHash(string name) => GetFirstValueOrDefault<ulong>(name, 0);

        /// <summary>
        /// Gets the extra <see cref="FileNode"/>.
        /// </summary>
        /// <param name="index">Index of the extra file.</param>
        /// <returns>The extra file if found.</returns>
        public FileNode? GetExtraFile(int index) => GetExtraFile($"extra{index}");

        /// <summary>
        /// Gets the extra <see cref="FileNode"/>.
        /// </summary>
        /// <param name="name">Name of the extra file.</param>
        /// <returns>The extra file if found.</returns>
        public FileNode? GetExtraFile(string name) => GetChildByHashOrNull<FileNode>(GetFirstValueOrDefault<ulong>(name, 0));
    }
}
