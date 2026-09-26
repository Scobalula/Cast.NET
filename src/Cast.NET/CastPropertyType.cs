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
namespace CastNet;

/// <summary>
/// Specifies the data type stored within a <see cref="CastProperty"/>.
/// </summary>
public enum CastPropertyType : ushort
{
    /// <summary>
    /// 8-bit unsigned integer.
    /// </summary>
    Byte = 'b',

    /// <summary>
    /// 16-bit unsigned integer.
    /// </summary>
    Short = 'h',

    /// <summary>
    /// 32-bit unsigned integer.
    /// </summary>
    Integer32 = 'i',

    /// <summary>
    /// 64-bit unsigned integer.
    /// </summary>
    Integer64 = 'l',

    /// <summary>
    /// Single precision floating point value.
    /// </summary>
    Float = 'f',

    /// <summary>
    /// Double precision floating point value.
    /// </summary>
    Double = 'd',

    /// <summary>
    /// Null terminated UTF-8 string.
    /// </summary>
    String = 's',

    /// <summary>
    /// Single precision vector with 2 components.
    /// </summary>
    Vector2 = 0x7632,

    /// <summary>
    /// Single precision vector with 3 components.
    /// </summary>
    Vector3 = 0x7633,

    /// <summary>
    /// Single precision vector with 4 components.
    /// </summary>
    Vector4 = 0x7634,
}
