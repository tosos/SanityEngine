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

namespace SanityEngine.Movement.SteeringBehaviors
{
    /// <summary>
    /// Base class for steering behaviors.
    /// </summary>
    public abstract class SteeringBehavior
    {
        float weight = 1.0f;
        bool enabled = true;

        /// <summary>
        /// The weight for this behavior. This is used in the weighted
        /// sum type combining steering managers.
        /// </summary>
        public float Weight
        {
            get { return weight; }
            set { weight = value; }
        }
        
        public bool Enabled
        {
        	get { return enabled; }
        	set { enabled = value; }
        }

        /// <summary>
        /// Update the behavior.
        /// </summary>
        /// <param name="actor">The actor being updated.</param>
        /// <param name="dt">The time since the last update, in seconds.
        /// </param>
        /// <returns>The kinematics object.</returns>
        public abstract Vector3 Update(SteeringManager manager, Actor actor,
			float dt);

        /// <summary>
        /// Steer toward the given point.
        /// </summary>
        /// <param name="actor">The actor for steering.</param>
        /// <param name="target">The target point.</param>
        /// <param name="dt">The delta time since the last call.</param>
        /// <returns></returns>
        protected Vector3 SteerToward(SteeringManager manager, Actor actor,
			Vector3 target, float dt)
        {
            Vector3 desired = target - actor.Position;
            float dist = desired.magnitude;
            if (dist > 0.0f)
            {
                desired *= manager.MaxSpeed / dist;
                return desired - actor.Velocity;
            }
            return Vector3.zero;
        }

        /// <summary>
        /// Steer away from the given point.
        /// </summary>
        /// <param name="actor">The actor for steering.</param>
        /// <param name="target">The target point.</param>
        /// <param name="dt">The delta time since the last call.</param>
        /// <returns></returns>
        protected Vector3 SteerAway(SteeringManager manager, Actor actor,
			Vector3 target, float dt)
        {
            Vector3 desired = actor.Position - target;
            float dist = desired.magnitude;
            if (dist > 0.0f)
            {
                desired *= manager.MaxSpeed / dist;
                return desired - actor.Velocity;
            }
            return Vector3.zero;
        }
    }
}
