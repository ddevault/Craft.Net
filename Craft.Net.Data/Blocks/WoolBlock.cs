using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public enum WoolColor
    {
        White = 0,
        Orange = 1,
        Magenta = 2,
        LightBlue = 3,
        Yellow = 4,
        Lime = 5,
        Pink = 6,
        Grey = 7,
        LightGrey = 8,
        Cyan = 9,
        Purple = 10,
        Blue = 11,
        Brown = 12,
        Green = 13,
        Red = 14,
        Black = 15
    }

    public class WoolBlock : Block
    {
        public WoolColor Color
        {
            get { return (WoolColor)Metadata; }
            set { Metadata = (byte)value; }
        }

        public WoolBlock()
        {
        }

        public WoolBlock(WoolColor color)
        {
            Metadata = (byte)color;
        }

        public override ushort Id
        {
            get { return 35; }
        }

        public override double Hardness
        {
            get { return 0.8; }
        }
    }
}