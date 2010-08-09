using UnityEngine;
using System.Collections;
using SanityEngine.Structure.Graph;

public class GridCell : UnityNode {	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnDrawGizmos()
	{
		Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation,
			Vector3.one);
		Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
		Gizmos.DrawCube(Vector3.zero, transform.localScale);
		Gizmos.color = new Color(0.75f, 0.75f, 0.75f, 0.5f);
		Gizmos.DrawWireCube(Vector3.zero, transform.localScale);
	}

	void OnDrawGizmosSelected()
	{
		if(edges == null) {
			return;
		}
		
		Gizmos.color = Color.red;
		foreach(UnityEdge edge in edges) {
			Gizmos.DrawLine(transform.position, edge.target.transform.position);
		}
	}
}
