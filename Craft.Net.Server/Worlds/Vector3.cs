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

        #region Constants

        public static Vector3 Zero
        {
            get
            {
                return new Vector3(0);
            }
        }

        public static Vector3 One
        {
            get
            {
                return new Vector3(1);
            }
        }

        public static Vector3 Up
        {
            get
            {
                return new Vector3(0, 1, 0);
            }
        }

        public static Vector3 Down
        {
            get
            {
                return new Vector3(0, -1, 0);
            }
        }

        public static Vector3 Left
        {
            get
            {
                return new Vector3(-1, 0, 0);
            }
        }

        public static Vector3 Right
        {
            get
            {
                return new Vector3(1, 0, 0);
            }
        }

        public static Vector3 Backwards
        {
            get
            {
                return new Vector3(0, 0, -1);
            }
        }

        public static Vector3 Forwards
        {
            get
            {
                return new Vector3(0, 0, 1);
            }
        }

        public static Vector3 South
        {
            get
            {
                return new Vector3(0, 0, 1);
            }
        }

        public static Vector3 North
        {
            get
            {
                return new Vector3(0, 0, -1);
            }
        }

        public static Vector3 West
        {
            get
            {
                return new Vector3(-1, 0, 0);
            }
        }

        public static Vector3 East
        {
            get
            {
                return new Vector3(1, 0, 0);
            }
        }

        #endregion
    }
}

