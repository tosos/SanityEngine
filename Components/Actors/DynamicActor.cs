using UnityEngine;
using System.Collections;

[AddComponentMenu("Sanity Engine/Actors/Dynamic Actor")]
public class DynamicActor : GameObjectActor {
	Transform xform;
	Vector3 oldPosition;
	Vector3 oldRotation;
	Vector3 velocity;
	Vector3 angularVelocity;
	
	void Start ()
	{
		velocity = Vector3.zero;
		angularVelocity = Vector3.zero;
		xform = transform;
		
		oldPosition = xform.position;
		oldRotation = xform.rotation.eulerAngles;
	}
	
	void FixedUpdate()
	{
		float scale = 1f / Time.fixedDeltaTime;
		velocity = (xform.position - oldPosition) * scale;
		Debug.Log(velocity);
		Vector3 rot = xform.rotation.eulerAngles;
		angularVelocity = (rot - oldRotation) * scale;
		oldPosition = xform.position;
		oldRotation = rot;
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
