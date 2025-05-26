// ------------------------------------------------------------------------
// Cast.NET - A .NET Library for reading and writing Cast files.
// Copyright(c) 2025 Philip/Scobalula
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

namespace Cast.NET.Nodes
{
    /// <summary>
    /// A class to hold a <see cref="CastNode"/> that contains an Animation.
    /// </summary>
    public class AnimationNode : CastNode
    {
        /// <summary>
        /// Gets the skeleton assigned to this animation, if none is assigned, null is returned.
        /// </summary>
        public SkeletonNode? Skeleton => TryGetFirstChild<SkeletonNode>(out var node) ? node : null;

        /// <summary>
        /// Gets all the curves stored within this animation.
        /// </summary>
        public CurveNode[] Curves => GetChildrenOfType<CurveNode>();

        /// <summary>
        /// Gets all the curve mode overrides stored within this animation.
        /// </summary>
        public CurveModeOverrideNode[] CurveModeOverrides => GetChildrenOfType<CurveModeOverrideNode>();

        /// <summary>
        /// Gets all the notification tracks stored within this animation.
        /// </summary>
        public NotificationTrackNode[] NotificationTracks => GetChildrenOfType<NotificationTrackNode>();

        /// <summary>
        /// Gets the framerate of this animation.
        /// </summary>
        public float Framerate => GetFirstValue("f", 30.0f);

        /// <summary>
        /// Gets if looping is enabled for this animation.
        /// </summary>
        public bool Looping => GetFirstValue("b", (byte)0) == 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationNode"/> class.
        /// </summary>
        public AnimationNode() : base(CastNodeIdentifier.Model) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        public AnimationNode(CastNodeIdentifier identifier) : base(identifier) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        public AnimationNode(CastNodeIdentifier identifier, ulong hash) : base(identifier, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        public AnimationNode(ulong hash) : base(CastNodeIdentifier.Model, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public AnimationNode(ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(CastNodeIdentifier.Model, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public AnimationNode(CastNodeIdentifier identifier, ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(identifier, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationNode"/> class.
        /// </summary>
        /// <param name="source">Node to copy from. A shallow copy is performed and references to the source are stored.</param>
        public AnimationNode(CastNode source) : base(source) { }

        /// <summary>
        /// Enumerates through all curves within this animation.
        /// </summary>
        /// <returns>An enumerable collection of curves within this animation.</returns>
        public IEnumerable<CurveNode> EnumerateCurves() => EnumerateChildrenOfType<CurveNode>();

        /// <summary>
        /// Enumerates through all curve move overrides within this animation.
        /// </summary>
        /// <returns>An enumerable collection of curve move overrides within this animation.</returns>
        public IEnumerable<CurveModeOverrideNode> EnumerateCurveModeOverrides() => EnumerateChildrenOfType<CurveModeOverrideNode>();

        /// <summary>
        /// Enumerates through all notification tracks within this animation.
        /// </summary>
        /// <returns>An enumerable collection of notification tracks within this animation.</returns>
        public IEnumerable<CurveNode> EnumerateNotificationTracks() => EnumerateChildrenOfType<CurveNode>();
    }
}
