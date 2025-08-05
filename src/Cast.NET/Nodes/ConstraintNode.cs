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

namespace Cast.NET.Nodes
{
    /// <summary>
    /// A class to hold a <see cref="CastNode"/> that contains an constraint.
    /// </summary>
    public class ConstraintNode : CastNode
    {
        /// <summary>
        /// Gets or Sets the name of this constraint.
        /// </summary>
        public string Name => GetStringValue("n", string.Empty);

        /// <summary>
        /// Gets or Sets the constraint type.
        /// </summary>
        public string ConstraintType { get => GetStringValue("ct"); set => AddString("ct", value); }

        /// <summary>
        /// Gets or Sets the hash of the constraint <see cref="BoneNode"/>.
        /// </summary>
        public ulong ConstraintBoneHash { get => GetFirstValue<ulong>("cb"); set => AddValue("cb", value); }

        /// <summary>
        /// Gets or Sets the hash of the target <see cref="BoneNode"/>.
        /// </summary>
        public ulong TargetBoneHash { get => GetFirstValue<ulong>("tb"); set => AddValue("tb", value); }

        /// <summary>
        /// Gets or Sets if to enable maintain offset.
        /// </summary>
        public bool MaintainOffset { get => GetFirstValue("mo", (byte)0) == 1; set => AddValue("mo", (byte)(value ? 1 : 0)); }

        /// <summary>
        /// Gets or Sets the custom offset.
        /// When getting the value, if no value is set, this returns a default value which is based off the <see cref="ConstraintType"/> per the cast specification.
        /// When setting a value, this is narrowed/expanded to the type based off the <see cref="ConstraintType"/> per the cast specification.
        /// </summary>
        public Vector4 CustomOffset
        {
            get
            {
                var defaultValue = ConstraintType switch
                {
                    "pt" => Vector4.Zero,
                    "or" => Vector4.UnitW,
                    "sc" => Vector4.One,
                    _ => throw new NotImplementedException()
                };
                return GetFirstValue("co", defaultValue);
            }
            set
            {
                switch(ConstraintType)
                {
                    case "pt":
                        AddValue("co", new Vector3(value.X, value.Y, value.Z)); break;
                    case "or":
                        AddValue("co", value); break;
                    case "sc":
                        AddValue("co", new Vector3(value.X, value.Y, value.Z)); break;
                    default:
                        throw new NotImplementedException(ConstraintType);
                }
            }
        }

        /// <summary>
        /// Gets or Sets the weight of this constraint.
        /// </summary>
        public float Weight { get => GetFirstValue<float>("wt"); set => AddValue("wt", value); }

        /// <summary>
        /// Gets or Sets if X is skipped.
        /// </summary>
        public bool SkipX { get => GetFirstValue("sx", (byte)0) == 1; set => AddValue("sx", (byte)(value ? 1 : 0)); }

        /// <summary>
        /// Gets or Sets if Y is skipped.
        /// </summary>
        public bool SkipY { get => GetFirstValue("sx", (byte)0) == 1; set => AddValue("sx", (byte)(value ? 1 : 0)); }

        /// <summary>
        /// Gets or Sets if Z is skipped.
        /// </summary>
        public bool SkipZ { get => GetFirstValue("sx", (byte)0) == 1; set => AddValue("sx", (byte)(value ? 1 : 0)); }

        /// <summary>
        /// Gets or Sets the start <see cref="BoneNode"/>.
        /// </summary>
        public BoneNode ConstraintBone { get => Parent?.TryGetChild<BoneNode>(ConstraintBoneHash, out var node) == true ? node : throw new KeyNotFoundException(nameof(ConstraintBoneHash)); set => ConstraintBoneHash = value.Hash; }

        /// <summary>
        /// Gets or Sets the target <see cref="BoneNode"/>.
        /// </summary>
        public BoneNode TargetBone { get => Parent?.TryGetChild<BoneNode>(TargetBoneHash, out var node) == true ? node : throw new KeyNotFoundException(nameof(TargetBoneHash)); set => TargetBoneHash = value.Hash; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintNode"/> class.
        /// </summary>
        public ConstraintNode() : base(CastNodeIdentifier.Constraint) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        public ConstraintNode(CastNodeIdentifier identifier) : base(identifier) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        public ConstraintNode(CastNodeIdentifier identifier, ulong hash) : base(identifier, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        public ConstraintNode(ulong hash) : base(CastNodeIdentifier.Constraint, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public ConstraintNode(ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(CastNodeIdentifier.Constraint, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public ConstraintNode(CastNodeIdentifier identifier, ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(identifier, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintNode"/> class.
        /// </summary>
        /// <param name="source">Node to copy from. A shallow copy is performed and references to the source are stored.</param>
        public ConstraintNode(CastNode source) : base(source) { }

        /// <inheritdoc/>
        public override string ToString() => Name;
    }
}
