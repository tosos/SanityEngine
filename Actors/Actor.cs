using System;
using UnityEngine;

namespace AIEngine.Actors
{
    /// <summary>
    /// Interface for all entities in the world. These could be the
    /// player(s), agents, or even just a point in space (i.e. a target).
    /// </summary>
    public interface Actor
    {
        /// <summary>
        /// This actor's velocity.
        /// </summary>
        Vector3 Velocity
        {
            get;
        }

        /// <summary>
        /// The position of the actor.
        /// </summary>
        Vector3 Position
        {
            get;
        }

        /// <summary>
        /// The vector pointing in the direction the actor is facing.
        /// </summary>
        Vector3 Facing
        {
            get;
        }

        /// <summary>
        /// The actor's maximum speed.
        /// </summary>
        float MaxSpeed
        {
            get;
        }

        /// <summary>
        /// The actor's maximum angular speed.
        /// </summary>
        float MaxAngSpeed
        {
            get;
        }

        /// <summary>
        /// Set a property on the given actor.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The string value of the property.</param>
        void SetProperty(string name, string value);

        /// <summary>
        /// Set a boolean property. This is likely some condition, such as
        /// 'is_dead'.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        void SetBoolProperty(string name);

        /// <summary>
        /// Clear a boolean property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        void ClearBoolProperty(string name);

        /// <summary>
        /// Get the value of a property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>The string value of the property.</returns>
        string GetProperty(string name);

        /// <summary>
        /// Check if a boolean property is set.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns><code>true</code> if the property is set</returns>
        bool HasBoolProperty(string name);
    }
}
