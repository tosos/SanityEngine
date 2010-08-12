using UnityEngine;
using System.Collections;
using SanityEngine.Movement.SteeringBehaviors;

[AddComponentMenu("Sanity Engine/Actors/Simple Force Actor")]
[RequireComponent(typeof(SteeringManagerComponent))]
public class SimpleForceActor : GameObjectActor {
	public float maxSpeed = 5.0f;
	public float maxAngularSpeed = 45.0f;
	public float maxAccel = 2.0f;
	public float linearDamp = 0.05f;
	Transform xform;
	SteeringManagerComponent manager;
	Vector3 velocity;
	
	void Awake ()
	{
		velocity = Vector3.zero;
		xform = transform;
		manager = GetComponent<SteeringManagerComponent>();
	}
	
	void LateUpdate ()
	{
		Vector3 force = manager.Force;
		float f = force.magnitude;
		if(f > maxAccel) {
			force *= maxAccel / f;
		}
		velocity += force;
		velocity *= 1f - linearDamp;
		xform.position += velocity;
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

	public override float MaxSpeed
	{
        get { return maxSpeed; }
    }

	public override float MaxAngSpeed
	{
        get { return maxAngularSpeed; }
    }
}
