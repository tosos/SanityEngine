using UnityEngine;
using System.Collections;
using SanityEngine.Structure.Graph;

public class PseudoGridCell : GameObjectNode {
#if false
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnDrawGizmos()
	{
		bool error = true;
		if(transform.parent) {
			PseudoGridGenerator grid = transform.parent.GetComponent<PseudoGridGenerator>();
			if(grid != null) {
				error = false;
				if(!grid.drawGrid) {
					return;
				}
			}
		}
		
		DrawCell(error);
	}
	
	void DrawCell(bool error)
	{
		Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation,
			Vector3.one);
		Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
		Gizmos.DrawCube(Vector3.zero, transform.localScale);
		Gizmos.color = new Color(0.75f, 0.75f, 0.75f, 0.5f);
		Gizmos.DrawWireCube(Vector3.zero, transform.localScale);
		Gizmos.matrix = Matrix4x4.identity;
	}
#endif
}
