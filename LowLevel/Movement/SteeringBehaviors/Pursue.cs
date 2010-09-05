//
// Copyright (C) 2010 The Sanity Engine Development Team
//
// This source code is licensed under the terms of the
// MIT License.
//
// For more information, see the file LICENSE

using UnityEngine;
using SanityEngine.Actors;

namespace SanityEngine.Movement.SteeringBehaviors
{
    /// <summary>
    /// Pursue an actor.
    /// </summary>
    public class Pursue : SteeringBehavior
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
        /// Update the behavior.
        /// </summary>
        /// <param name="manager">The steering manager.</param>
        /// <param name="actor">The actor being updated.</param>
        /// <param name="dt">The time since the last update, in seconds.
        /// </param>
        /// <returns>The kinematics object.</returns>
        public override Vector3 Update(SteeringManager manager, Actor actor,
			float dt)
        {
            if (target == null)
            {
                return Vector3.zero;
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
                Vector3 vel = uVel * manager.MaxSpeed;
                float combinedSpeed = (vel + target.Velocity).magnitude;
                float predictionTime = deltaPos.magnitude / combinedSpeed;
                targetPos += target.Velocity * predictionTime;
            }

            return SteerToward(manager, actor, targetPos, dt);
        }

        public override string GetDescription()
        {
        	return "Pursue a target using the predicted future location";
        }
    }
}
