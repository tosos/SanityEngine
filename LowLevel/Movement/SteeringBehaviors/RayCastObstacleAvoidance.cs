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
    public class RayCastObstacleAvoidance : SteeringBehavior
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
		LookAt lookAt = new LookAt();
		PointActor lookTarget;
		
		public RayCastObstacleAvoidance()
		{
			lookTarget = new PointActor(Vector3.zero);
			lookAt.Target = lookTarget;
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
			Steering steering = Steering.zero;
		
			Vector3 forward = actor.Forward;
			forward.y = 0f;
			float size = (maxAngle * 2f) / (numRays - 1);
			float dHalf = (maxDistance - minDistance) * 0.5f;
			float dSize = (maxDistance - minDistance) / (numRays - 1);
			float totalWeight = 0f;
			Vector3 force = Vector3.zero;
			Vector3 torque = Vector3.zero;
			for(int i = 0; i < numRays; i ++) {
				float angle = -maxAngle + i * size;
				
				float weight = 1f - Mathf.Abs(angle / maxAngle) * 0.25f;
				float dist = maxDistance - Mathf.Abs(-dHalf + i * dSize);
				Vector3 dir = Quaternion.AngleAxis(angle, Vector3.up) * forward;
				Debug.DrawLine(actor.Position, actor.Position + dir * dist, new Color(1f, 1f, 0f, weight));
                Collider[] col = Physics.OverlapSphere(actor.Position + dir * dist * .5f, 
                                                       dist * .5f, mask);
				if(col.Length > 0) {
                    RaycastHit hit;
                    Ray r = new Ray (actor.Position, col[0].transform.position - actor.Position);
                    col[0].Raycast (r, out hit, dist);
					float side = Vector3.Dot(hit.normal, actor.Right);
					Vector3 tangent = Vector3.zero;
					Vector3 half = Vector3.zero;
					if(side < 0f) { // Normal is to the left
						tangent = Vector3.Cross(actor.Up, hit.normal);
						half = tangent - actor.Right;
					} else { // Normal is to the right
						tangent = Vector3.Cross(hit.normal, actor.Up);
						half = actor.Right - tangent;
					}
					// tangent.Normalize();
					// half.Normalize();
					totalWeight += weight;
					float scale = 1f - hit.distance / maxDistance;
					Debug.DrawLine(hit.point, hit.point + tangent * manager.MaxForce * scale * weight, new Color(0f, 1f, 1f, scale));
					Debug.DrawLine(hit.point, hit.point + half * manager.MaxForce * scale * weight, new Color(1f, 0f, 0f, scale));
					lookTarget.Point = hit.point + tangent;
					torque += lookAt.Update(manager, actor, dt).Torque * scale * weight * 0.5f;
					force += half * manager.MaxForce * scale * weight;
				}
			}

			steering.Force = force / (totalWeight > 0f ? totalWeight : 1f);
			steering.Torque = torque;
			
			return steering;
        }

        public override string GetDescription()
        {
        	return "Avoid objects detected by forward facing rays";
        }
    }
}
