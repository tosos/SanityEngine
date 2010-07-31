//
// Copyright (C) 2010 The Sanity Engine Development Team
//
// This source code is licensed under the terms of the
// MIT License.
//
// For more information, see the file LICENSE

using System;
using System.Collections.Generic;
using System.Text;

namespace SanityEngine.Structure.Graph
{
    /// <summary>
    /// A node object.
    /// </summary>
    public interface Node<TNode, TEdge>
        where TNode : Node<TNode, TEdge>
        where TEdge : Edge<TNode, TEdge>
    {
        /// <summary>
        /// The graph this node belongs to.
        /// </summary>
        Graph<TNode, TEdge> Graph
        {
            get;
        }

        /// <summary>
        /// The number of edges originating from this node.
        /// </summary>
        int OutEdgeCount
        {
            get;
        }

        /// <summary>
        /// The number of edges terminating at this node.
        /// </summary>
        int InEdgeCount
        {
            get;
        }

        /// <summary>
        /// Get the specified out edge.
        /// </summary>
        /// <param name="index">the edge index</param>
        /// <returns>The requested edge</returns>
        TEdge GetOutEdge(int index);

        /// <summary>
        /// Get the specified in edge.
        /// </summary>
        /// <param name="index">the edge index</param>
        /// <returns>The requested edge</returns>
        TEdge GetInEdge(int index);
    }
}
