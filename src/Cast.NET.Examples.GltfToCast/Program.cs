using System.Numerics;
using CastNet.Nodes;
using SharpGLTF.Schema2;

namespace CastNet.Examples.GltfToCast;

/// <summary>
/// The entry point of the example.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Converts each glTF file passed on the command line to a cast file next to it.
    /// </summary>
    /// <param name="args">The files to convert.</param>
    public static void Main(string[] args)
    {
        foreach (var path in args)
        {
            var gltf = ModelRoot.Load(path);
            var skin = gltf.LogicalSkins.FirstOrDefault();
            var root = new RootNode();
            var model = root.AddNode(new ModelNode { Name = Path.GetFileNameWithoutExtension(path) });
            var materials = gltf.LogicalMaterials.Select(material => model.AddNode(new MaterialNode { Name = material.Name ?? $"material_{material.LogicalIndex}" })).ToArray();

            if (skin is not null)
            {
                var skeleton = model.AddNode<SkeletonNode>();
                var jointIndices = new Dictionary<Node, int>();

                for (var i = 0; i < skin.JointsCount; i++)
                {
                    var (joint, inverseBindMatrix) = skin.GetJoint(i);
                    Matrix4x4.Invert(inverseBindMatrix, out var bindMatrix);

                    skeleton.AddNode(new BoneNode { Name = joint.Name ?? $"joint_{i}", WorldPosition = bindMatrix.Translation, WorldRotation = Quaternion.CreateFromRotationMatrix(bindMatrix) });
                    jointIndices[joint] = i;
                }

                var bones = skeleton.Bones;

                foreach (var (joint, index) in jointIndices)
                {
                    if (joint.VisualParent is Node parent && jointIndices.TryGetValue(parent, out var parentIndex))
                        bones[index].ParentIndex = parentIndex;
                }

                skeleton.CalculateLocalTransforms();
            }

            foreach (var primitive in gltf.LogicalMeshes.SelectMany(mesh => mesh.Primitives).Where(primitive => primitive.DrawPrimitiveType == PrimitiveType.TRIANGLES))
            {
                var mesh = model.AddNode(new MeshNode { Name = primitive.LogicalParent.Name });
                var positions = primitive.GetVertexAccessor("POSITION").AsVector3Array().ToArray();
                var indices = primitive.GetIndices();
                var faces = new List<int>(indices.Count);

                for (var i = 0; i + 2 < indices.Count; i += 3)
                {
                    var (a, b, c) = ((int)indices[i], (int)indices[i + 1], (int)indices[i + 2]);

                    if (a != b && b != c && c != a)
                        faces.AddRange([a, b, c]);
                }

                mesh.Positions = CastArrayProperty.Create<Vector3>(positions);
                mesh.Faces = CastArrayProperty.CreateIndices([.. faces]);

                if (primitive.Material is not null)
                    mesh.Material = materials[primitive.Material.LogicalIndex];

                if (primitive.GetVertexAccessor("NORMAL") is Accessor normals)
                    mesh.Normals = CastArrayProperty.Create<Vector3>(normals.AsVector3Array().ToArray());

                if (primitive.GetVertexAccessor("TEXCOORD_0") is Accessor uvs)
                    mesh.SetUVLayer(0, CastArrayProperty.Create<Vector2>(uvs.AsVector2Array().ToArray()));

                var setCount = 0;

                while (skin is not null && primitive.GetVertexAccessor($"JOINTS_{setCount}") is not null && primitive.GetVertexAccessor($"WEIGHTS_{setCount}") is not null)
                    setCount++;

                if (setCount == 0)
                    continue;

                var influence = setCount * 4;
                var weightBones = new int[positions.Length * influence];
                var weightValues = new float[positions.Length * influence];

                for (var set = 0; set < setCount; set++)
                {
                    var joints = primitive.GetVertexAccessor($"JOINTS_{set}").AsVector4Array();
                    var weights = primitive.GetVertexAccessor($"WEIGHTS_{set}").AsVector4Array();

                    for (var vertex = 0; vertex < positions.Length; vertex++)
                    {
                        var offset = vertex * influence + set * 4;
                        var (joint, weight) = (joints[vertex], weights[vertex]);

                        (weightBones[offset], weightBones[offset + 1], weightBones[offset + 2], weightBones[offset + 3]) = ((int)joint.X, (int)joint.Y, (int)joint.Z, (int)joint.W);
                        (weightValues[offset], weightValues[offset + 1], weightValues[offset + 2], weightValues[offset + 3]) = (weight.X, weight.Y, weight.Z, weight.W);
                    }
                }

                mesh.MaximumWeightInfluence = influence;
                mesh.WeightBones = CastArrayProperty.CreateIndices(weightBones);
                mesh.WeightValues = CastArrayProperty.Create<float>(weightValues);
            }

            CastWriter.Save(Path.ChangeExtension(path, ".cast"), root);
            Console.WriteLine($"Converted: {path}");
        }
    }
}
