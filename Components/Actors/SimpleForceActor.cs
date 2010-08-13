using UnityEngine;
using System.Collections;
using SanityEngine.Movement.SteeringBehaviors;

[AddComponentMenu("Sanity Engine/Actors/Simple Force Actor")]
[RequireComponent(typeof(SteeringManagerComponent))]
public class SimpleForceActor : GameObjectActor {
	public float maxForce = 2.5f;
	public float maxSpeed = 5.0f;
	public float maxAngularForce = 2.5f;
	public float maxAngularSpeed = 5.0f;
	public float linearDamp = 0.05f;
	public float angularDamp = 0.05f;
	public bool twoDimensionalFacing = true;
	Transform xform;
	SteeringManagerComponent manager;
	Vector3 velocity;
	Vector3 angularVelocity;
	Vector3 desiredFacing;
	
	void Awake ()
	{
		velocity = Vector3.zero;
		angularVelocity = Vector3.zero;
		xform = transform;
		manager = GetComponent<SteeringManagerComponent>();
	}
	
	void LateUpdate ()
	{
		Vector3 force = manager.Force;
		velocity += force;
		velocity *= 1f - linearDamp;
		float speed = velocity.magnitude;
		if(speed > maxSpeed) {
			velocity *= maxSpeed / speed;
		}
		xform.position += velocity;
		
		Vector3 dir = xform.forward;
		if(velocity.magnitude > 0.001f) {
			dir = velocity / velocity.magnitude;
		}
		Vector3 delta = Quaternion.FromToRotation(xform.forward, dir).eulerAngles;
		delta.x = twoDimensionalFacing ? 0f : Mathf.DeltaAngle(0f, delta.x);
		delta.y = Mathf.DeltaAngle(0f, delta.y);
		delta.z = 0.0f;
			
		delta /= 5.0f;
		float a = delta.magnitude;
		if(a > maxAngularForce) {
			delta *= maxAngularForce / a;
		}
		delta -= angularVelocity;
		angularVelocity += delta;
		
		angularVelocity *= 1f - angularDamp;
		float angSpeed = angularVelocity.magnitude;
		if(angSpeed > maxAngularSpeed) {
			angularVelocity *= maxAngularSpeed / angSpeed;
		}
		xform.Rotate(angularVelocity, Space.World);
		
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
        get { return maxForce; }
    }
}
