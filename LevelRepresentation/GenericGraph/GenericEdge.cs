using System;
using System.Collections.Generic;
using System.Text;
using SanityEngine.Structure.Graph;

namespace SanityEngine.LevelRepresentation.GenericGraph
{
    public class GenericEdge<TData> : Edge<GenericNode<TData>, GenericEdge<TData>>
    {
        readonly GenericGraph<TData> graph;
        readonly GenericNode<TData> source;
        readonly GenericNode<TData> target;
        float cost;

        public GenericNode<TData> Source
        {
            get { return source; }
        }

        public GenericNode<TData> Target
        {
            get { return target; }
        }

        public float Cost
        {
            get { return cost; }
            set
            {
                cost = value;
                graph.MarkChanged(this);
            }
        }

        /// <summary>
        /// Create a generic edge.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="source">The source node.</param>
        /// <param name="target">The target node.</param>
        /// <param name="cost">The edge's cost.</param>
        public GenericEdge(GenericGraph<TData> graph, GenericNode<TData> source, GenericNode<TData> target, float cost)
        {
            this.graph = graph;
            this.source = source;
            this.target = target;
            this.cost = cost;
        }
    }
}
