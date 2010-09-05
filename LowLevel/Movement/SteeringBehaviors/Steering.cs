using UnityEngine;

namespace SanityEngine.Movement.SteeringBehaviors
{
	public struct Steering
	{
		public static readonly Steering zero = new Steering(Vector3.zero,
			Vector3.zero);
		
		public Vector3 Force;
		public Vector3 Torque;
		
		public Steering(Vector3 force, Vector3 torque)
		{
			this.Force = force;
			this.Torque = torque;
		}
		
		public static Steering operator*(Steering lhs, float rhs)
		{
			return new Steering(lhs.Force * rhs, lhs.Torque * rhs);
		}

		public static Steering operator/(Steering lhs, float rhs)
		{
			return new Steering(lhs.Force / rhs, lhs.Torque / rhs);
		}
		
		public static Steering operator+(Steering lhs, Steering rhs)
		{
			return new Steering(lhs.Force + rhs.Force, lhs.Torque + rhs.Torque);
		}
	}
}
