using UnityEngine;
using System.Collections.Generic;
using SanityEngine.Movement.SteeringBehaviors;

[AddComponentMenu("")]
public abstract class SteeringManagerComponent : GameObjectActor {
	protected Steering Steering {
		get { return steering; }
	}
	
	protected abstract float MaxSpeed
	{
		get;
	}
	
	public SteeringBehaviorAsset[] steeringAssets;
	
	Dictionary<string, SteeringBehaviorProxy> behaviors;
	SteeringManager manager;
	GameObjectActor actor;
	Steering steering;
	
	void Start () {
		actor = GetComponent<GameObjectActor>();
		manager = new SteeringManager();
			
		behaviors = new Dictionary<string, SteeringBehaviorProxy>();
		foreach(SteeringBehaviorAsset asset in steeringAssets) {
			SteeringBehaviorProxy proxy = new SteeringBehaviorProxy(asset);
			behaviors[asset.name] = proxy;
			foreach(SteeringBehavior behavior in proxy.behaviors) {
				manager.AddBehavior(behavior);
			}
		}
	}
	
	void Update () {
		manager.MaxSpeed = MaxSpeed;
		steering = manager.Update(actor, Time.deltaTime);
	}
	
	/// <summary>
	/// Access a steering behavior definition group by name.
	/// <summary>
	public SteeringBehaviorProxy this[string name]
	{
		get
		{
			return behaviors[name];
		}
	}
}
