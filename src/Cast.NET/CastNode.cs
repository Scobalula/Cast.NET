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
using System.Diagnostics;
using System.Numerics;

namespace CastNet;

/// <summary>
/// A node holding properties and child nodes.
/// </summary>
/// <param name="identifier">The node identifier.</param>
/// <param name="hash">The node hash.</param>
[DebuggerDisplay("{Identifier}, Hash = {Hash}, Children = {Children.Count}")]
public class CastNode(CastNodeIdentifier identifier, ulong hash)
{
    private readonly List<CastNode> _children = [];

    /// <summary>
    /// Gets the node identifier.
    /// </summary>
    public CastNodeIdentifier Identifier { get; } = identifier;

    /// <summary>
    /// Gets or sets the hash other nodes use to reference this node.
    /// </summary>
    public ulong Hash { get; set; } = hash;

    /// <summary>
    /// Gets the properties of this node, keyed by name.
    /// </summary>
    public Dictionary<string, CastProperty> Properties { get; } = [];

    /// <summary>
    /// Gets the child nodes of this node.
    /// </summary>
    public IReadOnlyList<CastNode> Children => _children;

    /// <summary>
    /// Gets the parent node, or <see langword="null"/> if this node has no parent.
    /// </summary>
    public CastNode? Parent { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CastNode"/> class with a unique hash.
    /// </summary>
    /// <param name="identifier">The node identifier.</param>
    public CastNode(CastNodeIdentifier identifier) : this(identifier, CastHash.Next())
    {
    }

    /// <summary>
    /// Creates a new node of the given type and adds it as a child of this node.
    /// </summary>
    /// <typeparam name="T">The node type.</typeparam>
    /// <returns>The added node.</returns>
    public T AddNode<T>() where T : CastNode, new() => AddNode(new T());

    /// <summary>
    /// Adds the node as a child of this node, removing it from its current parent.
    /// </summary>
    /// <typeparam name="T">The node type.</typeparam>
    /// <param name="node">The node to add.</param>
    /// <returns>The added node.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the node is this node or one of its ancestors.</exception>
    public T AddNode<T>(T node) where T : CastNode
    {
        for (var ancestor = this; ancestor is not null; ancestor = ancestor.Parent)
        {
            if (ancestor == node)
                throw new InvalidOperationException("A node cannot be added to itself or its descendants.");
        }

        node.Parent?._children.Remove(node);
        node.Parent = this;
        _children.Add(node);
        return node;
    }

    /// <summary>
    /// Removes the node from the children of this node.
    /// </summary>
    /// <param name="node">The node to remove.</param>
    /// <returns><see langword="true"/> if the node was removed, otherwise <see langword="false"/>.</returns>
    public bool RemoveNode(CastNode node)
    {
        if (!_children.Remove(node))
            return false;

        node.Parent = null;
        return true;
    }

    /// <summary>
    /// Gets the first child of the given type.
    /// </summary>
    /// <typeparam name="T">The node type.</typeparam>
    /// <returns>The first child of the given type, or <see langword="null"/> if none exists.</returns>
    public T? GetChild<T>() where T : CastNode
    {
        foreach (var child in _children)
        {
            if (child is T result)
                return result;
        }

        return null;
    }

    /// <summary>
    /// Finds the child of the given type with the given hash.
    /// </summary>
    /// <typeparam name="T">The node type.</typeparam>
    /// <param name="hash">The hash of the node.</param>
    /// <returns>The matching child, or <see langword="null"/> if none exists.</returns>
    public T? FindChild<T>(ulong hash) where T : CastNode
    {
        foreach (var child in _children)
        {
            if (child.Hash == hash && child is T result)
                return result;
        }

        return null;
    }

    /// <summary>
    /// Enumerates all children of the given type.
    /// </summary>
    /// <typeparam name="T">The node type.</typeparam>
    /// <returns>The children of the given type.</returns>
    public IEnumerable<T> EnumerateChildren<T>() where T : CastNode
    {
        foreach (var child in _children)
        {
            if (child is T result)
                yield return result;
        }
    }

    /// <summary>
    /// Gets the string property with the given name.
    /// </summary>
    /// <param name="name">The property name.</param>
    /// <returns>The string, or <see langword="null"/> if not found.</returns>
    public string? GetString(string name) => Properties.GetValueOrDefault(name) is CastStringProperty property ? property.Value : null;

    /// <summary>
    /// Sets the string property with the given name, removing it if <paramref name="value"/> is <see langword="null"/>.
    /// </summary>
    /// <param name="name">The property name.</param>
    /// <param name="value">The string to store.</param>
    public void SetString(string name, string? value)
    {
        if (value is null)
            Properties.Remove(name);
        else
            Properties[name] = new CastStringProperty(value);
    }

    /// <summary>
    /// Gets the array property with the given name.
    /// </summary>
    /// <param name="name">The property name.</param>
    /// <returns>The array, or <see langword="null"/> if not found.</returns>
    public CastArrayProperty? GetArray(string name) => Properties.GetValueOrDefault(name) as CastArrayProperty;

    /// <summary>
    /// Sets the array property with the given name, removing it if <paramref name="value"/> is <see langword="null"/>.
    /// </summary>
    /// <param name="name">The property name.</param>
    /// <param name="value">The array to store.</param>
    public void SetArray(string name, CastArrayProperty? value)
    {
        if (value is null)
            Properties.Remove(name);
        else
            Properties[name] = value;
    }

    /// <summary>
    /// Gets the first value of the array property with the given name.
    /// </summary>
    /// <typeparam name="T">The value type, matching the stored type.</typeparam>
    /// <param name="name">The property name.</param>
    /// <returns>The first value, or <see langword="null"/> if not found.</returns>
    /// <exception cref="InvalidCastException">Thrown if <typeparamref name="T"/> does not map to the stored type.</exception>
    public T? GetValue<T>(string name) where T : unmanaged => GetArray(name) is { Count: > 0 } array ? array.Get<T>(0) : null;

    /// <summary>
    /// Gets the first value of the array property with the given name.
    /// </summary>
    /// <typeparam name="T">The value type, matching the stored type.</typeparam>
    /// <param name="name">The property name.</param>
    /// <param name="defaultValue">The value returned if not found.</param>
    /// <returns>The first value, or <paramref name="defaultValue"/>.</returns>
    /// <exception cref="InvalidCastException">Thrown if <typeparamref name="T"/> does not map to the stored type.</exception>
    public T GetValue<T>(string name, T defaultValue) where T : unmanaged => GetValue<T>(name) ?? defaultValue;

    /// <summary>
    /// Gets the first value of the array property with the given name, converted to <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The numeric type to convert to.</typeparam>
    /// <param name="name">The property name.</param>
    /// <returns>The first value, or <see langword="null"/> if not found.</returns>
    public T? GetScalar<T>(string name) where T : unmanaged, INumberBase<T> => GetArray(name) is { Count: > 0 } array ? array.GetScalar<T>(0) : null;

    /// <summary>
    /// Gets the first value of the array property with the given name, converted to <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The numeric type to convert to.</typeparam>
    /// <param name="name">The property name.</param>
    /// <param name="defaultValue">The value returned if not found.</param>
    /// <returns>The first value, or <paramref name="defaultValue"/>.</returns>
    public T GetScalar<T>(string name, T defaultValue) where T : unmanaged, INumberBase<T> => GetScalar<T>(name) ?? defaultValue;

    /// <summary>
    /// Sets the array property with the given name to a single value.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="name">The property name.</param>
    /// <param name="value">The value to store.</param>
    public void SetValue<T>(string name, T value) where T : unmanaged => Properties[name] = CastArrayProperty.Create(value);

    /// <summary>
    /// Sets the array property with the given name to a single value, removing it if <paramref name="value"/> is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="name">The property name.</param>
    /// <param name="value">The value to store.</param>
    public void SetValue<T>(string name, T? value) where T : unmanaged => SetArray(name, value is null ? null : CastArrayProperty.Create(value.Value));

    /// <summary>
    /// Gets the boolean property with the given name. Any non-zero value is <see langword="true"/>.
    /// </summary>
    /// <param name="name">The property name.</param>
    /// <param name="defaultValue">The value returned if not found.</param>
    /// <returns>The boolean, or <paramref name="defaultValue"/>.</returns>
    public bool GetBoolean(string name, bool defaultValue) => GetScalar<double>(name) is double value ? value != 0 : defaultValue;

    /// <summary>
    /// Sets the boolean property with the given name.
    /// </summary>
    /// <param name="name">The property name.</param>
    /// <param name="value">The boolean to store.</param>
    public void SetBoolean(string name, bool value) => SetValue<byte>(name, value ? (byte)1 : (byte)0);

    /// <inheritdoc/>
    public override string ToString() => GetString("n") ?? GetString("nn") ?? Identifier.ToString();

    /// <summary>
    /// Finds the sibling node referenced by the hash in the property with the given name.
    /// </summary>
    /// <typeparam name="T">The node type.</typeparam>
    /// <param name="name">The property name.</param>
    /// <returns>The node, or <see langword="null"/> if not found.</returns>
    protected T? FindSibling<T>(string name) where T : CastNode => GetScalar<ulong>(name) is ulong hash ? Parent?.FindChild<T>(hash) : null;

    /// <summary>
    /// Replaces all children of the given type with the given nodes.
    /// </summary>
    /// <typeparam name="T">The node type.</typeparam>
    /// <param name="nodes">The nodes to add.</param>
    protected void ReplaceChildren<T>(IEnumerable<T> nodes) where T : CastNode
    {
        for (var i = _children.Count - 1; i >= 0; i--)
        {
            if (_children[i] is T child)
            {
                _children.RemoveAt(i);
                child.Parent = null;
            }
        }

        foreach (var node in nodes)
            AddNode(node);
    }
}
