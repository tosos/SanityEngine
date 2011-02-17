using UnityEngine;
using System.Collections;
using SanityEngine.Movement.SteeringBehaviors;

[AddComponentMenu("Sanity Engine/Steering Motors/Rigid Body Motor")]
[RequireComponent(typeof(Rigidbody), typeof(RigidbodyActor))]
public class RigidbodyMotor : SteeringManagerComponent {
	public float maxForce = 2.5f;
	public float maxSpeed = 5.0f;
	public float maxTorque = 720.0f;
	public float maxAngularSpeed = 1080.0f;
	Rigidbody body;
	Transform xform;
	Vector3 kinematicAngularVelocity;
	
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
		kinematicAngularVelocity = Vector3.zero;
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
		
		if(body.isKinematic) {
			float t = Time.fixedDeltaTime;
			Vector3 angAccel = steering.Torque / body.mass;
	
			xform.Rotate(kinematicAngularVelocity * t + angAccel * t * t, Space.World);

			kinematicAngularVelocity += angAccel * t;
			kinematicAngularVelocity -= kinematicAngularVelocity * rigidbody.angularDrag;
			float angSpeed = kinematicAngularVelocity.magnitude;
			if(angSpeed > maxAngularSpeed) {
				kinematicAngularVelocity *= maxAngularSpeed / angSpeed;
			}
		} else {
			body.AddTorque(desired);
		}

		/* TODO angular speed clamp?
		angularVelocity += angAccel * t;
		angularVelocity -= angularVelocity * angularDamp;
		float angSpeed = angularVelocity.magnitude;
		if(angSpeed > maxAngularSpeed) {
			angularVelocity *= maxAngularSpeed / angSpeed;
		}
		*/
	}
}
