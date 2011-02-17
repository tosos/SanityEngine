using UnityEngine;
using System.Collections;
using SanityEngine.Movement.SteeringBehaviors;

[AddComponentMenu("Sanity Engine/Steering Motors/Character Controller Motor"),
RequireComponent(typeof(CharacterControllerActor))]
public class CharacterControllerMotor : SteeringManagerComponent {
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
	
	public Vector3 Velocity
	{
		get { return velocity; }
	}
	
	public Vector3 AngularVelocity
	{
		get { return angularVelocity; }
	}
	
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
		
		if(controller.isGrounded) {
			if(velocity.y < 0f) {
				velocity.y = 0.0f;
			}
		} else {
			accel += Vector3.up * gravity;
		}
		controller.Move(velocity * t);// + 0.5f * accel * t * t);
		
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
}
