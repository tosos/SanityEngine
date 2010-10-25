using UnityEngine;
using System.Collections;

[AddComponentMenu("Sanity Engine/User Interaction/Nearest Mouse Interactor")]
public class NearestMouseInteractor : MonoBehaviour {
	public enum MouseButton {
		LEFT = 0,
		RIGHT = 1,
		MIDDLE = 2
	}
	
	public LayerMask clickLayerMask = -1;
	public MouseButton moveButton = MouseButton.RIGHT;
	public Grid grid;
	
	void OnGUI()
	{
		if(Event.current.type != EventType.MouseDown) {
			return;
		}
		
		if(Event.current.button != (int)moveButton) {
			return;
		}
		
		Vector3 pos = Event.current.mousePosition;
		pos.y = Screen.height - pos.y;
		Ray ray = Camera.main.ScreenPointToRay(pos);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, Mathf.Infinity, clickLayerMask.value)) {
			UnityNode node = grid.Quantize(hit.point);
			if(node != null) {
				SendMessage("SetGoalNode", node);
			}
		}
		
		Event.current.Use();
	}	
}
