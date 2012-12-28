using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data
{
    public interface IGrowableBlock
    {
        void Grow(World world, Vector3 position);
    }
}
