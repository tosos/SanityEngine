using System;
using System.Collections.Generic;
using System.Text;
using AIEngine.Actors;
using AIEngine.Utility.Math;

namespace AIEngine.Movement.SteeringBehaviors
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

            AIVector3 uVel = actor.Velocity;
            uVel.Normalize();
            AIVector3 uTargetVel = target.Velocity;
            uTargetVel.Normalize();
            float dv = AIVector3.Dot(uVel, uTargetVel);
            AIVector3 deltaPos = target.Position - actor.Position;
            AIVector3 targetPos = target.Position;
            if (AIVector3.Dot(deltaPos, uVel) < 0 || dv > -0.93)
            {
                AIVector3 vel = uVel * actor.MaxSpeed;
                float combinedSpeed = (vel + target.Velocity).Magnitude;
                float predictionTime = deltaPos.Magnitude / combinedSpeed;
                targetPos += target.Velocity * predictionTime;
            }

            return new Kinematics(SteerAway(actor, targetPos, dt), AIVector3.zero);
        }
    }
}
