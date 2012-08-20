using System;

namespace Craft.Net.Data
{
    /// <summary>
    /// Represents the location of an object in 3D space.
    /// </summary>
    public struct Vector3 : IEquatable<Vector3>
    {
        public double X, Y, Z;

        public Vector3(float value)
        {
            X = Y = Z = value;
        }

        public Vector3(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Converts this Vector3 to a string in the format &lt;x, y, z&gt;.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("<{0},{1},{2}>", X, Y, Z);
        }

        #region Math

        /// <summary>
        /// Truncates the decimal component of each part of this Vector3.
        /// </summary>
        public Vector3 Floor()
        {
            return new Vector3((int)X, (int)Y, (int)Z);
        }

        /// <summary>
        /// Calculates the distance between two Vector3 objects.
        /// </summary>
        public double DistanceTo(Vector3 other)
        {
            return Math.Sqrt(Math.Pow(other.Z - Z, 2) +
                             Math.Sqrt(Math.Pow(other.X - X, 2)) +
                             Math.Pow(other.Y - Y, 2));
        }

        #endregion

        #region Operators

        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return !a.Equals(b);
        }

        public static bool operator ==(Vector3 a, Vector3 b)
        {
            return a.Equals(b);
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.X + b.X,
                a.Y + b.Y,
                a.Z + b.Z);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.X - b.X,
                a.Y - b.Y,
                a.Z - b.Z);
        }

        public static Vector3 operator *(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.X*b.X,
                a.Y*b.Y,
                a.Z*b.Z);
        }

        public static Vector3 operator /(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.X/b.X,
                a.Y/b.Y,
                a.Z/b.Z);
        }

        #endregion

        #region Constants

        public static Vector3 Zero
        {
            get { return new Vector3(0); }
        }

        public static Vector3 One
        {
            get { return new Vector3(1); }
        }

        public static Vector3 Up
        {
            get { return new Vector3(0, 1, 0); }
        }

        public static Vector3 Down
        {
            get { return new Vector3(0, -1, 0); }
        }

        public static Vector3 Left
        {
            get { return new Vector3(-1, 0, 0); }
        }

        public static Vector3 Right
        {
            get { return new Vector3(1, 0, 0); }
        }

        public static Vector3 Backwards
        {
            get { return new Vector3(0, 0, -1); }
        }

        public static Vector3 Forwards
        {
            get { return new Vector3(0, 0, 1); }
        }

        public static Vector3 South
        {
            get { return new Vector3(0, 0, 1); }
        }

        public static Vector3 North
        {
            get { return new Vector3(0, 0, -1); }
        }

        public static Vector3 West
        {
            get { return new Vector3(-1, 0, 0); }
        }

        public static Vector3 East
        {
            get { return new Vector3(1, 0, 0); }
        }

        #endregion

        public bool Equals(Vector3 other)
        {
            return other.X == this.X && other.Y == this.Y && other.Z == this.Z;
        }
    }
}