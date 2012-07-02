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

        public static Vector3 Zero
        {
            get
            {
                return new Vector3(0);
            }
        }
    }
}

