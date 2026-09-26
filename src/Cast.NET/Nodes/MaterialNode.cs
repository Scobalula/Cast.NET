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
/// A material. Properties other than the name and type are slots, such as <c>albedo</c>, referencing a child <see cref="FileNode"/> or <see cref="ColorNode"/>.
/// </summary>
/// <param name="hash">The node hash.</param>
public sealed class MaterialNode(ulong hash) : CastNode(CastNodeIdentifier.Material, hash)
{
    /// <summary>
    /// Gets or sets the name of the material.
    /// </summary>
    public string Name { get => GetString("n") ?? string.Empty; set => SetString("n", value); }

    /// <summary>
    /// Gets or sets the material type.
    /// </summary>
    public string Type { get => GetString("t") ?? "pbr"; set => SetString("t", value); }

    /// <summary>
    /// Gets or sets the albedo slot.
    /// </summary>
    public CastNode? Albedo { get => GetSlot("albedo"); set => SetSlot("albedo", value); }

    /// <summary>
    /// Gets or sets the diffuse slot.
    /// </summary>
    public CastNode? Diffuse { get => GetSlot("diffuse"); set => SetSlot("diffuse", value); }

    /// <summary>
    /// Gets or sets the normal slot.
    /// </summary>
    public CastNode? Normal { get => GetSlot("normal"); set => SetSlot("normal", value); }

    /// <summary>
    /// Gets or sets the specular slot.
    /// </summary>
    public CastNode? Specular { get => GetSlot("specular"); set => SetSlot("specular", value); }

    /// <summary>
    /// Gets or sets the emissive slot.
    /// </summary>
    public CastNode? Emissive { get => GetSlot("emissive"); set => SetSlot("emissive", value); }

    /// <summary>
    /// Gets or sets the emissive mask slot.
    /// </summary>
    public CastNode? EmissiveMask { get => GetSlot("emask"); set => SetSlot("emask", value); }

    /// <summary>
    /// Gets or sets the gloss slot.
    /// </summary>
    public CastNode? Gloss { get => GetSlot("gloss"); set => SetSlot("gloss", value); }

    /// <summary>
    /// Gets or sets the roughness slot.
    /// </summary>
    public CastNode? Roughness { get => GetSlot("roughness"); set => SetSlot("roughness", value); }

    /// <summary>
    /// Gets or sets the ambient occlusion slot.
    /// </summary>
    public CastNode? AmbientOcclusion { get => GetSlot("ao"); set => SetSlot("ao", value); }

    /// <summary>
    /// Gets or sets the cavity slot.
    /// </summary>
    public CastNode? Cavity { get => GetSlot("cavity"); set => SetSlot("cavity", value); }

    /// <summary>
    /// Gets or sets the anisotropy slot.
    /// </summary>
    public CastNode? Anisotropy { get => GetSlot("aniso"); set => SetSlot("aniso", value); }

    /// <summary>
    /// Gets or sets the metalness slot.
    /// </summary>
    public CastNode? Metalness { get => GetSlot("metal"); set => SetSlot("metal", value); }

    /// <summary>
    /// Initializes a new instance of the <see cref="MaterialNode"/> class with a unique hash.
    /// </summary>
    public MaterialNode() : this(CastHash.Next())
    {
    }

    /// <summary>
    /// Gets the node assigned to the given slot.
    /// </summary>
    /// <param name="slot">The slot name, such as <c>albedo</c>.</param>
    /// <returns>The node, or <see langword="null"/> if the slot is empty.</returns>
    public CastNode? GetSlot(string slot) => GetScalar<ulong>(slot) is ulong hash ? FindChild<CastNode>(hash) : null;

    /// <summary>
    /// Assigns a node to the given slot, or clears it if <paramref name="node"/> is <see langword="null"/>.
    /// </summary>
    /// <param name="slot">The slot name, such as <c>albedo</c>.</param>
    /// <param name="node">The <see cref="FileNode"/> or <see cref="ColorNode"/> to assign.</param>
    /// <exception cref="InvalidOperationException">Thrown if the node already belongs to another node.</exception>
    public void SetSlot(string slot, CastNode? node)
    {
        if (node?.Parent is not null && node.Parent != this)
            throw new InvalidOperationException("The node already belongs to another node. Create a separate node for each material.");

        if (node is not null && node.Parent is null)
            AddNode(node);

        SetValue(slot, node?.Hash);
    }

    /// <summary>
    /// Enumerates the assigned slots.
    /// </summary>
    /// <returns>The slot names and their assigned nodes.</returns>
    public IEnumerable<KeyValuePair<string, CastNode>> EnumerateSlots()
    {
        foreach (var (name, property) in Properties)
        {
            if (property is CastArrayProperty { Type: CastPropertyType.Integer64, Count: > 0 } slot && FindChild<CastNode>(slot.Get<ulong>(0)) is CastNode node)
                yield return new(name, node);
        }
    }
}
