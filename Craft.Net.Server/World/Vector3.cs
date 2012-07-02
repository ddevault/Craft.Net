using System;

namespace Craft.Net.Server.World
{
    public struct Vector3
    {
        public float X, Y, Z;

        public Vector3(float Value)
        {
            X = Y = Z = Value;
        }
    }
}

