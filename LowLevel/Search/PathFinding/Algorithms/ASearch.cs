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
using SanityEngine.Structure.Path;

namespace SanityEngine.Search.PathFinding.Algorithms
{
    /// <summary>
    /// Implementation of the "A" search algorithm. Note that to make
    /// this an A* algorithm, the heuristic MUST give a value LESS than
    /// the "perfect" information (i.e. less than the actual cost from the
    /// given node to the goal)
    /// </summary>
    /// <typeparam name="TID">The node ID type.</typeparam>
    public class ASearch<TNode, TEdge> : BestFirstSearch<TNode, TEdge>
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

        private readonly Heuristic heuristic;

        /// <summary>
        /// Creates a best first search with the given heuristic.
        /// </summary>
        /// <param name="heuristic"></param>
        public ASearch(Heuristic heuristic)
        {
            this.heuristic = heuristic;
        }

        /// <summary>
        /// Estimates the cost by taking the cost of the node plus the
        /// heuristic estimate.
        /// </summary>
        /// <param name="next">The next node.</param>
        /// <param name="goal">The goal node.</param>
        /// <returns>The estimated cost.</returns>
        protected override float EstimateCost(TNode next, TNode goal)
        {
            return heuristic(next, goal);
        }
    }
}
