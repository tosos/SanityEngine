using UnityEngine;
using System.Collections.Generic ;
using SanityEngine.Actors;

public abstract class GameObjectActor : MonoBehaviour, Actor {
    public abstract Vector3 Velocity
    {
        get;
    }

    public abstract Vector3 Position
    {
        get;
    }

    public abstract Vector3 Facing
    {
        get;
    }

    public abstract float MaxSpeed
    {
        get;
    }

    public abstract float MaxAngSpeed
    {
        get;
    }

	Dictionary<string, string> props = new Dictionary<string, string>();
		
    public void SetProperty(string name, string value)
	{
		props[name] = value;
	}

    public void SetBoolProperty(string name)
	{
		props[name] = "t";
	}

    public void ClearBoolProperty(string name)
	{
		props.Remove(name);
	}

    public string GetProperty(string name)
	{
		return props[name];
	}

    public bool HasBoolProperty(string name)
	{
		return props.ContainsKey(name);
	}
}