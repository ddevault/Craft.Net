using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fNbt;

namespace Craft.Net.World
{
    public interface IDiskEntity
    {
        NbtCompound GetNbt();
    }
}
