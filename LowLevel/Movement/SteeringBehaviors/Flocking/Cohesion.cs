//
// Copyright (C) 2010 The Sanity Engine Development Team
//
// This source code is licensed under the terms of the
// MIT License.
//
// For more information, see the file LICENSE

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using SanityEngine.Actors;

namespace SanityEngine.Movement.SteeringBehaviors.Flocking
{
    /// <summary>
    /// Steer toward the average position of flock members.
    /// </summary>
    public class Cohesion : FlockingBehavior
    {
        PointActor target;
        Seek seeker = new Seek();

        /// <summary>
        /// Create a cohesion behavior.
        /// </summary>
        /// <param name="flock">The flock.</param>
        public Cohesion(Flock flock)
            : base(flock)
        {
            target = new PointActor(Vector3.zero);
            seeker.Target = target;
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
            int count = 0;
            Vector3 avg = Vector3.zero;
            foreach (Actor f in Flock.Members)
            {
                if (base.IsAffecting(actor, f))
                {
                    avg += f.Position;
                    count++;
                }
            }
            target.Point = count > 0 ? avg / count : avg;
            return seeker.Update(manager, actor, dt);
        }
    }
}
