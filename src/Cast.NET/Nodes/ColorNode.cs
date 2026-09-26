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
/// A color used by a material slot.
/// </summary>
/// <param name="hash">The node hash.</param>
public sealed class ColorNode(ulong hash) : CastNode(CastNodeIdentifier.Color, hash)
{
    /// <summary>
    /// Gets or sets the name of the color.
    /// </summary>
    public string? Name { get => GetString("n"); set => SetString("n", value); }

    /// <summary>
    /// Gets or sets the color space: <c>srgb</c> or <c>linear</c>.
    /// </summary>
    public string ColorSpace { get => GetString("cs") ?? "srgb"; set => SetString("cs", value); }

    /// <summary>
    /// Gets or sets the color.
    /// </summary>
    public Vector4 Rgba { get => GetValue("rgba", Vector4.One); set => SetValue("rgba", value); }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorNode"/> class with a unique hash.
    /// </summary>
    public ColorNode() : this(CastHash.Next())
    {
    }
}
