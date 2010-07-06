using System;
using System.Collections.Generic;
using AIEngine.Structure.Graph;

namespace AIEngine.PathRepresentation.Grid
{
    public class GridEdge : Edge<GridNode, GridEdge>
    {
        public GridNode Source
        {
            get { return source; }
        }

        public GridNode Target
        {
            get { return target; }
        }

        public float Cost
        {
            get { return cost; }
            set { cost = value; }
        }

        GridNode source;
        GridNode target;
        float cost;

        public GridEdge(GridNode source, GridNode target, float cost)
        {
            this.source = source;
            this.target = target;
            this.cost = cost;
        }
    }
}
