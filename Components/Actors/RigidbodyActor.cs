using UnityEngine;
using System.Collections;
using SanityEngine.Movement.SteeringBehaviors;

[AddComponentMenu("Sanity Engine/Actors/Rigid Body Actor")]
[RequireComponent(typeof(Rigidbody))]
public class RigidbodyActor : SteeringManagerComponent {
	public float maxForce = 2.5f;
	public float maxSpeed = 5.0f;
	public float maxTorque = 720.0f;
	public float maxAngularSpeed = 1080.0f;
	Rigidbody body;
	Transform xform;
	
	protected override float MaxSpeed
	{
		get { return maxSpeed; }
	}
	
	protected override float MaxAngularSpeed
	{
		get { return maxAngularSpeed * Mathf.Deg2Rad; }
	}
	
	void Start ()
	{
		body = rigidbody;
		xform = transform;
	}
	
	void FixedUpdate ()
	{
		Steering steering = base.Steering;
		
		Vector3 desired = steering.Force;
		float force = desired.magnitude;
		if(force > maxForce) {
			desired *= maxForce / force;
		}
		
		body.AddForce(desired);

		/* TODO speed clamp
		velocity += accel * t;
		velocity -= velocity * linearDamp;
		float speed = velocity.magnitude;
		if(speed > maxSpeed) {
			velocity *= maxSpeed / speed;
		}
		*/
		
		desired = steering.Torque;
		float torque2 = desired.magnitude;
		float max = maxTorque * Mathf.Deg2Rad;
		if(torque2 > max) {
			desired *= max / torque2;
		}

		body.AddTorque(desired);

		/* TODO angular speed clamp
		angularVelocity += angAccel * t;
		angularVelocity -= angularVelocity * angularDamp;
		float angSpeed = angularVelocity.magnitude;
		if(angSpeed > maxAngularSpeed) {
			angularVelocity *= maxAngularSpeed / angSpeed;
		}
		*/
	}
	
	public override Vector3 Velocity
    {
        get { return body.velocity; }
    }

	public override Vector3 AngularVelocity
    {
        get { return body.angularVelocity; }
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
