using UnityEngine;
using System.Collections;
using SanityEngine.Movement.SteeringBehaviors;

[RequireComponent(typeof(SteeringManagerComponent))]
public abstract class SteeringBehaviorComponent : MonoBehaviour {
	public string behaviorName;
	public float weight = 1.0f;
	
	public abstract SteeringBehavior Behavior
	{
			get;
	}
}
