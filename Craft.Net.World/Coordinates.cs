using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.World
{
    public struct Coordinates2D
    {
        public int X, Z;

        public Coordinates2D(int x, int z)
        {
            X = x;
            Z = z;
        }

        public static Coordinates2D operator +(Coordinates2D a, Coordinates2D b)
        {
            return new Coordinates2D(a.X + b.X, a.Z + b.Z);
        }

        public static Coordinates2D operator -(Coordinates2D a, Coordinates2D b)
        {
            return new Coordinates2D(a.X - b.X, a.Z - b.Z);
        }

        public static Coordinates2D operator *(Coordinates2D a, Coordinates2D b)
        {
            return new Coordinates2D(a.X * b.X, a.Z * b.Z);
        }

        public static Coordinates2D operator /(Coordinates2D a, Coordinates2D b)
        {
            return new Coordinates2D(a.X / b.X, a.Z / b.Z);
        }

        public static Coordinates2D operator %(Coordinates2D a, Coordinates2D b)
        {
            return new Coordinates2D(a.X % b.X, a.Z % b.Z);
        }

        public static bool operator !=(Coordinates2D a, Coordinates2D b)
        {
            return !a.Equals(b);
        }

        public static bool operator ==(Coordinates2D a, Coordinates2D b)
        {
            return a.Equals(b);
        }

        public static explicit operator Coordinates2D(Coordinates3D a)
        {
            return new Coordinates2D(a.X, a.Z);
        }

        public override string ToString()
        {
            return string.Format("<{0},{2}>", X, Z);
        }

        public bool Equals(Coordinates2D other)
        {
            return other.X.Equals(X) && other.Z.Equals(Z);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Coordinates2D)) return false;
            return Equals((Coordinates2D)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = X.GetHashCode();
                result = (result * 397) ^ Z.GetHashCode();
                return result;
            }
        }
    }

    public struct Coordinates3D
    {
        public int X, Y, Z;

        public Coordinates3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Coordinates3D operator +(Coordinates3D a, Coordinates3D b)
        {
            return new Coordinates3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Coordinates3D operator -(Coordinates3D a, Coordinates3D b)
        {
            return new Coordinates3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Coordinates3D operator *(Coordinates3D a, Coordinates3D b)
        {
            return new Coordinates3D(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }

        public static Coordinates3D operator /(Coordinates3D a, Coordinates3D b)
        {
            return new Coordinates3D(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        }

        public static Coordinates3D operator %(Coordinates3D a, Coordinates3D b)
        {
            return new Coordinates3D(a.X % b.X, a.Y % b.Y, a.Z % b.Z);
        }

        public static bool operator !=(Coordinates3D a, Coordinates3D b)
        {
            return !a.Equals(b);
        }

        public static bool operator ==(Coordinates3D a, Coordinates3D b)
        {
            return a.Equals(b);
        }

        public static explicit operator Coordinates3D(Coordinates2D a)
        {
            return new Coordinates3D(a.X, 0, a.Z);
        }

        public override string ToString()
        {
            return string.Format("<{0},{1},{2}>", X, Y, Z);
        }

        public bool Equals(Coordinates3D other)
        {
            return other.X.Equals(X) && other.Y.Equals(Y) && other.Z.Equals(Z);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Coordinates3D)) return false;
            return Equals((Coordinates3D)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = X.GetHashCode();
                result = (result * 397) ^ Y.GetHashCode();
                result = (result * 397) ^ Z.GetHashCode();
                return result;
            }
        }
    }
}
