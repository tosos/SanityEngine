using UnityEngine;
using AIEngine.Actors;

namespace AIEngine.Movement.SteeringBehaviors
{
    /// <summary>
    /// Seek toward the given target.
    /// </summary>
    public class Seek : SteeringBehavior
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
        /// Update the seek behavior.
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

            return new Kinematics(SteerToward(actor, target.Position, dt), Vector3.zero);
        }
    }
}
