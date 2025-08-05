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

using System.Transactions;
using System.Xml.Linq;

namespace Cast.NET.Nodes
{
    /// <summary>
    /// A class to hold a <see cref="CastNode"/> that contains a Curve Mode Override.
    /// </summary>
    public class CurveModeOverrideNode : CastNode
    {
        /// <summary>
        /// Gets or Sets the name of the node this overrides targets.
        /// </summary>
        public string NodeName { get => GetStringValue("nn"); set => AddString("nn", value); }

        /// <summary>
        /// Gets or Sets the curve's mode.
        /// </summary>
        public string Mode { get => GetStringValue("m"); set => AddString("m", value); }

        /// <summary>
        /// Gets or Sets if translation curves are to be overriden.
        /// </summary>
        public bool OverrideTranslationCurves { get => GetFirstValue("ot", (byte)0) == 1; set => AddValue("ot", (byte)(value ? 1 : 0)); }

        /// <summary>
        /// Gets or Sets if rotation curves are to be overriden.
        /// </summary>
        public bool OverrideRotationCurves { get => GetFirstValue("or", (byte)0) == 1; set => AddValue("ot", (byte)(value ? 1 : 0)); }

        /// <summary>
        /// Gets or Sets if scale curves are to be overriden.
        /// </summary>
        public bool OverrideScaleCurves { get => GetFirstValue("os", (byte)0) == 1; set => AddValue("ot", (byte)(value ? 1 : 0)); }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurveModeOverrideNode"/> class.
        /// </summary>
        public CurveModeOverrideNode() : base(CastNodeIdentifier.CurveModeOverride) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurveModeOverrideNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        public CurveModeOverrideNode(CastNodeIdentifier identifier) : base(identifier) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurveModeOverrideNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        public CurveModeOverrideNode(CastNodeIdentifier identifier, ulong hash) : base(identifier, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurveModeOverrideNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        public CurveModeOverrideNode(ulong hash) : base(CastNodeIdentifier.CurveModeOverride, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurveModeOverrideNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public CurveModeOverrideNode(ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(CastNodeIdentifier.CurveModeOverride, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public CurveModeOverrideNode(CastNodeIdentifier identifier, ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(identifier, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurveModeOverrideNode"/> class.
        /// </summary>
        /// <param name="source">Node to copy from. A shallow copy is performed and references to the source are stored.</param>
        public CurveModeOverrideNode(CastNode source) : base(source) { }

        /// <inheritdoc/>
        public override string ToString() => NodeName;
    }
}
