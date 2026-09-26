using System.Numerics;
using System.Runtime.InteropServices;
using CastNet.Nodes;

namespace CastNet.Examples.SEToCast;

/// <summary>
/// Converts SEAnim files to cast.
/// </summary>
internal static class SEAnimConverter
{
    private static readonly string[] Modes = ["absolute", "additive", "relative"];

    /// <summary>
    /// Converts the SEAnim file at the given path.
    /// </summary>
    /// <param name="path">The path of the SEAnim file.</param>
    /// <returns>The root node of the converted cast.</returns>
    /// <exception cref="InvalidDataException">Thrown if the file is not a supported SEAnim file.</exception>
    public static RootNode Convert(string path)
    {
        using var reader = new BinaryReader(File.OpenRead(path));

        if (!reader.ReadBytes(6).AsSpan().SequenceEqual("SEAnim"u8) || reader.ReadUInt16() != 1)
            throw new InvalidDataException($"{path} is not a version 1 SEAnim file.");

        var headerSize = reader.ReadUInt16();
        var animationMode = Modes[Math.Min(reader.ReadByte(), (byte)2)];
        var flags = reader.ReadByte();
        var dataFlags = reader.ReadByte();
        var highPrecision = (reader.ReadByte() & 1) != 0;

        reader.ReadUInt16();

        var framerate = reader.ReadSingle();
        var frameCount = reader.ReadInt32();
        var boneCount = reader.ReadInt32();
        var modifierCount = reader.ReadByte();

        reader.ReadBytes(3);

        var noteCount = reader.ReadInt32();

        reader.BaseStream.Position = headerSize + 8;

        var root = new RootNode();
        var animation = root.AddNode(new AnimationNode { Name = Path.GetFileNameWithoutExtension(path), Framerate = framerate, Looping = (flags & 1) != 0 });
        var boneNames = new string[boneCount];
        var boneModes = new string[boneCount];

        for (var i = 0; i < boneCount; i++)
        {
            boneNames[i] = reader.ReadNullTerminatedString();
            boneModes[i] = animationMode;
        }

        for (var i = 0; i < modifierCount; i++)
        {
            var bone = boneCount <= byte.MaxValue ? reader.ReadByte() : reader.ReadUInt16();
            boneModes[bone] = Modes[Math.Min(reader.ReadByte(), (byte)2)];
        }

        for (var i = 0; i < boneCount; i++)
        {
            reader.ReadByte();

            if ((dataFlags & 1) != 0)
                ReadCurves(reader, animation, boneNames[i], boneModes[i], frameCount, highPrecision, ["tx", "ty", "tz"]);

            if ((dataFlags & 2) != 0)
                ReadCurves(reader, animation, boneNames[i], boneModes[i] == "additive" ? "additive" : "absolute", frameCount, highPrecision, ["rq"]);

            if ((dataFlags & 4) != 0)
                ReadCurves(reader, animation, boneNames[i], boneModes[i], frameCount, highPrecision, ["sx", "sy", "sz"]);
        }

        var notifications = new Dictionary<string, List<int>>();

        for (var i = 0; i < noteCount; i++)
        {
            var frame = ReadFrame(reader, frameCount);
            var name = reader.ReadNullTerminatedString();

            if (!notifications.TryGetValue(name, out var frames))
                notifications[name] = frames = [];

            frames.Add(frame);
        }

        foreach (var (name, frames) in notifications)
            animation.AddNode(new NotificationTrackNode { Name = name, KeyFrames = CastArrayProperty.CreateIndices<int>(CollectionsMarshal.AsSpan(frames)) });

        return root;
    }

    private static void ReadCurves(BinaryReader reader, AnimationNode animation, string nodeName, string mode, int frameCount, bool highPrecision, string[] properties)
    {
        var keyCount = ReadFrame(reader, frameCount);
        var componentCount = properties.Length == 1 ? 4 : 3;
        var frames = new int[keyCount];
        var values = new float[keyCount * componentCount];

        for (var key = 0; key < keyCount; key++)
        {
            frames[key] = ReadFrame(reader, frameCount);

            for (var component = 0; component < componentCount; component++)
                values[key * componentCount + component] = highPrecision ? (float)reader.ReadDouble() : reader.ReadSingle();
        }

        if (keyCount == 0)
            return;

        if (componentCount == 4)
        {
            animation.AddNode(new CurveNode { NodeName = nodeName, KeyPropertyName = properties[0], Mode = mode, KeyFrames = CastArrayProperty.CreateIndices<int>(frames), KeyValues = CastArrayProperty.Create<Vector4>(MemoryMarshal.Cast<float, Vector4>(values)) });
            return;
        }

        for (var component = 0; component < componentCount; component++)
        {
            var keyValues = new CastArrayProperty(CastPropertyType.Float, keyCount);

            for (var key = 0; key < keyCount; key++)
                keyValues.Add(values[key * componentCount + component]);

            animation.AddNode(new CurveNode { NodeName = nodeName, KeyPropertyName = properties[component], Mode = mode, KeyFrames = CastArrayProperty.CreateIndices<int>(frames), KeyValues = keyValues });
        }
    }

    private static int ReadFrame(BinaryReader reader, int frameCount) => frameCount <= byte.MaxValue ? reader.ReadByte() : frameCount <= ushort.MaxValue ? reader.ReadUInt16() : reader.ReadInt32();
}
