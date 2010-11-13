using UnityEngine;
using System.Collections;
using SanityEngine.Utility;
using SanityEngine.Utility.Heuristics;
using SanityEngine.Structure.Graph;
using SanityEngine.Structure.Graph.NavMesh;

public class EuclideanHeuristic : Heuristic {
	public float Heuristic(Node n1, Node n2)
	{
		NavMeshNode node1 = (NavMeshNode)n1;
		NavMeshNode node2 = (NavMeshNode)n2;
		return DistanceMetrics.Euclidean(node1.Position, node2.Position);
	}
}
