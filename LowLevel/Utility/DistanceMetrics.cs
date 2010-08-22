using UnityEngine;
using System.Collections;
using SanityEngine.Structure.Graph;
using SanityEngine.Structure.Graph.NavMesh;

namespace SanityEngine.Utility
{
	public class DistanceMetrics {
		
		public static float Manhattan(Vector3 v1, Vector3 v2)
		{
			Vector3 res = v2 - v1;
			return Mathf.Abs(res.x) + Mathf.Abs(res.y) + Mathf.Abs(res.z);
		}
		
		public static float Euclidean(Vector3 v1, Vector3 v2)
		{
			return Vector3.Distance(v1, v2);
		}

		public static float Chebyshev(Vector3 v1, Vector3 v2)
		{
			Vector3 res = v2 - v1;
			return Mathf.Max(Mathf.Abs(res.x), Mathf.Max(Mathf.Abs(res.y),
				Mathf.Abs(res.z)));
		}
	}
}
