using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using AIEngine.Actors;

namespace AIEngine.Movement.SteeringBehaviors
{
    /// <summary>
    /// Look in the direction of your velocity.
    /// </summary>
    public class LookWhereYouAreGoing : SteeringBehavior
    {
        /// <summary>
        /// Update the behavior.
        /// </summary>
        /// <param name="actor">The actor being updated.</param>
        /// <param name="dt">The time since the last update, in seconds.
        /// </param>
        /// <returns>The kinematics object.</returns>
        public override Kinematics Update(Actor actor, float dt)
        {
            Vector3 face = actor.Velocity;
            Vector3 axis = Vector3.Cross(face, actor.Facing);
			float mag = axis.magnitude;
			if(mag < float.Epsilon) {
				face.y += float.Epsilon;
				axis = Vector3.Cross(face, actor.Facing);
			}
            axis.Normalize();
            axis *= Vector3.Angle(face, actor.Facing);
            return new Kinematics(Vector3.zero, axis * actor.MaxAngSpeed);
        }
    }
}
