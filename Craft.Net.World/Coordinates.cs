using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.World
{
    public struct Coordinates
    {
        public int X, Z;

        public Coordinates(int x, int z)
        {
            X = x;
            Z = z;
        }

        public static Coordinates operator +(Coordinates a, Coordinates b)
        {
            return new Coordinates(a.X + b.X, a.Z + b.Z);
        }

        public static Coordinates operator -(Coordinates a, Coordinates b)
        {
            return new Coordinates(a.X - b.X, a.Z - b.Z);
        }

        public static Coordinates operator *(Coordinates a, Coordinates b)
        {
            return new Coordinates(a.X * b.X, a.Z * b.Z);
        }

        public static Coordinates operator /(Coordinates a, Coordinates b)
        {
            return new Coordinates(a.X / b.X, a.Z / b.Z);
        }
    }
}
