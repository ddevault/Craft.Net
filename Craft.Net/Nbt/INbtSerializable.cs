using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fNbt;

namespace Craft.Net.Nbt
{
    public interface INbtSerializable
    {
        NbtCompound Serialize();
        void Deserialize(NbtCompound value);
    }
}
