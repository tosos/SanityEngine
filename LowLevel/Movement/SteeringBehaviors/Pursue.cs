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
        /// <returns>The steering object.</returns>
        public override Steering Update(SteeringManager manager, Actor actor,
			float dt)
        {
            if (target == null)
            {
                return Steering.zero;
            }

            Vector3 uVel = actor.Velocity;
            Vector3 uTargetVel = target.Velocity;
            Vector3 deltaPos = target.Position - actor.Position;
            Vector3 targetPos = target.Position;
			if(manager.IsPlanar) {
				uVel.y = 0f;
				uTargetVel.y = 0f;
				deltaPos.y = 0f;
				targetPos.y = 0f;
			}
			
            uVel.Normalize();
            uTargetVel.Normalize();
            float dv = Vector3.Dot(uVel, uTargetVel);
            if (Vector3.Dot(deltaPos, uVel) < 0f || dv > -0.93f)
            {
                Vector3 vel = uVel * manager.MaxForce;
                float combinedSpeed = (vel + target.Velocity).magnitude;
				if(combinedSpeed > 0f) {
                	float predictionTime = deltaPos.magnitude / combinedSpeed;
                	targetPos += target.Velocity * predictionTime;
				}
            }

            return new Steering(SteerToward(manager, actor, targetPos,
				dt), Vector3.zero);
        }

        public override string GetDescription()
        {
        	return "Pursue a target using the predicted future location";
        }
    }
}
