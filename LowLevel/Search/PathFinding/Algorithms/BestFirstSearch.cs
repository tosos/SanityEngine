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
    public class BestFirstSearch : PathFinder
    {
        private class NodeData : IComparable<NodeData>
        {
            public readonly Node Node;
            public bool OnOpen = false;
            public bool OnClosed = false;
            public float gScore = 0.0f;
            public float fScore = 0.0f;
            public NodeData Previous;
            public Edge TakenEdge;

            public NodeData(Node node)
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
        /// <param name="graph">The graph to search.</para>
        /// <param name="start">The start node.</param>
        /// <param name="goal">The goal node.</param>
        /// <returns>The path or <code>null</code> if no path could be found.</returns>
        public Path Search(Graph graph, Node start, Node goal)
        {
            Dictionary<Node, NodeData> nodeData = new Dictionary<Node, NodeData>();
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
                    Stack<Edge> steps = new Stack<Edge>();
                    while (step != null)
                    {
                        if (step.TakenEdge != null)
                        {
                            steps.Push(step.TakenEdge);
                        }
                        step = step.Previous;
                    }
                    Path path = new Path(graph, steps.ToArray());
                    return path;
                }

                node.OnOpen = false;
                node.OnClosed = true;
				int outCount = graph.GetOutEdgeCount(node.Node);
                for (int i = 0; i < outCount; i++)
                {
                    Edge edge = graph.GetOutEdge(node.Node, i);
					if(edge.Cost == Mathf.Infinity) {
						continue;
					}

                    Node nextNode = edge.Target;

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
        protected virtual float EstimateCost(Node next, Node goal)
        {
            return 0.0f;
        }
    }
}
