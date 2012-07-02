using System;

namespace Craft.Net.Server.Worlds
{
    public struct Vector3
    {
        public float X, Y, Z;

        public Vector3(float Value)
        {
            X = Y = Z = Value;
        }

        public Vector3(float X, float Y, float Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public Vector3 Floor()
        {
            return new Vector3((int)X, (int)Y, (int)Z);
        }

        public static Vector3 Zero
        {
            get
            {
                return new Vector3(0);
            }
        }
    }
}

