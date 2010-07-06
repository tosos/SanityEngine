using System;
using System.Collections.Generic;
using AIEngine.Structure.Graph;

namespace AIEngine.PathRepresentation.Grid
{
    /// <summary>
    /// A specialization of graph representing a simple grid.
    /// 
    /// This takes a grid sampler which is used to translate
    /// grid coordinates into node IDs for use in the path finding
    /// algorithms.
    /// </summary>
    public class GridGraph : FiniteGraph<GridNode, GridEdge>
    {
        GridSampler gridSampler;
        GridNode[,] nodes;
        GraphChangeHelper<GridNode, GridEdge> helper = new GraphChangeHelper<GridNode, GridEdge>();

        /// <summary>
        /// Create a grid graph.
        /// </summary>
        /// <param name="gridSampler">The grid sampler to use to construct the
        /// graph.</param>
        public GridGraph(GridSampler gridSampler)
        {
            this.gridSampler = gridSampler;
            int w = gridSampler.Width;
            int h = gridSampler.Height;
            nodes = new GridNode[h, w];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    nodes[y, x] = new GridNode(this, x, y);
                }
            }
            for (int y = 0; y < h; y++)
            {
                int minY = y <= 0 ? 0 : y - 1;
                int maxY = y >= h - 1 ? h - 1 : y + 1;
                for (int x = 0; x < w; x++)
                {
                    int minX = x <= 0 ? 0 : x - 1;
                    int maxX = x >= w - 1 ? w - 1 : x + 1;
                    for (int ey = minY; ey <= maxY; ey++)
                    {
                        for (int ex = minX; ex <= maxX; ex++)
                        {
                            if (ex == x && ey == y)
                            {
                                continue;
                            }
                            float cost = gridSampler.GetEdgeCost(x, y, ex, ey);
                            if (cost != float.PositiveInfinity)
                            {
                                GridEdge edge = new GridEdge(nodes[y, x], nodes[ey, ex], cost);
                                nodes[y, x].AddOutEdge(edge);
                                nodes[ey, ex].AddInEdge(edge);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The number of nodes in the graph.
        /// </summary>
        public int NodeCount
        {
            get { return gridSampler.Width * gridSampler.Height; }
        }

        /// <summary>
        /// Mark the edge between two grid squares as being changed
        /// </summary>
        /// <param name="x1">The x-coordinate of the source square.</param>
        /// <param name="y1">The y-coordinate of the source square.</param>
        /// <param name="x2">The x-coordinate of the destination square.</param>
        /// <param name="y2">The y-coordinate of the destination square.</param>
        public void EdgeChanged(int x1, int y1, int x2, int y2)
        {
            GridNode src = nodes[y1, x1];
            GridNode dest = nodes[y2, x2];
            float cost = gridSampler.GetEdgeCost(x1, y1, x2, y2);
            for (int i = 0; i < src.OutEdgeCount; i++)
            {
                GridEdge edge = src.GetOutEdge(i);
                if (edge.Target == dest)
                {
                    if (cost != float.PositiveInfinity)
                    {
                        ((GridEdge)edge).Cost = cost;
                    }
                    else
                    {
                        src.RemoveOutEdge(i);
                    }
                    helper.MarkChanged(edge);
                    break;
                }
            }
            for (int i = 0; i < src.InEdgeCount; i++)
            {
                GridEdge edge = src.GetInEdge(i);
                if (edge.Target == dest)
                {
                    if (cost != float.PositiveInfinity)
                    {
                        ((GridEdge)edge).Cost = cost;
                    }
                    else
                    {
                        src.RemoveInEdge(i);
                    }
                    helper.MarkChanged(edge);
                    break;
                }
            }
        }

        /// <summary>
        /// <code>true</code> if the graph has changed since the last reset.
        /// </summary>
        public bool HasChanged
        {
            get { return helper.HasChanged; }
        }

        /// <summary>
        /// Get the edge changes since the last reset.
        /// </summary>
        /// <returns>The list of changed edges.</returns>
        public GridEdge[] GetChangedEdges()
        {
            return helper.GetChangedEdges();
        }

        /// <summary>
        /// Reset the change record.
        /// </summary>
        public void ResetChanges()
        {
            helper.Reset();
        }

        public GridNode GetNode(int x, int y)
        {
            return nodes[y, x];
        }
    }
}
