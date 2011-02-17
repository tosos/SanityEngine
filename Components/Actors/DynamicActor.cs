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
	
	void LateUpdate()
	{
		float scale = 1f / Time.deltaTime;
		velocity = (xform.position - oldPosition) * scale;
		angularVelocity = (xform.rotation.eulerAngles - oldRotation) * scale;
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
