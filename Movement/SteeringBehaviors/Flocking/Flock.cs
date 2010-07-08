using System;
using System.Collections.Generic;
using SanityEngine.Actors;

namespace SanityEngine.Movement.SteeringBehaviors.Flocking
{
    /// <summary>
    /// Shareable flock object.
    /// </summary>
    public class Flock
    {
        List<Actor> flock;
        IList<Actor> readOnlyFlock;

        /// <summary>
        /// The members of the flock.
        /// </summary>
        public IList<Actor> Members
        {
            get { return readOnlyFlock; }
        }

        /// <summary>
        /// Create a new flock.
        /// </summary>
        public Flock()
        {
            flock = new List<Actor>();
            readOnlyFlock = flock.AsReadOnly();
        }

        /// <summary>
        /// Add an actor to the flock.
        /// </summary>
        /// <param name="actor">The actor to add.</param>
        public void AddToFlock(Actor actor)
        {
            flock.Add(actor);
        }

        /// <summary>
        /// Remove an actor from the flock.
        /// </summary>
        /// <param name="actor"></param>
        public void RemoveFromFlock(Actor actor)
        {
            flock.Remove(actor);
        }
    }
}
