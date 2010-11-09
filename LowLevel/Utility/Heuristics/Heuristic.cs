using UnityEngine;
using System.Collections;
using SanityEngine.Structure.Graph;
using SanityEngine.Structure.Graph.NavMesh;

namespace SanityEngine.Utility.Heuristics
{
	public interface Heuristic {
		float Heuristic(Node n1, Node n2);
	}
}
