using UnityEngine;
using System.Collections.Generic;
using SanityEngine.Movement.SteeringBehaviors;

[AddComponentMenu("")]
public abstract class SteeringManagerComponent : GameObjectActor {
	protected Vector3 Force {
		get { return force; }
	}
	
	protected abstract float MaxForce
	{
		get;
	}
	
	SteeringManager manager;
	GameObjectActor actor;
	Vector3 force;
	bool initialized;
	
	void Start () {
		initialized = false;
	}
	
	void Update () {
		if(!initialized) {
			actor = GetComponent<GameObjectActor>();
			manager = new SteeringManager();
			SteeringBehaviorComponent[] behaviors =
				GetComponents<SteeringBehaviorComponent>();
			foreach(SteeringBehaviorComponent behavior in behaviors) {
				manager.AddBehavior(behavior.Behavior);
			}
			initialized = true;
		}
		manager.MaxForce = MaxForce;
		force = manager.Update(actor, Time.deltaTime);
	}
}
