using System;
using System.Collections.Generic;
using System.Text;
using AIEngine.Actors;
using AIEngine.Utility.Math;

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
            AIVector3 face = actor.Velocity;
            AIVector3 axis = AIVector3.Cross(face, actor.Facing);
			float mag = axis.Magnitude;
			if(mag < float.Epsilon) {
				face.y += float.Epsilon;
				axis = AIVector3.Cross(face, actor.Facing);
			}
            axis.Normalize();
            axis *= AIVector3.Angle(face, actor.Facing);
            return new Kinematics(AIVector3.zero, axis * actor.MaxAngSpeed);
        }
    }
}
