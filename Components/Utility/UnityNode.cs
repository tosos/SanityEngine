using UnityEngine;
using System.Collections;
using SanityEngine.Structure.Graph;
using SanityEngine.Structure.Graph.NavMesh;

/// <summary>
/// Specialized version of a NavMeshNode without generic parameters.
/// </summary>
public interface UnityNode : NavMeshNode<UnityNode, UnityEdge>
{
}
