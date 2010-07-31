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

namespace SanityEngine.LevelRepresentation.GenericGraph
{
    public class GenericNode<TData> : Node<GenericNode<TData>, GenericEdge<TData>>
    {
        readonly GenericGraph<TData> graph;
        List<GenericEdge<TData>> outEdges = new List<GenericEdge<TData>>();
        List<GenericEdge<TData>> inEdges = new List<GenericEdge<TData>>();
        TData data;

        public TData Data
        {
            get { return data; }
        }

        public Graph<GenericNode<TData>, GenericEdge<TData>> Graph
        {
            get { return graph; }
        }

        public int OutEdgeCount
        {
            get { return outEdges.Count; }
        }

        public int InEdgeCount
        {
            get { return inEdges.Count; }
        }

        public GenericEdge<TData> GetOutEdge(int index)
        {
            return outEdges[index];
        }

        public GenericEdge<TData> GetInEdge(int index)
        {
            return inEdges[index];
        }

        /// <summary>
        /// Create a generic node.
        /// </summary>
        /// <param name="id">The node ID.</param>
        /// <param name="graph">The graph this node is in.</param>
        /// <param name="data">The node's data.</param>
        public GenericNode(GenericGraph<TData> graph, TData data)
        {
            this.graph = graph;
            this.data = data;
        }

        /// <summary>
        /// Add an outgoing edge to this node.
        /// </summary>
        /// <param name="edge">The edge to add.</param>
        public void AddOutEdge(GenericEdge<TData> edge)
        {
            outEdges.Add(edge);
        }

        /// <summary>
        /// Add an incoming edge to this node.
        /// </summary>
        /// <param name="edge">The edge to add.</param>
        public void AddInEdge(GenericEdge<TData> edge)
        {
            inEdges.Add(edge);
        }

        /// <summary>
        /// Get the specified edge as a GenericEdge object.
        /// </summary>
        /// <param name="index">The edge index.</param>
        /// <returns>The GenericEdge object.</returns>
        public GenericEdge<TData> GetGenericEdge(int index)
        {
            return outEdges[index];
        }
    }
}
