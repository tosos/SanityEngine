using UnityEngine;
using System.Collections;
using SanityEngine.Structure.Graph;
using SanityEngine.Structure.Graph.Spatial;

namespace SanityEngine.Utility
{
	public class Heuristics {
		
		public static float Manhattan<TNode, TEdge>(TNode n1, TNode n2)
			where TNode : SpatialNode<TNode, TEdge>
			where TEdge : Edge<TNode, TEdge>
		{
			return DistanceMetrics.Manhattan(n1.Position, n2.Position);
		}
		
		public static float Euclidean<TNode, TEdge>(TNode n1, TNode n2)
			where TNode : SpatialNode<TNode, TEdge>
			where TEdge : Edge<TNode, TEdge>
		{
			return DistanceMetrics.Euclidean(n1.Position, n2.Position);
		}

		public static float Chebyshev<TNode, TEdge>(TNode n1, TNode n2)
			where TNode : SpatialNode<TNode, TEdge>
			where TEdge : Edge<TNode, TEdge>
		{
			return DistanceMetrics.Chebyshev(n1.Position, n2.Position);
		}
	}
}
