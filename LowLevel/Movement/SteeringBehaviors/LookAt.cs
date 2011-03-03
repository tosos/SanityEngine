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
    /// Turn toward the given target.
    /// </summary>
    public class LookAt : SteeringBehavior
    {
        Actor target;
		float angleThreshold = 90.0f;

        /// <summary>
        /// The target actor.
        /// </summary>
        public virtual Actor Target
        {
            get { return target; }
            set { target = value; }
        }
				
        /// <summary>
        /// The angle inside which the turning will slow down.
        /// </summary>
		public float AngleThreshold
		{
			get { return angleThreshold; }
			set { angleThreshold = value; }
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
            if (target == null)
            {
                return Steering.zero;
            }

			Vector3 dir = target.Position - actor.Position;
			Vector3 facing = actor.Forward;
			
			dir.y = manager.IsPlanar ? 0f : dir.y;
			facing.y = manager.IsPlanar ? 0f : facing.y;
			
			Vector3 delta = Quaternion.FromToRotation(facing, dir).eulerAngles;
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
        	return "Turn toward a target at maximum angular speed";
        }
    }
}
