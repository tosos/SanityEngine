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
		/// The max force to be applied in the steering behaviors.
		/// </summary>
		public float MaxForce
		{
			get { return maxForce; }
			set { maxForce = value; }
		}

		/// <summary>
		/// The max torque to be applied in the steering behaviors.
		/// </summary>
		public float MaxTorque
		{
			get { return maxTorque; }
			set { maxTorque = value; }
		}
		
		/// <summary>
		/// If this is set, the behaviors ignore the y component.
		/// </summary>
		public bool IsPlanar
		{
			get { return isPlanar; }
			set { isPlanar = value; }
		}
		
        /// <summary>
        /// The currently registered behaviors.
        /// </summary>
        protected List<SteeringBehavior> behaviors = new List<SteeringBehavior>();
		
		float maxForce = 1.0f;
		float maxTorque = 1.0f;
		bool isPlanar = false;

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
				if(behavior.Enabled) {
					Steering steering = behavior.Update(this, actor, dt);
                	result += steering * behavior.Weight;
				}
            }
			
			if(isPlanar) {
				result.Force.y = 0f;
				result.Torque.x = 0f;
				result.Torque.z = 0f;
			}

			float force = result.Force.magnitude;
			if(force > 0f) {
				float max = Mathf.Min(force, maxForce);
				result.Force *= (max / force);
			}
	
			float torque = result.Torque.magnitude;
			if(torque > 0f) {
				float max = Mathf.Min(torque, maxTorque);
				result.Torque *= (max / torque);
			}
			
            return result;
        }
    }
}
