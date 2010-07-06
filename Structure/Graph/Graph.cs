using System;
using System.Collections.Generic;
using System.Text;

namespace AIEngine.Structure.Graph
{
    /// <summary>
    /// Interface for any general graph object.
    /// </summary>
    /// <remarks>
    /// All graphs are considered to be directed at this time.
    /// </remarks>
    public interface Graph<TNode, TEdge>
        where TNode : Node<TNode, TEdge>
        where TEdge : Edge<TNode, TEdge>
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
        TEdge[] GetChangedEdges();


        /// <summary>
        /// Reset the changed state.
        /// </summary>
        void ResetChanges();
    }
}
