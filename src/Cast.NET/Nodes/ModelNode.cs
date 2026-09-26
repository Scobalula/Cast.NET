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
/// A model.
/// </summary>
/// <param name="hash">The node hash.</param>
public sealed class ModelNode(ulong hash) : CastNode(CastNodeIdentifier.Model, hash)
{
    /// <summary>
    /// Gets or sets the name of the model.
    /// </summary>
    public string? Name { get => GetString("n"); set => SetString("n", value); }

    /// <summary>
    /// Gets or sets the position of the model.
    /// </summary>
    public Vector3? Position { get => GetValue<Vector3>("p"); set => SetValue("p", value); }

    /// <summary>
    /// Gets or sets the rotation of the model.
    /// </summary>
    public Quaternion? Rotation { get => GetValue<Quaternion>("r"); set => SetValue("r", value); }

    /// <summary>
    /// Gets or sets the scale of the model.
    /// </summary>
    public Vector3? Scale { get => GetValue<Vector3>("s"); set => SetValue("s", value); }

    /// <summary>
    /// Gets the skeleton of the model, or <see langword="null"/> if the model has no skeleton.
    /// </summary>
    public SkeletonNode? Skeleton => GetChild<SkeletonNode>();

    /// <summary>
    /// Gets or sets the meshes. Setting this replaces all existing meshes.
    /// </summary>
    public MeshNode[] Meshes { get => [.. EnumerateChildren<MeshNode>()]; set => ReplaceChildren(value); }

    /// <summary>
    /// Gets or sets the materials. Setting this replaces all existing materials.
    /// </summary>
    public MaterialNode[] Materials { get => [.. EnumerateChildren<MaterialNode>()]; set => ReplaceChildren(value); }

    /// <summary>
    /// Gets or sets the blend shapes. Setting this replaces all existing blend shapes.
    /// </summary>
    public BlendShapeNode[] BlendShapes { get => [.. EnumerateChildren<BlendShapeNode>()]; set => ReplaceChildren(value); }

    /// <summary>
    /// Gets or sets the hairs. Setting this replaces all existing hairs.
    /// </summary>
    public HairNode[] Hairs { get => [.. EnumerateChildren<HairNode>()]; set => ReplaceChildren(value); }

    /// <summary>
    /// Initializes a new instance of the <see cref="ModelNode"/> class with a unique hash.
    /// </summary>
    public ModelNode() : this(CastHash.Next())
    {
    }

    /// <summary>
    /// Enumerates the meshes.
    /// </summary>
    /// <returns>The meshes.</returns>
    public IEnumerable<MeshNode> EnumerateMeshes() => EnumerateChildren<MeshNode>();

    /// <summary>
    /// Enumerates the materials.
    /// </summary>
    /// <returns>The materials.</returns>
    public IEnumerable<MaterialNode> EnumerateMaterials() => EnumerateChildren<MaterialNode>();

    /// <summary>
    /// Enumerates the blend shapes.
    /// </summary>
    /// <returns>The blend shapes.</returns>
    public IEnumerable<BlendShapeNode> EnumerateBlendShapes() => EnumerateChildren<BlendShapeNode>();

    /// <summary>
    /// Enumerates the hairs.
    /// </summary>
    /// <returns>The hairs.</returns>
    public IEnumerable<HairNode> EnumerateHairs() => EnumerateChildren<HairNode>();
}
