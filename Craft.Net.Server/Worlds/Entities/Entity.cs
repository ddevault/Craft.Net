using System;

namespace Craft.Net.Server.Worlds.Entities
{
    public class Entity
    {
        public int Id;
        public Vector3 _Position;
        public Vector3 Position
        {
            get
            {
                return _Position;
            }
            set
            {
                OldPosition = _Position;
                _Position = value;
            }
        }
        public Vector3 OldPosition;
        public float Yaw, Pitch;
        public bool OnGround;
        public Dimension Dimension;

        public Entity()
        {
        }
    }
}

