namespace Craft.Net.Data.Entities
{
    public abstract class Entity
    {
        public Dimension Dimension;
        public int Id;
        public Vector3 OldPosition;
        public bool OnGround;
        public Vector3 Position;

        private float _Pitch;
        public float OldPitch;
        public float Pitch
        {
            get
            {
                return _Pitch;
            }
            set
            {
                OldPitch = _Pitch;
                _Pitch = value;
            }
        }

        private float _Yaw;
        public float OldYaw;
        public float Yaw
        {
            get
            {
                return _Yaw;
            }
            set
            {
                OldYaw = _Yaw;
                _Yaw = value;
            }
        }

        public abstract Size Size { get; }
    }
}