using UnityEngine;
using System.Collections;
using SanityEngine.Structure.Graph;
using SanityEngine.Structure.Graph.NavMesh;

/// <summary>
/// Specialized version of a NavMeshNode that has a handle back to a
/// UnityGraph.
/// </summary>
public interface UnityNode : NavMeshNode<UnityNode, UnityEdge>
{
	UnityGraph NavMesh
	{
		get;
	}
}
