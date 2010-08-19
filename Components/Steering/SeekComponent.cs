using UnityEngine;
using System.Collections;
using SanityEngine.Movement.SteeringBehaviors;

[AddComponentMenu("Sanity Engine/Steering/Behaviors/Seek")]
public class SeekComponent : SteeringBehaviorComponent {
	public GameObjectActor target;
	
	Seek seek;	
	
	public override SteeringBehavior Behavior
	{
			get { return seek; }
	}
	
	void Awake() {
		seek = new Seek();
	}
	
	void Update() {
		Behavior.Weight = weight;
		seek.Target = target;
	}
}
