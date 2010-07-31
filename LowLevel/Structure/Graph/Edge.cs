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
    /// An edge object.
    /// </summary>
    /// <typeparam name="TNode">The node type</typeparam>
    public interface Edge<TNode, TEdge>
        where TNode : Node<TNode, TEdge>
        where TEdge : Edge<TNode, TEdge>
    {
        /// <summary>
        /// The source node.
        /// </summary>
        TNode Source
        {
            get;
        }

        /// <summary>
        /// The target node;
        /// </summary>
        TNode Target
        {
            get;
        }

        /// <summary>
        /// The cost of this edge.
        /// </summary>
        float Cost
        {
            get;
        }
    }
}
