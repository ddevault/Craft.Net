using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data
{
    public interface IGrowableBlock
    {
        /// <summary>
        /// Causes the specified block to grow. Return true if growth occured.
        /// </summary>
        bool Grow(World world, Vector3 position, bool instant);
    }
}
