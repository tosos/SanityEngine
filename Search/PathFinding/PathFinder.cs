using System;
using System.Collections.Generic;
using System.Text;
using AIEngine.Structure.Graph;
using AIEngine.Structure.Path;

namespace AIEngine.Search.PathFinding
{
    /// <summary>
    /// Path finding interface.
    /// </summary>
    /// <typeparam name="TID">The node ID type.</typeparam>
    public interface PathFinder<TNode, TEdge>
        where TNode : Node<TNode, TEdge>
        where TEdge : Edge<TNode, TEdge>
    {
        /// <summary>
        /// Perform a search from the start to the goal.
        /// </summary>
        /// <param name="start">The start node.</param>
        /// <param name="goal">The goal node.</param>
        /// <returns>The path or <code>null</code> if no path could be found.</returns>
        Path<TNode, TEdge> Search(TNode start, TNode goal);
    }
}
