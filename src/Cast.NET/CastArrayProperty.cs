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
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CastNet;

/// <summary>
/// A property holding an array of numbers or vectors.
/// </summary>
/// <remarks>
/// Signed integers map to the unsigned type of the same width and <see cref="Quaternion"/> maps to <see cref="CastPropertyType.Vector4"/>.
/// </remarks>
public sealed class CastArrayProperty : CastProperty
{
    private byte[] _buffer;

    private int _count;

    /// <inheritdoc/>
    public override CastPropertyType Type { get; }

    /// <inheritdoc/>
    public override int Count => _count;

    /// <summary>
    /// Gets the size in bytes of a single value.
    /// </summary>
    public int Stride { get; }

    /// <summary>
    /// Initializes a new, empty instance of the <see cref="CastArrayProperty"/> class.
    /// </summary>
    /// <param name="type">The data type stored within the property.</param>
    public CastArrayProperty(CastPropertyType type) : this(type, 0)
    {
    }

    /// <summary>
    /// Initializes a new, empty instance of the <see cref="CastArrayProperty"/> class with the given capacity.
    /// </summary>
    /// <param name="type">The data type stored within the property.</param>
    /// <param name="capacity">The number of values to allocate space for.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="type"/> is <see cref="CastPropertyType.String"/> or unknown.</exception>
    public CastArrayProperty(CastPropertyType type, int capacity)
    {
        Type = type;
        Stride = type switch
        {
            CastPropertyType.Byte => 1,
            CastPropertyType.Short => 2,
            CastPropertyType.Integer32 => 4,
            CastPropertyType.Integer64 => 8,
            CastPropertyType.Float => 4,
            CastPropertyType.Double => 8,
            CastPropertyType.Vector2 => 8,
            CastPropertyType.Vector3 => 12,
            CastPropertyType.Vector4 => 16,
            _ => throw new ArgumentException($"{type} is not an array property type.", nameof(type)),
        };
        _buffer = new byte[checked(capacity * Stride)];
    }

    /// <summary>
    /// Creates a property holding a single value.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="value">The value to store.</param>
    /// <returns>The created property.</returns>
    public static CastArrayProperty Create<T>(T value) where T : unmanaged
    {
        var property = new CastArrayProperty(TypeOf<T>(), 1);
        property.Add(value);
        return property;
    }

    /// <summary>
    /// Creates a property holding a copy of the values.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="values">The values to store.</param>
    /// <returns>The created property.</returns>
    public static CastArrayProperty Create<T>(ReadOnlySpan<T> values) where T : unmanaged
    {
        var property = new CastArrayProperty(TypeOf<T>(), values.Length);
        property.AddRange(values);
        return property;
    }

    /// <summary>
    /// Creates a property holding the values in the smallest integer type that fits them.
    /// </summary>
    /// <typeparam name="T">The integer type of the values.</typeparam>
    /// <param name="values">The values to store.</param>
    /// <returns>The created property.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if a value is negative.</exception>
    public static CastArrayProperty CreateIndices<T>(ReadOnlySpan<T> values) where T : unmanaged, IBinaryInteger<T>
    {
        var maximum = T.Zero;

        foreach (var value in values)
        {
            if (T.IsNegative(value))
                throw new ArgumentOutOfRangeException(nameof(values), value, "Indices cannot be negative.");

            maximum = T.Max(maximum, value);
        }

        var largest = ulong.CreateChecked(maximum);
        var type = largest <= byte.MaxValue ? CastPropertyType.Byte : largest <= ushort.MaxValue ? CastPropertyType.Short : largest <= uint.MaxValue ? CastPropertyType.Integer32 : CastPropertyType.Integer64;
        var property = new CastArrayProperty(type, values.Length);

        property.AddScalarRange(values);
        return property;
    }

    /// <summary>
    /// Gets the raw bytes of the values.
    /// </summary>
    /// <returns>A span over the underlying storage.</returns>
    public Span<byte> AsBytes() => _buffer.AsSpan(0, _count * Stride);

    /// <summary>
    /// Gets the values without copying.
    /// </summary>
    /// <typeparam name="T">The value type, matching <see cref="Type"/>.</typeparam>
    /// <returns>A span over the underlying storage.</returns>
    /// <exception cref="InvalidCastException">Thrown if <typeparamref name="T"/> does not map to <see cref="Type"/>.</exception>
    public Span<T> AsSpan<T>() where T : unmanaged
    {
        if (TypeOf<T>() != Type)
            throw new InvalidCastException($"Property stores {Type} values, which cannot be accessed as {typeof(T).Name}.");

        return MemoryMarshal.Cast<byte, T>(AsBytes());
    }

    /// <summary>
    /// Gets the value at the given index.
    /// </summary>
    /// <typeparam name="T">The value type, matching <see cref="Type"/>.</typeparam>
    /// <param name="index">The index of the value.</param>
    /// <returns>The value at the given index.</returns>
    /// <exception cref="InvalidCastException">Thrown if <typeparamref name="T"/> does not map to <see cref="Type"/>.</exception>
    public T Get<T>(int index) where T : unmanaged => AsSpan<T>()[index];

    /// <summary>
    /// Gets the value at the given index, converted to <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The numeric type to convert to.</typeparam>
    /// <param name="index">The index of the value.</param>
    /// <returns>The converted value.</returns>
    /// <exception cref="InvalidCastException">Thrown if the property stores vectors.</exception>
    /// <exception cref="OverflowException">Thrown if the value cannot be represented by <typeparamref name="T"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T GetScalar<T>(int index) where T : unmanaged, INumberBase<T>
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual((uint)index, (uint)_count, nameof(index));

        ref var value = ref _buffer[index * Stride];

        return Type switch
        {
            CastPropertyType.Byte => T.CreateChecked(value),
            CastPropertyType.Short => T.CreateChecked(Unsafe.ReadUnaligned<ushort>(ref value)),
            CastPropertyType.Integer32 => T.CreateChecked(Unsafe.ReadUnaligned<uint>(ref value)),
            CastPropertyType.Integer64 => T.CreateChecked(Unsafe.ReadUnaligned<ulong>(ref value)),
            CastPropertyType.Float => T.CreateChecked(Unsafe.ReadUnaligned<float>(ref value)),
            CastPropertyType.Double => T.CreateChecked(Unsafe.ReadUnaligned<double>(ref value)),
            _ => throw new InvalidCastException($"Property stores {Type} values, which are not scalars."),
        };
    }

    /// <summary>
    /// Copies the values into the destination, converted to <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The numeric type to convert to.</typeparam>
    /// <param name="destination">The span to copy into.</param>
    /// <exception cref="InvalidCastException">Thrown if the property stores vectors.</exception>
    /// <exception cref="OverflowException">Thrown if a value cannot be represented by <typeparamref name="T"/>.</exception>
    public void CopyTo<T>(Span<T> destination) where T : unmanaged, INumberBase<T>
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(destination.Length, _count, nameof(destination));

        switch (Type)
        {
            case CastPropertyType.Byte: Convert<byte, T>(AsSpan<byte>(), destination); break;
            case CastPropertyType.Short: Convert<ushort, T>(AsSpan<ushort>(), destination); break;
            case CastPropertyType.Integer32: Convert<uint, T>(AsSpan<uint>(), destination); break;
            case CastPropertyType.Integer64: Convert<ulong, T>(AsSpan<ulong>(), destination); break;
            case CastPropertyType.Float: Convert<float, T>(AsSpan<float>(), destination); break;
            case CastPropertyType.Double: Convert<double, T>(AsSpan<double>(), destination); break;
            default: throw new InvalidCastException($"Property stores {Type} values, which are not scalars.");
        }
    }

    /// <summary>
    /// Copies the values into a new array, converted to <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The numeric type to convert to.</typeparam>
    /// <returns>The converted values.</returns>
    /// <exception cref="InvalidCastException">Thrown if the property stores vectors.</exception>
    /// <exception cref="OverflowException">Thrown if a value cannot be represented by <typeparamref name="T"/>.</exception>
    public T[] ToArray<T>() where T : unmanaged, INumberBase<T>
    {
        var result = new T[_count];
        CopyTo<T>(result);
        return result;
    }

    /// <summary>
    /// Adds a value.
    /// </summary>
    /// <typeparam name="T">The value type, matching <see cref="Type"/>.</typeparam>
    /// <param name="value">The value to add.</param>
    /// <exception cref="InvalidCastException">Thrown if <typeparamref name="T"/> does not map to <see cref="Type"/>.</exception>
    public void Add<T>(T value) where T : unmanaged => AddRange(new ReadOnlySpan<T>(in value));

    /// <summary>
    /// Adds the values.
    /// </summary>
    /// <typeparam name="T">The value type, matching <see cref="Type"/>.</typeparam>
    /// <param name="values">The values to add.</param>
    /// <exception cref="InvalidCastException">Thrown if <typeparamref name="T"/> does not map to <see cref="Type"/>.</exception>
    public void AddRange<T>(ReadOnlySpan<T> values) where T : unmanaged
    {
        var start = AsSpan<T>().Length;

        Resize(start + values.Length);
        values.CopyTo(AsSpan<T>()[start..]);
    }

    /// <summary>
    /// Adds a value, converted to the stored type.
    /// </summary>
    /// <typeparam name="T">The numeric type of the value.</typeparam>
    /// <param name="value">The value to add.</param>
    /// <exception cref="InvalidCastException">Thrown if the property stores vectors.</exception>
    /// <exception cref="OverflowException">Thrown if the value cannot be represented by the stored type.</exception>
    public void AddScalar<T>(T value) where T : unmanaged, INumberBase<T> => AddScalarRange(new ReadOnlySpan<T>(in value));

    /// <summary>
    /// Adds the values, converted to the stored type. Nothing is added if a value does not fit.
    /// </summary>
    /// <typeparam name="T">The numeric type of the values.</typeparam>
    /// <param name="values">The values to add.</param>
    /// <exception cref="InvalidCastException">Thrown if the property stores vectors.</exception>
    /// <exception cref="OverflowException">Thrown if a value cannot be represented by the stored type.</exception>
    public void AddScalarRange<T>(ReadOnlySpan<T> values) where T : unmanaged, INumberBase<T>
    {
        if (Type is CastPropertyType.Vector2 or CastPropertyType.Vector3 or CastPropertyType.Vector4)
            throw new InvalidCastException($"Property stores {Type} values, which are not scalars.");

        var start = _count;
        Resize(start + values.Length);

        try
        {
            switch (Type)
            {
                case CastPropertyType.Byte: Convert(values, AsSpan<byte>()[start..]); break;
                case CastPropertyType.Short: Convert(values, AsSpan<ushort>()[start..]); break;
                case CastPropertyType.Integer32: Convert(values, AsSpan<uint>()[start..]); break;
                case CastPropertyType.Integer64: Convert(values, AsSpan<ulong>()[start..]); break;
                case CastPropertyType.Float: Convert(values, AsSpan<float>()[start..]); break;
                default: Convert(values, AsSpan<double>()[start..]); break;
            }
        }
        catch (OverflowException)
        {
            Resize(start);
            throw;
        }
    }

    /// <summary>
    /// Sets the number of values. New values are zero.
    /// </summary>
    /// <param name="count">The new number of values.</param>
    public void Resize(int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);

        var size = checked(count * Stride);

        if (size > _buffer.Length)
            Array.Resize(ref _buffer, Math.Max(size, _buffer.Length * 2));
        else if (count < _count)
            _buffer.AsSpan(size, (_count - count) * Stride).Clear();

        _count = count;
    }

    /// <inheritdoc/>
    public override string ToString() => $"{Type}[{_count}]";

    private static void Convert<TFrom, TTo>(ReadOnlySpan<TFrom> source, Span<TTo> destination) where TFrom : unmanaged, INumberBase<TFrom> where TTo : unmanaged, INumberBase<TTo>
    {
        if (typeof(TFrom) == typeof(TTo))
        {
            MemoryMarshal.Cast<TFrom, TTo>(source).CopyTo(destination);
            return;
        }

        for (var i = 0; i < source.Length; i++)
            destination[i] = TTo.CreateChecked(source[i]);
    }

    private static CastPropertyType TypeOf<T>() where T : unmanaged
    {
        if (typeof(T) == typeof(byte) || typeof(T) == typeof(sbyte))
            return CastPropertyType.Byte;
        if (typeof(T) == typeof(ushort) || typeof(T) == typeof(short))
            return CastPropertyType.Short;
        if (typeof(T) == typeof(uint) || typeof(T) == typeof(int))
            return CastPropertyType.Integer32;
        if (typeof(T) == typeof(ulong) || typeof(T) == typeof(long))
            return CastPropertyType.Integer64;
        if (typeof(T) == typeof(float))
            return CastPropertyType.Float;
        if (typeof(T) == typeof(double))
            return CastPropertyType.Double;
        if (typeof(T) == typeof(Vector2))
            return CastPropertyType.Vector2;
        if (typeof(T) == typeof(Vector3))
            return CastPropertyType.Vector3;
        if (typeof(T) == typeof(Vector4) || typeof(T) == typeof(Quaternion))
            return CastPropertyType.Vector4;

        throw new NotSupportedException($"{typeof(T).Name} is not a supported cast property type.");
    }
}
