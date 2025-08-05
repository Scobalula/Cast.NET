// ------------------------------------------------------------------------
// Cast.NET - A .NET Library for reading and writing Cast files.
// Copyright(c) 2025 Philip/Scobalula
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
using System.Runtime.InteropServices;

namespace Cast.NET.Nodes
{
    /// <summary>
    /// A class to hold a <see cref="CastNode"/> that contains a Curve.
    /// </summary>
    public class CurveNode : CastNode
    {
        /// <summary>
        /// Gets or Sets the name of the node this curve targets.
        /// </summary>
        public string NodeName { get => GetStringValue("nn"); set => AddString("nn", value); }

        /// <summary>
        /// Gets or Sets the key this curve targets.
        /// </summary>
        public string KeyPropertyName { get => GetStringValue("kp"); set => AddString("kp", value); }

        /// <summary>
        /// Gets or Sets the raw key frame buffer stored within this curve.
        /// </summary>
        public CastProperty KeyFrameBuffer { get => GetProperty("kb"); set => Properties["kb"] = value; }

        /// <summary>
        /// Gets or Sets the raw key value buffer stored within this curve.
        /// </summary>
        public CastProperty KeyValueBuffer { get => GetProperty("kv"); set => Properties["kv"] = value; }

        /// <summary>
        /// Gets or Sets the curve's mode.
        /// </summary>
        public string Mode { get => GetStringValue("m", "relative"); set => AddString("m", value); }

        /// <summary>
        /// Gets or Sets the additive blend weight.
        /// </summary>
        public float AdditiveBlendWeight { get => GetFirstValue("ab", 0.0f); set => AddValue("ab", value); }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurveNode"/> class.
        /// </summary>
        public CurveNode() : base(CastNodeIdentifier.Curve) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurveNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        public CurveNode(CastNodeIdentifier identifier) : base(identifier) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurveNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        public CurveNode(CastNodeIdentifier identifier, ulong hash) : base(identifier, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurveNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        public CurveNode(ulong hash) : base(CastNodeIdentifier.Curve, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurveNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public CurveNode(ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(CastNodeIdentifier.Curve, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public CurveNode(CastNodeIdentifier identifier, ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(identifier, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurveNode"/> class.
        /// </summary>
        /// <param name="source">Node to copy from. A shallow copy is performed and references to the source are stored.</param>
        public CurveNode(CastNode source) : base(source) { }

        /// <summary>
        /// Enumerates through key frames.
        /// </summary>
        /// <returns>An enumerable collection of the key frames.</returns>
        /// <exception cref="NotSupportedException">Thrown if the underlying <see cref="CastProperty"/> type is not supported.</exception>
        public IEnumerable<double> EnumerateKeyFrames()
        {
            return KeyFrameBuffer switch
            {
                CastArrayProperty<byte> { Values: var v }   => v.Select(x => (double)x),
                CastArrayProperty<ushort> { Values: var v } => v.Select(x => (double)x),
                CastArrayProperty<uint> { Values: var v }   => v.Select(x => (double)x),
                _ => throw new NotSupportedException($"Unimplemented buffer type {KeyFrameBuffer.GetType()}")
            };
        }

        /// <summary>
        /// Enumerates through frames and their corresponding values.
        /// </summary>
        /// <returns>An enumerable collection of frames and their corresponding values.</returns>
        /// <typeparam name="T">The type to request.</typeparam>
        /// <exception cref="DataMisalignedException">Thrown if underlying buffers have different counts.</exception>
        /// <exception cref="NotSupportedException">Thrown if the underlying <see cref="CastProperty"/> type is not supported.</exception>
        public IEnumerable<(double, T)> EnumerateKeys<T>() where T : unmanaged
        {
            var keyFrameBuffer = KeyFrameBuffer;
            var keyValueBuffer = KeyValueBuffer;

            if (KeyValueBuffer.ValueCount != KeyFrameBuffer.ValueCount)
                throw new DataMisalignedException($"KeyValueBuffer and KeyFrameBuffer for node: {NodeName} have different lengths.");

            if (keyValueBuffer is not CastArrayProperty<T> keyValueBufferAsType)
                throw new NotSupportedException($"Requested key value buffer of type: {typeof(T)} but underlying type is {keyValueBuffer.GetType()} for node: {NodeName}");

            return KeyFrameBuffer switch
            {
                CastArrayProperty<byte> { Values: var v } => v.Select(x => (double)x).Zip(keyValueBufferAsType.Values),
                CastArrayProperty<ushort> { Values: var v } => v.Select(x => (double)x).Zip(keyValueBufferAsType.Values),
                CastArrayProperty<uint> { Values: var v } => v.Select(x => (double)x).Zip(keyValueBufferAsType.Values),
                _ => throw new NotSupportedException($"Unimplemented buffer type {KeyFrameBuffer.GetType()}")
            };
        }

        /// <summary>
        /// Enumerates through key values.
        /// </summary>
        /// <returns>An enumerable collection of key values.</returns>
        /// <typeparam name="T">The type to request.</typeparam>
        /// <returns>An enumerable collection of key values.</returns>
        /// <exception cref="NotSupportedException">Thrown if the underlying <see cref="CastProperty"/> type is not supported.</exception>
        public IEnumerable<T> EnumerateKeyValues<T>() where T : unmanaged
        {
            if (KeyValueBuffer is CastArrayProperty<T> array)
            {
                return array.Values;
            }
            else
            {
                throw new NotSupportedException($"Requested key value buffer of type: {typeof(T)} but underlying type is {KeyValueBuffer.GetType()} for node: {NodeName}");
            }
        }

        /// <inheritdoc/>
        public override string ToString() => NodeName;
    }
}
