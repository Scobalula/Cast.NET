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

namespace Cast.NET
{
    /// <summary>
    /// A class to hold a <see cref="CastProperty"/> that holds various data types.
    /// </summary>
    [DebuggerDisplay("Identifier = {Identifier}")]
    public abstract class CastProperty
    {
        /// <summary>
        /// Gets the property identifier that describes the data held in this property.
        /// </summary>
        public abstract CastPropertyIdentifier Identifier { get; protected set; }

        /// <summary>
        /// Gets the total number of values in this property.
        /// </summary>
        public abstract int ValueCount { get; }

        /// <summary>
        /// Gets the total raw size of the data in this property.
        /// </summary>
        internal abstract int DataSize { get; }
    }
}
