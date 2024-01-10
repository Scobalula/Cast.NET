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
using System.Runtime.InteropServices;

namespace Cast.NET
{
    public static class CastHelpers
    {
        /// <summary>
        /// Reads an array from the provided <see cref="BinaryReader"/> into a <see cref="List{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type we are reading.</typeparam>
        /// <param name="reader">The <see cref="BinaryReader"/> we are reading from.</param>
        /// <param name="count">The number of elements in the array</param>
        /// <returns>The resulting list.</returns>
        /// <exception cref="EndOfStreamException">Thrown when attempt was made to read from the end of the stream.</exception>
        public static List<T> ReadList<T>(BinaryReader reader, int count) where T : unmanaged
        {
            var sizeOfValue = Unsafe.SizeOf<T>();
            var sizeOfBuffer = sizeOfValue * count;
            Span<byte> buffer = sizeOfBuffer > 1024 ? new byte[sizeOfBuffer] : stackalloc byte[sizeOfBuffer];
            if (reader.Read(buffer) != buffer.Length)
                throw new EndOfStreamException();
            return new(MemoryMarshal.Cast<byte, T>(buffer).ToArray());
        }

        /// <summary>
        /// Creates a <see cref="Quaternion"/> from the provided <see cref="Vector4"/>.
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion CreateQuaternionFromVector4(Vector4 vec) => new(vec.X, vec.Y, vec.Z, vec.W);
        public static Vector4 CreateVector4FromQuaternion(Quaternion vec) => new(vec.X, vec.Y, vec.Z, vec.W);
    }
}
