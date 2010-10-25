using UnityEngine;
using System.Collections;
using SanityEngine.Structure.Graph;

/// <summary>
/// Specialized version of Edge without generic parameters.
/// </summary>
public interface UnityEdge : Edge<UnityNode, UnityEdge> {
}
