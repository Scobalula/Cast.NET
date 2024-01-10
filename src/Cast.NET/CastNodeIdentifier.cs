// ------------------------------------------------------------------------
// Cast.NET - A .NET Library for reading and writing Cast files.
// Copyright(c) 2024 Philip/Scobalula
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
using Cast.NET.Nodes;

namespace Cast.NET
{
    /// <summary>
    /// Specifies the supported <see cref="CastNode"/> identifiers. Unsupported types will resolve to <see cref="CastNode"/>.
    /// </summary>
    public enum CastNodeIdentifier : uint
    {
        /// <summary>
        /// A node that contains a root <see cref="CastNode"/>.
        /// </summary>
        Root                = 0x746F6F72,

        /// <summary>
        /// A node that contains a <see cref="ModelNode"/>.
        /// </summary>
        Model               = 0x6C646F6D,

        /// <summary>
        /// A node that contains a <see cref="MeshNode"/>.
        /// </summary>
        Mesh                = 0x6873656D,

        /// <summary>
        /// A node that contains a <see cref="BlendShapeNode"/>.
        /// </summary>
        BlendShape          = 0x68736C62,

        /// <summary>
        /// A node that contains a <see cref="SkeletonNode"/>.
        /// </summary>
        Skeleton            = 0x6C656B73,

        /// <summary>
        /// A node that contains a <see cref="BoneNode"/>.
        /// </summary>
        Bone                = 0x656E6F62,

        /// <summary>
        /// A node that contains a <see cref="IKHandle"/>.
        /// </summary>
        IKHandle            = 0x64686B69,

        /// <summary>
        /// A node that contains a <see cref="ConstraintNode"/>.
        /// </summary>
        Constraint          = 0x74736E63,

        /// <summary>
        /// A node that contains a <see cref="AnimationNode"/>.
        /// </summary>
        Animation           = 0x6D696E61,

        /// <summary>
        /// A node that contains a <see cref="CurveNode"/>.
        /// </summary>
        Curve               = 0x76727563,

        /// <summary>
        /// A node that contains a <see cref="NotificationTrackNode"/>.
        /// </summary>
        NotificationTrack   = 0x6669746E,

        /// <summary>
        /// A node that contains a <see cref="MaterialNode"/>.
        /// </summary>
        Material            = 0x6C74616D,

        /// <summary>
        /// A node that contains a <see cref="FileNode"/>.
        /// </summary>
        File                = 0x656C6966,
    };
}
