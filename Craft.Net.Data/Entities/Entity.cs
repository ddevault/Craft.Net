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

        private Vector3 position;
        public Vector3 Position
        {
            get { return position; }
            set
            {
                OldPosition = position;
                position = value;
            }
        }

        public abstract Size Size { get; }
    }
}