using UnityEngine;
using System.Collections;
using SanityEngine.Movement.SteeringBehaviors;
using SanityEngine.Movement.SteeringBehaviors.Flocking;

[AddComponentMenu("Sanity Engine/Steering/Behaviors/Flocking/Alignment")]
public class AlignmentComponent : SteeringBehaviorComponent {
	public FlockComponent flock;
	
	Alignment alignment;	
	
	public override SteeringBehavior Behavior
	{
			get { return alignment; }
	}
	
	void Start() {
		alignment = new Alignment(flock.Flock);
	}
}
