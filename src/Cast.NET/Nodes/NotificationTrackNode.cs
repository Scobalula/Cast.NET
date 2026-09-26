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
namespace CastNet.Nodes;

/// <summary>
/// A named notification and the frames it fires on.
/// </summary>
/// <param name="hash">The node hash.</param>
public sealed class NotificationTrackNode(ulong hash) : CastNode(CastNodeIdentifier.NotificationTrack, hash)
{
    /// <summary>
    /// Gets or sets the name of the notification.
    /// </summary>
    public string Name { get => GetString("n") ?? string.Empty; set => SetString("n", value); }

    /// <summary>
    /// Gets or sets the frames the notification fires on, stored as any integer type.
    /// </summary>
    public CastArrayProperty? KeyFrames { get => GetArray("kb"); set => SetArray("kb", value); }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationTrackNode"/> class with a unique hash.
    /// </summary>
    public NotificationTrackNode() : this(CastHash.Next())
    {
    }
}
