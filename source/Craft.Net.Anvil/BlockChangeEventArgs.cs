using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Anvil
{
    public class BlockChangeEventArgs : EventArgs
    {
        public Coordinates3D Coordinates { get; set; }

        public BlockChangeEventArgs(Coordinates3D coordinates)
        {
            Coordinates = coordinates;
        }
    }
}
