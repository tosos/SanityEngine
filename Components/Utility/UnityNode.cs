using UnityEngine;
using System.Collections;
using SanityEngine.Structure.Graph;
using SanityEngine.Structure.Graph.NavMesh;

public abstract class UnityNode : MonoBehaviour,
	NavMeshNode<UnityNode, UnityEdge>
{
	public UnityEdge[] edges;
	
	UnityGraph graph;

	void Awake () {
		foreach(UnityEdge edge in edges) {
			edge.SetSource(this);
		}
		graph = transform.parent.GetComponent<UnityGraph>();
	}
	
	public Vector3 Position
	{
		get { return transform.position; }
	}
	
    public Graph<UnityNode, UnityEdge> Graph
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

    public UnityEdge GetOutEdge(int index)
    {
    	return edges[index];
    }

    public UnityEdge GetInEdge(int index)
    {
    	return null; // FIXME
    }
}
