using System;
using System.Collections.Generic;
using System.Text;
using AIEngine.Actors;
using AIEngine.Utility.Math;

namespace AIEngine.Movement.SteeringBehaviors
{
    /// <summary>
    /// Seek to the target, slowing to a stop when you arrive.
    /// </summary>
    public class Arrive : SteeringBehavior
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

        float arriveRadius = 5.0f;
        float timeToTarget = 5.0f;

        /// <summary>
        /// The maximum distance to start the arrive behavior.
        /// </summary>
        public float ArriveRadius
        {
            get { return arriveRadius; }
            set { arriveRadius = value; }
        }

        /// <summary>
        /// The "time to target" parameter. This controls the rate of
        /// slowdown as the agent arrives at its target.
        /// </summary>
        public float TimeToTarget
        {
            get { return timeToTarget; }
            set { timeToTarget = value; }
        }

        /// <summary>
        /// Update the behavior.
        /// </summary>
        /// <param name="actor">The actor being updated.</param>
        /// <param name="dt">The time since the last update, in seconds.
        /// </param>
        /// <returns>The kinematics object.</returns>
        public override Kinematics Update(Actor actor, float dt)
        {
            if (Target == null)
            {
                return Kinematics.zero;
            }

            AIVector3 delta = Target.Position - actor.Position;
            float dist = delta.Magnitude;
            if (dist > 0.0f)
            {
                delta /= timeToTarget;
                float m = delta.Magnitude;
                if (m > actor.MaxSpeed)
                {
                    delta *= actor.MaxSpeed / m;
                }
                return new Kinematics(delta - actor.Velocity, AIVector3.zero);
            }

            return Kinematics.zero;
        }
    }
}
