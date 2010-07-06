using System;
using System.Collections.Generic;
using System.Text;

namespace AIEngine.Utility.Math
{
    /// <summary>
    /// Simple 3D Vector.
    /// </summary>
    public struct AIVector3
    {
        /// <summary>
        /// The zero vector.
        /// </summary>
        public static readonly AIVector3 zero = new AIVector3(0f, 0f, 0f);

        /// <summary>
        /// The x-coordinate.
        /// </summary>
        public float x;

        /// <summary>
        /// The y-coordinate.
        /// </summary>
        public float y;

        /// <summary>
        /// The z-coordinate.
        /// </summary>
        public float z;

        /// <summary>
        /// The magnitude of the vector, squared.
        /// </summary>
        public float SqrMagnitude
        {
            get { return x * x + y * y + z * z; }
        }

        /// <summary>
        /// The magnitude of the vector.
        /// </summary>
        public float Magnitude
        {
            get { return (float)System.Math.Sqrt(x * x + y * y + z * z); }
        }

        /// <summary>
        /// Create a vector.
        /// </summary>
        /// <param name="x">The x-coordinate</param>
        /// <param name="y">The y-coordinate</param>
        /// <param name="z">The z-coordinate</param>
        public AIVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Set the length of the vector to 1.
        /// </summary>
        public void Normalize()
        {
            this /= Magnitude;
        }

        /// <summary>
        /// Add two vectors.
        /// </summary>
        /// <param name="lhs">The first vector.</param>
        /// <param name="rhs">The second vector.</param>
        /// <returns>The resulting vector.</returns>
        public static AIVector3 operator +(AIVector3 lhs, AIVector3 rhs)
        {
            return new AIVector3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        }

        /// <summary>
        /// Take the difference of two vectors.
        /// </summary>
        /// <param name="lhs">The first vector.</param>
        /// <param name="rhs">The second vector.</param>
        /// <returns>The resulting vector.</returns>
        public static AIVector3 operator -(AIVector3 lhs, AIVector3 rhs)
        {
            return new AIVector3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        }

        /// <summary>
        /// Negate a vector.
        /// </summary>
        /// <param name="operand">The vector.</param>
        /// <returns>The resulting vector.</returns>
        public static AIVector3 operator -(AIVector3 operand)
        {
            return new AIVector3(-operand.x, -operand.y, -operand.z);
        }

        /// <summary>
        /// Multiply a vector by a scalar.
        /// </summary>
        /// <param name="lhs">The vector.</param>
        /// <param name="rhs">The scalar.</param>
        /// <returns>The resulting vector.</returns>
        public static AIVector3 operator *(AIVector3 lhs, float rhs)
        {
            return new AIVector3(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
        }

        /// <summary>
        /// Multiply a vector by a scalar.
        /// </summary>
        /// <param name="lhs">The scalar.</param>
        /// <param name="rhs">The vector.</param>
        /// <returns>The resulting vector.</returns>
        public static AIVector3 operator *(float lhs, AIVector3 rhs)
        {
            return new AIVector3(rhs.x * lhs, rhs.y * lhs, rhs.z * lhs);
        }

        /// <summary>
        /// Divide a vector by a scalar.
        /// </summary>
        /// <param name="lhs">The vector.</param>
        /// <param name="rhs">The scalar.</param>
        /// <returns>The resulting vector.</returns>
        public static AIVector3 operator /(AIVector3 lhs, float rhs)
        {
            return new AIVector3(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
        }

        /// <summary>
        /// Linearly interpolate between two vectors.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <param name="param">The parameter (0 to 1)</param>
        /// <returns>The resulting vector.</returns>
        public static AIVector3 Lerp(AIVector3 v1, AIVector3 v2, float param)
        {
            return v1 * (1.0f - param) + v2 * param;
        }

        /// <summary>
        /// Calculate the scalar(dot) product of two vectors.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>The scalar product.</returns>
        public static float Dot(AIVector3 v1, AIVector3 v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }

        /// <summary>
        /// Calculate the vector(cross) product of two vectors.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>The vector product.</returns>
        public static AIVector3 Cross(AIVector3 v1, AIVector3 v2)
        {
            float x = v1.y * v2.z - v1.z * v2.y;
            float y = v1.z * v2.x - v1.x * v2.z;
            float z = v1.x * v2.y - v1.y * v2.x;
            return new AIVector3(x, y, z);
        }

        /// <summary>
        /// Calculate the angle between two vectors.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>The angle in degrees.</returns>
        public static float Angle(AIVector3 v1, AIVector3 v2)
        {
            v1.Normalize();
            v2.Normalize();
            return (float)(System.Math.Acos(AIVector3.Dot(v1, v2)) / System.Math.PI) * 180.0f;
        }

        /// <summary>
        /// String representation of the vector, for debugging purposes.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return "(" + x + ", " + y + ", " + z +")";
        }
    }
}
