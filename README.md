# Cast.NET

<p align="center">
	<img src="img/logo.png" alt="Cast"/>
</p>

Cast.NET is a .NET library for reading and writing cast files. [Cast](https://github.com/dtzxporter/cast) is an open source container for models, animations, materials and more designed by DTZxPorter.

Cast.NET gives you fast, typed access to every node in the cast specification while still letting you work with the raw properties of any node.

# Requirements

Cast.NET targets .NET 10 and is tested on Windows and Linux.

# Installing

```
dotnet add package Cast.NET
```

Releases are versioned by date and build number (`yyyy.M.d.build`). See the [changelog](CHANGELOG.md) for what changed in each release.

# Documentation

Documentation is available here: [https://scobalula.github.io/Cast.NET/](https://scobalula.github.io/Cast.NET/).

# Reading

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

# Writing

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

# Array Properties

Buffers such as vertex positions, face indices and key frames are stored as a `CastArrayProperty`, which keeps values in their raw binary form:

* `AsSpan<T>()` gives zero-copy access when `T` matches the stored type, for example `mesh.Positions.AsSpan<Vector3>()`.
* `GetScalar<T>(index)`, `CopyTo<T>(span)` and `ToArray<T>()` convert from any stored numeric type, so `mesh.Faces.ToArray<int>()` works whether the file stores bytes, shorts or integers.
* `CastArrayProperty.Create<T>(values)` builds a property from a span, and `CastArrayProperty.CreateIndices(values)` picks the narrowest integer type that fits.

Signed integers map to the unsigned type of the same width and `Quaternion` maps to `Vector4`.

# Examples

The `src` folder contains a glTF to cast converter, an SEModel and SEAnim to cast converter, and a tool that dumps the contents of a cast file to text.

# License/Disclaimers

Cast.NET is licensed under the [MIT license](LICENSE.md). Cast.NET is a third-party library and is not associated with DTZxPorter or anyone who has worked on Cast, any issues with Cast.NET should be directed to this repo. This library comes with no warranty, please refer to the [license](LICENSE.md) file for more information.

# Attribution

Icons by Smashicons (Modified)
