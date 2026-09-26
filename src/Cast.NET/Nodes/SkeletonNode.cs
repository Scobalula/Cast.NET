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

namespace CastNet.Nodes;

/// <summary>
/// A skeleton.
/// </summary>
/// <param name="hash">The node hash.</param>
public sealed class SkeletonNode(ulong hash) : CastNode(CastNodeIdentifier.Skeleton, hash)
{
    /// <summary>
    /// Gets or sets the bones in index order. Setting this replaces all existing bones.
    /// </summary>
    public BoneNode[] Bones { get => [.. EnumerateChildren<BoneNode>()]; set => ReplaceChildren(value); }

    /// <summary>
    /// Gets or sets the IK handles. Setting this replaces all existing IK handles.
    /// </summary>
    public IKHandleNode[] IKHandles { get => [.. EnumerateChildren<IKHandleNode>()]; set => ReplaceChildren(value); }

    /// <summary>
    /// Gets or sets the constraints. Setting this replaces all existing constraints.
    /// </summary>
    public ConstraintNode[] Constraints { get => [.. EnumerateChildren<ConstraintNode>()]; set => ReplaceChildren(value); }

    /// <summary>
    /// Initializes a new instance of the <see cref="SkeletonNode"/> class with a unique hash.
    /// </summary>
    public SkeletonNode() : this(CastHash.Next())
    {
    }

    /// <summary>
    /// Enumerates the bones.
    /// </summary>
    /// <returns>The bones.</returns>
    public IEnumerable<BoneNode> EnumerateBones() => EnumerateChildren<BoneNode>();

    /// <summary>
    /// Enumerates the IK handles.
    /// </summary>
    /// <returns>The IK handles.</returns>
    public IEnumerable<IKHandleNode> EnumerateIKHandles() => EnumerateChildren<IKHandleNode>();

    /// <summary>
    /// Enumerates the constraints.
    /// </summary>
    /// <returns>The constraints.</returns>
    public IEnumerable<ConstraintNode> EnumerateConstraints() => EnumerateChildren<ConstraintNode>();

    /// <summary>
    /// Finds the bone with the given name.
    /// </summary>
    /// <param name="name">The name of the bone.</param>
    /// <returns>The bone, or <see langword="null"/> if not found.</returns>
    public BoneNode? FindBone(string name) => EnumerateBones().FirstOrDefault(bone => bone.Name == name);

    /// <summary>
    /// Calculates the local transforms of all bones from their world transforms.
    /// </summary>
    public void CalculateLocalTransforms()
    {
        var bones = Bones;

        foreach (var bone in bones)
        {
            var worldPosition = bone.WorldPosition ?? Vector3.Zero;
            var worldRotation = bone.WorldRotation ?? Quaternion.Identity;

            if ((uint)bone.ParentIndex >= bones.Length)
            {
                bone.LocalPosition = worldPosition;
                bone.LocalRotation = worldRotation;
                continue;
            }

            var parent = bones[bone.ParentIndex];
            var inverseParentRotation = Quaternion.Inverse(parent.WorldRotation ?? Quaternion.Identity);

            bone.LocalPosition = Vector3.Transform(worldPosition - (parent.WorldPosition ?? Vector3.Zero), inverseParentRotation);
            bone.LocalRotation = inverseParentRotation * worldRotation;
        }
    }

    /// <summary>
    /// Calculates the world transforms of all bones from their local transforms.
    /// </summary>
    /// <exception cref="InvalidDataException">Thrown if the bone hierarchy contains a cycle.</exception>
    public void CalculateWorldTransforms()
    {
        var bones = Bones;
        var calculated = new bool[bones.Length];
        var remaining = bones.Length;

        while (remaining > 0)
        {
            var progressed = false;

            for (var i = 0; i < bones.Length; i++)
            {
                var parentIndex = bones[i].ParentIndex;
                var isRoot = (uint)parentIndex >= bones.Length;

                if (calculated[i] || (!isRoot && !calculated[parentIndex]))
                    continue;

                var localPosition = bones[i].LocalPosition ?? Vector3.Zero;
                var localRotation = bones[i].LocalRotation ?? Quaternion.Identity;
                var parentPosition = isRoot ? Vector3.Zero : bones[parentIndex].WorldPosition ?? Vector3.Zero;
                var parentRotation = isRoot ? Quaternion.Identity : bones[parentIndex].WorldRotation ?? Quaternion.Identity;

                bones[i].WorldPosition = Vector3.Transform(localPosition, parentRotation) + parentPosition;
                bones[i].WorldRotation = parentRotation * localRotation;
                calculated[i] = true;
                progressed = true;
                remaining--;
            }

            if (!progressed)
                throw new InvalidDataException("The bone hierarchy contains a cycle.");
        }
    }
}
