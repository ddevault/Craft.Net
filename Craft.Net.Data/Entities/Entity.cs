using System;
using System.Collections.Generic;
using System.ComponentModel;
using Craft.Net.Data.Blocks;
using Craft.Net.Data.Metadata;

namespace Craft.Net.Data.Entities
{
    public abstract class Entity : INotifyPropertyChanged
    {
        protected Entity()
        {
            Fire = -20;
            EnablePhysicsNotifications = true;
        }

        #region State

        public int Id { get; set; }
        public Vector3 OldPosition { get; set; }
        public DateTime LastPositionUpdate { get; set; }
        /// <summary>
        /// When set to false, OnPropertyChanged is not fired for
        /// Position or Velocity.
        /// </summary>
        protected bool EnablePhysicsNotifications { get; set; }
        public Vector3 Position
        {
            get { return position; }
            set
            {
                OldPosition = position;
                LastPositionUpdate = DateTime.Now;
                position = value;
                if (EnablePhysicsNotifications)
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

        #endregion

        #region Physics

        public static bool EnableEntitySleeping = true;

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

        public virtual float TerminalVelocity
        {
            get { return -1; }
        }

        public virtual float Drag
        {
            get { return 0.4f; }
        }

        /// <summary>
        /// Determines whether or not this entity will be updated in
        /// collision tests.
        /// </summary>
        public bool PhysicsAsleep { get; private set; }

        public virtual CollisionTests TestsToPerform
        {
            get { return CollisionTests.EntityToEnviornment; }
        }

        // TODO: Consider refactoring physics into seperate namespace
        /// <summary>
        /// Run to recalculate velocity and movement.
        /// Should run once a second.
        /// </summary>
        public virtual void PhysicsUpdate(World world)
        {
            if (PhysicsAsleep && EnableEntitySleeping)
                return; // TODO: Wake up on collision
            EnablePhysicsNotifications = false;
            if (Velocity == Vector3.Zero && Math.Floor(Position.Y) == Position.Y &&
                (World.IsValidPosition(Position + Vector3.Down) && world.GetBlock(Position + Vector3.Down).Solid) &&
                EnableEntitySleeping)
            {
                PhysicsAsleep = true;
                OnSleep(world);
                return;
            }
            Velocity += new Vector3(0, -AccelerationDueToGravity, 0);
            // TODO: Apply velocity changes in increments of one to avoid falling through blocks
            Position += Velocity;
            if ((TestsToPerform & CollisionTests.EntityToEnviornment) == CollisionTests.EntityToEnviornment)
            {
                // Handle block intersections
                for (double x = Math.Floor(Position.X); x < Position.X + Size.Width; x++)
                    for (double y = Math.Floor(Position.Y); y < Position.Y + Size.Height; y++)
                        for (double z = Math.Floor(Position.Z); z < Position.Z + Size.Depth; z++)
                        {
                            if (y >= 0 && y <= Chunk.Height)
                            {
                                var blockPosition = new Vector3(x, y, z);
                                var block = world.GetBlock(blockPosition);
                                if (block == 0)
                                    continue;
                                var box = new BoundingBox(blockPosition, blockPosition + block.Size);
                                if (box.Intersects(BoundingBox) && Velocity != Vector3.Zero)
                                {
                                    var collision = DataUtility.GetCollisionPoint(Velocity);
                                    // Apply velocity change and reset position
                                    switch (collision)
                                    {
                                        case CollisionPoint.PositiveX:
                                            Velocity = new Vector3(0, Velocity.Y, Velocity.Z);
                                            Position = new Vector3(
                                                blockPosition.X - Size.Width,
                                                Position.Y,
                                                Position.Z);
                                            break;
                                        case CollisionPoint.NegativeX:
                                            Velocity = new Vector3(0, Velocity.Y, Velocity.Z);
                                            Position = new Vector3(
                                                blockPosition.X + block.Size.Width,
                                                Position.Y,
                                                Position.Z);
                                            break;
                                        case CollisionPoint.PositiveY:
                                            Velocity = new Vector3(Velocity.X, 0, Velocity.Z);
                                            Position = new Vector3(
                                                Position.X,
                                                blockPosition.Y - Size.Height,
                                                Position.Z);
                                            break;
                                        case CollisionPoint.NegativeY:
                                            Velocity = new Vector3(Velocity.X, 0, Velocity.Z);
                                            Position = new Vector3(
                                                Position.X,
                                                blockPosition.Y + block.Size.Height,
                                                Position.Z);
                                            break;
                                        case CollisionPoint.PositiveZ:
                                            Velocity = new Vector3(Velocity.X, Velocity.Y, 0);
                                            Position = new Vector3(
                                                Position.X,
                                                Position.Y,
                                                blockPosition.Z - Size.Depth);
                                            break;
                                        case CollisionPoint.NegativeZ:
                                            Velocity = new Vector3(Velocity.X, Velocity.Y, 0);
                                            Position = new Vector3(
                                                Position.X,
                                                Position.Y,
                                                blockPosition.Z + block.Size.Depth);
                                            break;
                                    }
                                }
                            }
                        }
            }
            Velocity *= 1 - Drag;
            OnPropertyChanged("Position");
            EnablePhysicsNotifications = true;
        }

        protected virtual void OnSleep(World world) // TODO: Add world object to class
        {
        }

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