using System;
using System.Collections.Generic;
using System.Text;

namespace SanityEngine.Structure.Graph
{
    /// <summary>
    /// Helper class for storing graph changes.
    /// </summary>
    /// <typeparam name="TID">The graph ID type.</typeparam>
    public class GraphChangeHelper<TNode, TEdge>
        where TNode : Node<TNode, TEdge>
        where TEdge : Edge<TNode, TEdge>
    {
        List<TEdge> changedEdges = new List<TEdge>();

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
        public void MarkChanged(TEdge edge)
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
        public TEdge[] GetChangedEdges()
        {
            return changedEdges.ToArray();
        }
    }
}
