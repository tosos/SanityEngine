using UnityEngine;
using System.Collections;
using SanityEngine.Movement.SteeringBehaviors;

[AddComponentMenu("Sanity Engine/Actors/Character Controller Actor"),
 RequireComponent(typeof(CharacterController))]
public class CharacterControllerActor : GameObjectActor {
	Transform xform;
	Vector3 oldPosition;
	Vector3 oldRotation;
	Vector3 velocity;
	Vector3 angularVelocity;
	CharacterControllerMotor motor;
	
	void Start ()
	{
		velocity = Vector3.zero;
		angularVelocity = Vector3.zero;
		xform = transform;
		motor = GetComponent<CharacterControllerMotor>();
		
		oldPosition = xform.position;
		oldRotation = xform.rotation.eulerAngles;
	}
	
	void LateUpdate()
	{
		if(motor) {
			velocity = motor.Velocity;
			angularVelocity = motor.AngularVelocity;
		} else {
			float scale = 1f / Time.deltaTime;
			velocity = (xform.position - oldPosition) * scale;
			angularVelocity = (xform.rotation.eulerAngles - oldRotation) * scale;
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
