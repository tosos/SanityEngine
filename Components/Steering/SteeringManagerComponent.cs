using UnityEngine;
using System.Collections.Generic;
using SanityEngine.Movement.SteeringBehaviors;

[AddComponentMenu("")]
public abstract class SteeringManagerComponent : MonoBehaviour {
	protected abstract float MaxForce
	{
		get;
	}
	
	protected abstract float MaxTorque
	{
		get;
	}
	
	public SteeringBehaviorAsset[] steeringAssets;
	public bool isPlanar = false;
	
	Dictionary<string, SteeringBehaviorProxy> behaviors;
	SteeringManager manager;
	GameObjectActor actor;
	
	void Awake () {
		actor = GetComponent<GameObjectActor>();
		if(!actor) {
			Debug.LogError("Steering manager is not attached to an actor!");
		}
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
		manager.IsPlanar = isPlanar;
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
