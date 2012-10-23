using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Craft.Net.Data.Test
{
    [TestFixture]
    public class DataUtilityTest
    {
        [Test]
        public void TestRotation()
        {
            Assert.AreEqual(Vector3.Left, DataUtility.RotateY(Vector3.Forwards, DataUtility.DegreesToRadians(90)).Floor());
            Assert.AreEqual(Vector3.Right, DataUtility.RotateY(Vector3.Forwards, DataUtility.DegreesToRadians(-90)).Floor());
            // TODO: Investigate rotation on X axis
        }
    }
}
