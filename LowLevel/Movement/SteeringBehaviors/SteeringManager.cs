//
// Copyright (C) 2010 The Sanity Engine Development Team
//
// This source code is licensed under the terms of the
// MIT License.
//
// For more information, see the file LICENSE

using UnityEngine;
using System.Collections.Generic;
using SanityEngine.Actors;

namespace SanityEngine.Movement.SteeringBehaviors
{
    /// <summary>
    /// A simple steering manager using a weight sum combination method.
    /// </summary>
    public class SteeringManager
    {
		/// <summary>
		/// The max speed to be applied in the steering behaviors.
		/// </summary>
		public float MaxSpeed
		{
			get { return maxSpeed; }
			set { maxSpeed = value; }
		}

		/// <summary>
		/// The max angular speed to be applied in the steering behaviors.
		/// </summary>
		public float MaxAngularSpeed
		{
			get { return maxAngularSpeed; }
			set { maxAngularSpeed = value; }
		}
		
        /// <summary>
        /// The currently registered behaviors.
        /// </summary>
        protected List<SteeringBehavior> behaviors = new List<SteeringBehavior>();
		
		float maxSpeed = 1.0f;
		float maxAngularSpeed = 1.0f;

        /// <summary>
        /// Add a steering behavior to be updated by this manager.
        /// </summary>
        /// <param name="behavior">The steering behavior.</param>
        public void AddBehavior(SteeringBehavior behavior)
        {
            behaviors.Add(behavior);
        }

        /// <summary>
        /// Remove a steering behavior from this manager.
        /// </summary>
        /// <param name="behavior">The steering behavior.</param>
        public void RemoveBehavior(SteeringBehavior behavior)
        {
            behaviors.Remove(behavior);
        }

        /// <summary>
        /// Update the steering behaviors managed by this steering manager.
        /// </summary>
        /// <param name="actor">The actor to update on.</param>
        /// <param name="dt">The delta time since the last update
        /// (in seconds)</param>
        /// <returns>The combined Steering object.</returns>
        public virtual Steering Update(Actor actor, float dt)
        {
            Steering result = Steering.zero;
            foreach (SteeringBehavior behavior in behaviors)
            {
                result += behavior.Update(this, actor, dt) * behavior.Weight;
            }
            
            result /= behaviors.Count;
			
            return result;
        }
    }
}
