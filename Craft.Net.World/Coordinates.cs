using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.World
{
    public struct Coordinates
    {
        public int X, Y;

        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Coordinates operator +(Coordinates a, Coordinates b)
        {
            return new Coordinates(a.X + b.X, a.Y + b.Y);
        }

        public static Coordinates operator -(Coordinates a, Coordinates b)
        {
            return new Coordinates(a.X - b.X, a.Y - b.Y);
        }

        public static Coordinates operator *(Coordinates a, Coordinates b)
        {
            return new Coordinates(a.X * b.X, a.Y * b.Y);
        }

        public static Coordinates operator /(Coordinates a, Coordinates b)
        {
            return new Coordinates(a.X / b.X, a.Y / b.Y);
        }
    }
}
