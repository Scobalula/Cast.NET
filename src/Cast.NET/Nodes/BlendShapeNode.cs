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
/// A blend shape target for a mesh.
/// </summary>
/// <param name="hash">The node hash.</param>
public sealed class BlendShapeNode(ulong hash) : CastNode(CastNodeIdentifier.BlendShape, hash)
{
    /// <summary>
    /// Gets or sets the name of the blend shape.
    /// </summary>
    public string Name { get => GetString("n") ?? string.Empty; set => SetString("n", value); }

    /// <summary>
    /// Gets or sets the base mesh this shape deforms, resolved from the parent model.
    /// </summary>
    public MeshNode? BaseShape { get => FindSibling<MeshNode>("b"); set => SetValue("b", value?.Hash); }

    /// <summary>
    /// Gets or sets the deformed vertex indices, stored as any integer type.
    /// </summary>
    public CastArrayProperty? VertexIndices { get => GetArray("vi"); set => SetArray("vi", value); }

    /// <summary>
    /// Gets or sets the deformed vertex positions, one per entry in <see cref="VertexIndices"/>.
    /// </summary>
    public CastArrayProperty? VertexPositions { get => GetArray("vp"); set => SetArray("vp", value); }

    /// <summary>
    /// Gets or sets the maximum weight the shape deforms to.
    /// </summary>
    public float TargetWeightScale { get => GetScalar("ts", 1.0f); set => SetValue("ts", value); }

    /// <summary>
    /// Initializes a new instance of the <see cref="BlendShapeNode"/> class with a unique hash.
    /// </summary>
    public BlendShapeNode() : this(CastHash.Next())
    {
    }
}
