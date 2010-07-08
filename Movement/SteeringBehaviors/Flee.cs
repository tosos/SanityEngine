using UnityEngine;
using SanityEngine.Actors;

namespace SanityEngine.Movement.SteeringBehaviors
{
    /// <summary>
    /// Flee from the given target.
    /// </summary>
    public class Flee : SteeringBehavior
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
        /// Update the flee behavior.
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

            return new Kinematics(SteerAway(actor, target.Position, dt), Vector3.zero);
        }
    }
}
