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
		bool twoDimensionalFacing = true;
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
        /// Use two-dimensional facing? (Y = up)
        /// </summary>
        public virtual bool TwoDimensionalFacing
        {
            get { return twoDimensionalFacing; }
            set { twoDimensionalFacing = value; }
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
			dir.Normalize();
			Vector3 delta = Quaternion.FromToRotation(actor.Facing, dir).eulerAngles;
			delta.x = twoDimensionalFacing ? 0f : Mathf.DeltaAngle(0f, delta.x);
			delta.y = Mathf.DeltaAngle(0f, delta.y);
			delta.z = 0.0f;
			float angDist = delta.magnitude;
			if(angDist > 0.0f) {
				float torque = manager.MaxTorque * (angDist / angleThreshold);
				torque = Mathf.Min(torque, manager.MaxTorque);
				delta *= torque / angDist;
			}
			return new Steering(false, Vector3.zero,
				true, delta - actor.AngularVelocity);
		}

        public override string GetDescription()
        {
        	return "Turn toward a target at maximum angular speed";
        }
    }
}
