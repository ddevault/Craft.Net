using Craft.Net.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Physics
{
    public interface IAABBEntity : IPhysicsEntity
    {
        BoundingBox BoundingBox { get; }
        Size Size { get; }

        void TerrainCollision(PhysicsEngine engine, Vector3 collisionPoint, Vector3 collisionDirection);
    }
}
