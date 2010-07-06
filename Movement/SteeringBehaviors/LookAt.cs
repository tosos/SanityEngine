using System;
using System.Collections.Generic;
using System.Text;
using AIEngine.Actors;
using AIEngine.Utility.Math;

namespace AIEngine.Movement.SteeringBehaviors
{
    /// <summary>
    /// Look at the target.
    /// </summary>
    public class LookAt : SteeringBehavior
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
        /// Update the look at behavior.
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

            AIVector3 face = target.Position - actor.Position;
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
