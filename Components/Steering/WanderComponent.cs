using UnityEngine;
using System.Collections;
using SanityEngine.Movement.SteeringBehaviors;

[AddComponentMenu("Sanity Engine/Steering/Behaviors/Wander")]
public class WanderComponent : SteeringBehaviorComponent {
	public float minCountdownTime = 1.0f;
	public float maxCountdownTime = 2.0f;
    public float maxDeviation = 0.2f;
	Wander wander;	
	
	public override SteeringBehavior Behavior
	{
			get { return wander; }
	}
	
	void Awake() {
		wander = new Wander();
	}
	
	void Update()
	{
		wander.MinCountdownTime = minCountdownTime;
		wander.MaxCountdownTime = maxCountdownTime;
		wander.MaxDeviation = maxDeviation;
	}
}
