//
// Copyright (C) 2010 The Sanity Engine Development Team
//
// This source code is licensed under the terms of the
// MIT License.
//
// For more information, see the file LICENSE

using UnityEngine;
using SanityEngine.Actors;

namespace SanityEngine.Movement.SteeringBehaviors
{
    /// <summary>
    /// Turn toward the direction of movement.
    /// </summary>
    public class LookWhereYouAreGoing : SteeringBehavior
    {
		float velocityFaceThreshold = 0.001f;
		float angleThreshold = 90.0f;

        /// <summary>
        /// The angle inside which the turning will slow down.
        /// </summary>
		public float AngleThreshold
		{
			get { return angleThreshold; }
			set { angleThreshold = value; }
		}

        /// <summary>
        /// The minimum velocity threshold before turning
        /// </summary>
		public float VelocityFaceThreshold
		{
			get { return velocityFaceThreshold; }
			set { velocityFaceThreshold = value; }
		}
		
        /// <summary>
        /// Update the behavior.
        /// </summary>
        /// <param name="manager">The steering manager.</param>
        /// <param name="actor">The actor being updated.</param>
        /// <param name="dt">The time since the last update, in seconds.
        /// </param>
        /// <returns>The steering object.</returns>
        public override Steering Update(SteeringManager manager, Actor actor,
			float dt)
        {
			Vector3 dir = actor.Facing;
			if(actor.Velocity.magnitude > velocityFaceThreshold) {
				dir = actor.Velocity / actor.Velocity.magnitude;
			}
			
			Vector3 delta = Quaternion.FromToRotation(actor.Facing, dir).eulerAngles;
			Vector3 angVel = actor.AngularVelocity;
			
			if(manager.IsPlanar) {
				angVel.x = 0f;
				delta.x = 0f;
			} else {
				delta.x = Mathf.DeltaAngle(0f, delta.x);
			}
			
			angVel.z = 0f;
			delta.z = 0.0f;
			delta.y = Mathf.DeltaAngle(0f, delta.y);
			
			
			float angDist = delta.magnitude;
			if(angDist > 0.0f) {
				float torque = manager.MaxTorque * (angDist / angleThreshold);
				torque = Mathf.Min(torque, manager.MaxTorque);
				delta *= torque / angDist;
			}
			return new Steering(Vector3.zero, delta - angVel);
		}

        public override string GetDescription()
        {
        	return "Turn toward the direction of movement at maximum angular speed";
        }
    }
}
