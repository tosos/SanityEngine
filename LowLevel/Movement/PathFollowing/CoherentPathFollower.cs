//
// Copyright (C) 2010 The Sanity Engine Development Team
//
// This source code is licensed under the terms of the
// MIT License.
//
// For more information, see the file LICENSE

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using SanityEngine.Structure.Path;
using SanityEngine.Structure.Graph;
using SanityEngine.Structure.Graph.NavMesh;

namespace SanityEngine.Movement.PathFollowing
{
    /// <summary>
    /// A coherent path follower. This path follower ensures that the actor
    /// does not backtrack on the path, nor seek too far forward.
    /// </summary>
    /// <typeparam name="TID">The node ID type.</typeparam>
    public class CoherentPathFollower<TNode, TEdge>
        where TNode : NavMeshNode<TNode, TEdge>
        where TEdge : Edge<TNode, TEdge>
    {
		List<Segment> segments = new List<Segment>();
		float totalLength = 0.0f;
		
		struct Segment
		{
			public Vector3 origin;
			public Vector3 dir;
			public float start;
			public float end;
		}
		
        /// <summary>
        /// Create a path follower.
        /// </summary>
        public CoherentPathFollower()
        {
        }
		
		/// <summary>
		/// Sets the path.
		/// </summary>
		/// <param name="newPath">The new path.</param>
		public void SetPath(Path<TNode, TEdge> path)
        {
			segments.Clear();
			totalLength = 0.0f;
			for(int i = 1;path != null && i < path.StepCount; i ++) {
                TEdge edge = path.GetStep(i);
				Segment s = new Segment();
				Vector3 pos1 = edge.Source.Position;
                Vector3 pos2 = edge.Target.Position;
				s.origin = pos1;
				s.dir = Vector3.Normalize(pos2 - pos1);
				s.start = totalLength;
                totalLength += (pos2 - pos1).magnitude;
				s.end = totalLength;
				segments.Add(s);
			}
        }

        /// <summary>
        /// Get the next parameter based on the agent's position.
        /// </summary>
        /// <param name="pos">The agent's position.</param>
        /// <returns>The next target parameter.</returns>
        /// <seealso cref="GetPosition"/>
        public float GetNextParameter(Vector3 pos, float min, float max)
        {
			float nearestDist = Mathf.Infinity;
			float nearestParam = Mathf.Min(min, totalLength);
        	foreach(Segment s in segments) {
            	if(s.end < min) {
            	    continue;
				}
        	    if(s.start > max) {
    	            break;
				}
	            float p0 = Mathf.Max(s.start, min) - s.start;
            	float p1 = Mathf.Min(s.end, max) - s.start;
        	    Vector3 v = pos - s.origin;
    	        float proj = Vector3.Dot(s.dir, v);
	            float param = s.start + proj;
            	if(param < s.start || param > s.end) {
        	        continue;
				}
    	        proj = Mathf.Max(p0, Mathf.Min(p1, proj));
	            Vector3 projPoint = s.origin + s.dir * proj;
            	float dist = Vector3.Distance(pos, projPoint);
        	    if(dist < nearestDist) {
    	            nearestDist = dist;
					nearestParam = s.start + proj;
				}
			}
	        return nearestParam;

        }

        /// <summary>
        /// Get a world position based on the given parameter. This parameter
        /// is normally calculated using GetNextParameter().
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns>The world position.</returns>
        /// <seealso cref="GetNextParameter"/>
        public Vector3 GetPosition(float param)
        {
	        foreach(Segment s in segments) {
    	        if(param < s.start || param > s.end) {
        	        continue;
				}
            	float p = param - s.start;
	            return s.origin + p * s.dir;
			}
    	    Segment seg = segments[segments.Count - 1];
        	return seg.origin + (seg.end - seg.start) * seg.dir;
        }
    }
}
