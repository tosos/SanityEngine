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
    /// Helper class for storing graph changes.
    /// </summary>
    public class GraphChangeHelper
    {
        List<Edge> changedEdges = new List<Edge>();

        /// <summary>
        /// <code>true</code> if there are changes.
        /// </summary>
        public bool HasChanged
        {
            get { return changedEdges.Count > 0; }
        }

        /// <summary>
        /// Mark the given edge as changed.
        /// </summary>
        /// <param name="edge">The edge to mark.</param>
        public void MarkChanged(Edge edge)
        {
            changedEdges.Add(edge);
        }

        /// <summary>
        /// Reset the changes.
        /// </summary>
        public void Reset()
        {
            changedEdges.Clear();
        }

        /// <summary>
        /// Get the list of changed edges since the last reset.
        /// </summary>
        /// <returns>The list of changed edges.</returns>
        public Edge[] GetChangedEdges()
        {
            return changedEdges.ToArray();
        }
    }
}
