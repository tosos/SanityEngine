using UnityEngine;
using System.Collections;
using SanityEngine.Structure.Graph;
using SanityEngine.Structure.Graph.NavMesh;

namespace SanityEngine.Utility.Heuristics
{
	public interface Heuristic {
		float Heuristic(UnityNode n1, UnityNode n2);
	}
}
