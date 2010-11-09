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
    /// An edge object.
    /// </summary>
	public interface Edge
    {
        /// <summary>
        /// The source node.
        /// </summary>
        Node Source
        {
            get;
        }

        /// <summary>
        /// The target node.
        /// </summary>
        Node Target
        {
            get;
        }

        /// <summary>
        /// The cost of this edge.
        /// </summary>
        float Cost
        {
            get;
        }
    }
}
