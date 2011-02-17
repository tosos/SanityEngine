using UnityEngine;
using System.Collections;
using SanityEngine.Movement.SteeringBehaviors;

[AddComponentMenu("Sanity Engine/Actors/Simple Force Actor")]
public class SimpleForceActor : SteeringManagerComponent {
	public float mass = 1.0f;
	public float maxForce = 2.5f;
	public float maxSpeed = 5.0f;
	public float maxTorque = 720.0f;
	public float maxAngularSpeed = 1080.0f;
	public float linearDamp = 0.05f;
	public float angularDamp = 0.05f;
	public float gravity = -9.81f;
	Transform xform;
	CharacterController controller;
	Vector3 velocity;
	Vector3 angularVelocity;
	
	void Start ()
	{
		velocity = Vector3.zero;
		angularVelocity = Vector3.zero;
		xform = transform;
		controller = GetComponent<CharacterController>();
	}
	
	protected override float MaxForce
	{
		get { return maxForce; }
	}
	
	protected override float MaxTorque
	{
		get { return maxTorque; }
	}

	protected override void SteeringUpdate (Steering steering)
	{
		float t = Time.fixedDeltaTime;

		Vector3 accel = steering.Force / mass;
		
		if(controller == null) {
			accel += Vector3.up * gravity;
			xform.Translate(velocity * t + 0.5f * accel * t * t, Space.World);
		} else {
			if(controller.isGrounded) {
				if(velocity.y < 0f) {
					velocity.y = 0.0f;
				}
			} else {
				accel += Vector3.up * gravity;
			}
			controller.Move(velocity * t);// + 0.5f * accel * t * t);
		}
		velocity += accel * t;
		velocity -= velocity * linearDamp;
		float speed = velocity.magnitude;
		if(speed > maxSpeed) {
			velocity *= maxSpeed / speed;
		}
		
		Vector3 angAccel = steering.Torque / mass;

		xform.Rotate(angularVelocity * t + angAccel * t * t, Space.World);

		angularVelocity += angAccel * t;
		angularVelocity -= angularVelocity * angularDamp;
		float angSpeed = angularVelocity.magnitude;
		if(angSpeed > maxAngularSpeed) {
			angularVelocity *= maxAngularSpeed / angSpeed;
		}
	}
		
	public override Vector3 Velocity
    {
        get { return velocity; }
    }

	public override Vector3 AngularVelocity
    {
        get { return angularVelocity; }
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
