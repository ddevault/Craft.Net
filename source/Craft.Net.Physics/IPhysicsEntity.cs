using Craft.Net.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Physics
{
    public interface IPhysicsEntity
    {
        Vector3 Position { get; set; }
        Vector3 Velocity { get; set; }
        float AccelerationDueToGravity { get; }
        float Drag { get; }

        bool BeginUpdate();
        void EndUpdate(Vector3 newPosition);
    }
}
