using System.Numerics;
using System.Runtime.InteropServices;
using CastNet.Nodes;

namespace CastNet.Examples.SEToCast;

/// <summary>
/// Converts SEModel files to cast.
/// </summary>
internal static class SEModelConverter
{
    /// <summary>
    /// Converts the SEModel file at the given path.
    /// </summary>
    /// <param name="path">The path of the SEModel file.</param>
    /// <returns>The root node of the converted cast.</returns>
    /// <exception cref="InvalidDataException">Thrown if the file is not a supported SEModel file.</exception>
    public static RootNode Convert(string path)
    {
        using var reader = new BinaryReader(File.OpenRead(path));

        if (!reader.ReadBytes(7).AsSpan().SequenceEqual("SEModel"u8) || reader.ReadUInt16() != 1)
            throw new InvalidDataException($"{path} is not a version 1 SEModel file.");

        var headerSize = reader.ReadUInt16();

        reader.ReadByte();

        var bonePresence = reader.ReadByte();
        var meshPresence = reader.ReadByte();
        var boneCount = reader.ReadInt32();
        var meshCount = reader.ReadInt32();
        var materialCount = reader.ReadInt32();

        reader.BaseStream.Position = headerSize + 9;

        var root = new RootNode();
        var model = root.AddNode(new ModelNode { Name = Path.GetFileNameWithoutExtension(path) });
        var skeleton = boneCount > 0 ? model.AddNode<SkeletonNode>() : null;
        var boneNames = new string[boneCount];

        for (var i = 0; i < boneCount; i++)
            boneNames[i] = reader.ReadNullTerminatedString();

        for (var i = 0; i < boneCount; i++)
        {
            reader.ReadByte();

            var bone = skeleton!.AddNode(new BoneNode { Name = boneNames[i], ParentIndex = reader.ReadInt32() });

            if ((bonePresence & 1) != 0)
            {
                bone.WorldPosition = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                bone.WorldRotation = new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            }

            if ((bonePresence & 2) != 0)
            {
                bone.LocalPosition = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                bone.LocalRotation = new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            }

            if ((bonePresence & 4) != 0)
                bone.Scale = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        var meshMaterials = new List<(MeshNode Mesh, int Material)>(meshCount);

        for (var i = 0; i < meshCount; i++)
        {
            reader.ReadByte();

            var layerCount = reader.ReadByte();
            var influences = reader.ReadByte();
            var vertexCount = reader.ReadInt32();
            var faceCount = reader.ReadInt32();
            var mesh = model.AddNode(new MeshNode { Positions = reader.ReadArray(CastPropertyType.Vector3, vertexCount) });

            if ((meshPresence & 1) != 0)
            {
                var interleavedLayers = new Vector2[vertexCount * layerCount];
                reader.BaseStream.ReadExactly(MemoryMarshal.AsBytes(interleavedLayers.AsSpan()));

                for (var layer = 0; layer < layerCount; layer++)
                {
                    var uvLayer = new CastArrayProperty(CastPropertyType.Vector2, vertexCount);

                    for (var vertex = 0; vertex < vertexCount; vertex++)
                        uvLayer.Add(interleavedLayers[vertex * layerCount + layer]);

                    mesh.SetUVLayer(layer, uvLayer);
                }
            }

            if ((meshPresence & 2) != 0)
                mesh.Normals = reader.ReadArray(CastPropertyType.Vector3, vertexCount);

            if ((meshPresence & 4) != 0)
                mesh.SetColorLayer(0, reader.ReadArray(CastPropertyType.Integer32, vertexCount));

            if ((meshPresence & 8) != 0 && influences > 0)
            {
                var weightBones = new int[vertexCount * influences];
                var weightValues = new float[vertexCount * influences];

                for (var weight = 0; weight < weightBones.Length; weight++)
                {
                    weightBones[weight] = boneCount <= byte.MaxValue ? reader.ReadByte() : boneCount <= ushort.MaxValue ? reader.ReadUInt16() : reader.ReadInt32();
                    weightValues[weight] = reader.ReadSingle();
                }

                mesh.MaximumWeightInfluence = influences;
                mesh.WeightBones = CastArrayProperty.CreateIndices<int>(weightBones);
                mesh.WeightValues = CastArrayProperty.Create<float>(weightValues);
            }

            mesh.Faces = reader.ReadArray(vertexCount <= byte.MaxValue ? CastPropertyType.Byte : vertexCount <= ushort.MaxValue ? CastPropertyType.Short : CastPropertyType.Integer32, faceCount * 3);

            var materialIndices = new int[layerCount];

            for (var layer = 0; layer < layerCount; layer++)
                materialIndices[layer] = reader.ReadInt32();

            meshMaterials.Add((mesh, layerCount > 0 ? materialIndices[0] : -1));
        }

        var materials = new MaterialNode[materialCount];

        for (var i = 0; i < materialCount; i++)
        {
            materials[i] = model.AddNode(new MaterialNode { Name = reader.ReadNullTerminatedString() });

            if (!reader.ReadBoolean())
                continue;

            foreach (var slot in (string[])["diffuse", "normal", "specular"])
            {
                var texture = reader.ReadNullTerminatedString();

                if (texture.Length > 0)
                    materials[i].SetSlot(slot, new FileNode { Path = texture });
            }
        }

        foreach (var (mesh, material) in meshMaterials)
        {
            if (material >= 0 && material < materials.Length)
                mesh.Material = materials[material];
        }

        return root;
    }
}
