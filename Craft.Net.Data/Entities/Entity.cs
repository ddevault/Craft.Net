namespace Craft.Net.Data.Entities
{
    public abstract class Entity
    {
        public Dimension Dimension;
        public int Id;

        public Vector3 OldPosition;
        public bool OnGround;
        public float Pitch;
        public float Yaw;
        public Vector3 Position;

        public abstract Size Size { get; }
    }
}