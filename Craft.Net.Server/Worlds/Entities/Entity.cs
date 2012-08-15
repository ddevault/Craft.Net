namespace Craft.Net.Server.Worlds.Entities
{
    public abstract class Entity
    {
        public Dimension Dimension;
        public int Id;

        public Vector3 OldPosition;
        public bool OnGround;
        public float Pitch;
        public float Yaw;
        public Vector3 _Position;

        public Vector3 Position
        {
            get { return _Position; }
            set
            {
                OldPosition = _Position;
                _Position = value;
            }
        }

        public abstract Size Size { get; }
    }
}