using System;
using System.Collections.Generic;
using System.Text;
using AIEngine.Structure.Graph;

namespace AIEngine.LevelRepresentation.GenericGraph
{
    /// <summary>
    /// A generic graph with nodes containing references to arbitrary data.
    /// </summary>
    /// <typeparam name="TData">The data type.</typeparam>
    public class GenericGraph<TData> : FiniteGraph<GenericNode<TData>, GenericEdge<TData>>
    {
        List<GenericNode<TData>> nodes = new List<GenericNode<TData>>();
        GraphChangeHelper<GenericNode<TData>, GenericEdge<TData>> helper =
            new GraphChangeHelper<GenericNode<TData>, GenericEdge<TData>>();

        /// <summary>
        /// The number of nodes.
        /// </summary>
        public int NodeCount
        {
            get { return nodes.Count; }
        }

        /// <summary>
        /// <code>true</code> if the graph has changed since the last reset.
        /// </summary>
        public bool HasChanged
        {
            get { return helper.HasChanged; }
        }

        /// <summary>
        /// Get the list of changed edges since the last reset.
        /// </summary>
        /// <returns>The list of changed edges.</returns>
        public GenericEdge<TData>[] GetChangedEdges()
        {
            return helper.GetChangedEdges();
        }

        /// <summary>
        /// Reset the change record.
        /// </summary>
        public void ResetChanges()
        {
            helper.Reset();
        }

        /// <summary>
        /// Add a new node.
        /// </summary>
        /// <param name="data">The data stored at the node</param>
        /// <returns>The new node</returns>
        public GenericNode<TData> AddNode(TData data)
        {
            GenericNode<TData> node = new GenericNode<TData>(this, data);
            nodes.Add(node);
            return node;
        }

        /// <summary>
        /// Add a new edge.
        /// </summary>
        /// <param name="source">The index of the source node.</param>
        /// <param name="target">The index of the destination node.</param>
        /// <param name="cost">The cost of the edge.</param>
        /// <returns>The new edge</returns>
        public GenericEdge<TData> AddEdge(int source, int target, float cost)
        {
            GenericEdge<TData> edge = new GenericEdge<TData>(this, nodes[source], nodes[target], cost);
            nodes[source].AddOutEdge(edge);
            nodes[target].AddInEdge(edge);
            helper.MarkChanged(edge);
            return edge;
        }

        internal void MarkChanged(GenericEdge<TData> edge)
        {
            helper.MarkChanged(edge);
        }
    }
}
