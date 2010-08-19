using UnityEngine;
using System.Collections;
using SanityEngine.Movement.SteeringBehaviors;
using SanityEngine.Movement.SteeringBehaviors.Flocking;

[AddComponentMenu("Sanity Engine/Steering/Behaviors/Flocking/Separation")]
public class SeparationComponent : SteeringBehaviorComponent {
	public FlockComponent flock;
	public float maxDistance = 5.0f;
	
	Separation separation;	
	
	public override SteeringBehavior Behavior
	{
		get { return separation; }
	}
	
	void Start() {
		separation = new Separation(flock.Flock);
	}
	
	void Update() {
		separation.MaxDistance = maxDistance;
	}
}
