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
	public bool twoDimensionalFacing = true;
	Transform xform;
	CharacterController controller;
	Vector3 velocity;
	Vector3 angularVelocity;
	Vector3 desiredFacing;
	
	void Start ()
	{
		velocity = Vector3.zero;
		angularVelocity = Vector3.zero;
		xform = transform;
		controller = GetComponent<CharacterController>();
	}
	
	protected override float MaxSpeed
	{
		get { return maxSpeed; }
	}
	
	protected override float MaxAngularSpeed
	{
		get { return maxAngularSpeed; }
	}

	void LateUpdate ()
	{
		float t = Time.deltaTime;
		Steering steering = base.Steering;
		
		Vector3 desired = steering.Force;
		float force = desired.magnitude;
		if(force > maxForce) {
			desired *= maxForce / force;
		}
		Vector3 accel = desired / mass;
		
		if(controller == null) {
			xform.Translate(velocity * t + 0.5f * accel * t * t, Space.World);
		} else {
			controller.Move(velocity * t);// + 0.5f * accel * t * t);
		}
		velocity += accel * t;
		velocity -= velocity * linearDamp;
		float speed = velocity.magnitude;
		if(speed > maxSpeed) {
			velocity *= maxSpeed / speed;
		}
		
		Vector3 angAccel  = steering.Torque;
		float torque2 = angAccel.magnitude;
		if(torque2 > maxTorque) {
			angAccel *= maxTorque / torque2;
		}

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
