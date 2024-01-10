// ------------------------------------------------------------------------
// Cast.NET - A .NET Library for reading and writing Cast files.
// Copyright(c) 2024 Philip/Scobalula
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
using System.Diagnostics.CodeAnalysis;

namespace Cast.NET
{
    /// <summary>
    /// A class that holds a <see cref="CastNode"/>.
    /// </summary>
    [DebuggerDisplay("Identifier = {Identifier}")]
    public class CastNode
    {
        /// <summary>
        /// Internal parent value.
        /// </summary>
        private CastNode? _parent;

        /// <summary>
        /// Gets or Sets the identifier.
        /// </summary>
        public CastNodeIdentifier Identifier { get; set; }

        /// <summary>
        /// Gets or Sets the hash of the node.
        /// </summary>
        public ulong Hash { get; set; }

        /// <summary>
        /// Gets or Sets the properties.
        /// </summary>
        public Dictionary<string, CastProperty> Properties { get; set; }

        /// <summary>
        /// Gets or Sets the children of this node.
        /// </summary>
        public List<CastNode> Children { get; set; }

        /// <summary>
        /// Gets or Sets the parent node.
        /// </summary>
        public CastNode? Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent?.Children.Remove(this);
                _parent = value;
                _parent?.Children.Add(this);
            }
        }

        /// <summary>
        /// Gets the total raw size of the data within the node including its header, properties, and children.
        /// </summary>
        internal int DataSize
        {
            get
            {
                // Current header size
                var result = 24;

                foreach (var prop in Properties)
                {
                    result += 8 + prop.Key.Length;
                    result += prop.Value.DataSize;
                }
                foreach (var node in Children)
                {
                    result += node.DataSize;
                }

                return result;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        public CastNode(CastNodeIdentifier identifier)
        {
            Identifier = identifier;
            Hash = 0;
            Properties = new();
            Children = new();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        public CastNode(CastNodeIdentifier identifier, ulong hash)
        {
            Identifier = identifier;
            Hash = hash;
            Properties = new();
            Children = new();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public CastNode(CastNodeIdentifier identifier, ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children)
        {
            Identifier = identifier;
            Hash = hash;
            Properties = properties ?? new();
            Children = new();

            if (children != null)
            {
                foreach (var child in children)
                {
                    child.Parent = this;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastNode"/> class.
        /// </summary>
        /// <param name="source">
        /// Node to copy from. A shallow copy is performed and references to 
        /// the source are stored and all child nodes becoming a child of this node.
        /// </param>
        public CastNode(CastNode source)
        {
            Identifier = source.Identifier;
            Hash = source.Hash;
            Properties = source.Properties;
            Children = new();

            foreach (var child in source.Children)
            {
                child.Parent = this;
            }
        }

        /// <summary>
        /// Adds the node to the child list of this node.
        /// </summary>
        /// <typeparam name="T">The type to add.</typeparam>
        /// <returns>The node that was added.</returns>
        public T AddNode<T>() where T : CastNode, new()
        {
            var node = new T
            {
                Parent = this
            };
            return node;
        }

        /// <summary>
        /// Adds the node to the child list of this node.
        /// </summary>
        /// <typeparam name="T">The type to add.</typeparam>
        /// <param name="node">The node to add.</param>
        /// <returns>The node that was added.</returns>
        public T AddNode<T>(T node) where T : CastNode
        {
            node.Parent = this;
            return node;
        }

        /// <summary>
        /// Gets the number of children within this node with the given identifier.
        /// </summary>
        /// <param name="id">The identifier to look for.</param>
        /// <returns>The number of children with the given identifier.</returns>
        public int ChildCountOfType(CastNodeIdentifier id)
        {
            var result = 0;

            foreach (var child in Children)
            {
                if(child.Identifier == id)
                {
                    result++;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the number of children within this node of the given type.
        /// </summary>
        /// <typeparam name="T">The type to look for.</typeparam>
        /// <returns>The number of children of the given type.</returns>
        public int ChildCountOfType<T>() where T : CastNode
        {
            var result = 0;
            var t = typeof(T);

            foreach (var child in Children)
            {
                if (child.GetType() == t)
                {
                    result++;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the first child with the given identifier.
        /// </summary>
        /// <param name="id">The identifier to look for.</param>
        /// <returns>The child with the given identifier if found, otherwise null.</returns>
        public CastNode? GetFirstChildOfTypeOrNull(CastNodeIdentifier id)
        {
            foreach (var child in Children)
            {
                if (child.Identifier == id)
                {
                    return child;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the first child of the given type.
        /// </summary>
        /// <typeparam name="T">The type to look for.</typeparam>
        /// <returns>The child of the given type if found, otherwise null.</returns>
        public T? GetFirstChildOfTypeOrNull<T>() where T : CastNode
        {
            var t = typeof(T);

            foreach (var child in Children)
            {
                if (child.GetType() == t)
                {
                    return (T)child;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the first child with the given hash.
        /// </summary>
        /// <typeparam name="T">The type to look for.</typeparam>
        /// <param name="hash">The hash to look for.</param>
        /// <returns>The child with the given hash and of the given type if found, otherwise null.</returns>
        public T? GetChildByHashOrNull<T>(ulong hash) where T : CastNode
        {
            if (hash != 0)
            {
                if (Children.Find(x => x.Hash == hash) is T result)
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the child at the given index with the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException">Thrown if the node at the given index is of an invalid type.</exception>
        public T GetChild<T>(int index) where T : CastNode
        {
            if (Children[index] is not T r)
                throw new InvalidCastException();

            return r;
        }

        /// <summary>
        /// Gets all children with the given identifier.
        /// </summary>
        /// <param name="id">The identifier to look for.</param>
        /// <returns>All children found with the given identifier.</returns>
        public CastNode[] GetChildrenOfType(CastNodeIdentifier id)
        {
            var results = new List<CastNode>(Children.Count);

            foreach (var child in Children)
            {
                if (child.Identifier == id)
                {
                    results.Add(child);
                }
            }

            return results.ToArray();
        }

        /// <summary>
        /// Gets all children of the given type.
        /// </summary>
        /// <typeparam name="T">The type to look for.</typeparam>
        /// <returns>All children found of the given type.</returns>
        public T[] GetChildrenOfType<T>() where T : CastNode
        {
            var t = typeof(T);
            var results = new List<T>(Children.Count);

            foreach (var child in Children)
            {
                if (child.GetType() == t)
                {
                    results.Add((T)child);
                }
            }

            return results.ToArray();
        }

        /// <summary>
        /// Enumerates through all children with the given identifier.
        /// </summary>
        /// <param name="id">The identifier to look for.</param>
        /// <returns>An enumerable collection of all children with the given identifier.</returns>
        public IEnumerable<CastNode> EnumerateChildrenOfType(CastNodeIdentifier id)
        {
            foreach (var x in Children)
            {
                if (x.Identifier == id)
                {
                    yield return x;
                }
            }
        }

        /// <summary>
        /// Enumerates through all children of the given type.
        /// </summary>
        /// <typeparam name="T">The type to look for</typeparam>
        /// <returns>An enumerable collection of all children of the given type.</returns>
        public IEnumerable<T> EnumerateChildrenOfType<T>() where T : CastNode
        {
            var t = typeof(T);

            foreach (var x in Children)
            {
                if (x.GetType() == t)
                {
                    yield return (T)x;
                }
            }
        }

        /// <summary>
        /// Gets a string value with the given property key.
        /// </summary>
        /// <param name="propKey">The property key to look for.</param>
        /// <returns>The string value for the given property key.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if a property with the given key was not found.</exception>
        /// <exception cref="InvalidCastException">Thrown if the property stored with the given key is of an invalid type.</exception>
        public string GetStringValue(string propKey)
        {
            if (!Properties.TryGetValue(propKey, out var prop))
            {
                throw new KeyNotFoundException($"Failed to find property: {propKey} in node of type: {GetType()}");
            }
            if (prop is not CastStringProperty stringProp)
            {
                throw new InvalidCastException($"Invalid property type: {prop} for key: {propKey} in node of type: {GetType()}");
            }

            return stringProp.Value;
        }

        /// <summary>
        /// Gets a string value with the given property key.
        /// </summary>
        /// <param name="propKey">The property key to look for.</param>
        /// <param name="defaultValue">The default value to return.</param>
        /// <returns>
        /// The string value for the given property key.
        /// If a property with the given key is not found or has an invalid type, the default value is returned.
        /// </returns>
        public string GetStringValueOrDefault(string propKey, string defaultValue)
        {
            if (Properties.TryGetValue(propKey, out var prop) && prop is CastStringProperty stringProp)
            {
                return stringProp.Value;
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets the first value in the array with the given property key.
        /// </summary>
        /// <typeparam name="T">The type to look for.</typeparam>
        /// <param name="propKey">The property key to look for.</param>
        /// <returns>The first value within the array with the given property key.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if a property with the given key was not found.</exception>
        /// <exception cref="InvalidCastException">Thrown if the property stored with the given key is of an invalid type.</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown if the array contains no elements.</exception>
        public T GetFirstValue<T>(string propKey) where T : unmanaged
        {
            if (!Properties.TryGetValue(propKey, out var prop))
            {
                throw new KeyNotFoundException($"Failed to find property: {propKey} in node of type: {GetType()}");
            }
            if (prop is not CastArrayProperty<T> arrayProp)
            {
                throw new InvalidCastException($"Invalid property type: {prop} for key: {propKey} in node of type: {GetType()}");
            }
            if(arrayProp.Values.Count == 0)
            {
                throw new IndexOutOfRangeException($"Array for key: {propKey} in node of type: {GetType()} contains no elements");
            }

            return arrayProp.Values[0];
        }

        /// <summary>
        /// Gets the first value in the array with the given property key.
        /// </summary>
        /// <typeparam name="T">The type to look for.</typeparam>
        /// <param name="propKey">The property key to look for.</param>
        /// <param name="defaultValue">The default value to return.</param>
        /// <returns>The first value within the array with the given property key, otherwise the default value.</returns>
        public T GetFirstValueOrDefault<T>(string propKey, T defaultValue) where T : unmanaged
        {
            if (Properties.TryGetValue(propKey, out var prop) && prop is CastArrayProperty<T> arrayProp)
            {
                return arrayProp.GetFirstValue(defaultValue);
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets the first integer in the array for the given bit count.
        /// </summary>
        /// <param name="propKey">The property key to look for.</param>
        /// <param name="defaultValue">The default value to return.</param>
        /// <param name="maxBits">The max number of bits for the integer type.</param>
        /// <returns>The first value within the array with the given property key, otherwise the default value.</returns>
        public ulong GetFirstIntegerOrDefault(string propKey, ulong defaultValue, uint maxBits)
        {
            if (Properties.TryGetValue(propKey, out var prop))
            {
                if (prop is CastArrayProperty<ulong> aul && maxBits >= 64)
                {
                    return aul.GetFirstValue(defaultValue);
                }
                else if (prop is CastArrayProperty<uint> aup && maxBits >= 32)
                {
                    return aup.GetFirstValue((uint)defaultValue);
                }
                else if(prop is CastArrayProperty<ushort> asp && maxBits >= 16)
                {
                    return asp.GetFirstValue((ushort)defaultValue);
                }
                else if(prop is CastArrayProperty<byte> abp && maxBits >= 8)
                {
                    return abp.GetFirstValue((byte)defaultValue);
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets the property with the given property key.
        /// </summary>
        /// <param name="propKey">The property key to look for.</param>
        /// <returns>The property.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if a property with the given key was not found.</exception>
        public CastProperty GetProperty(string propKey)
        {
            if (!Properties.TryGetValue(propKey, out var result))
            {
                throw new KeyNotFoundException();
            }

            return result;
        }

        /// <summary>
        /// Gets the property with the given property key.
        /// </summary>
        /// <param name="propKey">The property key to look for.</param>
        /// <returns>The property if found, otherwise null.</returns>
        public CastProperty? GetPropertyOrNull(string propKey)
        {
            if (!Properties.TryGetValue(propKey, out var result))
            {
                return null;
            }

            return result;
        }


        /// <summary>
        /// Gets the property with the given property key.
        /// </summary>
        /// <typeparam name="T">The type to look for.</typeparam>
        /// <param name="propKey">The property key to look for.</param>
        /// <returns>The property.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if a property with the given key was not found.</exception>
        public T GetProperty<T>(string propKey) where T : CastProperty
        {
            if (!Properties.TryGetValue(propKey, out var result) && result is not T)
            {
                throw new KeyNotFoundException();
            }

            return (T)result;
        }

        /// <summary>
        /// Gets the property of the given type with the given property key.
        /// </summary>
        /// <typeparam name="T">The type to look for.</typeparam>
        /// <param name="propKey">The property key to look for.</param>
        /// <returns>The property if found, otherwise null.</returns>
        public T? GetPropertyOrNull<T>(string propKey) where T : CastProperty
        {
            if (!Properties.TryGetValue(propKey, out var result) && result is not T)
            {
                return null;
            }

            return (T)result;
        }

        /// <summary>
        /// Add the value to the properties, overwriting any previous property with the given key.
        /// </summary>
        /// <typeparam name="T">The type to add.</typeparam>
        /// <param name="propKey">The property key to look for.</param>
        /// <param name="value">The value to add.</param>
        /// <returns>The resulting property that was added to the node.</returns>
        public CastStringProperty AddString(string propKey, string value)
        {
            var result = new CastStringProperty(value);
            Properties[propKey] = result;
            return result;
        }

        /// <summary>
        /// Add the value to the properties, overwriting any previous property with the given key.
        /// </summary>
        /// <typeparam name="T">The type to add.</typeparam>
        /// <param name="propKey">The property key to look for.</param>
        /// <returns>The resulting property that was added to the node.</returns>
        public CastArrayProperty<T> AddArray<T>(string propKey) where T : unmanaged
        {
            var result = new CastArrayProperty<T>();
            Properties[propKey] = result;
            return result;
        }

        /// <summary>
        /// Add the value to the properties, overwriting any previous property with the given key.
        /// </summary>
        /// <typeparam name="T">The type to add.</typeparam>
        /// <param name="propKey">The property key to look for.</param>
        /// <param name="value">The value to add.</param>
        /// <returns>The resulting property that was added to the node.</returns>
        public CastArrayProperty<T> AddArray<T>(string propKey, List<T> value) where T : unmanaged
        {
            var result = new CastArrayProperty<T>(value);
            Properties[propKey] = result;
            return result;
        }

        /// <summary>
        /// Add the value to the properties, overwriting any previous property with the given key.
        /// </summary>
        /// <typeparam name="T">The type to add.</typeparam>
        /// <param name="propKey">The property key to look for.</param>
        /// <param name="value">The value to add.</param>
        /// <returns>The resulting property that was added to the node.</returns>
        public CastArrayProperty<T> AddArray<T>(string name, IEnumerable<T> value) where T : unmanaged
        {
            var result = new CastArrayProperty<T>(value);
            Properties[name] = result;
            return result;
        }

        /// <summary>
        /// Add the value to the properties, overwriting any previous property with the given key.
        /// </summary>
        /// <typeparam name="T">The type to add.</typeparam>
        /// <param name="propKey">The property key to look for.</param>
        /// <param name="value">The value to add.</param>
        /// <returns>The resulting property that was added to the node.</returns>
        public CastArrayProperty<T> AddValue<T>(string propKey, T value) where T : unmanaged
        {
            var result = new CastArrayProperty<T>(value);
            Properties[propKey] = result;
            return result;
        }
    }
}
