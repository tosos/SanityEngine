//
// Copyright (C) 2010 The Sanity Engine Development Team
//
// This source code is licensed under the terms of the
// MIT License.
//
// For more information, see the file LICENSE

using UnityEngine;
using System.Collections.Generic;
using System.Text;
using SanityEngine.Actors;

namespace SanityEngine.Movement.SteeringBehaviors
{
    /// <summary>
    /// Seek to the target, slowing to a stop when you arrive.
    /// </summary>
    public class Arrive : SteeringBehavior
    {
        Actor target;

        /// <summary>
        /// The target actor.
        /// </summary>
        public virtual Actor Target
        {
            get { return target; }
            set { target = value; }
        }

        float arriveRadius = 5.0f;

        /// <summary>
        /// The maximum distance to start the arrive behavior.
        /// </summary>
        public float ArriveRadius
        {
            get { return arriveRadius; }
            set { arriveRadius = value; }
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
            if (Target == null)
            {
                return Steering.zero;
            }

            Vector3 delta = Target.Position - actor.Position;
            float dist = delta.magnitude;
            if (dist > 0.0f)
            {
				float force = manager.MaxSpeed * (dist / arriveRadius);
				force = Mathf.Min(force, manager.MaxSpeed);
                delta *= force / dist;
            }

            return new Steering(true, delta - actor.Velocity, false,
				Vector3.zero);
        }
        
        public override string GetDescription()
        {
        	return "Move toward a target, slowing to a stop near it once"
        		+ " you are inside the arrive radius";
        }
    }
}
