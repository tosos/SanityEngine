using System;
using System.Collections.Generic;
using System.Text;
using AIEngine.Actors;
using AIEngine.Utility.Math;

namespace AIEngine.Movement.SteeringBehaviors
{
    /// <summary>
    /// Wander around in a random, yet natural, fashion.
    /// </summary>
    public class Wander : Seek
    {
        /// <summary>
        /// The current target point.
        /// </summary>
        public new Actor Target 
        {
            get { return target; }
        }

        float minCountdownTime = 1.0f;
        float maxCountdownTime = 2.0f;
        float maxDeviation = 0.2f;
        float wanderAngle = 0.0f;
        AIPointActor target;

        /// <summary>
        /// The minumum time until changing the wander target.
        /// </summary>
        public float MinCountdownTime
        {
            get { return minCountdownTime; }
            set { minCountdownTime = value; }
        }

        /// <summary>
        /// The maximum time until changing the wander target.
        /// </summary>
        public float MaxCountdownTime
        {
            get { return maxCountdownTime; }
            set { maxCountdownTime = value; }
        }

        /// <summary>
        /// The maximum deviation angle.
        /// </summary>
        public float MaxDeviation
        {
            get { return maxDeviation; }
            set { maxDeviation = value; }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Wander()
        {
            wanderAngle = AIRandom.Uniform() * 360.0f;
            target = new AIPointActor(AIVector3.zero);
            base.Target = target;
        }

        /// <summary>
        /// Update the wander behavior.
        /// </summary>
        /// <param name="actor">The actor being updated.</param>
        /// <param name="dt">The time since the last update, in seconds.
        /// </param>
        /// <returns>The kinematics object.</returns>
        public override Kinematics Update(Actor actor, float dt)
        {
            float angle = AIRandom.Uniform(-maxDeviation, maxDeviation);
            wanderAngle += angle;
            int radius = 200;
            AIVector3 currPos = actor.Position;
            float x = currPos.x + radius * (float)Math.Cos(wanderAngle);
            float z = currPos.z + radius * (float)Math.Sin(wanderAngle);
            target.Point = new AIVector3(x, currPos.y, z);
            return base.Update(actor, dt);
        }
    }
}
