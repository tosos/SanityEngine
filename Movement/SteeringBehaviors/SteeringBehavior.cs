using System;
using System.Collections.Generic;
using System.Text;
using AIEngine.Actors;
using AIEngine.Utility.Math;

namespace AIEngine.Movement.SteeringBehaviors
{
    /// <summary>
    /// Base class for steering behaviors.
    /// </summary>
    public abstract class SteeringBehavior
    {
        float weight = 1.0f;

        /// <summary>
        /// The weight for this behavior. This is used in the weighted
        /// sum type combining steering managers.
        /// </summary>
        public float Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        /// <summary>
        /// Update the behavior.
        /// </summary>
        /// <param name="actor">The actor being updated.</param>
        /// <param name="dt">The time since the last update, in seconds.
        /// </param>
        /// <returns>The kinematics object.</returns>
        public abstract Kinematics Update(Actor actor, float dt);

        /// <summary>
        /// Steer toward the given point.
        /// </summary>
        /// <param name="actor">The actor for steering.</param>
        /// <param name="target">The target point.</param>
        /// <param name="dt">The delta time since the last call.</param>
        /// <returns></returns>
        protected AIVector3 SteerToward(Actor actor, AIVector3 target, float dt)
        {
            AIVector3 desired = target - actor.Position;
            float dist = desired.Magnitude;
            if (dist > 0.0f)
            {
                desired *= actor.MaxSpeed / dist;
                return desired - actor.Velocity;
            }
            return AIVector3.zero;
        }

        /// <summary>
        /// Steer away from the given point.
        /// </summary>
        /// <param name="actor">The actor for steering.</param>
        /// <param name="target">The target point.</param>
        /// <param name="dt">The delta time since the last call.</param>
        /// <returns></returns>
        protected AIVector3 SteerAway(Actor actor, AIVector3 target, float dt)
        {
            AIVector3 desired = actor.Position - target;
            float dist = desired.Magnitude;
            if (dist > 0.0f)
            {
                desired *= actor.MaxSpeed / dist;
                return desired - actor.Velocity;
            }
            return AIVector3.zero;
        }
    }
}
