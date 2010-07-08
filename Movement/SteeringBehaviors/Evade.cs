using UnityEngine;
using System.Collections.Generic;
using SanityEngine.Actors;

namespace SanityEngine.Movement.SteeringBehaviors
{
    /// <summary>
    /// Evade an actor.
    /// </summary>
    public class Evade : SteeringBehavior
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

        /// <summary>
        /// Update the pursue behavior.
        /// </summary>
        /// <param name="actor">The actor being updated.</param>
        /// <param name="dt">The time since the last update, in seconds.
        /// </param>
        /// <returns>The kinematics object.</returns>
        public override Kinematics Update(Actor actor, float dt)
        {
            if (target == null)
            {
                return Kinematics.zero;
            }

            Vector3 uVel = actor.Velocity;
            uVel.Normalize();
            Vector3 uTargetVel = target.Velocity;
            uTargetVel.Normalize();
            float dv = Vector3.Dot(uVel, uTargetVel);
            Vector3 deltaPos = target.Position - actor.Position;
            Vector3 targetPos = target.Position;
            if (Vector3.Dot(deltaPos, uVel) < 0 || dv > -0.93)
            {
                Vector3 vel = uVel * actor.MaxSpeed;
                float combinedSpeed = (vel + target.Velocity).magnitude;
                float predictionTime = deltaPos.magnitude / combinedSpeed;
                targetPos += target.Velocity * predictionTime;
            }

            return new Kinematics(SteerAway(actor, targetPos, dt), Vector3.zero);
        }
    }
}
