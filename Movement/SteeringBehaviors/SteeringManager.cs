using System;
using System.Collections.Generic;
using System.Text;
using SanityEngine.Actors;

namespace SanityEngine.Movement.SteeringBehaviors
{
    /// <summary>
    /// A simple steering manager using a weight sum combination method.
    /// </summary>
    public class SteeringManager
    {
        /// <summary>
        /// The currently registered behaviors.
        /// </summary>
        protected List<SteeringBehavior> behaviors = new List<SteeringBehavior>();

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
        /// <returns>The combined Kinematics object.</returns>
        public virtual Kinematics Update(Actor actor, float dt)
        {
            Kinematics result = Kinematics.zero;
            foreach (SteeringBehavior behavior in behaviors)
            {
                result += behavior.Update(actor, dt) * behavior.Weight;
            }
            
            result /= behaviors.Count;

            return result;
        }
    }
}
