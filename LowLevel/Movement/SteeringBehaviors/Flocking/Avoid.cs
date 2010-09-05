//
// Copyright (C) 2010 The Sanity Engine Development Team
//
// This source code is licensed under the terms of the
// MIT License.
//
// For more information, see the file LICENSE

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SanityEngine.Actors;

namespace SanityEngine.Movement.SteeringBehaviors.Flocking
{
    /// <summary>
    /// Avoid future collisions with other actors.
    /// In other words, get out of each other's way.
    /// </summary>
    public class Avoid : FlockingBehavior
    {
        PointActor target;
        Flee flee = new Flee();

        /// <summary>
        /// Create an avoid behavior.
        /// </summary>
        /// <param name="flock">The flock.</param>
        public Avoid(Flock flock)
            : base(flock)
        {
            target = new PointActor(Vector3.zero);
            flee.Target = target;
        }

        /// <summary>
        /// Update the behavior.
        /// </summary>
        /// <param name="manager">The steering manager.</param>
        /// <param name="actor">The actor being updated.</param>
        /// <param name="dt">The time since the last update, in seconds.
        /// </param>
        /// <returns>The kinematics object.</returns>
        public override Vector3 Update(SteeringManager manager, Actor actor,
			float dt)
        {
            float smallestHitTime = float.PositiveInfinity;
            Vector3 tgt = Vector3.zero;
            bool hit = false;

            foreach (Actor other in Flock.Members)
            {
                if (!base.IsAffecting(actor, other))
                {
                    continue;
                }

                Vector3 a = actor.Position - other.Position;
                Vector3 b = actor.Velocity - other.Velocity;

                // Calculate future hit time
                float hTime = -(Vector3.Dot(a, b)) / b.sqrMagnitude;

                // We want the smallest hit time but it must be greater
                // than zero. Zero and smaller means no collision
                if (hTime < smallestHitTime && hTime > 0)
                {
                    smallestHitTime = hTime;
                    // If we have the smallest hit time, compute where that
                    // actor will be at that time and set it to the target
                    hit = true;
                    tgt = smallestHitTime * other.Velocity + other.Position;
                }
            }
            //Flee from the target
            if (hit)
            {
                target.Point = tgt;
                return flee.Update(manager, actor, dt);
            }

            return Vector3.zero;
        }

        public override string GetDescription()
        {
        	return "Avoid future collisions with other flock members";
        }
    }
}