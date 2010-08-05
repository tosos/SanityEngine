using UnityEngine;
using System.Collections;
using SanityEngine.Structure.Graph;

[System.Serializable]
public class UnityEdge : Edge<UnityNode, UnityEdge> {
	public UnityNode source;
	public UnityNode target;
	public float cost;
		
	public UnityEdge()
	{
	}
		
	public UnityEdge(GridCell target, float cost)
	{
		this.target = target;
		this.cost = cost;
	}

    public UnityNode Source
    {
        get { return source; }
    }

    public UnityNode Target
    {
        get { return target; }
    }

    public float Cost
    {
        get { return cost; }
    }
    
    internal void SetSource(UnityNode source)
    {
    	this.source = source;
    }
}
