using UnityEngine;
using System.Collections.Generic;
using SanityEngine.Movement.SteeringBehaviors;

[AddComponentMenu("Sanity Engine/Steering/Steering Manager")]
public class SteeringManagerComponent : MonoBehaviour {
	public Vector3 Force {
		get { return force; }
	}
	
	SteeringManager manager;
	GameObjectActor actor;
	Vector3 force;
	
	void Awake () {
		manager = new SteeringManager();
		actor = GetComponent<GameObjectActor>();
		
		SteeringBehaviorComponent[] behaviors =
			GetComponents<SteeringBehaviorComponent>();
		foreach(SteeringBehaviorComponent behavior in behaviors) {
			manager.AddBehavior(behavior.Behavior);
		}
	}
	
	void Update () {
		force = manager.Update(actor, Time.deltaTime);
	}
}
