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
    /// Seek toward the given target.
    /// </summary>
    public class Seek : SteeringBehavior
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

            return new Steering(true, SteerToward(manager, actor,
				target.Position, dt), false, Vector3.zero);
        }

        public override string GetDescription()
        {
        	return "Move toward a target at maximum speed";
        }
    }
}
