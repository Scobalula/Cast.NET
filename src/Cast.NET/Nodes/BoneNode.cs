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
/// A skeleton bone.
/// </summary>
/// <param name="hash">The node hash.</param>
public sealed class BoneNode(ulong hash) : CastNode(CastNodeIdentifier.Bone, hash)
{
    /// <summary>
    /// Gets or sets the name of the bone.
    /// </summary>
    public string Name { get => GetString("n") ?? string.Empty; set => SetString("n", value); }

    /// <summary>
    /// Gets or sets the index of the parent bone within the skeleton, or -1 if this bone has no parent.
    /// </summary>
    public int ParentIndex { get => GetArray("p") is { Count: > 0 } parent ? (parent.Type == CastPropertyType.Integer32 ? parent.Get<int>(0) : parent.GetScalar<int>(0)) : -1; set => SetValue("p", value); }

    /// <summary>
    /// Gets or sets whether segment scale compensation is enabled.
    /// </summary>
    public bool SegmentScaleCompensate { get => GetBoolean("ssc", true); set => SetBoolean("ssc", value); }

    /// <summary>
    /// Gets or sets the position relative to the parent bone.
    /// </summary>
    public Vector3? LocalPosition { get => GetValue<Vector3>("lp"); set => SetValue("lp", value); }

    /// <summary>
    /// Gets or sets the rotation relative to the parent bone.
    /// </summary>
    public Quaternion? LocalRotation { get => GetValue<Quaternion>("lr"); set => SetValue("lr", value); }

    /// <summary>
    /// Gets or sets the position in world space.
    /// </summary>
    public Vector3? WorldPosition { get => GetValue<Vector3>("wp"); set => SetValue("wp", value); }

    /// <summary>
    /// Gets or sets the rotation in world space.
    /// </summary>
    public Quaternion? WorldRotation { get => GetValue<Quaternion>("wr"); set => SetValue("wr", value); }

    /// <summary>
    /// Gets or sets the local scale.
    /// </summary>
    public Vector3? Scale { get => GetValue<Vector3>("s"); set => SetValue("s", value); }

    /// <summary>
    /// Initializes a new instance of the <see cref="BoneNode"/> class with a unique hash.
    /// </summary>
    public BoneNode() : this(CastHash.Next())
    {
    }
}
