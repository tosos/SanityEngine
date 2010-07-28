using UnityEngine;
using System.Collections;
using SanityEngine.Movement.SteeringBehaviors;

[AddComponentMenu("Sanity Engine/Steering/Behaviors/Arrive")]
public class ArriveComponent : SteeringBehaviorComponent {
	public GameObjectActor target;
	public float arriveRadius = 5.0f;
    public float timeToTarget = 5.0f;
	
	Arrive arrive;
	
	public override SteeringBehavior Behavior
	{
			get { return arrive; }
	}
	
	void Awake() {
		arrive = new Arrive();
	}
	
	void Update()
	{
		arrive.Target = target;
		arrive.ArriveRadius = arriveRadius;
		arrive.TimeToTarget = timeToTarget;
	}
}
