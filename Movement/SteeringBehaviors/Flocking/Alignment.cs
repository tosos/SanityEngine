using System;
using System.Collections.Generic;
using System.Text;
using AIEngine.Actors;
using AIEngine.Utility.Math;

namespace AIEngine.Movement.SteeringBehaviors.Flocking
{
    /// <summary>
    /// Steer toward the average heading of the flock.
    /// </summary>
    public class Alignment : FlockingBehavior
    {
        /// <summary>
        /// Create an alignment behavior.
        /// </summary>
        /// <param name="flock">The flock.</param>
        public Alignment(Flock flock)
            : base(flock)
        {
        }

        /// <summary>
        /// Update the alignment behavior.
        /// </summary>
        /// <param name="actor">The actor being updated.</param>
        /// <param name="dt">The time since the last update, in seconds.
        /// </param>
        /// <returns>The kinematics object.</returns>
        public override Kinematics Update(Actor actor, float dt)
        {
            AIVector3 accum = AIVector3.zero;
            int count = 0;
            foreach (Actor f in Flock.Members)
            {
                if (base.IsAffecting(actor, f))
                {
                    AIVector3 vel = f.Velocity;
                    vel.Normalize();
                    accum += vel;
                    count++;
                }
            }
            accum /= count > 0 ? count : 1.0f;
            return new Kinematics(SteerToward(actor, actor.Position + accum, dt), AIVector3.zero);
        }
    }
}
