//
// Copyright (C) 2010 The Sanity Engine Development Team
//
// This source code is licensed under the terms of the
// MIT License.
//
// For more information, see the file LICENSE

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using SanityEngine.Structure.Graph;
using SanityEngine.Structure.Path;
using SanityEngine.Utility.Containers;

namespace SanityEngine.Search.PathFinding.Algorithms
{
    /// <summary>
    /// A base class for best-first search algorithms.
    /// </summary>
    /// <typeparam name="TID">The node ID type.</typeparam>
    public class BestFirstSearch<TNode, TEdge> : PathFinder<TNode, TEdge>
        where TNode : Node<TNode, TEdge>
        where TEdge : Edge<TNode, TEdge>
    {
        private class NodeData : IComparable<NodeData>
        {
            public readonly TNode Node;
            public bool OnOpen = false;
            public bool OnClosed = false;
            public float gScore = 0.0f;
            public float fScore = 0.0f;
            public NodeData Previous;
            public TEdge TakenEdge;

            public NodeData(TNode node)
            {
                this.Node = node;
            }

            public int CompareTo(NodeData rhs)
            {
                return fScore.CompareTo(rhs.fScore);
            }
        }

        /// <summary>
        /// Perform a search from the start to the goal.
        /// </summary>
        /// <param name="start">The start node.</param>
        /// <param name="goal">The goal node.</param>
        /// <returns>The path or <code>null</code> if no path could be found.</returns>
        public Path<TNode, TEdge> Search(TNode start, TNode goal)
        {
            Dictionary<TNode, NodeData> nodeData = new Dictionary<TNode, NodeData>();
            PriorityQueue<NodeData> openList = new PriorityQueue<NodeData>();

            openList.Clear();
            nodeData.Clear();

            NodeData startData = new NodeData(start);
            nodeData[start] = startData;
            openList.Enqueue(startData);

            while (!openList.Empty)
            {
                NodeData node = openList.Dequeue();
                if (node.Node.Equals(goal))
                {
                    NodeData step = node;
                    Stack<TEdge> steps = new Stack<TEdge>();
                    while (step != null)
                    {
                        if (step.TakenEdge != null)
                        {
                            steps.Push(step.TakenEdge);
                        }
                        step = step.Previous;
                    }
                    Path<TNode, TEdge> path = new Path<TNode, TEdge>(goal.Graph, steps.ToArray());
                    return path;
                }

                node.OnOpen = false;
                node.OnClosed = true;
                for (int i = 0; i < node.Node.OutEdgeCount; i++)
                {
                    TEdge edge = node.Node.GetOutEdge(i);
					if(edge.Cost == Mathf.Infinity) {
						continue;
					}

                    TNode nextNode = edge.Target;

                    NodeData next = null;
                    if (nodeData.ContainsKey(nextNode))
                    {
                        next = nodeData[nextNode];
                    }
                    else
                    {
                        next = new NodeData(nextNode);
                        nodeData[nextNode] = next;
                    }

                    if (next.OnClosed)
                    {
                        continue;
                    }

                    float gScore = node.gScore + edge.Cost;

                    if (!next.OnOpen)
                    {
                        next.OnOpen = true;
                        next.Previous = node;
                        next.TakenEdge = edge;
                        next.gScore = gScore;
                        next.fScore = next.gScore + EstimateCost(nextNode, goal);
                        openList.Enqueue(next);
                    }
                    else if (gScore < next.gScore)
                    {
                        next.Previous = node;
                        next.TakenEdge = edge;
                        next.gScore = gScore;
                        next.fScore = next.gScore + EstimateCost(nextNode, goal);
                        openList.Fix(next);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Calculate the estimated cost to the next node.
        /// </summary>
        /// <param name="next">The next node.</param>
        /// <param name="goal">The goal node.</param>
        /// <returns>The estimated cost.</returns>
        protected virtual float EstimateCost(TNode next, TNode goal)
        {
            return 0.0f;
        }
    }
}
