using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class WallSignBlock : Block
    {
        public WallSignBlock()
        {
            SignData = new SignTileEntity();
        }

        public WallSignBlock(Direction direction) : this()
        {
            Metadata = (byte)direction;
        }

        public override ushort Id
        {
            get { return 68; }
        }

        public SignTileEntity SignData { get; set; }

        public override TileEntity TileEntity
        {
            get
            {
                return SignData;
            }
            set { SignData = (SignTileEntity)value; }
        }
    }
}
