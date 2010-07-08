using System;
using System.Collections.Generic;
using System.Text;

namespace SanityEngine.LevelRepresentation.Grid
{
    /// <summary>
    /// Simple heightmap grid builder. Gives all nodes a cost of 1, and gives
    /// edges the cost equal to 1.0 + the difference between the height of 
    /// grid tiles. If the magnitude if the difference is greater than
    /// maxSlope, no edge is created.
    /// </summary>
    public class HeightmapGridSampler : GridSampler
    {
        /// <summary>
        /// Interface for generic heightmaps.
        /// </summary>
        public interface Heightmap
        {
            /// <summary>
            /// The width of the heightmap.
            /// </summary>
            int Width
            {
                get;
            }

            /// <summary>
            /// The height of the heightmap.
            /// </summary>
            int Height
            {
                get;
            }

            /// <summary>
            /// Get the height at the given point on the heightmap.
            /// </summary>
            /// <param name="x">The x-coordinate.</param>
            /// <param name="y">The y-coordinate.</param>
            /// <returns>The height value.</returns>
            float GetHeight(int x, int y);
        }

        Heightmap heightmap;
        float maxSlope;

        /// <summary>
        /// The width of the grid.
        /// </summary>
        public int Width
        {
            get { return heightmap.Width; }
        }

        /// <summary>
        /// The height of the grid.
        /// </summary>
        public int Height
        {
            get { return heightmap.Height; }
        }

        /// <summary>
        /// Create a heightmap sampler.
        /// </summary>
        /// <param name="heightmap">The heightmap to sample.</param>
        /// <param name="maxSlope">The maximum slope for which to create an
        /// edge.</param>
        public HeightmapGridSampler(Heightmap heightmap, float maxSlope)
        {
            this.heightmap = heightmap;
            this.maxSlope = maxSlope;
        }

        /// <summary>
        /// Get the cost of an edge from one tile to another. Note: Should
        /// never be less than 0, and PositiveInfinity means there is no edge.
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public float GetEdgeCost(int x1, int y1, int x2, int y2)
        {
            float h1 = heightmap.GetHeight(x1, y1);
            float h2 = heightmap.GetHeight(x2, y2);
			
			if(h1 == float.PositiveInfinity || h2 == float.PositiveInfinity) {
				return float.PositiveInfinity;
			}
			
            float slope = h2 - h1;
            if (Math.Abs(slope) >= maxSlope)
            {
                return float.PositiveInfinity;
            }
            if (slope <= 0.0f)
            {
                return 1.0f;
            }
            return 1.0f + slope;
        }
    }
}
