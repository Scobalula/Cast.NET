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
namespace CastNet.Nodes;

/// <summary>
/// An animation curve for a single node property.
/// </summary>
/// <param name="hash">The node hash.</param>
public sealed class CurveNode(ulong hash) : CastNode(CastNodeIdentifier.Curve, hash)
{
    /// <summary>
    /// Gets or sets the name of the node this curve animates.
    /// </summary>
    public string NodeName { get => GetString("nn") ?? string.Empty; set => SetString("nn", value); }

    /// <summary>
    /// Gets or sets the property this curve animates: <c>rq</c>, <c>tx</c>, <c>ty</c>, <c>tz</c>, <c>sx</c>, <c>sy</c>, <c>sz</c>, <c>bs</c> or <c>vb</c>.
    /// </summary>
    public string KeyPropertyName { get => GetString("kp") ?? string.Empty; set => SetString("kp", value); }

    /// <summary>
    /// Gets or sets the key frames, stored as any integer type.
    /// </summary>
    public CastArrayProperty? KeyFrames { get => GetArray("kb"); set => SetArray("kb", value); }

    /// <summary>
    /// Gets or sets the key values: quaternions for rotations, floats for translations, scales and blend shapes, any integer type for visibility.
    /// </summary>
    public CastArrayProperty? KeyValues { get => GetArray("kv"); set => SetArray("kv", value); }

    /// <summary>
    /// Gets or sets the curve mode: <c>additive</c>, <c>absolute</c> or <c>relative</c>.
    /// </summary>
    public string Mode { get => GetString("m") ?? "relative"; set => SetString("m", value); }

    /// <summary>
    /// Gets or sets the blend weight applied to additive curves.
    /// </summary>
    public float AdditiveBlendWeight { get => GetScalar("ab", 1.0f); set => SetValue("ab", value); }

    /// <summary>
    /// Initializes a new instance of the <see cref="CurveNode"/> class with a unique hash.
    /// </summary>
    public CurveNode() : this(CastHash.Next())
    {
    }
}
