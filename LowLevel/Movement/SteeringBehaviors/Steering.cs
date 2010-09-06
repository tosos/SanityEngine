using UnityEngine;

namespace SanityEngine.Movement.SteeringBehaviors
{
	public struct Steering
	{
		public static readonly Steering zero = new Steering(false, Vector3.zero,
			false, Vector3.zero);
		
		public Vector3 Force;
		public bool HasForce;
		public Vector3 Torque;
		public bool HasTorque;
		
		public Steering(bool hasForce, Vector3 force, bool hasTorque,
			Vector3 torque)
		{
			this.Force = force;
			this.HasForce = hasForce;
			this.Torque = torque;
			this.HasTorque = hasTorque;
		}
		
		public static Steering operator*(Steering lhs, float rhs)
		{
			return new Steering(lhs.HasForce, lhs.Force * rhs,
				lhs.HasTorque, lhs.Torque * rhs);
		}
		
		public static Steering operator+(Steering lhs, Steering rhs)
		{
			return new Steering(lhs.HasForce | rhs.HasForce,
				lhs.Force + rhs.Force, lhs.HasTorque | rhs.HasTorque,
				lhs.Torque + rhs.Torque);
		}
	}
}
