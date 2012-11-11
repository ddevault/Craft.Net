using System;
using System.Collections.Generic;
using System.ComponentModel;
using Craft.Net.Data.Blocks;
using Craft.Net.Data.Events;
using Craft.Net.Data.Metadata;

namespace Craft.Net.Data.Entities
{
    public abstract class Entity : INotifyPropertyChanged
    {
        protected Entity()
        {
            Fire = -20;
        }

        #region State

        public int Id { get; set; }
        public Vector3 OldPosition { get; set; }
        public DateTime LastPositionUpdate { get; set; }
        public Vector3 Position
        {
            get { return position; }
            set
            {
                OldPosition = position;
                LastPositionUpdate = DateTime.Now;
                position = value;
                OnPropertyChanged("Position");
            }
        }

        /// <summary>
        /// In meters per tick
        /// </summary>
        public Vector3 Velocity
        {
            get { return velocity; }
            set
            {
                velocity = value;
                OnPropertyChanged("Velocity");
            }
        }

        public int FallDistance { get; set; }
        // The Y location that falling began at
        internal double FallStart { get; set; }
        /// <summary>
        /// The number of ticks that remain before an entity
        /// on fire is put out. Negative values are indicitive
        /// of how long the entity may stand in a fire-creating
        /// block before catching fire.
        /// </summary>
        public int Fire
        {
            get { return fire; }
            set
            {
                fire = value;
                OnPropertyChanged("Fire");
            }
        }

        public bool IsOnFire
        {
            get { return Fire > 0; }
        }

        public bool OnGround
        {
            get { return onGround; }
            set
            {
                onGround = value;
                OnPropertyChanged("OnGround");
            }
        }

        public Dimension Dimension
        {
            get { return dimension; }
            set
            {
                dimension = value;
                OnPropertyChanged("Dimension");
            }
        }

        private float pitch;
        public float OldPitch { get; set; }
        public float Pitch
        {
            get
            {
                return pitch;
            }
            set
            {
                OldPitch = pitch;
                pitch = value;
                OnPropertyChanged("Pitch");
            }
        }

        private Dimension dimension;
        private bool onGround;
        private int fire;
        private Vector3 position;
        private Vector3 velocity;
        private float yaw;
        public float OldYaw { get; set; }
        public float Yaw
        {
            get
            {
                return yaw;
            }
            set
            {
                OldYaw = yaw;
                yaw = value;
                OnPropertyChanged("Yaw");
            }
        }

        public abstract Size Size { get; }

        public event EventHandler<EntityTerrainCollisionEventArgs> TerrainCollision;

        #endregion

        #region Physics

        public virtual BoundingBox BoundingBox
        {
            get
            {
                return new BoundingBox(Position, Position + Size);
            }
        }

        // All units are in meters per second squared
        public virtual float AccelerationDueToGravity
        {
            get { return 0; }
        }

        public virtual float Drag
        {
            get { return 0.4f; }
        }

        public virtual CollisionTests TestsToPerform
        {
            get { return CollisionTests.EntityToEnviornment; }
        }

        /// <summary>
        /// Run to recalculate velocity and movement.
        /// Should run once a second.
        /// </summary>
        private BoundingBox TempBoundingBox;
        public virtual void PhysicsUpdate(World world)
        {
            // I don't know much about game physics, this code is open for pull requests.

            // Calculate movement
            bool fireEvent = Velocity != Vector3.Zero;

            Velocity -= new Vector3(0, AccelerationDueToGravity, 0);
            Velocity *= Drag;
            Vector3 collisionPoint;
            // Do terrain collisions
            if (!AdjustVelocityY(world, out collisionPoint))
                fireEvent = false;

            if (fireEvent && TerrainCollision != null)
                TerrainCollision(this, new EntityTerrainCollisionEventArgs
                    {
                        Entity = this,
                        Block = collisionPoint,
                        World = world
                    });

            Position += Velocity;
        }

        #region Per-axis Physics

        /// <summary>
        /// Performs terrain collision tests and adjusts the Y-axis velocity accordingly
        /// </summary>
        protected bool AdjustVelocityY(World world, out Vector3 collision)
        {
            collision = Vector3.Zero;
            if (Velocity.Y == 0)
                return false;
            // Do some enviornment guessing to improve speed
            int minX = (int)Position.X - (Position.X < 0 ? 1 : 0);
            int maxX = (int)(Position.X + Size.Width) - (Position.X < 0 ? 1 : 0);
            int minZ = (int)Position.Z - (Position.Z < 0 ? 1 : 0);
            int maxZ = (int)(Position.Z + Size.Depth) - (Position.Z < 0 ? 1 : 0);
            int minY, maxY;

            // Expand bounding box to include area to be tested
            if (Velocity.Y < 0)
            {
                TempBoundingBox = new BoundingBox(
                    new Vector3(BoundingBox.Min.X, BoundingBox.Min.Y + Velocity.Y, BoundingBox.Min.Z) - (Size / 2),
                    new Vector3(BoundingBox.Max.X, BoundingBox.Max.Y, BoundingBox.Max.Z) - (Size / 2));

                maxY = (int)(TempBoundingBox.Min.Y);
                minY = (int)(TempBoundingBox.Min.Y + Velocity.Y);
            }
            else
            {
                TempBoundingBox = new BoundingBox(BoundingBox.Min - (Size / 2), new Vector3(
                    BoundingBox.Max.X, BoundingBox.Max.Y + Velocity.Y, BoundingBox.Max.Z) - (Size / 2));
                minY = (int)(BoundingBox.Max.Y);
                maxY = (int)(BoundingBox.Max.Y + Velocity.Y);
            }

            // Clamp Y into map boundaries
            if (minY < 0) minY = 0; if (minY >= World.Height) minY = World.Height - 1;

            // Do terrain checks
            double? collisionPoint = null;
            BoundingBox blockBox;
            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    for (int z = minZ; z <= maxZ; z++)
                    {
                        var position = new Vector3(x, y, z);
                        var block = world.GetBlock(position);
                        if (block.BoundingBox == null)
                            continue;
                        blockBox = new BoundingBox(block.BoundingBox.Value.Min + position,
                            block.BoundingBox.Value.Max + position);
                        if (TempBoundingBox.Intersects(blockBox))
                        {
                            if (Velocity.Y < 0)
                            {
                                if (!collisionPoint.HasValue)
                                    collisionPoint = blockBox.Max.Y;
                                else if (collisionPoint.Value < blockBox.Max.Y)
                                    collisionPoint = blockBox.Max.Y;
                            }
                            else
                            {
                                if (!collisionPoint.HasValue)
                                    collisionPoint = blockBox.Min.Y;
                                else if (collisionPoint.Value > blockBox.Min.Y)
                                    collisionPoint = blockBox.Min.Y;
                            }
                            collision = position;
                        }
                    }
                }
            }

            if (collisionPoint != null)
            {
                if (Velocity.Y < 0)
                {
                    Velocity = new Vector3(Velocity.X,
                        Velocity.Y + (collisionPoint.Value - TempBoundingBox.Min.Y),
                        Velocity.Z);
                }
                // TODO: Collisions for entities moving up
                return true;
            }

            return false;
        }

        #endregion

        #endregion

        public virtual MetadataDictionary Metadata
        {
            get
            {
                var dictionary = new MetadataDictionary();
                dictionary[0] = new MetadataByte(0, 0); // Flags
                dictionary[8] = new MetadataInt(8, 0); // Potion effects
                return dictionary;
            }
        }

        public bool IsUnderwater(World world)
        {
            var position = new Vector3(Position.X,
                Position.Y + Size.Height, Position.Z);
            if (!World.IsValidPosition(position))
                return false;
            var block = world.GetBlock(position);
            return block is WaterFlowingBlock || block is WaterStillBlock;
        }

        public bool IsOnGround(World world)
        {
            if (Math.Truncate(Position.Y) != Position.Y)
                return false;
            if (!World.IsValidPosition(Position))
                return false;
            var block = world.GetBlock(Position + Vector3.Down);
            return block != 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    [Flags]
    public enum CollisionTests
    {
        None,
        EntityToEntity,
        EntityToEnviornment
    }
}