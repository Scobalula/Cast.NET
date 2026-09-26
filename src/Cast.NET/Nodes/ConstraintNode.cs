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
/// A constraint between two bones.
/// </summary>
/// <param name="hash">The node hash.</param>
public sealed class ConstraintNode(ulong hash) : CastNode(CastNodeIdentifier.Constraint, hash)
{
    /// <summary>
    /// Gets or sets the name of the constraint.
    /// </summary>
    public string? Name { get => GetString("n"); set => SetString("n", value); }

    /// <summary>
    /// Gets or sets the constraint type: <c>pt</c> (point), <c>or</c> (orient) or <c>sc</c> (scale).
    /// </summary>
    public string ConstraintType { get => GetString("ct") ?? string.Empty; set => SetString("ct", value); }

    /// <summary>
    /// Gets or sets the constrained bone, resolved from the parent skeleton.
    /// </summary>
    public BoneNode? ConstraintBone { get => FindSibling<BoneNode>("cb"); set => SetValue("cb", value?.Hash); }

    /// <summary>
    /// Gets or sets the target bone, resolved from the parent skeleton.
    /// </summary>
    public BoneNode? TargetBone { get => FindSibling<BoneNode>("tb"); set => SetValue("tb", value?.Hash); }

    /// <summary>
    /// Gets or sets whether the initial offset between the bones is maintained.
    /// </summary>
    public bool MaintainOffset { get => GetBoolean("mo", false); set => SetBoolean("mo", value); }

    /// <summary>
    /// Gets or sets the custom offset: a quaternion for orient constraints, XYZ for point and scale constraints. <see cref="ConstraintType"/> must be set first.
    /// </summary>
    public Vector4 CustomOffset
    {
        get => GetArray("co") switch
        {
            { Type: CastPropertyType.Vector3, Count: > 0 } offset => new Vector4(offset.Get<Vector3>(0), 0.0f),
            { Type: CastPropertyType.Vector4, Count: > 0 } offset => offset.Get<Vector4>(0),
            _ => ConstraintType switch { "or" => Vector4.UnitW, "sc" => new Vector4(Vector3.One, 0.0f), _ => Vector4.Zero },
        };
        set => SetArray("co", ConstraintType switch
        {
            "or" => CastArrayProperty.Create(value),
            "pt" or "sc" => CastArrayProperty.Create(value.AsVector3()),
            _ => throw new InvalidOperationException("Set ConstraintType before CustomOffset."),
        });
    }

    /// <summary>
    /// Gets or sets the weight of the constraint.
    /// </summary>
    public float Weight { get => GetScalar("wt", 1.0f); set => SetValue("wt", value); }

    /// <summary>
    /// Gets or sets whether the X axis is skipped.
    /// </summary>
    public bool SkipX { get => GetBoolean("sx", false); set => SetBoolean("sx", value); }

    /// <summary>
    /// Gets or sets whether the Y axis is skipped.
    /// </summary>
    public bool SkipY { get => GetBoolean("sy", false); set => SetBoolean("sy", value); }

    /// <summary>
    /// Gets or sets whether the Z axis is skipped.
    /// </summary>
    public bool SkipZ { get => GetBoolean("sz", false); set => SetBoolean("sz", value); }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConstraintNode"/> class with a unique hash.
    /// </summary>
    public ConstraintNode() : this(CastHash.Next())
    {
    }
}
