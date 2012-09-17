using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Entities;
using Craft.Net.Data.Generation;
using NUnit.Framework;

namespace Craft.Net.Data.Test
{
    [TestFixture]
    public class EntityTest
    {
        [Test]
        public void TestBlockCollision()
        {
            Level level = new Level();
            // TODO: Don't use player entities
            var player = new PlayerEntity(Difficulty.Normal);
            player.Position = new Vector3(0, 10, 0);
            level.World.Entities.Add(player);
            for (int i = 0; i < 10; i++)
                player.PhysicsUpdate(level.World);
            Assert.AreEqual(player.Position.Y, 4);
        }

        [Test]
        public void TestCollisionPoint()
        {
            CollisionPoint point;
            point = DataUtility.GetCollisionPoint(new Vector3(10, 1, 1));
            Assert.AreEqual(CollisionPoint.PositiveX, point);
            point = DataUtility.GetCollisionPoint(new Vector3(-10, 1, 1));
            Assert.AreEqual(CollisionPoint.NegativeX, point);
            point = DataUtility.GetCollisionPoint(new Vector3(1, 10, 1));
            Assert.AreEqual(CollisionPoint.PositiveY, point);
            point = DataUtility.GetCollisionPoint(new Vector3(1, -10, 1));
            Assert.AreEqual(CollisionPoint.NegativeY, point);
            point = DataUtility.GetCollisionPoint(new Vector3(1, 1, 10));
            Assert.AreEqual(CollisionPoint.PositiveZ, point);
            point = DataUtility.GetCollisionPoint(new Vector3(1, 1, -10));
            Assert.AreEqual(CollisionPoint.NegativeZ, point);
        }
    }
}
