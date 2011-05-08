//
// Copyright (C) 2010 The Sanity Engine Development Team
//
// This source code is licensed under the terms of the
// MIT License.
//
// For more information, see the file LICENSE

using UnityEngine;
using SanityEngine.Actors;

namespace SanityEngine.Movement.SteeringBehaviors.Flocking
{
    /// <summary>
    /// Steer toward the average heading of the flock.
    /// </summary>
    public class Alignment : FlockingBehavior
    {
        /// <summary>
        /// Update the behavior.
        /// </summary>
        /// <param name="manager">The steering manager.</param>
        /// <param name="actor">The actor being updated.</param>
        /// <param name="dt">The time since the last update, in seconds.
        /// </param>
        /// <returns>The steering object.</returns>
        protected override Steering FlockingUpdate(SteeringManager manager, Actor actor,
			float dt)
        {
            Vector3 accum = Vector3.zero;
            int count = 0;
            foreach (Actor f in Flock.Members)
            {
                Vector3 vel = f.Velocity;
                vel.Normalize();
                accum += vel;
                count++;
            }
			
            accum /= count > 0 ? count : 1.0f;
            return new Steering(SteerToward(manager, actor,
				actor.Position + accum, dt), Vector3.zero);
        }

        public override string GetDescription()
        {
        	return "Steer toward the average direction of the visible"
        		+ " flock members";
        }
    }
}
