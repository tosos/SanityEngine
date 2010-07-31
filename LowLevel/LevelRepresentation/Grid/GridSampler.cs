//
// Copyright (C) 2010 The Sanity Engine Development Team
//
// This source code is licensed under the terms of the
// MIT License.
//
// For more information, see the file LICENSE

using UnityEngine;
using System;

namespace SanityEngine.LevelRepresentation.Grid
{
    /// <summary>
    /// Interface got getting information about a grid.
    /// </summary>
    public interface GridSampler
    {
        /// <summary>
        /// The width of the grid.
        /// </summary>
        int Width
        {
            get;
        }

        /// <summary>
        /// The height of the grid.
        /// </summary>
        int Height
        {
            get;
        }
		
        /// <summary>
        /// Get the node's position in world space.
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <returns></returns>
		Vector3 GetNodePosition(int x, int y);

        /// <summary>
        /// Get the cost of an edge from one tile to another. Note: Should
        /// never be less than 0, and PositiveInfinity means there is no edge.
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns>The edge cost.</returns>
        float GetEdgeCost(int x1, int y1, int x2, int y2);
    }
}
