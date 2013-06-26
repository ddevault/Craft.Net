using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Client.Events
{
    using Data;
    using Data.Blocks;

    public class SignUpdateReceivedEventArgs : EventArgs
    {
        public SignTileEntity Sign { get; set; }
        public Vector3 Position { get; set; }

        public SignUpdateReceivedEventArgs(Vector3 position, SignTileEntity sign)
        {
            Sign = sign;
            Position = position;
        }
    }
}
