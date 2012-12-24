using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Craft.Net.Data.Blocks;
using Craft.Net.Data.Entities;
using Craft.Net.Data.Generation;
using NUnit.Framework;

namespace Craft.Net.Data.Test
{
    [TestFixture]
    public class EntityTest
    {
        [Test]
        public void TestVerticalBlockCollision()
        {
            Level level = new Level();
            level.World.EntityUpdateTimer.Change(Timeout.Infinite, Timeout.Infinite); // Manual updates only

            for (int y = 10; y < 100; y += 10)
            {
                var entity = new ItemEntity(new Vector3(3, y, 3), new ItemStack(1, 1));
                level.World.Entities.Add(entity);

                for (int i = 0; i < 1000; i++)
                    entity.PhysicsUpdate(level.World);

                Assert.AreEqual(4, (int)(entity.Position.Y));

                level.World.Entities.Remove(entity);
            }
        }

        [Test]
        public void TestXBlockCollision()
        {
            Level level = new Level();
            level.World.EntityUpdateTimer.Change(Timeout.Infinite, Timeout.Infinite); // Manual updates only

            // Build a wall to throw the item at
            for (int z = -10; z < 10; z++)
                for (int y = 0; y < 23; y++)
                    level.World.SetBlock(new Vector3(3, y, z), new StoneBlock());

            var entity = new ItemEntity(new Vector3(0, 10, 0), new ItemStack(1, 1));
            entity.Velocity = new Vector3(3, 0, 0); // Throw item
            level.World.Entities.Add(entity);

            for (int i = 0; i < 1000; i++)
                entity.PhysicsUpdate(level.World);

            Assert.AreEqual(2, (int)(entity.Position.X));

            level.World.Entities.Remove(entity);
        }

        [Test]
        public void TestZBlockCollision()
        {
            Level level = new Level();
            level.World.EntityUpdateTimer.Change(Timeout.Infinite, Timeout.Infinite); // Manual updates only

            // Build a wall to throw the item at
            for (int x = -10; x < 10; x++)
                for (int y = 0; y < 23; y++)
                    level.World.SetBlock(new Vector3(x, y, 3), new StoneBlock());

            var entity = new ItemEntity(new Vector3(0, 10, 0), new ItemStack(1, 1));
            entity.Velocity = new Vector3(0, 0, 3); // Throw item
            level.World.Entities.Add(entity);

            for (int i = 0; i < 1000; i++)
                entity.PhysicsUpdate(level.World);

            Assert.AreEqual(2, (int)(entity.Position.Z));

            level.World.Entities.Remove(entity);
        }

        [Test]
        public void TestSandDrop()
        {
            Level level = new Level();
            level.World.EntityUpdateTimer.Change(Timeout.Infinite, Timeout.Infinite); // Manual updates only

            var entity = new BlockEntity(new Vector3(0, 10, 0), new SandBlock());
            level.World.Entities.Add(entity);

            for (int i = 0; i < 500; i++)
            {
                if (level.World.Entities.Count == 0)
                    break;
                entity.PhysicsUpdate(level.World);
            }

            Assert.AreEqual(new SandBlock(), level.World.GetBlock(new Vector3(0, 4, 0)));
        }

        [Test]
        public void TestCollisionPoint()
        {
            CollisionPoint point;
            point = MathHelper.GetCollisionPoint(new Vector3(10, 1, 1));
            Assert.AreEqual(CollisionPoint.PositiveX, point);
            point = MathHelper.GetCollisionPoint(new Vector3(-10, 1, 1));
            Assert.AreEqual(CollisionPoint.NegativeX, point);
            point = MathHelper.GetCollisionPoint(new Vector3(1, 10, 1));
            Assert.AreEqual(CollisionPoint.PositiveY, point);
            point = MathHelper.GetCollisionPoint(new Vector3(1, -10, 1));
            Assert.AreEqual(CollisionPoint.NegativeY, point);
            point = MathHelper.GetCollisionPoint(new Vector3(1, 1, 10));
            Assert.AreEqual(CollisionPoint.PositiveZ, point);
            point = MathHelper.GetCollisionPoint(new Vector3(1, 1, -10));
            Assert.AreEqual(CollisionPoint.NegativeZ, point);
        }
    }
}
