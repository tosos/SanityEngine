using UnityEngine;
using System.Collections;
using SanityEngine.Structure.Graph;
using SanityEngine.Structure.Graph.NavMesh;

public abstract class UnityGraph : MonoBehaviour,
	NavMesh
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
    public abstract bool HasChanged
    {
        get;
    }

    public abstract Edge[] GetChangedEdges();

    public abstract void ResetChanges();
	
    public abstract int GetOutEdgeCount(Node node);

    public abstract int GetInEdgeCount(Node node);

    public abstract Edge GetOutEdge(Node node, int index);

    public abstract Edge GetInEdge(Node node, int index);
	
   	public abstract NavMeshNode Quantize(Vector3 pos);
}
