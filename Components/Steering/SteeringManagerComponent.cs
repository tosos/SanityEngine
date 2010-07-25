using UnityEngine;
using System.Collections.Generic;
using SanityEngine.Movement.SteeringBehaviors;

[AddComponentMenu("Sanity Engine/Steering/Steering Manager")]
public class SteeringManagerComponent : MonoBehaviour {
	public Kinematics Kinematics {
		get { return kinematics; }
	}
	
	SteeringManager manager;
	GameObjectActor actor;
	Kinematics kinematics;
	
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
		kinematics = manager.Update(actor, Time.deltaTime);
	}
}
