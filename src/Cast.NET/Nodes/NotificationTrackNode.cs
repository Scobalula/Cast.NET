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

namespace Cast.NET.Nodes
{
    /// <summary>
    /// A class to hold a <see cref="CastNode"/> that contains a Notification Track.
    /// </summary>
    public class NotificationTrackNode : CastNode
    {
        /// <summary>
        /// Gets the name of the notification.
        /// </summary>
        public string NodeName => GetStringValue("n");

        /// <summary>
        /// Gets the raw key frame buffer stored within this notification track.
        /// </summary>
        public CastProperty KeyFrameBuffer => GetProperty("vn");

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationTrackNode"/> class.
        /// </summary>
        public NotificationTrackNode() : base(CastNodeIdentifier.Model) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationTrackNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        public NotificationTrackNode(CastNodeIdentifier identifier) : base(identifier) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationTrackNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        public NotificationTrackNode(CastNodeIdentifier identifier, ulong hash) : base(identifier, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationTrackNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        public NotificationTrackNode(ulong hash) : base(CastNodeIdentifier.Model, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationTrackNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public NotificationTrackNode(ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(CastNodeIdentifier.Model, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public NotificationTrackNode(CastNodeIdentifier identifier, ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(identifier, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationTrackNode"/> class.
        /// </summary>
        /// <param name="source">Node to copy from. A shallow copy is performed and references to the source are stored.</param>
        public NotificationTrackNode(CastNode source) : base(source) { }
    }
}
