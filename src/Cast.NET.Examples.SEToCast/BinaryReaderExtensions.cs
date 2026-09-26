using System.Runtime.InteropServices;
using System.Text;

namespace CastNet.Examples.SEToCast;

/// <summary>
/// Provides methods for reading SE format data from a <see cref="BinaryReader"/>.
/// </summary>
internal static class BinaryReaderExtensions
{
    /// <summary>
    /// Reads a null terminated UTF-8 string.
    /// </summary>
    /// <param name="reader">The reader to read from.</param>
    /// <returns>The string read.</returns>
    public static string ReadNullTerminatedString(this BinaryReader reader)
    {
        var bytes = new List<byte>();
        byte value;

        while ((value = reader.ReadByte()) != 0)
            bytes.Add(value);

        return Encoding.UTF8.GetString(CollectionsMarshal.AsSpan(bytes));
    }

    /// <summary>
    /// Reads the given number of values of the given type directly into a new <see cref="CastArrayProperty"/>.
    /// </summary>
    /// <param name="reader">The reader to read from.</param>
    /// <param name="type">The type of the values.</param>
    /// <param name="count">The number of values.</param>
    /// <returns>The property holding the values read.</returns>
    public static CastArrayProperty ReadArray(this BinaryReader reader, CastPropertyType type, int count)
    {
        var property = new CastArrayProperty(type, count);

        property.Resize(count);
        reader.BaseStream.ReadExactly(property.AsBytes());

        return property;
    }
}
