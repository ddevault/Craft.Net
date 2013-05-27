using System;
using System.Collections.Generic;
using System.ComponentModel;
using Craft.Net.Data.Blocks;
using Craft.Net.Data.Events;
using Craft.Net.Metadata;

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
        protected bool EnablePositionUpdates = true;
        public virtual Vector3 Position
        {
            get { return position; }
            set
            {
                OldPosition = position;
                LastPositionUpdate = DateTime.Now;
                position = value;
                if (EnablePositionUpdates)
                    OnPropertyChanged("Position");
            }
        }

        protected bool EnableVelocityUpdates = true;
        protected Vector3 PrePhysicsVelocity { get; set; }
        /// <summary>
        /// In meters per tick
        /// </summary>
        public Vector3 Velocity
        {
            get { return velocity; }
            set
            {
                velocity = value;
                if (EnableVelocityUpdates)
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

        /// <summary>
        /// Determines whether or not Entity.Metadata shall be sent to the client.
        /// </summary>
        public virtual bool IncludeMetadataOnClient
        {
            get
            {
                return false;
            }
        }

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
            // How this works:
            // The bounding box of each entity is extended by the velocity on each axis.
            // If there are any collisions, the velocity is adjusted.
            // Move the entity.
            
            // I don't know much about game physics, this code is open for pull requests.
            bool oldVelocityEnabled = EnableVelocityUpdates;
            EnableVelocityUpdates = false;
            PrePhysicsVelocity = Velocity;

            // Calculate movement
            bool fireEvent = Velocity != Vector3.Zero;

            Velocity *= Drag;
            Velocity -= new Vector3(0, AccelerationDueToGravity, 0);
            Vector3 collisionPoint, collisionDirection;
            if (Position.Y + Velocity.Y >= 0 && Position.Y + Velocity.Y <= 255) // Don't do checks outside the map
            {
                // Do terrain collisions
                if (AdjustVelocityX(world, out collisionPoint, out collisionDirection))
                {
                    if (TerrainCollision != null && fireEvent)
                        TerrainCollision(this, new EntityTerrainCollisionEventArgs
                        {
                            Entity = this,
                            Block = collisionPoint,
                            World = world,
                            Direction = collisionDirection
                        });
                }
                if (AdjustVelocityY(world, out collisionPoint, out collisionDirection))
                {
                    // Adjust horizontal velocity for friction
                    // TODO: Consider doing this in the X/Z direction
                    Velocity *= new Vector3(0.2, 0, 0.2);
                    if (TerrainCollision != null && fireEvent)
                        TerrainCollision(this, new EntityTerrainCollisionEventArgs
                            {
                                Entity = this,
                                Block = collisionPoint,
                                World = world,
                                Direction = collisionDirection
                            });
                }
                if (AdjustVelocityZ(world, out collisionPoint, out collisionDirection))
                {
                    if (TerrainCollision != null && fireEvent)
                        TerrainCollision(this, new EntityTerrainCollisionEventArgs
                        {
                            Entity = this,
                            Block = collisionPoint,
                            World = world,
                            Direction = collisionDirection
                        });
                }
            }

            EnableVelocityUpdates = oldVelocityEnabled;
            if (EnableVelocityUpdates)
                OnPropertyChanged("Velocity");

            Position += Velocity;
        }

        #region Per-axis Physics

        // TODO: There's a lot of code replication here, perhaps it can be consolidated
        /// <summary>
        /// Performs terrain collision tests and adjusts the X-axis velocity accordingly
        /// </summary>
        /// <returns>True if the entity collides with the terrain</returns>
        protected bool AdjustVelocityX(World world, out Vector3 collision, out Vector3 collisionDirection)
        {
            collision = Vector3.Zero;
            collisionDirection = Vector3.Zero;
            if (Velocity.X == 0)
                return false;
            // Do some enviornment guessing to improve speed
            int minY = (int)Position.Y - (Position.Y < 0 ? 1 : 0);
            int maxY = (int)(Position.Y + Size.Width) - (Position.Y < 0 ? 1 : 0);
            int minZ = (int)Position.Z - (Position.Z < 0 ? 1 : 0);
            int maxZ = (int)(Position.Z + Size.Depth) - (Position.Z < 0 ? 1 : 0);
            int minX, maxX;

            // Expand bounding box to include area to be tested
            if (Velocity.X < 0)
            {
                TempBoundingBox = new BoundingBox(
                    new Vector3(BoundingBox.Min.X + Velocity.X, BoundingBox.Min.Y, BoundingBox.Min.Z) - (Size / 2),
                    new Vector3(BoundingBox.Max.X, BoundingBox.Max.Y, BoundingBox.Max.Z) - (Size / 2));

                maxX = (int)(TempBoundingBox.Max.X);
                minX = (int)(TempBoundingBox.Min.X + Velocity.X);
            }
            else
            {
                TempBoundingBox = new BoundingBox(BoundingBox.Min - (Size / 2), new Vector3(
                    BoundingBox.Max.X + Velocity.X, BoundingBox.Max.Y, BoundingBox.Max.Z) - (Size / 2));
                minX = (int)(BoundingBox.Min.X);
                maxX = (int)(BoundingBox.Max.X + Velocity.X);
            }

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
                            if (Velocity.X < 0)
                            {
                                if (!collisionPoint.HasValue)
                                    collisionPoint = blockBox.Max.X;
                                else if (collisionPoint.Value < blockBox.Max.X)
                                    collisionPoint = blockBox.Max.X;
                            }
                            else
                            {
                                if (!collisionPoint.HasValue)
                                    collisionPoint = blockBox.Min.X;
                                else if (collisionPoint.Value > blockBox.Min.X)
                                    collisionPoint = blockBox.Min.X;
                            }
                            collision = position;
                        }
                    }
                }
            }

            if (collisionPoint != null)
            {
                if (Velocity.X < 0)
                {
                    Velocity = new Vector3(
                        Velocity.X - (TempBoundingBox.Min.X - collisionPoint.Value),
                        Velocity.Y,
                        Velocity.Z);
                    collisionDirection = Vector3.Left;
                }
                else if (Velocity.X > 0)
                {
                    Velocity = new Vector3(
                        Velocity.X - (TempBoundingBox.Max.X - collisionPoint.Value),
                        Velocity.Y,
                        Velocity.Z);
                    collisionDirection = Vector3.Right;
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Performs terrain collision tests and adjusts the Y-axis velocity accordingly
        /// </summary>
        /// <returns>True if the entity collides with the terrain</returns>
        protected bool AdjustVelocityY(World world, out Vector3 collision, out Vector3 collisionDirection)
        {
            collision = Vector3.Zero;
            collisionDirection = Vector3.Zero;
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

                maxY = (int)(TempBoundingBox.Max.Y);
                minY = (int)(TempBoundingBox.Min.Y + Velocity.Y);
            }
            else
            {
                TempBoundingBox = new BoundingBox(BoundingBox.Min - (Size / 2), new Vector3(
                    BoundingBox.Max.X, BoundingBox.Max.Y + Velocity.Y, BoundingBox.Max.Z) - (Size / 2));
                minY = (int)(BoundingBox.Min.Y);
                maxY = (int)(BoundingBox.Max.Y + Velocity.Y);
            }

            // Clamp Y into map boundaries
            if (minY < 0) minY = 0; if (minY >= World.Height) minY = World.Height - 1;
            if (maxY < 0) maxY = 0; if (maxY >= World.Height) maxY = World.Height - 1;

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
                    // Do block event
                    var block = world.GetBlock(collision);
                    block.OnBlockWalkedOn(world, collision, this);
                    Velocity = new Vector3(Velocity.X,
                        Velocity.Y + (collisionPoint.Value - TempBoundingBox.Min.Y),
                        Velocity.Z);
                    collisionDirection = Vector3.Down;
                }
                else if (Velocity.Y > 0)
                {
                    Velocity = new Vector3(Velocity.X,
                        Velocity.Y - (TempBoundingBox.Max.Y - collisionPoint.Value),
                        Velocity.Z);
                    collisionDirection = Vector3.Up;
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Performs terrain collision tests and adjusts the Z-axis velocity accordingly
        /// </summary>
        /// <returns>True if the entity collides with the terrain</returns>
        protected bool AdjustVelocityZ(World world, out Vector3 collision, out Vector3 collisionDirection)
        {
            collision = Vector3.Zero;
            collisionDirection = Vector3.Zero;
            if (Velocity.Z == 0)
                return false;
            // Do some enviornment guessing to improve speed
            int minX = (int)Position.X - (Position.X < 0 ? 1 : 0);
            int maxX = (int)(Position.X + Size.Depth) - (Position.X < 0 ? 1 : 0);
            int minY = (int)Position.Y - (Position.Y < 0 ? 1 : 0);
            int maxY = (int)(Position.Y + Size.Width) - (Position.Y < 0 ? 1 : 0);
            int minZ, maxZ;

            // Expand bounding box to include area to be tested
            if (Velocity.Z < 0)
            {
                TempBoundingBox = new BoundingBox(
                    new Vector3(BoundingBox.Min.X, BoundingBox.Min.Y, BoundingBox.Min.Z + Velocity.Z) - (Size / 2),
                    new Vector3(BoundingBox.Max.X, BoundingBox.Max.Y, BoundingBox.Max.Z) - (Size / 2));

                maxZ = (int)(TempBoundingBox.Max.Z);
                minZ = (int)(TempBoundingBox.Min.Z + Velocity.Z);
            }
            else
            {
                TempBoundingBox = new BoundingBox(BoundingBox.Min - (Size / 2), new Vector3(
                    BoundingBox.Max.X, BoundingBox.Max.Y, BoundingBox.Max.Z + Velocity.Z) - (Size / 2));
                minZ = (int)(BoundingBox.Min.Z);
                maxZ = (int)(BoundingBox.Max.Z + Velocity.Z);
            }

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
                            if (Velocity.Z < 0)
                            {
                                if (!collisionPoint.HasValue)
                                    collisionPoint = blockBox.Max.Z;
                                else if (collisionPoint.Value < blockBox.Max.Z)
                                    collisionPoint = blockBox.Max.Z;
                            }
                            else
                            {
                                if (!collisionPoint.HasValue)
                                    collisionPoint = blockBox.Min.Z;
                                else if (collisionPoint.Value > blockBox.Min.Z)
                                    collisionPoint = blockBox.Min.Z;
                            }
                            collision = position;
                        }
                    }
                }
            }

            if (collisionPoint != null)
            {
                if (Velocity.Z < 0)
                {
                    Velocity = new Vector3(
                        Velocity.X,
                        Velocity.Y,
                        Velocity.Z - (TempBoundingBox.Min.Z - collisionPoint.Value));
                    collisionDirection = Vector3.Backwards;
                }
                else if (Velocity.Z > 0)
                {
                    Velocity = new Vector3(
                        Velocity.X,
                        Velocity.Y,
                        Velocity.Z - (TempBoundingBox.Max.Z - collisionPoint.Value));
                    collisionDirection = Vector3.Forwards;
                }
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
                dictionary[0] = new MetadataByte(0); // Flags
                dictionary[1] = new MetadataShort(300);
                return dictionary;
            }
        }

        public virtual void UsedByEntity(World world, bool leftClick, LivingEntity usedBy)
        {
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