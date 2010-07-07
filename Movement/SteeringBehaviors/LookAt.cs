using UnityEngine;
using AIEngine.Actors;

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

            Vector3 face = target.Position - actor.Position;
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
