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
    public class ObjectAvoidance : SteeringBehavior
    {
		public int NumRays
		{
			get { return numRays; }
			set { numRays = value; }
		}
		
		public float MaxAngle
		{
			get { return maxAngle; }
			set { maxAngle = value; }
		}
		
		public float MaxDistance
		{
			get { return maxDistance; }
			set { maxDistance = value; }
		}
		
		public float MinDistance
		{
			get { return minDistance; }
			set { minDistance = value; }
		}
		
		public int Mask
		{
			get { return mask; }
			set { mask = value; }
		}
		
		int numRays = 5;
		float maxAngle = 15f;
		float maxDistance = 5f;
		float minDistance = 3f;
		int mask = -1;

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
			Steering steering = Steering.zero;
		
						
			Vector3 forward = actor.Facing;
			forward.y = 0f;
			float size = (maxAngle * 2f) / (numRays - 1);
			float dHalf = (maxDistance - minDistance) * 0.5f;
			float dSize = (maxDistance - minDistance) / (numRays - 1);
			float totalWeight = 0f;
			Vector3 force = Vector3.zero;
			for(int i = 0; i < numRays; i ++) {
				float angle = -maxAngle + i * size;
				
				float weight = 1f - Mathf.Abs(angle / maxAngle) * 0.25f;
				float dist = maxDistance - Mathf.Abs(-dHalf + i * dSize);
				Vector3 dir = Quaternion.AngleAxis(angle, Vector3.up) * forward;
				RaycastHit hit;
				if(Physics.Raycast(actor.Position, dir, out hit, dist, mask)) {
					totalWeight += weight;
					float scale = 1f - hit.distance / maxDistance;
					force += hit.normal * manager.MaxForce * scale * weight;
				}
			}

			steering.Force = force / (totalWeight > 0f ? totalWeight : 1f);
			
			return steering;
        }

        public override string GetDescription()
        {
        	return "Avoid objects detected by forward facing rays";
        }
    }
}
