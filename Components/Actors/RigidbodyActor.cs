using UnityEngine;
using System.Collections;
using SanityEngine.Movement.SteeringBehaviors;

[AddComponentMenu("Sanity Engine/Actors/Rigid Body Actor")]
[RequireComponent(typeof(Rigidbody))]
public class RigidbodyActor : SteeringManagerComponent {
	public float maxForce = 1.0f;
	public float maxAccel = 2.0f;
	Rigidbody body;
	Transform xform;
	Vector3 lastForce;
	
	void Awake ()
	{
		body = rigidbody;
		xform = transform;
	}
	
	void LateUpdate ()
	{
		lastForce = base.Force;
	}
	
	void FixedUpdate ()
	{
		body.AddForce(lastForce);
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

	public override float MaxForce
	{
        get { return maxForce; }
    }
}
