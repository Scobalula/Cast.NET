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
/// The root node of a cast file.
/// </summary>
/// <param name="hash">The node hash.</param>
public sealed class RootNode(ulong hash) : CastNode(CastNodeIdentifier.Root, hash)
{
    /// <summary>
    /// Gets or sets the models. Setting this replaces all existing models.
    /// </summary>
    public ModelNode[] Models { get => [.. EnumerateChildren<ModelNode>()]; set => ReplaceChildren(value); }

    /// <summary>
    /// Gets or sets the animations. Setting this replaces all existing animations.
    /// </summary>
    public AnimationNode[] Animations { get => [.. EnumerateChildren<AnimationNode>()]; set => ReplaceChildren(value); }

    /// <summary>
    /// Gets or sets the instances. Setting this replaces all existing instances.
    /// </summary>
    public InstanceNode[] Instances { get => [.. EnumerateChildren<InstanceNode>()]; set => ReplaceChildren(value); }

    /// <summary>
    /// Gets the metadata of the scene, or <see langword="null"/> if the scene has no metadata.
    /// </summary>
    public MetadataNode? Metadata => GetChild<MetadataNode>();

    /// <summary>
    /// Initializes a new instance of the <see cref="RootNode"/> class with a unique hash.
    /// </summary>
    public RootNode() : this(CastHash.Next())
    {
    }

    /// <summary>
    /// Enumerates the models.
    /// </summary>
    /// <returns>The models.</returns>
    public IEnumerable<ModelNode> EnumerateModels() => EnumerateChildren<ModelNode>();

    /// <summary>
    /// Enumerates the animations.
    /// </summary>
    /// <returns>The animations.</returns>
    public IEnumerable<AnimationNode> EnumerateAnimations() => EnumerateChildren<AnimationNode>();

    /// <summary>
    /// Enumerates the instances.
    /// </summary>
    /// <returns>The instances.</returns>
    public IEnumerable<InstanceNode> EnumerateInstances() => EnumerateChildren<InstanceNode>();
}
