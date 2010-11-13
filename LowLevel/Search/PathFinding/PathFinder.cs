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
using SanityEngine.Structure.Graph;
using SanityEngine.Structure.Path;

namespace SanityEngine.Search.PathFinding
{
    /// <summary>
    /// Path finding interface.
    /// </summary>
    /// <typeparam name="TID">The node ID type.</typeparam>
    public interface PathFinder
    {
        /// <summary>
        /// Perform a search from the start to the goal.
        /// </summary>
        /// <param name="graph">The graph to search.</para>
        /// <param name="start">The start node.</param>
        /// <param name="goal">The goal node.</param>
        /// <returns>The path or <code>null</code> if no path could be found.</returns>
        Path Search(Graph graph, Node start, Node goal);
    }
}
