using UnityEngine;
using System.Collections;
using SanityEngine.Structure.Graph;

[System.Serializable]
public class GameObjectEdge : Edge {
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

    public Node Source
    {
        get { return source; }
    }

    public Node Target
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
