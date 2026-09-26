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
/// Hair strands made of particles.
/// </summary>
/// <param name="hash">The node hash.</param>
public sealed class HairNode(ulong hash) : CastNode(CastNodeIdentifier.Hair, hash)
{
    /// <summary>
    /// Gets or sets the name of the hair.
    /// </summary>
    public string? Name { get => GetString("n"); set => SetString("n", value); }

    /// <summary>
    /// Gets or sets the segment count of each strand, stored as any integer type. A strand has one more particle than segments.
    /// </summary>
    public CastArrayProperty? Segments { get => GetArray("se"); set => SetArray("se", value); }

    /// <summary>
    /// Gets or sets the world space particles of every strand, in order.
    /// </summary>
    public CastArrayProperty? Particles { get => GetArray("pt"); set => SetArray("pt", value); }

    /// <summary>
    /// Gets or sets the material assigned to this hair, resolved from the parent model.
    /// </summary>
    public MaterialNode? Material { get => FindSibling<MaterialNode>("m"); set => SetValue("m", value?.Hash); }

    /// <summary>
    /// Initializes a new instance of the <see cref="HairNode"/> class with a unique hash.
    /// </summary>
    public HairNode() : this(CastHash.Next())
    {
    }
}
