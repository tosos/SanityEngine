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
    /// Interface for any general graph object.
    /// </summary>
    /// <remarks>
    /// All graphs are considered to be directed at this time.
    /// </remarks>
    public interface Graph
    {
        /// <summary>
        /// Has this graph changed since the last reset?
        /// </summary>
        bool HasChanged
        {
            get;
        }

        /// <summary>
        /// Get a list of changed edges since the last time the
        /// changed were reset.
        /// </summary>
        /// <returns>the list of changed edges</returns>
        Edge[] GetChangedEdges();


        /// <summary>
        /// Reset the changed state.
        /// </summary>
        void ResetChanges();
		
		/// <summary>
        /// The number of edges originating from the node.
        /// </summary>
        /// <param name="node">The node</para>
        /// <returns>the number of edges</returns>
        int GetOutEdgeCount(Node node);

        /// <summary>
        /// The number of edges terminating at the node.
        /// </summary>
        /// <param name="node">The node</para>
        /// <returns>the number of edges</returns>
        int GetInEdgeCount(Node node);

        /// <summary>
        /// Get the specified out edge.
        /// </summary>
        /// <param name="node">The node</para>
        /// <param name="index">the edge index</param>
        /// <returns>The requested edge</returns>
        Edge GetOutEdge(Node node, int index);

        /// <summary>
        /// Get the specified in edge.
        /// </summary>
        /// <param name="node">The node</para>
        /// <param name="index">the edge index</param>
        /// <returns>The requested edge</returns>
        Edge GetInEdge(Node node, int index);
    }
}
