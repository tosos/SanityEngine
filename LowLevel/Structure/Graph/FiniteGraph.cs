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

namespace SanityEngine.Structure.Graph
{
    /// <summary>
    /// Specialized version of the graph interface for finite graphs.
    /// </summary>
    public interface FiniteGraph : Graph
    {
        /// <summary>
        /// Get the number of nodes.
        /// </summary>
        int NodeCount
        {
            get;
        }
		
		Node GetNode(int node);
    }
}
