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
using System.Text;

namespace Cast.NET
{
    /// <summary>
    /// A class to hold a <see cref="CastProperty"/> value that contains a <see cref="string"/>.
    /// </summary>
    public class CastStringProperty : CastProperty
    {
        /// <inheritdoc/>
        public override CastPropertyIdentifier Identifier { get; protected set; }

        /// <summary>
        /// Gets or Sets the value assigned to this property.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets the total number of values in this property.
        /// </summary>
        public override int ValueCount => 1;

        /// <summary>
        /// Gets the total raw size of the data within the property
        /// </summary>
        internal override int DataSize => Value.Length + 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="CastStringProperty"/> class.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        public CastStringProperty()
        {
            Identifier = CastPropertyIdentifier.String;
            Value = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastStringProperty"/> class with the provided value.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="value">Value to assign to this property.</param>
        public CastStringProperty(string value)
        {
            Identifier = CastPropertyIdentifier.String;
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastStringProperty"/> class with the provided value.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="value">Value to assign to this property.</param>
        public CastStringProperty(StringBuilder value)
        {
            Identifier = CastPropertyIdentifier.String;
            Value = value.ToString();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Value;
        }
    }
}
