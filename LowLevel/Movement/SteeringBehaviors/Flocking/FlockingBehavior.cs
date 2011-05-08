//
// Copyright (C) 2010 The Sanity Engine Development Team
//
// This source code is licensed under the terms of the
// MIT License.
//
// For more information, see the file LICENSE

using UnityEngine;
using SanityEngine.Actors;

namespace SanityEngine.Movement.SteeringBehaviors.Flocking
{
    /// <summary>
    /// Base class for flock behaviors.
    /// </summary>
    public abstract class FlockingBehavior : SteeringBehavior
    {
        Flock flock = null;

        /// <summary>
        /// The entire flock.
        /// </summary>
        public Flock Flock
        {
            get { return flock; }
			set { flock = value; }
        }
		
		public sealed override Steering Update (SteeringManager manager, Actor actor, float dt)
		{
			if(flock == null) {
				return Steering.zero;
			}
			
			return FlockingUpdate(manager, actor, dt);
		}
		
		protected abstract Steering FlockingUpdate (SteeringManager manager, Actor actor, float dt);
    }
}
