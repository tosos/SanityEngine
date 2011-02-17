using UnityEngine;
using System.Collections.Generic;
using SanityEngine.Movement.SteeringBehaviors;

[AddComponentMenu("")]
public abstract class SteeringManagerComponent : GameObjectActor {
	protected abstract float MaxForce
	{
		get;
	}
	
	protected abstract float MaxTorque
	{
		get;
	}
	
	public SteeringBehaviorAsset[] steeringAssets;
	
	Dictionary<string, SteeringBehaviorProxy> behaviors;
	SteeringManager manager;
	GameObjectActor actor;
	
	void Awake () {
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
	
	protected void FixedUpdate () {
		manager.MaxForce = MaxForce;
		manager.MaxTorque = MaxTorque;
		SteeringUpdate(manager.Update(actor, Time.fixedDeltaTime));
	}
	
	protected abstract void SteeringUpdate(Steering steering);
	
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
