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
/// An animation.
/// </summary>
/// <param name="hash">The node hash.</param>
public sealed class AnimationNode(ulong hash) : CastNode(CastNodeIdentifier.Animation, hash)
{
    /// <summary>
    /// Gets or sets the name of the animation.
    /// </summary>
    public string? Name { get => GetString("n"); set => SetString("n", value); }

    /// <summary>
    /// Gets or sets the framerate of the animation.
    /// </summary>
    public float Framerate { get => GetScalar("fr", 30.0f); set => SetValue("fr", value); }

    /// <summary>
    /// Gets or sets whether the animation loops.
    /// </summary>
    public bool Looping { get => GetBoolean("lo", false); set => SetBoolean("lo", value); }

    /// <summary>
    /// Gets the skeleton of the animation, or <see langword="null"/> if the animation has no skeleton.
    /// </summary>
    public SkeletonNode? Skeleton => GetChild<SkeletonNode>();

    /// <summary>
    /// Gets or sets the curves. Setting this replaces all existing curves.
    /// </summary>
    public CurveNode[] Curves { get => [.. EnumerateChildren<CurveNode>()]; set => ReplaceChildren(value); }

    /// <summary>
    /// Gets or sets the curve mode overrides. Setting this replaces all existing overrides.
    /// </summary>
    public CurveModeOverrideNode[] CurveModeOverrides { get => [.. EnumerateChildren<CurveModeOverrideNode>()]; set => ReplaceChildren(value); }

    /// <summary>
    /// Gets or sets the notification tracks. Setting this replaces all existing tracks.
    /// </summary>
    public NotificationTrackNode[] NotificationTracks { get => [.. EnumerateChildren<NotificationTrackNode>()]; set => ReplaceChildren(value); }

    /// <summary>
    /// Initializes a new instance of the <see cref="AnimationNode"/> class with a unique hash.
    /// </summary>
    public AnimationNode() : this(CastHash.Next())
    {
    }

    /// <summary>
    /// Enumerates the curves.
    /// </summary>
    /// <returns>The curves.</returns>
    public IEnumerable<CurveNode> EnumerateCurves() => EnumerateChildren<CurveNode>();

    /// <summary>
    /// Enumerates the curve mode overrides.
    /// </summary>
    /// <returns>The curve mode overrides.</returns>
    public IEnumerable<CurveModeOverrideNode> EnumerateCurveModeOverrides() => EnumerateChildren<CurveModeOverrideNode>();

    /// <summary>
    /// Enumerates the notification tracks.
    /// </summary>
    /// <returns>The notification tracks.</returns>
    public IEnumerable<NotificationTrackNode> EnumerateNotificationTracks() => EnumerateChildren<NotificationTrackNode>();
}
