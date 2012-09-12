using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class SignPostBlock : Block
    {
        public SignPostBlock()
        {
            SignData = new SignTileEntity();
        }

        public override ushort Id
        {
            get { return 63; }
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

    public class SignTileEntity : TileEntity
    {
        public override string Id
        {
            get
            {
                return "Sign";
            }
        }

        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }
        public string Text4 { get; set; }
    }
}
