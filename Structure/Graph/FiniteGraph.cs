using System;
using System.Collections.Generic;
using System.Text;

namespace AIEngine.Structure.Graph
{
    /// <summary>
    /// Specialized version of the graph interface for finite graphs.
    /// </summary>
    public interface FiniteGraph<TNode, TEdge> : Graph<TNode, TEdge>
        where TNode : Node<TNode, TEdge>
        where TEdge : Edge<TNode, TEdge>
    {
        /// <summary>
        /// Get the number of nodes.
        /// </summary>
        int NodeCount
        {
            get;
        }
    }
}
