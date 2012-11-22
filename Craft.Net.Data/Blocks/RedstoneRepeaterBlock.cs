using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
   public class RedstoneRepeaterBlock : Block
   {
      public byte Ticks
      {
         get
         {
            return (byte)((Metadata & 0xC) >> 2);
         }
         set
         {
            value %= 4;
            Metadata &= unchecked((byte)~0xC);
            Metadata |= (byte)(value << 2);
         }
      }

      public Direction Direction
      {
         get
         {
            byte meta = (byte)(Metadata & 0x3);
            switch (meta)
            {
               case 0:
                  return Direction.North;
               case 1:
                  return Direction.East;
               case 2:
                  return Direction.South;
               default:
                  return Direction.West;
               }
            }
            set
            {
               Metadata &= unchecked((byte)~0x3);
               switch (value)
               {
                  case Direction.North:
                     Metadata |= 0;
                     break;
                  case Direction.East:
                     Metadata |= 1;
                     break;
                  case Direction.South:
                     Metadata |= 2;
                     break;
                  case Direction.West:
                     Metadata |= 3;
                     break;
                  }
               }
            }

            public RedstoneRepeaterBlock()
            {
            }

            public RedstoneRepeaterBlock(Direction direction)
            {
               Direction = direction;
            }

            public override ushort Id
            {
               get { return 93; }
            }

            public override bool OnBlockRightClicked(Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, World world, Entities.Entity usedBy)
            {
               Ticks++;
               world.SetBlock(clickedBlock, this);
               return false;
            }
         }
}