using System;
using System.Threading;
using Craft.Net.Common;
using Craft.Net.Physics;

namespace Craft.Net.Client
{
    public partial class MinecraftClient : IAABBEntity
    {
        #region Constants
        public float AccelerationDueToGravity
        {
            // 32 blocks per second squared / ( ticks per second) = x blocks per tick squared
            get { return 0.08f;}
        }

        public float Drag
        {
            get { return 1 - 0.02f;}
        }

        private static Size _size = new Size(0.8, 1.6, 0.8);

        public Size Size
        {
            get
            {
                return new Size(_size);
            }
        }
        #endregion

        private object _velocityLock = new object();
        private Vector3 _velocity = new Vector3();
        public Vector3 Velocity
        {
            get
            {
                Vector3 ret;
                lock (_velocityLock)
                {
                    ret = _velocity;
                }
                return ret;
            }

            set
            {
                lock (_velocityLock)
                {
                    _velocity = value;
                }
            }
        }

        private static BoundingBox _boundingBox = new BoundingBox(new Vector3(0), new Vector3(_size.Width, _size.Height, _size.Depth));
        // Make the magic in PhysicsEngine work
        private static Vector3 _positionOffset = new Vector3(_size.Width / 2, _size.Height / 2 , _size.Depth / 2);

        public BoundingBox BoundingBox
        {
            get
            {
                BoundingBox box;
                lock (_positionLock)
                {
                    box = _boundingBox.OffsetBy(_position - _positionOffset);
                }
                return box;
            }
        }

        public bool BeginUpdate()
        {
            return true;
        }

        public void EndUpdate(Vector3 newPosition)
        {
            Position = newPosition;
        }
        
        public void TerrainCollision(PhysicsEngine engine, Vector3 collisionPoint, Vector3 collisionDirection)
        {
            if (collisionDirection == Vector3.Down)
            {
                _onGround = true;
            }
        }
    }
}