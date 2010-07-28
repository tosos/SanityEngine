//
// Copyright (C) 2010 The Sanity Engine Development Team
//
// This source code is licensed under the terms of the
// MIT License.
//
// For more information, see the file LICENSE

using UnityEngine;
using System.Collections.Generic;
using SanityEngine.Actors;

namespace SanityEngine.Movement.SteeringBehaviors
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
		PointActor target;

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
            wanderAngle = Random.Range(0f, 360f);
            target = new PointActor(Vector3.zero);
            base.Target = target;
        }

        /// <summary>
        /// Update the wander behavior.
        /// </summary>
        /// <param name="actor">The actor being updated.</param>
        /// <param name="dt">The time since the last update, in seconds.
        /// </param>
        /// <returns>The kinematics object.</returns>
        public override Vector3 Update(Actor actor, float dt)
        {
            float angle = Random.Range(-maxDeviation, maxDeviation);
            wanderAngle += angle;
            int radius = 200;
            Vector3 currPos = actor.Position;
            float x = currPos.x + radius * Mathf.Cos(wanderAngle);
            float z = currPos.z + radius * Mathf.Sin(wanderAngle);
            target.Point = new Vector3(x, currPos.y, z);
            return base.Update(actor, dt);
        }
    }
}
