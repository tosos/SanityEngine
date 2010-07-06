using System;
using System.Collections.Generic;
using System.Text;
using AIEngine.Actors;
using AIEngine.Utility.Math;

namespace AIEngine.Movement.SteeringBehaviors.Flocking
{
    /// <summary>
    /// Separation from flock members.
    /// </summary>
    public class Separation : FlockingBehavior
    {
        /// <summary>
        /// Create an separation behavior.
        /// </summary>
        /// <param name="flock">The flock.</param>
        public Separation(Flock flock)
            : base(flock)
        {
        }

        /// <summary>
        /// Update the separation behavior.
        /// </summary>
        /// <param name="actor">The actor being updated.</param>
        /// <param name="dt">The time since the last update, in seconds.
        /// </param>
        /// <returns>The kinematics object.</returns>
        public override Kinematics Update(Actor actor, float dt)
        {
            float threshold = base.MaxDistance;

            AIVector3 accum = AIVector3.zero;
            foreach (Actor f in Flock.Members)
            {
                if (base.IsAffecting(actor, f))
                {
                    AIVector3 v = actor.Position - f.Position;
                    float d = v.Magnitude;
                    float str = Math.Max(0.0f, (threshold - d) / threshold) * actor.MaxSpeed;
                    accum += (v / d) * str;
                }
            }
            return new Kinematics(accum, AIVector3.zero);
        }
    }
}
