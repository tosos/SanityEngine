using System;
using System.Collections.Generic;
using System.Text;
using AIEngine.Actors;
using AIEngine.Utility.Math;

namespace AIEngine.Movement.SteeringBehaviors.Flocking
{
    /// <summary>
    /// Steer toward the average position of flock members.
    /// </summary>
    public class Cohesion : FlockingBehavior
    {
        AIPointActor target;
        Seek seeker = new Seek();

        /// <summary>
        /// Create a cohesion behavior.
        /// </summary>
        /// <param name="flock">The flock.</param>
        public Cohesion(Flock flock)
            : base(flock)
        {
            target = new AIPointActor(AIVector3.zero);
            seeker.Target = target;
        }

        /// <summary>
        /// Update the cohesion behavior.
        /// </summary>
        /// <param name="actor">The actor being updated.</param>
        /// <param name="dt">The time since the last update, in seconds.
        /// </param>
        /// <returns>The kinematics object.</returns>
        public override Kinematics Update(Actor actor, float dt)
        {
            int count = 0;
            AIVector3 avg = AIVector3.zero;
            foreach (Actor f in Flock.Members)
            {
                if (base.IsAffecting(actor, f))
                {
                    avg += f.Position;
                    count++;
                }
            }
            target.Point = count > 0 ? avg / count : avg;
            return seeker.Update(actor, dt);
        }
    }
}
