using System;
using System.Collections.Generic;
using SanityEngine.Structure.Graph;

namespace SanityEngine.LevelRepresentation.Grid
{
    public class GridNode : Node<GridNode, GridEdge>
    {
        public int X
        {
            get { return x; }
        }

        public int Y
        {
            get { return y; }
        }

        public Graph<GridNode, GridEdge> Graph
        {
            get { return graph; }
        }

        public int OutEdgeCount
        {
            get { return outEdges.Count; }
        }

        public int InEdgeCount
        {
            get { return inEdges.Count; }
        }

        public GridEdge GetOutEdge(int index)
        {
            return outEdges[index];
        }

        public GridEdge GetInEdge(int index)
        {
            return inEdges[index];
        }

        GridGraph graph;
        int x;
        int y;
        List<GridEdge> outEdges = new List<GridEdge>();
        List<GridEdge> inEdges = new List<GridEdge>();

        public GridNode(GridGraph graph, int x, int y)
        {
            this.graph = graph;
            this.x = x;
            this.y = y;
        }

        public void AddOutEdge(GridEdge edge)
        {
            outEdges.Add(edge);
        }

        public void AddInEdge(GridEdge edge)
        {
            inEdges.Add(edge);
        }

        public void RemoveOutEdge(int index)
        {
            outEdges.RemoveAt(index);
        }

        public void RemoveInEdge(int index)
        {
            inEdges.RemoveAt(index);
        }
    }
}
