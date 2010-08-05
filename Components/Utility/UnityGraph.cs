using UnityEngine;
using System.Collections;
using SanityEngine.Structure.Graph;
using SanityEngine.Structure.Graph.Spatial;

public abstract class UnityGraph : MonoBehaviour,
	SpatialGraph<UnityNode, UnityEdge>
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

    public abstract UnityEdge[] GetChangedEdges();

    public abstract void ResetChanges();
	
   	public abstract UnityNode Quantize(Vector3 pos);
}
