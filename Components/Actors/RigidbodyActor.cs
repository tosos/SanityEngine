using UnityEngine;
using System.Collections;
using SanityEngine.Movement.SteeringBehaviors;

[AddComponentMenu("Sanity Engine/Actors/Rigid Body Actor")]
[RequireComponent(typeof(Rigidbody), typeof(SteeringManagerComponent))]
public class RigidbodyActor : GameObjectActor {
	public float maxSpeed = 1.0f;
	public float maxAngularSpeed = 1.0f;
	public float maxAccel = 2.0f;
	Rigidbody body;
	Transform xform;
	SteeringManagerComponent manager;
	Vector3 lastForce;
	
	void Awake ()
	{
		body = rigidbody;
		xform = transform;
		manager = GetComponent<SteeringManagerComponent>();
	}
	
	void LateUpdate ()
	{
		lastForce = manager.Force;
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

	public override float MaxSpeed
	{
        get { return maxSpeed; }
    }

	public override float MaxAngSpeed
	{
        get { return maxAngularSpeed; }
    }
}
