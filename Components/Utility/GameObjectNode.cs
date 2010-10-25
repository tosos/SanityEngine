using UnityEngine;
using System.Collections;
using SanityEngine.Structure.Graph;
using SanityEngine.Structure.Graph.NavMesh;

/// <summary>
/// Specialized version of a UnityNode that has an associated GameObject.
/// </summary>
public abstract class GameObjectNode : MonoBehaviour, UnityNode
{
	public GameObjectEdge[] edges;
	
	public NavMesh<UnityNode, UnityEdge> NavMesh
	{
		get { return graph; }
	}
	
	UnityGraph graph;

	void Awake () {
		foreach(GameObjectEdge edge in edges) {
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
