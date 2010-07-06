using System.Collections;
using System.Collections.Generic;
using AIEngine.Actors;
using AIEngine.Utility.Math;

namespace AIEngine.Movement.SteeringBehaviors.Flocking
{
    /// <summary>
    /// Avoid future collisions with other actors.
    /// In other words, get out of each other's way.
    /// </summary>
    public class Avoid : FlockingBehavior
    {
        AIPointActor target;
        Flee flee = new Flee();

        /// <summary>
        /// Create an avoid behavior.
        /// </summary>
        /// <param name="flock">The flock.</param>
        public Avoid(Flock flock)
            : base(flock)
        {
            target = new AIPointActor(AIVector3.zero);
            flee.Target = target;
        }

        /// <summary>
        /// Update the avoid behavior.
        /// </summary>
        /// <param name="actor">The actor being updated.</param>
        /// <param name="dt">The time since the last update, in seconds.
        /// </param>
        /// <returns>The kinematics object.</returns>
        public override Kinematics Update(Actor actor, float dt)
        {
            float smallestHitTime = float.PositiveInfinity;
            AIVector3 tgt = AIVector3.zero;
            bool hit = false;

            foreach (Actor other in Flock.Members)
            {
                if (!base.IsAffecting(actor, other))
                {
                    continue;
                }

                AIVector3 a = actor.Position - other.Position;
                AIVector3 b = actor.Velocity - other.Velocity;

                // Calculate future hit time
                float hTime = -(AIVector3.Dot(a, b)) / b.SqrMagnitude;

                // We want the smallest hit time but it must be greater
                // than zero. Zero and smaller means no collision
                if (hTime < smallestHitTime && hTime > 0)
                {
                    smallestHitTime = hTime;
                    // If we have the smallest hit time, compute where that
                    // actor will be at that time and set it to the target
                    hit = true;
                    tgt = smallestHitTime * other.Velocity + other.Position;
                }
            }
            //Flee from the target
            if (hit)
            {
                target.Point = tgt;
                return flee.Update(actor, dt);
            }

            return Kinematics.zero;
        }
    }
}