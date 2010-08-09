using UnityEngine;
using System.Collections;
using SanityEngine.Utility;
using SanityEngine.Utility.Heuristics;

public class EuclideanHeuristic : Heuristic {
	public float Heuristic(UnityNode n1, UnityNode n2)
	{
		return DistanceMetrics.Euclidean(n1.Position, n2.Position);
	}
}
