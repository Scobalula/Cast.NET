using System.Numerics;
using Cast.NET.Nodes;
using SharpGLTF.Schema2;

namespace Cast.NET.Example.GltfToCast
{
    public class Program
    {
        public static void ConsumeGltfSkin(Skin? gltfSkin, SkeletonNode skeleton)
        {
            if (gltfSkin is not null)
            {
                for (int i = 0; i < gltfSkin.JointsCount; i++)
                {
                    var (joint, invBindMatrix) = gltfSkin.GetJoint(i);

                    Matrix4x4.Invert(invBindMatrix, out var inverted);

                    var bone = skeleton.AddNode<BoneNode>();
                    var rot = Quaternion.CreateFromRotationMatrix(inverted);
                    var pos = inverted.Translation;

                    bone.AddString("n", joint.Name);
                    bone.AddValue("wp", new Vector3(pos.X, pos.Y, pos.Z));
                    bone.AddValue("wr", new Vector4(rot.X, rot.Y, rot.Z, rot.W));
                }

                // Resolve parent indices after we can safely assume we've
                // resolved all bones at this point.
                for (int i = 0; i < gltfSkin.JointsCount; i++)
                {
                    var (joint, _) = gltfSkin.GetJoint(i);

                    foreach (var child in joint.VisualChildren)
                    {
                        skeleton.Bones[child.LogicalIndex].AddValue("p", (uint)joint.LogicalIndex);
                    }
                }

                // Finally calculate our local transforms.
                skeleton.CalculateLocalTransforms();
            }
        }

        public static List<ulong> ConsumeGltfMaterials(ModelRoot gltfModel, ModelNode model)
        {
            var materialHashes = new List<ulong>();

            foreach (var gltfMaterial in gltfModel.LogicalMaterials)
            {
                var mat = new MaterialNode(gltfMaterial.Name, "pbr");
                materialHashes.Add(mat.Hash);
                mat.Parent = model;
            }

            return materialHashes;
        }

        public static void ConsumeGltfWeights(Skin? gltfSkin, MeshPrimitive gltfPrimitive, MeshNode meshNode, int vertCount, int boneCount)
        {
            if (gltfSkin is not null)
            {
                // Run a check on how many set of joints/weights we have to compute how many
                // weights we are processing for this mesh. Gltf stores arrays similar to OpenGL/DX
                int boneSets = 0;
                while (gltfPrimitive.GetVertexAccessor($"JOINTS_{boneSets}") is not null)
                    boneSets++;

                if (boneSets > 0)
                {
                    var boneWeights = meshNode.AddArray<float>("wv", vertCount * boneSets);
                    meshNode.AddValue("mi", (byte)(boneSets * 4));

                    // We allocate bone indices depending on the type
                    CastProperty boneIndices;
                    if (boneCount <= 0xFF)
                        boneIndices = meshNode.AddArray<byte>("wb", vertCount * boneSets);
                    else if (boneCount <= 0xFFFF)
                        boneIndices = meshNode.AddArray<ushort>("wb", vertCount * boneSets);
                    else
                        boneIndices = meshNode.AddArray<uint>("wb", vertCount * boneSets);

                    for (int i = 0; i < boneSets; i++)
                    {
                        var jointsAccessor = gltfPrimitive.GetVertexAccessor($"JOINTS_{i}");
                        var weightsAccessor = gltfPrimitive.GetVertexAccessor($"WEIGHTS_{i}");

                        if (jointsAccessor is null || weightsAccessor is null)
                            throw new Exception("Null skinning info.");

                        var joints = jointsAccessor.AsVector4Array();
                        var weights = weightsAccessor.AsVector4Array();

                        for (int w = 0; w < joints.Count; w++)
                        {
                            var joint = joints[w];
                            var weight = weights[w];

                            if (boneIndices is CastArrayProperty<byte> bb)
                            {
                                bb.Values.Add((byte)joint.X);
                                bb.Values.Add((byte)joint.Y);
                                bb.Values.Add((byte)joint.Z);
                                bb.Values.Add((byte)joint.W);
                            }
                            else if (boneIndices is CastArrayProperty<ushort> bs)
                            {
                                bs.Values.Add((ushort)gltfSkin.GetJoint((int)joint.X).Joint.LogicalIndex);
                                bs.Values.Add((ushort)gltfSkin.GetJoint((int)joint.Y).Joint.LogicalIndex);
                                bs.Values.Add((ushort)gltfSkin.GetJoint((int)joint.Z).Joint.LogicalIndex);
                                bs.Values.Add((ushort)gltfSkin.GetJoint((int)joint.W).Joint.LogicalIndex);
                            }
                            else if (boneIndices is CastArrayProperty<uint> bi)
                            {
                                bi.Values.Add((uint)gltfSkin.GetJoint((int)joint.X).Joint.LogicalIndex);
                                bi.Values.Add((uint)gltfSkin.GetJoint((int)joint.Y).Joint.LogicalIndex);
                                bi.Values.Add((uint)gltfSkin.GetJoint((int)joint.Z).Joint.LogicalIndex);
                                bi.Values.Add((uint)gltfSkin.GetJoint((int)joint.W).Joint.LogicalIndex);
                            }

                            boneWeights.Add(weight.X);
                            boneWeights.Add(weight.Y);
                            boneWeights.Add(weight.Z);
                            boneWeights.Add(weight.W);
                        }
                    }
                }
            }
        }

        public static void ConsumeGltfFaces(MeshPrimitive gltfPrimitive, MeshNode meshNode, int vertCount)
        {
            var gltfFaceIndices = gltfPrimitive.IndexAccessor.AsIndicesArray();
            var faceCount = gltfFaceIndices.Count;

            if (vertCount <= 0xFF)
            {
                var faceIndices = meshNode.AddArray<byte>("f", faceCount);

                for (int i = 0; i < faceCount / 3; i++)
                {
                    var i0 = gltfFaceIndices[i * 3 + 0];
                    var i1 = gltfFaceIndices[i * 3 + 1];
                    var i2 = gltfFaceIndices[i * 3 + 2];

                    if (i0 != i1 && i1 != i2 && i2 != i0)
                    {
                        faceIndices.Add((byte)i0);
                        faceIndices.Add((byte)i1);
                        faceIndices.Add((byte)i2);
                    }
                }
            }
            else if (vertCount <= 0xFFFF)
            {
                var faceIndices = meshNode.AddArray<ushort>("f", faceCount);

                for (int i = 0; i < faceCount / 3; i++)
                {
                    var i0 = gltfFaceIndices[i * 3 + 0];
                    var i1 = gltfFaceIndices[i * 3 + 1];
                    var i2 = gltfFaceIndices[i * 3 + 2];

                    if (i0 != i1 && i1 != i2 && i2 != i0)
                    {
                        faceIndices.Add((ushort)i0);
                        faceIndices.Add((ushort)i1);
                        faceIndices.Add((ushort)i2);
                    }
                }
            }
            else
            {
                var faceIndices = meshNode.AddArray<uint>("f", faceCount);

                for (int i = 0; i < faceCount / 3; i++)
                {
                    var i0 = gltfFaceIndices[i * 3 + 0];
                    var i1 = gltfFaceIndices[i * 3 + 1];
                    var i2 = gltfFaceIndices[i * 3 + 2];

                    if (i0 != i1 && i1 != i2 && i2 != i0)
                    {
                        faceIndices.Add(i0);
                        faceIndices.Add(i1);
                        faceIndices.Add(i2);
                    }
                }
            }
        }

        public static void ConsumeGltfVertices(MeshPrimitive gltfPrimitive, MeshNode meshNode, out int vertCount)
        {
            // Add positions and indices, these are the ones we require, everything else is optional.
            var gltfPositions = gltfPrimitive.GetVertexAccessor("POSITION").AsVector3Array();
            meshNode.AddArray("vp", gltfPositions);

            // Check for normals.
            var normAccessor = gltfPrimitive.GetVertexAccessor("NORMAL");
            if (normAccessor is not null)
            {
                var gltfNormals = normAccessor.AsVector3Array();
                meshNode.AddArray("vn", gltfNormals);
            }

            // Check for uv layers, you may want to parse all layers, but for this example we'll stick with layer 0.
            var uvAccessor = gltfPrimitive.GetVertexAccessor("TEXCOORD_0");
            if (uvAccessor is not null)
            {
                var gltfUVs = uvAccessor.AsVector2Array();
                meshNode.AddValue("ul", (byte)1);
                meshNode.AddArray("u0", gltfUVs);
            }

            vertCount = gltfPositions.Count;
        }

        public static void ConvertGltfFile(string fileName)
        {
            // NOTE: Doesn't take into account a mesh being used by multiple nodes. May fail on some models.
            // The purpose of this is to show how to use the lib, not to make a true Gltf to Cast converter
            // but if you want to help, feel free to file a PR improving this! :D
            // TODO: Add animations and shapes to this example!
            var gltfModel = ModelRoot.Load(fileName);
            var gltfSkin = gltfModel.LogicalSkins.Count == 0 ? null : gltfModel.LogicalSkins[0];
            var root = new CastNode(CastNodeIdentifier.Root);
            var model = root.AddNode<ModelNode>();
            var skeleton = model.AddNode<SkeletonNode>();

            // First consume the skin, grabbing all bones if any exist.
            ConsumeGltfSkin(gltfSkin, skeleton);

            // Required to determine our bone indices data type, this isn't necessary for Cast files,
            // if you don't care for their size, you can skip this and just write big numbers, this helps
            // reduce size however for complicated models that contain counts that can fit in 1/2 byte values.
            var boneCount = skeleton.GetChildCount(CastNodeIdentifier.Bone);

            // Next our beautiful materials, at this point you would also want to add images.
            var materialHashes = ConsumeGltfMaterials(gltfModel, model);

            // Next our juicy meshes.
            foreach (var gltfMesh in gltfModel.LogicalMeshes)
            {
                foreach (var gltfPrimitive in gltfMesh.Primitives)
                {
                    // This example only supports triangles!
                    if (gltfPrimitive.DrawPrimitiveType != PrimitiveType.TRIANGLES)
                    {
                        Console.WriteLine($"| WARNING: Primitive in: {gltfMesh.Name} has unsupported primitive type: {gltfPrimitive.DrawPrimitiveType}. Skipping primitive.");
                        continue;
                    }

                    var meshNode = new MeshNode();
                    // Add material hash.
                    meshNode.AddValue("m", materialHashes[gltfPrimitive.Material.LogicalIndex]);
                    // We require vert count to determine face index sizes, like bones, this is optional, you ccould
                    // opt to just store ushort.
                    ConsumeGltfVertices(gltfPrimitive, meshNode, out int vertCount);
                    ConsumeGltfFaces(gltfPrimitive, meshNode, vertCount);
                    ConsumeGltfWeights(gltfSkin, gltfPrimitive, meshNode, vertCount, boneCount);
                    // Finally add it to the parent, assigning a parent handles adding/removing from the child list.
                    meshNode.Parent = model;
                }
            }

            CastWriter.Save(Path.ChangeExtension(fileName, ".cast"), root);
        }

        public static void Main(string[] args)
        {
            foreach (var file in args)
            {
                if (Path.GetExtension(file).Equals(".gltf", StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine($"Converting: {file}...");
                    try
                    {
                        ConvertGltfFile(file);
                        Console.WriteLine($"Converted: {file}...");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error: {e.Message}");
                    }
                }
            }
        }
    }
}