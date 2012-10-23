using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class TorchBlock : Block
    {
        public override ushort Id
        {
            get { return 50; }
        }

        public override bool OnBlockPlaced(World world, Vector3 position, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entities.Entity usedBy)
        {
            if (clickedSide == Vector3.East)
                this.Metadata = 0x1;
            else if (clickedSide == Vector3.West)
                this.Metadata = 0x2;
            else if (clickedSide == Vector3.South)
                this.Metadata = 0x3;
            else if (clickedSide == Vector3.North)
                this.Metadata = 0x4;
            else if (clickedSide == Vector3.Up)
                this.Metadata = 0x5;
            else
                return false;
            return true;
        }
    }
}
