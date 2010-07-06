using System;
using System.Collections.Generic;
using System.Text;
using AIEngine.Utility.Math;

namespace AIEngine.Movement.SteeringBehaviors
{
    /// <summary>
    /// Kinematics for steering behaviors.
    /// </summary>
    public struct Kinematics
    {
        /// <summary>
        /// Zero velocity kinematics object.
        /// </summary>
        public static Kinematics zero = new Kinematics(AIVector3.zero, AIVector3.zero);

        /// <summary>
        /// The desired force vector.
        /// </summary>
        public readonly AIVector3 Force;
        
        /// <summary>
        /// The desired torque vector.
        /// </summary>
        public readonly AIVector3 Torque;

        /// <summary>
        /// Create a kinematics object.
        /// </summary>
        /// <param name="force">The desired force vector.</param>
        /// <param name="torque">The desired torque vector.</param>
        public Kinematics(AIVector3 force, AIVector3 torque)
        {
            this.Force = force;
            this.Torque = torque;
        }

        /// <summary>
        /// Add two kinematics objects together.
        /// </summary>
        /// <param name="lhs">The left hand side.</param>
        /// <param name="rhs">The right hand side.</param>
        /// <returns>Thew new kinematics object.</returns>
        public static Kinematics operator +(Kinematics lhs, Kinematics rhs)
        {
            return new Kinematics(lhs.Force + rhs.Force, lhs.Torque + rhs.Torque);
        }

        /// <summary>
        /// Multiply a kinematics vectors by a constant.
        /// </summary>
        /// <param name="lhs">The kinematics object.</param>
        /// <param name="rhs">The constant.</param>
        /// <returns>The new kinematics object.</returns>
        public static Kinematics operator *(Kinematics lhs, float rhs)
        {
            return new Kinematics(lhs.Force * rhs, lhs.Torque * rhs);
        }

        /// <summary>
        /// Multiply a kinematics vectors by a constant.
        /// </summary>
        /// <param name="lhs">The constant.</param>
        /// <param name="rhs">The kinematics object.</param>
        /// <returns>The new kinematics object.</returns>
        public static Kinematics operator *(float lhs, Kinematics rhs)
        {
            return new Kinematics(rhs.Force * lhs, rhs.Torque * lhs);
        }

        /// <summary>
        /// Divide a kinematics vectors by a constant.
        /// </summary>
        /// <param name="lhs">The kinematics object.</param>
        /// <param name="rhs">The constant.</param>
        /// <returns>The new kinematics object.</returns>
        public static Kinematics operator /(Kinematics lhs, float rhs)
        {
            return new Kinematics(lhs.Force / rhs, lhs.Torque / rhs);
        }
    }
}
