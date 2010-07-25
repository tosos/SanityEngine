using UnityEngine;
using System.Collections;
using SanityEngine.Movement.SteeringBehaviors;
using SanityEngine.Movement.SteeringBehaviors.Flocking;

[AddComponentMenu("Sanity Engine/Steering/Behaviors/Flocking/Cohesion")]
public class SeparationComponent : SteeringBehaviorComponent {
	public FlockComponent flock;
	
	Separation separation;	
	
	public override SteeringBehavior Behavior
	{
			get { return separation; }
	}
	
	void Start() {
		separation = new Separation(flock.Flock);
	}
}
