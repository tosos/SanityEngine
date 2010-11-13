using UnityEngine;
using System.Collections;
using SanityEngine.Structure.Graph;
using SanityEngine.Structure.Graph.NavMesh;

/// <summary>
/// Specialized version of a UnityNode that has an associated GameObject.
/// </summary>
public abstract class GameObjectNode : MonoBehaviour, NavMeshNode
{
	public Vector3 Position
	{
		get { return transform.position; }
	}
	
	public bool Equals (Node other)
	{
		GameObjectNode node = other as GameObjectNode;
		return node != null && GetInstanceID() == node.GetInstanceID();
	}
}
