# Changelog

## 2026.9.26.1

The first stable release. This release breaks compatibility with the alpha, so check the upgrade notes below.

### New

- Support for everything in the current Cast spec, including hair, IK handle target offsets, metadata scene roots, and any material slot (`metal`, `extra0`, ...).
- Array properties can be read as any number type. Faces stored as bytes can be read straight into an `int[]` with `ToArray<int>()`.
- `CastArrayProperty.CreateIndices` stores indices in the smallest type that fits.
- `AddScalar` and `AddScalarRange` add values of any number type to an existing array.
- Old files that store vertex colors in `vc` are read as color layer 0.
- Packages now include XML documentation and debug symbols.
- `RootNode` helpers for models, animations, instances and metadata.
- `MeshNode.GetColors` returns any color layer as `Vector4` colors.
- `MeshNode.VertexCount`, `MeshNode.FaceCount` and `SkeletonNode.FindBone`.
- Setting a uv or color layer updates the layer count.
- Nodes return their name from `ToString`.
- New example that converts SEModel and SEAnim files to Cast.

### Fixed

- Non-English names and text are no longer corrupted when saving.
- `ConstraintNode.SkipY` and `SkipZ` now save to the right properties.
- `CurveModeOverrideNode` rotation and scale overrides now save correctly.
- Setting `BoneNode.SegmentScaleCompensate` no longer throws.
- Scale constraint offsets are no longer lost.
- Hair and curve mode override nodes now load as their proper types.
- `AnimationNode.Skeleton` no longer throws when the animation has curves.
- Setting collections like `AnimationNode.Curves` or `ModelNode.Meshes` now keeps the other children and links the new nodes to their parent.
- Default values now match the Cast spec.
- `SkeletonNode.CalculateWorldTransforms` now gives the right positions, including for bones listed before their parents.
- New nodes no longer reuse hashes found in files written by Cast.NET or the official Cast tools.
- Adding a node to itself or one of its children now throws instead of crashing on save.

### Faster

- Loading and saving are around 40% faster and use about 40% less memory.

### Upgrading from the alpha

- Requires .NET 10.
- The namespace is now `CastNet`. The package is still called `Cast.NET`.
- `Cast.RootNodes` is now `Roots`, and holds `RootNode` instances.
- `CastArrayProperty<T>` is now `CastArrayProperty`. Use `AsSpan<T>()`, `Get<T>(index)` or `ToArray<T>()` instead of `Values`.
- `AddString`, `AddValue` and `AddArray` are now `SetString`, `SetValue` and `SetArray`.
- `GetStringValue` and `GetFirstValue` are now `GetString`, `GetValue` and `GetScalar`.
- Child lookups are now `GetChild<T>()`, `FindChild<T>(hash)` and `EnumerateChildren<T>()`.
- Use `AddNode` and `RemoveNode` to change a node's children.
- Nodes get a unique hash automatically. `CastHasher` has been removed.
- Node properties that referenced other nodes by hash now return the node itself, for example `mesh.Material`.
- Optional values like `BoneNode.LocalPosition` are now nullable instead of using `TryGet` methods.
- Material hash properties like `AlbedoHash` have been removed. Use `Albedo`, or `GetSlot` and `SetSlot` for any slot.
- `CastPropertyIdentifier` is now `CastPropertyType`, and `ValueCount` is now `Count`.
