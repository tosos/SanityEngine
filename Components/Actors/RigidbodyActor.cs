using UnityEngine;
using System.Collections;
using SanityEngine.Movement.SteeringBehaviors;

[AddComponentMenu("Sanity Engine/Actors/Rigid Body Actor")]
[RequireComponent(typeof(Rigidbody))]
public class RigidbodyActor : GameObjectActor {
	Rigidbody body;
	Transform xform;
	
	void Awake()
	{
		body = rigidbody;
		xform = transform;
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

    public override Vector3 Forward
    {
        get { return xform.forward; }
    }

	public override Vector3 Right
    {
        get { return xform.right; }
    }
    
	public override Vector3 Up
    {
        get { return xform.up; }
    }
}
