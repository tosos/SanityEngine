using UnityEngine;
using System.Collections;
using SanityEngine.Structure.Graph;

public class GridCell : MonoBehaviour, Node<GridCell, GridCell.Edge> {
	[System.Serializable]
	public class Edge : Edge<GridCell, GridCell.Edge> {
		public GridCell target;
		public float cost;
		
		public Edge()
		{
		}
		
		public Edge(GridCell target, float cost)
		{
			this.target = target;
			this.cost = cost;
		}

        public GridCell Source
        {
            get { return null; /* FIXME */ }
        }

        public GridCell Target
        {
            get { return target; }
        }

        public float Cost
        {
            get { return cost; }
        }
	}

	public Edge[] edges;
	GridGenerator graph;
	
	void Awake () {
		graph = transform.parent.GetComponent<GridGenerator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnDrawGizmos()
	{
		Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
		Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
		Gizmos.DrawCube(Vector3.zero, transform.localScale);
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(Vector3.zero, transform.localScale);
	}

	void OnDrawGizmosSelected()
	{
		if(edges == null) {
			return;
		}
		
		Gizmos.color = Color.red;
		foreach(Edge edge in edges) {
			Gizmos.DrawLine(transform.position, edge.target.transform.position);
		}
	}
	
    public Graph<GridCell, GridCell.Edge> Graph
    {
        get { return graph; }
    }

    public int OutEdgeCount
    {
        get { return edges.Length; }
    }

    public int InEdgeCount
    {
        get { return 0; /* FIXME */ }
    }

    public Edge GetOutEdge(int index)
    {
    	return edges[index];
    }

    public Edge GetInEdge(int index)
    {
    	return null; // FIXME
    }
}
