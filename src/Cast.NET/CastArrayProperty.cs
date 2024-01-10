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
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cast.NET
{
    /// <summary>
    /// A class to hold an array of primitive properties within a <see cref="CastNode"/>.
    /// </summary>
    /// <typeparam name="T">The data type of the elements within the property. This type must be of a supported type.</typeparam>
    public class CastArrayProperty<T> : CastProperty where T : unmanaged
    {
        /// <inheritdoc/>
        public override CastPropertyIdentifier Identifier { get; protected set; }

        /// <summary>
        /// Gets or Sets the values assigned to this property.
        /// </summary>
        public List<T> Values { get; set; }

        /// <summary>
        /// Gets the total number of values in this property.
        /// </summary>
        public override int ValueCount => Values.Count;

        /// <summary>
        /// Gets the total raw size of the data within the property
        /// </summary>
        internal override int DataSize => ValueCount * Unsafe.SizeOf<T>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CastArrayProperty{T}"/> class.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        public CastArrayProperty()
        {
            Identifier = Identifiers[typeof(T)];
            Values = new();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastArrayProperty{T}"/> class.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="initialCapacity">Number of values to allocate in the list.</param>
        public CastArrayProperty(int initialCapacity)
        {
            Identifier = Identifiers[typeof(T)];
            Values = new(initialCapacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastArrayProperty{T}"/> class.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="values">Values to assign to this property.</param>
        public CastArrayProperty(List<T> values)
        {
            Identifier = Identifiers[typeof(T)];
            Values = new(values);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastArrayProperty{T}"/> class.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="values">Values to assign to this property.</param>
        public CastArrayProperty(IEnumerable<T> values)
        {
            Identifier = Identifiers[typeof(T)];
            Values = new(values);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastArrayProperty{T}"/> class.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="value">Value to assign to this property.</param>
        public CastArrayProperty(T value)
        {
            Identifier = Identifiers[typeof(T)];
            Values = new()
            {
                value
            };
        }

        /// <summary>
        /// Gets the first element in the array. If the array if empty, the null is returned.
        /// </summary>
        /// <returns>Null if the array is empty, otherwise the value at index 0.</returns>
        public T? GetFirstValue()
        {
            if (Values.Count == 0)
                return null;
            else
                return Values[0];
        }

        /// <summary>
        /// Gets the first element in the array. If the array if empty, the default value is returned.
        /// </summary>
        /// <param name="defaultVal">Default value to return if the array is empty.</param>
        /// <returns>Default value if the array is empty, otherwise the value at index 0.</returns>
        public T GetFirstValue(T defaultVal)
        {
            if (Values.Count == 0)
                return defaultVal;
            else
                return Values[0];
        }

        /// <summary>
        /// Sets the first value of the array to the provided value. If the array is not empty, it overwirtes the value currently stored.
        /// </summary>
        /// <param name="value">Value to set at the start of the array</param>
        public void SetFirstValue(T value)
        {
            if (Values.Count == 0)
                Values.Add(value);
            else
                Values[0] = value;
        }

        /// <summary>
        /// Adds a value to the end of the <see cref="Values"/>.
        /// </summary>
        /// <param name="value">The value to add.</param>
        public void Add(T value) => Values.Add(value);

        /// <summary>
        /// A map of supported types to identifiers.
        /// </summary>
        internal static readonly Dictionary<Type, CastPropertyIdentifier> Identifiers = new()
        {
            { typeof(byte), CastPropertyIdentifier.Byte },
            { typeof(ushort), CastPropertyIdentifier.Short },
            { typeof(uint), CastPropertyIdentifier.Integer32 },
            { typeof(ulong), CastPropertyIdentifier.Integer64 },
            { typeof(float), CastPropertyIdentifier.Float },
            { typeof(double), CastPropertyIdentifier.Double },
            { typeof(Vector2), CastPropertyIdentifier.Vector2 },
            { typeof(Vector3), CastPropertyIdentifier.Vector3 },
            { typeof(Vector4), CastPropertyIdentifier.Vector4 },
        };
    }
}
