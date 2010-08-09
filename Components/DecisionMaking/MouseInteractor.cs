using UnityEngine;
using System.Collections;

[AddComponentMenu("Sanity Engine/User Interaction/Mouse Interactor")]
public class MouseInteractor : DecisionMaker {
	public LayerMask clickLayerMask = -1;
	UnityNode moveTarget;
	
	void OnGUI()
	{
		if(Event.current.type != EventType.MouseDown) {
			return;
		}
		
		if(Event.current.button != 0) {
			return;
		}
		
		Vector3 pos = Event.current.mousePosition;
		pos.y = Screen.height - pos.y;
		Ray ray = Camera.main.ScreenPointToRay(pos);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, Mathf.Infinity, -1))
		{
			// TODO iterate through all, searching for a UnityNode
			moveTarget = hit.transform.GetComponent<UnityNode>();
		}
		
		Event.current.Use();
	}	
	
	public override UnityNode GetMovementTarget()
	{
		return moveTarget;
	}
}
