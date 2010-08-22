using UnityEngine;
using System.Collections;
using SanityEngine.Movement.SteeringBehaviors;

[AddComponentMenu("Sanity Engine/Actors/Simple Force Actor")]
public class SimpleForceActor : SteeringManagerComponent {
	public float mass = 1.0f;
	public float maxForce = 2.5f;
	public float maxSpeed = 5.0f;
	public float maxTorque = 2.5f;
	public float maxAngularSpeed = 5.0f;
	public float angleThreshold = 90.0f;
	public float velocityFaceThreshold = 0.001f;
	public float linearDamp = 0.05f;
	public float angularDamp = 0.05f;
	public bool twoDimensionalFacing = true;
	Transform xform;
	CharacterController controller;
	Vector3 velocity;
	Vector3 angularVelocity;
	Vector3 desiredFacing;
	
	void Awake ()
	{
		velocity = Vector3.zero;
		angularVelocity = Vector3.zero;
		xform = transform;
		controller = GetComponent<CharacterController>();
	}
	
	void LateUpdate ()
	{
		float t = Time.deltaTime;
		Vector3 desired = (base.Force - velocity) / t;
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
		
		Vector3 dir = xform.forward;
		if(velocity.magnitude > velocityFaceThreshold) {
			dir = velocity / velocity.magnitude;
		}
		Vector3 delta = Quaternion.FromToRotation(xform.forward, dir).eulerAngles;
		delta.x = twoDimensionalFacing ? 0f : Mathf.DeltaAngle(0f, delta.x);
		delta.y = Mathf.DeltaAngle(0f, delta.y);
		delta.z = 0.0f;
		float angDist = delta.magnitude;
		if(angDist > 0.0f) {
			float torque = maxTorque * (angDist / angleThreshold);
			torque = Mathf.Min(torque, maxTorque);
			delta *= torque / angDist;
		}
		delta -= angularVelocity;
		
		Vector3 angAccel  = (delta - angularVelocity) / t;
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
        get { return maxSpeed; }
    }
}
