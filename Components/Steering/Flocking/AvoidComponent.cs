using UnityEngine;
using System.Collections;
using SanityEngine.Movement.SteeringBehaviors;
using SanityEngine.Movement.SteeringBehaviors.Flocking;

[AddComponentMenu("Sanity Engine/Steering/Behaviors/Flocking/Avoid")]
public class AvoidComponent : SteeringBehaviorComponent {
	public FlockComponent flock;
	
	Avoid avoid;	
	
	public override SteeringBehavior Behavior
	{
			get { return avoid; }
	}
	
	void Start() {
		avoid = new Avoid(flock.Flock);
	}
}
