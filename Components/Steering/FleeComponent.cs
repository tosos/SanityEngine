using UnityEngine;
using System.Collections;
using SanityEngine.Movement.SteeringBehaviors;

[AddComponentMenu("Sanity Engine/Steering/Behaviors/Flee")]
public class FleeComponent : SteeringBehaviorComponent {
	public GameObjectActor target;
	
	Flee flee;	
	
	public override SteeringBehavior Behavior
	{
			get { return flee; }
	}
	
	void Awake() {
		flee = new Flee();
	}
	
	void Update()
	{
		Behavior.Weight = weight;
		flee.Target = target;
	}
}
