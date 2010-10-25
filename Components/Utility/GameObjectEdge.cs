using UnityEngine;
using System.Collections;
using SanityEngine.Structure.Graph;

[System.Serializable]
public class GameObjectEdge : UnityEdge {
	public GameObjectNode source;
	public GameObjectNode target;
	public float cost;
		
	public GameObjectEdge()
	{
	}
		
	public GameObjectEdge(GameObjectNode target, float cost)
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
    
    internal void SetSource(GameObjectNode source)
    {
    	this.source = source;
    }
}
