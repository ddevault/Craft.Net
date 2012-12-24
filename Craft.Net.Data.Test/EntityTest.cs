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

            for (double x = 3; x >= -3; x -= 0.5)
            {
                for (double z = 3; z >= -3; z -= 0.5)
                {
                    for (int y = 10; y < 100; y += 10)
                    {
                        var entity = new ItemEntity(new Vector3(x, y, z), new Slot(1, 1));
                        level.World.Entities.Add(entity);

                        for (int i = 0; i < 1000; i++)
                            entity.PhysicsUpdate(level.World);

                        Assert.AreEqual(4, (int)entity.Position.Y);

                        level.World.Entities.Remove(entity);
                    }
                }
            }
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
