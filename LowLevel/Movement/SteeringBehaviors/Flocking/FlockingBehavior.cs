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
    /// Base class for flock behaviors.
    /// </summary>
    public abstract class FlockingBehavior : SteeringBehavior
    {
        float maxAngle = 270.0f;
        float maxDistance = 5.0f;
        Flock flock = new Flock();

        /// <summary>
        /// The maximum angle from the facing of the agent to consider
        /// other actors in the flock.
        /// </summary>
        public float MaxAngle
        {
            get { return maxAngle; }
            set { maxAngle = value; }
        }

        /// <summary>
        /// The maximum distance to consider the other actors in the flock.
        /// </summary>
        public float MaxDistance
        {
            get { return maxDistance; }
            set { maxDistance = value; }
        }

        /// <summary>
        /// The entire flock.
        /// </summary>
        public Flock Flock
        {
            get { return flock; }
        }

        /// <summary>
        /// Create a flocking behavior.
        /// </summary>
        /// <param name="flock">The flock.</param>
        protected FlockingBehavior(Flock flock)
        {
            this.flock = flock;
        }

        /// <summary>
        /// Check to see if an actor should affect the flocking behavior.
        /// </summary>
        /// <param name="actor">The current actor.</param>
        /// <param name="other">The other actor being tested.</param>
        /// <returns><code>true</code> if the other actor affects the current.
        /// </returns>
        protected bool IsAffecting(Actor actor, Actor other)
        {
        	if(!other.Alive) {
        		return false;
        	}
        	
            Vector3 f = actor.Facing;
            Vector3 v = other.Position - actor.Position;
            float dist = v.magnitude;
            if (dist > maxDistance)
            {
                return false;
            }
            f.Normalize();
            v /= dist;
            float angle = Mathf.Acos(Vector3.Dot(f, v));
            return (angle < maxAngle);
        }
    }
}
