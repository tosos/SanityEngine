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
	
	protected override float MaxForce
	{
		get { return maxForce; }
	}
	
	protected override float MaxTorque
	{
		get { return maxTorque; }
	}
	
	void Start ()
	{
		body = rigidbody;
		xform = transform;
	}
	
	protected override void SteeringUpdate (Steering steering)
	{
		Vector3 desired = steering.Force;
		
		body.AddForce(desired);

		/* TODO speed clamp?
		velocity += accel * t;
		velocity -= velocity * linearDamp;
		float speed = velocity.magnitude;
		if(speed > maxSpeed) {
			velocity *= maxSpeed / speed;
		}
		*/
		
		desired = steering.Torque * Mathf.Deg2Rad;

		body.AddTorque(desired);

		/* TODO angular speed clamp?
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
