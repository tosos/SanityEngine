using UnityEngine;
using System.Collections;
using SanityEngine.Movement.SteeringBehaviors;

[AddComponentMenu("Sanity Engine/Steering/Behaviors/Evade")]
public class EvadeComponent : SteeringBehaviorComponent {
	public GameObjectActor target;
	
	Evade evade;	
	
	public override SteeringBehavior Behavior
	{
			get { return evade; }
	}
	
	void Awake() {
		evade = new Evade();
	}
	
	void Update()
	{
		Behavior.Weight = weight;
		evade.Target = target;
	}
}
