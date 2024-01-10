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

namespace Cast.NET
{
    /// <summary>
    /// A class to hold an instance of a Cast object.
    /// </summary>
    public class Cast
    {
        /// <summary>
        /// Gets or Sets the Root Nodes
        /// </summary>
        public List<CastNode> RootNodes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cast"/> class.
        /// </summary>
        public Cast()
        {
            RootNodes = new();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cast"/> class.
        /// </summary>
        /// <param name="rootNodes">The root nodes.</param>
        public Cast(List<CastNode> rootNodes)
        {
            RootNodes = rootNodes;
        }

        /// <summary>
        /// Adds the node to the child list of this node.
        /// </summary>
        /// <typeparam name="T">The type to add.</typeparam>
        /// <returns>The node that was added.</returns>
        public T AddNode<T>() where T : CastNode, new()
        {
            var node = new T();
            RootNodes.Add(node);
            return node;
        }

        /// <summary>
        /// Adds the node to the child list of this node.
        /// </summary>
        /// <typeparam name="T">The type to add.</typeparam>
        /// <param name="node">The node to add.</param>
        /// <returns>The node that was added.</returns>
        public T AddNode<T>(T node) where T : CastNode
        {
            RootNodes.Add(node);
            return node;
        }
    }
}
