# Cast.NET

Cast.NET is a .NET library for reading and writing cast files. [Cast](https://github.com/dtzxporter/cast) is an open source container for models, animations, materials and more designed by DTZxPorter.

Cast.NET provides you with the ability the read and write these files in an easy and efficient way in .NET. It provides high level access to cast files while also allowing you to work with them in any way you want.

# Requirements

Cast.NET requires .NET 6.0 or higher and has been tested on both Windows and Linux.

# Installing

To install Cast.NET you can either pull the code and reference or use package installer:

```
dotnet add package Cast.NET
```

```
Install-Package Cast.NET
```

# Documentation

Documentation is available here: [https://scobalula.github.io/Cast.NET/](https://scobalula.github.io/Cast.NET/). Documentation for Cast.NET is constantly evolving, if you want to help out, please feel free to file a PR!

# Quick Usage Examples

A quick example of loading a model and printing its materials:

```cs
var cast = CastReader.Load("your_cast_file.cast");
var root = cast.RootNodes[0];

foreach (var model in root.EnumerateChildrenOfType<ModelNode>())
{
    if(model.Skeleton is not null)
    {
        foreach (var bone in model.Skeleton.EnumerateBones())
        {
            Console.WriteLine(bone.Name);
        }
    }

    foreach (var material in model.EnumerateChildrenOfType<MaterialNode>())
    {
        Console.WriteLine(material.Name);

        Console.WriteLine($"\tAlbedoFile: {material.AlbedoFile?.Path}");
        Console.WriteLine($"\tDiffuseFile: {material.DiffuseFile?.Path}");
        Console.WriteLine($"\tNormalFile: {material.NormalFile?.Path}");
        Console.WriteLine($"\tSpecularFile: {material.SpecularFile?.Path}");
        Console.WriteLine($"\tEmissiveFile: {material.EmissiveFile?.Path}");
        Console.WriteLine($"\tGlossFile: {material.GlossFile?.Path}");
        Console.WriteLine($"\tRoughnessFile: {material.RoughnessFile?.Path}");
        Console.WriteLine($"\tAmbientOcclusionFile: {material.AmbientOcclusionFile?.Path}");
        Console.WriteLine($"\tCavityFile: {material.CavityFile?.Path}");
    }
}
```

A quick example of building a simple skeleton only model:

```cs
using System.Numerics;

var root = new CastNode(CastNodeIdentifier.Root);
var model = root.AddNode<ModelNode>();
var skeleton = model.AddNode<SkeletonNode>();

for (int i = 0; i < 16; i++)
{
    var bone = skeleton.AddNode<BoneNode>();

    bone.AddString("n", $"bone_{i}");
    bone.AddValue("p", (uint)(i - 1));
    bone.AddValue("lp", new Vector3(0, 0, i));
    bone.AddValue("lr", new Vector4(0, 0, 0, 1));
}

CastWriter.Save("your_cast_file.cast", root);
```
# In-depth Examples

More in-depth examples are included in the source code, these include a basic Gltf to cast converter and a simple project that dumps all information in the cast file to a text file. The examples are constantly evolving with more being added as time goes on. If you're interested in helping out, feel free to file a PR with an example to help other learn how to use the library.

# License/Disclaimers

Cast.NET is currently in an alpha state but tests show it's perfectly usable, the API may have breaking changes pre-release but I hope to avoid anything that would break current usage.

Cast.NET is licensed under the [MIT license](LICENSE). Cast.NET is a third-party library and is not associated with DTZxPorter or anyone who has worked on Cast, any issues with Cast.NET should be directed to this repo. This library comes with no warranty, please refer to the [license](LICENSE) file for more information.