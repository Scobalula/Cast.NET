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
/// An IK chain between bones.
/// </summary>
/// <param name="hash">The node hash.</param>
public sealed class IKHandleNode(ulong hash) : CastNode(CastNodeIdentifier.IKHandle, hash)
{
    /// <summary>
    /// Gets or sets the name of the IK handle.
    /// </summary>
    public string? Name { get => GetString("n"); set => SetString("n", value); }

    /// <summary>
    /// Gets or sets the bone the chain starts at, resolved from the parent skeleton.
    /// </summary>
    public BoneNode? StartBone { get => FindSibling<BoneNode>("sb"); set => SetValue("sb", value?.Hash); }

    /// <summary>
    /// Gets or sets the bone the chain ends at, resolved from the parent skeleton.
    /// </summary>
    public BoneNode? EndBone { get => FindSibling<BoneNode>("eb"); set => SetValue("eb", value?.Hash); }

    /// <summary>
    /// Gets or sets the bone the chain targets, resolved from the parent skeleton.
    /// </summary>
    public BoneNode? TargetBone { get => FindSibling<BoneNode>("tb"); set => SetValue("tb", value?.Hash); }

    /// <summary>
    /// Gets or sets the offset applied to the target.
    /// </summary>
    public Vector3? TargetOffset { get => GetValue<Vector3>("to"); set => SetValue("to", value); }

    /// <summary>
    /// Gets or sets the pole vector bone, resolved from the parent skeleton.
    /// </summary>
    public BoneNode? PoleVectorBone { get => FindSibling<BoneNode>("pv"); set => SetValue("pv", value?.Hash); }

    /// <summary>
    /// Gets or sets the pole (twist) bone, resolved from the parent skeleton.
    /// </summary>
    public BoneNode? PoleBone { get => FindSibling<BoneNode>("pb"); set => SetValue("pb", value?.Hash); }

    /// <summary>
    /// Gets or sets whether the rotation of the target bone is applied to the end bone.
    /// </summary>
    public bool UseTargetRotation { get => GetBoolean("tr", false); set => SetBoolean("tr", value); }

    /// <summary>
    /// Initializes a new instance of the <see cref="IKHandleNode"/> class with a unique hash.
    /// </summary>
    public IKHandleNode() : this(CastHash.Next())
    {
    }
}
