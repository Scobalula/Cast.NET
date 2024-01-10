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

namespace Cast.NET
{
    /// <summary>
    /// Specifies the supported <see cref="CastProperty"/> identifiers.
    /// </summary>
    public enum CastPropertyIdentifier : ushort
    {
        /// <summary>
        /// 8-Bit Integer (uint8_t/byte)
        /// </summary>
        Byte = 'b',

        /// <summary>
        /// 16-Bit Integer (uint16_t/ushort)
        /// </summary>
        Short = 'h',

        /// <summary>
        /// 32-Bit Integer (uint32_t/uint)
        /// </summary>
        Integer32 = 'i',

        /// <summary>
        /// 64-Bit Integer (uint64_t/ulong)
        /// </summary>
        Integer64 = 'l',

        /// <summary>
        /// Single Precision Value (float)
        /// </summary>
        Float = 'f',

        /// <summary>
        /// Double Precision Value (double)
        /// </summary>
        Double = 'd',

        /// <summary>
        /// Null terminated UTF-8 string
        /// </summary>
        String = 's',

        /// <summary>
        /// Float precision vector XYZ
        /// </summary>
        Vector2 = 0x7632,

        /// <summary>
        /// Float precision vector XYZ
        /// </summary>
        Vector3 = 0x7633,

        /// <summary>
        /// Float precision vector XYZW
        /// </summary>
        Vector4 = 0x7634
    }
}
