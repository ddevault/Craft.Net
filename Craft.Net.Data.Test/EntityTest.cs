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
        public void TestBlockCollision()
        {
            const int iterations = 100000;

            Level level = new Level();
            // TODO: Don't use player entities
            var entity = new ItemEntity(new Vector3(0, 10, 0), new Slot(1, 1));
            level.World.EntityUpdateTimer.Change(Timeout.Infinite, Timeout.Infinite);
            level.World.Entities.Add(entity);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < iterations; i++)
                entity.PhysicsUpdate(level.World);
            stopwatch.Stop();
            double sleepSeconds = stopwatch.Elapsed.TotalSeconds;
            Assert.AreEqual(4, entity.Position.Y);

            Entity.EnableEntitySleeping = false;

            entity = new ItemEntity(new Vector3(0, 10, 0), new Slot(1, 1));
            level.World.Entities.Add(entity);
            stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < iterations; i++)
                entity.PhysicsUpdate(level.World);
            stopwatch.Stop();
            double unsleepSeconds = stopwatch.Elapsed.TotalSeconds;
            Assert.AreEqual(4, entity.Position.Y);

            Console.WriteLine("Speed results (seconds): " +
                "\nWith sleeping: " + sleepSeconds + 
                "\nWithout sleeping: " + unsleepSeconds +
                "\n" + iterations + " total iterations");
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
