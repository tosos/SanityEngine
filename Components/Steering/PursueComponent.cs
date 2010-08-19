using UnityEngine;
using System.Collections;
using SanityEngine.Movement.SteeringBehaviors;

[AddComponentMenu("Sanity Engine/Steering/Behaviors/Pursue")]
public class PursueComponent : SteeringBehaviorComponent {
	public GameObjectActor target;
	
	Pursue pursue;	
	
	public override SteeringBehavior Behavior
	{
			get { return pursue; }
	}
	
	void Awake() {
		pursue = new Pursue();
	}
	
	void Update()
	{
		Behavior.Weight = weight;
		pursue.Target = target;
	}
}
