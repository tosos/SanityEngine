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
    /// <typeparam name="TID">The node ID type.</typeparam>
    public class LPAStarSearch<TNode, TEdge> : PathFinder<TNode, TEdge>
        where TNode : Node<TNode, TEdge>
        where TEdge : Edge<TNode, TEdge>
    {
        /// <summary>
        /// Heuristic callback.
        /// </summary>
        /// <param name="current">The current node.</param>
        /// <param name="goal">The goal node.</param>
        /// <returns>The estimated cost from the current node to the goal
        /// </returns>
        public delegate float Heuristic(TNode current, TNode goal);

        private const float epsilon = 0.00001f;

        private class NodeData : IComparable<NodeData>
        {
            public delegate float CalcHScore(TNode node);

            public readonly TNode Node;
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

            public NodeData(TNode node, CalcHScore calcHScore)
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
        Dictionary<TNode, NodeData> nodeData = new Dictionary<TNode, NodeData>();
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
        public Path<TNode, TEdge> Search(TNode start, TNode goal)
        {
            if (this.start == null)
            {
                this.start = new NodeData(start, CalcHScore);
                this.goal = new NodeData(goal, CalcHScore);
                nodeData[start] = this.start;
                nodeData[goal] = this.goal;
                Initialize();
            }
            else
            {
                TEdge[] changed = start.Graph.GetChangedEdges();
                foreach (TEdge c in changed)
                {
					NodeData src = GetNodeData(c.Source);
					NodeData tgt = GetNodeData(c.Target);
                    UpdateState(src);
                    UpdateState(tgt);
                }
				start.Graph.ResetChanges();
            }

            ComputePlan();
            if (float.IsInfinity(this.start.G))
            {
                return null;
            }

            List<TEdge> steps = new List<TEdge>();

            NodeData node = this.start;
            while (node != this.goal)
            {
                float min = float.PositiveInfinity;
                NodeData nextNode = null;
                TEdge takenEdge = default(TEdge);
                for (int i = 0; i < node.Node.OutEdgeCount; i++)
                {
                    TEdge edge = node.Node.GetOutEdge(i);
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

            return new Path<TNode, TEdge>(start.Graph, steps.ToArray());
        }

        private void Initialize()
        {
            goal.RHS = 0.0f;
            UpdateState(goal);
        }

        private void UpdateState(NodeData node)
        {
            if (node != goal)
            {
                float min = float.PositiveInfinity;
                for (int i = 0; i < node.Node.OutEdgeCount; i++)
                {
                    TEdge edge = node.Node.GetOutEdge(i);
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

        private void ComputePlan()
        {
            while (!queue.Empty && (queue.Peek().CompareTo(start) < 0
                || Math.Abs(start.RHS - start.G) >= epsilon))
            {
                NodeData node = queue.Dequeue();
                node.OnQueue = false;
                if (node.G > node.RHS)
                {
                    node.G = node.RHS;
                    for (int i = 0; i < node.Node.InEdgeCount; i++)
                    {
                        UpdateState(GetNodeData(node.Node.GetInEdge(i).Source));
                    }
                }
                else
                {
                    node.G = float.PositiveInfinity;
                    for (int i = 0; i < node.Node.InEdgeCount; i++)
                    {
                        UpdateState(GetNodeData(node.Node.GetInEdge(i).Source));
                    }
                    UpdateState(node);
                }
            }
        }

        private NodeData GetNodeData(TNode node)
        {
            if (nodeData.ContainsKey(node))
            {
                return nodeData[node];
            }
            NodeData data = new NodeData(node, CalcHScore);
            nodeData[node] = data;
            return data;
        }

        private float CalcHScore(TNode node)
        {
            return heuristic(node, start.Node);
        }
		
        /// <summary>
        /// Get the "G" score of a node (for debugging).
        /// </summary>
        /// <param name="node">The node</param>
        /// <returns>The node's "G" score.</returns>
		public float GetG(TNode node)
		{
			return GetNodeData(node).G;
		}

        /// <summary>
        /// Get the "RHS" score of a node (for debugging).
        /// </summary>
        /// <param name="node">The node</param>
        /// <returns>The node's "RHS" score.</returns>
        public float GetRHS(TNode node)
		{
			return GetNodeData(node).RHS;
		}
    }
}
