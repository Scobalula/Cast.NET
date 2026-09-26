# Quick Start

## Reading

```cs
using CastNet;
using CastNet.Nodes;

var cast = CastReader.Load("model.cast");

foreach (var model in cast.Roots[0].EnumerateModels())
{
    foreach (var bone in model.Skeleton?.Bones ?? [])
        Console.WriteLine($"{bone.Name}: parent {bone.ParentIndex}");

    foreach (var mesh in model.EnumerateMeshes())
        Console.WriteLine($"{mesh.Name}: {mesh.VertexCount} vertices, {mesh.FaceCount} faces, material {mesh.Material?.Name}");

    foreach (var material in model.EnumerateMaterials())
        Console.WriteLine($"{material.Name}: {(material.Albedo as FileNode)?.Path}");
}
```

## Writing

```cs
using System.Numerics;
using CastNet;
using CastNet.Nodes;

var root = new RootNode();
var model = root.AddNode(new ModelNode { Name = "chain" });
var skeleton = model.AddNode<SkeletonNode>();

for (var i = 0; i < 16; i++)
    skeleton.AddNode(new BoneNode { Name = $"bone_{i}", ParentIndex = i - 1, LocalPosition = new Vector3(0, 0, 1), LocalRotation = Quaternion.Identity });

skeleton.CalculateWorldTransforms();

CastWriter.Save("chain.cast", root);
```

## Array Properties

Buffers such as vertex positions, face indices and key frames are stored as a `CastArrayProperty`, which keeps values in their raw binary form:

* `AsSpan<T>()` gives zero-copy access when `T` matches the stored type, for example `mesh.Positions.AsSpan<Vector3>()`.
* `GetScalar<T>(index)`, `CopyTo<T>(span)` and `ToArray<T>()` convert from any stored numeric type, so `mesh.Faces.ToArray<int>()` works whether the file stores bytes, shorts or integers.
* `CastArrayProperty.Create<T>(values)` builds a property from a span, and `CastArrayProperty.CreateIndices(values)` picks the narrowest integer type that fits.

Signed integers map to the unsigned type of the same width and `Quaternion` maps to `Vector4`.

