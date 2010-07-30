using UnityEngine;
using System.Collections;
using SanityEngine.Structure.Graph;

public abstract class GraphComponent : MonoBehaviour {
	public abstract SpatialGraph<TNode, TEdge> GetGraph<TNode, TEdge>()
		where TNode : Node<TNode, TEdge>
		where TEdge : Edge<TNode, TEdge>;
}
