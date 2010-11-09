//
// Copyright (C) 2010 The Sanity Engine Development Team
//
// This source code is licensed under the terms of the
// MIT License.
//
// For more information, see the file LICENSE

using UnityEngine;
using System.Collections.Generic;
using System.Text;
using SanityEngine.Search.PathFinding;
using SanityEngine.Structure.Graph;

namespace SanityEngine.Structure.Path
{
    /// <summary>
    /// A path through a graph.
    /// </summary>
    public class Path
    {
        readonly Graph.Graph graph;
        Edge[] steps;
		
        /// <summary>
        /// The graph this path refers to.
        /// </summary>
        public Graph.Graph Graph
        {
            get { return graph; }
        }

        /// <summary>
        /// The number of steps in this path.
        /// </summary>
        public int StepCount
        {
            get { return steps.Length; }
        }

        /// <summary>
        /// Get the node index for the given step.
        /// </summary>
        /// <param name="step">The 0-based step number</param>
        /// <returns>The node index for the step.</returns>
        public Edge GetStep(int step)
        {
            return steps[step];
        }

        /// <summary>
        /// Create a path.
        /// </summary>
        /// <param name="graph">The graph this path is through.</param>
        /// <param name="steps">The edges.</param>
        public Path(Graph.Graph graph, Edge[] steps)
        {
            this.graph = graph;
            this.steps = steps;
        }
		
		public bool TestValidity()
		{
			for(int i = 0; i < StepCount; i ++) {
				if(GetStep(i).Cost == Mathf.Infinity) {
					return false;
				}
			}
			return true;
		}

        /// <summary>
        /// String representation of a path for debugging purposes.
        /// </summary>
        /// <returns>A string representation of the path.</returns>
        public override string ToString()
        {
            string str = "[";
            foreach (Edge step in steps)
            {
                str += " " + step.ToString() + " ";
            }
            return str + "]";
        }
    }
}
