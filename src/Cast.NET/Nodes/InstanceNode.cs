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
/// A placed instance of another cast file.
/// </summary>
/// <param name="hash">The node hash.</param>
public sealed class InstanceNode(ulong hash) : CastNode(CastNodeIdentifier.Instance, hash)
{
    /// <summary>
    /// Gets or sets the name of the instance.
    /// </summary>
    public string? Name { get => GetString("n"); set => SetString("n", value); }

    /// <summary>
    /// Gets or sets the referenced file.
    /// </summary>
    public FileNode? ReferenceFile
    {
        get => GetScalar<ulong>("rf") is ulong hash ? FindChild<FileNode>(hash) : null;
        set
        {
            if (value?.Parent is not null && value.Parent != this)
                throw new InvalidOperationException("The file already belongs to another node. Create a separate file node for each instance.");

            if (value is not null && value.Parent is null)
                AddNode(value);

            SetValue("rf", value?.Hash);
        }
    }

    /// <summary>
    /// Gets or sets the position of the instance.
    /// </summary>
    public Vector3 Position { get => GetValue("p", Vector3.Zero); set => SetValue("p", value); }

    /// <summary>
    /// Gets or sets the rotation of the instance.
    /// </summary>
    public Quaternion Rotation { get => GetValue("r", Quaternion.Identity); set => SetValue("r", value); }

    /// <summary>
    /// Gets or sets the scale of the instance.
    /// </summary>
    public Vector3 Scale { get => GetValue("s", Vector3.One); set => SetValue("s", value); }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstanceNode"/> class with a unique hash.
    /// </summary>
    public InstanceNode() : this(CastHash.Next())
    {
    }
}
