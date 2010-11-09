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
using SanityEngine.Search.PathFinding;
using SanityEngine.Structure.Graph;
using SanityEngine.Structure.Path;
using SanityEngine.Utility.Containers;

namespace SanityEngine.Search.PathFinding.Algorithms
{
    /// <summary>
    /// Implementation of the Lifelong Planning A* algorithm.
    /// </summary>
    public class LPAStarSearch : PathFinder
    {
        /// <summary>
        /// Heuristic callback.
        /// </summary>
        /// <param name="current">The current node.</param>
        /// <param name="goal">The goal node.</param>
        /// <returns>The estimated cost from the current node to the goal
        /// </returns>
        public delegate float Heuristic(Node current, Node goal);

        private const float epsilon = 0.00001f;

        private class NodeData : IComparable<NodeData>
        {
            public delegate float CalcHScore(Node node);

            public readonly Node Node;
            public bool OnQueue = false;

            public float RHS
            {
                get { return rhsScore; }
                set
                {
                    rhsScore = value;
                    UpdateK();
                }
            }

            public float G
            {
                get { return gScore; }
                set
                {
                    gScore = value;
                    UpdateK();
                }
            }

            public float K1
            {
                get { return k1; }
            }

            public float K2
            {
                get { return k2; }
            }

            CalcHScore calcHScore;
            float gScore = float.PositiveInfinity;
            float rhsScore = float.PositiveInfinity;
            float k1 = float.PositiveInfinity;
            float k2 = float.PositiveInfinity;

            public NodeData(Node node, CalcHScore calcHScore)
            {
                this.Node = node;
                this.calcHScore = calcHScore;
            }

            public int CompareTo(NodeData rhs)
            {
                if (Math.Abs(k1 - rhs.k1) < epsilon)
                {
                    return k2.CompareTo(rhs.k2);
                }
                return k1.CompareTo(rhs.k1);
            }

            private void UpdateK()
            {
                float r = (float)Math.Min(gScore, rhsScore);
                k1 = r + calcHScore(Node);
                k2 = r;
            }
        }

        Heuristic heuristic;
        PriorityQueue<NodeData> queue = new PriorityQueue<NodeData>();
        Dictionary<Node, NodeData> nodeData = new Dictionary<Node, NodeData>();
        NodeData start;
        NodeData goal;

        /// <summary>
        /// Create an LPA* search with the given heuristic.
        /// </summary>
        /// <param name="heuristic">The heuristic function.</param>
        public LPAStarSearch(Heuristic heuristic)
        {
            this.heuristic = heuristic;
        }

        /// <summary>
        /// Perform a search from the start to the goal.
        /// </summary>
        /// <param name="start">The start node.</param>
        /// <param name="goal">The goal node.</param>
        /// <returns>The path or <code>null</code> if no path could be found.</returns>
        public Path Search(Graph graph, Node start, Node goal)
        {
            if (this.start == null)
            {
                this.start = new NodeData(start, CalcHScore);
                this.goal = new NodeData(goal, CalcHScore);
                nodeData[start] = this.start;
                nodeData[goal] = this.goal;
                Initialize(graph);
            }
            else
            {
                Edge[] changed = graph.GetChangedEdges();
                foreach (Edge c in changed)
                {
					NodeData src = GetNodeData(c.Source);
					NodeData tgt = GetNodeData(c.Target);
                    UpdateState(graph, src);
                    UpdateState(graph, tgt);
                }
				graph.ResetChanges();
            }

            ComputePlan(graph);
            if (float.IsInfinity(this.start.G))
            {
                return null;
            }

            List<Edge> steps = new List<Edge>();

            NodeData node = this.start;
            while (node != this.goal)
            {
                float min = float.PositiveInfinity;
                NodeData nextNode = null;
                Edge takenEdge = default(Edge);
				int outCount = graph.GetOutEdgeCount(node.Node);
                for (int i = 0; i < outCount; i++)
                {
                    Edge edge = graph.GetOutEdge(node.Node, i);
                    NodeData testNode = nodeData[edge.Target];
                    float cost = edge.Cost + testNode.G;
                    if (cost < min)
                    {
                        min = cost;
                        nextNode = testNode;
                        takenEdge = edge;
                    }
                }

                node = nextNode;
                steps.Add(takenEdge);
            }

            return new Path(graph, steps.ToArray());
        }

        private void Initialize(Graph graph)
        {
            goal.RHS = 0.0f;
            UpdateState(graph, goal);
        }

        private void UpdateState(Graph graph, NodeData node)
        {
            if (node != goal)
            {
                float min = float.PositiveInfinity;
				int outCount = graph.GetOutEdgeCount(node.Node);
                for (int i = 0; i < outCount; i++)
                {
                    Edge edge = graph.GetOutEdge(node.Node, i);
                    NodeData target = GetNodeData(edge.Target);
                    float total = edge.Cost + target.G;
                    if (total < min)
                    {
                        min = total;
                    }
                }
                node.RHS = min;
            }
			
            // Used to compare two floats for near-equality.
            bool close = node.G == node.RHS || Math.Abs(node.G - node.RHS) < epsilon;
            if (node.OnQueue && !close)
            {
                queue.Fix(node);
            }
            else if (node.OnQueue && close)
            {
                queue.Remove(node);
                node.OnQueue = false;
            }
            else if (!node.OnQueue && !close)
            {
                queue.Enqueue(node);
                node.OnQueue = true;
            }
        }

        private void ComputePlan(Graph graph)
        {
            while (!queue.Empty && (queue.Peek().CompareTo(start) < 0
                || Math.Abs(start.RHS - start.G) >= epsilon))
            {
                NodeData node = queue.Dequeue();
                node.OnQueue = false;
                if (node.G > node.RHS)
                {
                    node.G = node.RHS;
					int inCount = graph.GetInEdgeCount(node.Node);
                    for (int i = 0; i < inCount; i++)
                    {
                        UpdateState(graph, GetNodeData(graph.GetInEdge(node.Node, i).Source));
                    }
                }
                else
                {
                    node.G = float.PositiveInfinity;
					int inCount = graph.GetInEdgeCount(node.Node);
                    for (int i = 0; i < inCount; i++)
                    {
                        UpdateState(graph, GetNodeData(graph.GetInEdge(node.Node, i).Source));
                    }
                    UpdateState(graph, node);
                }
            }
        }

        private NodeData GetNodeData(Node node)
        {
            if (nodeData.ContainsKey(node))
            {
                return nodeData[node];
            }
            NodeData data = new NodeData(node, CalcHScore);
            nodeData[node] = data;
            return data;
        }

        private float CalcHScore(Node node)
        {
            return heuristic(node, start.Node);
        }
		
        /// <summary>
        /// Get the "G" score of a node (for debugging).
        /// </summary>
        /// <param name="node">The node</param>
        /// <returns>The node's "G" score.</returns>
		public float GetG(Node node)
		{
			return GetNodeData(node).G;
		}

        /// <summary>
        /// Get the "RHS" score of a node (for debugging).
        /// </summary>
        /// <param name="node">The node</param>
        /// <returns>The node's "RHS" score.</returns>
        public float GetRHS(Node node)
		{
			return GetNodeData(node).RHS;
		}
    }
}
