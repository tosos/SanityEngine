using UnityEngine;
using System.Collections;
using SanityEngine.Movement.SteeringBehaviors;

[AddComponentMenu("Sanity Engine/Actors/Rigid Body Actor")]
[RequireComponent(typeof(Rigidbody))]
public class RigidbodyActor : SteeringManagerComponent {
	public float maxSpeed = 1.0f;
	Rigidbody body;
	Transform xform;
	Steering lastSteering;
	
	protected override float MaxSpeed
	{
		get { return maxSpeed; }
	}
	
	void Awake ()
	{
		body = rigidbody;
		xform = transform;
	}
	
	void LateUpdate ()
	{
		lastSteering = base.Steering;
	}
	
	void FixedUpdate ()
	{
		body.AddForce(lastSteering.Force);
	}
	
	public override Vector3 Velocity
    {
        get { return body.velocity; }
    }

    public override Vector3 Position
    {
        get { return xform.position; }
    }

    public override Vector3 Facing
    {
        get { return xform.forward; }
    }
}
