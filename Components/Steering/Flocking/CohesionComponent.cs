using UnityEngine;
using System.Collections;
using SanityEngine.Movement.SteeringBehaviors;
using SanityEngine.Movement.SteeringBehaviors.Flocking;

[AddComponentMenu("Sanity Engine/Steering/Behaviors/Flocking/Cohesion")]
public class CohesionComponent : SteeringBehaviorComponent {
	public FlockComponent flock;
	
	Cohesion cohesion;	
	
	public override SteeringBehavior Behavior
	{
			get { return cohesion; }
	}
	
	void Start() {
		cohesion = new Cohesion(flock.Flock);
	}
}
