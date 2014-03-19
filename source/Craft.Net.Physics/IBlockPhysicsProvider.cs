using System;
using Craft.Net.Anvil;
using Craft.Net.Common;

namespace Craft.Net.Physics
{
    public interface IBlockPhysicsProvider
    {
        BoundingBox? GetBoundingBox(World world, Coordinates3D coordinates);
    }
}