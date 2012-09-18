using System;
using System.Collections.Generic;
using System.Linq;

namespace Craft.Net.Data.Blocks
{
    public class NoteBlock : Block
    {
        public override ushort Id
        {
            get { return 25; }
        }

        public override double Hardness
        {
            get { return 0.8; }
        }

        public override bool OnBlockRightClicked(Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, World world, Entities.Entity usedBy)
        {
            return false;
        }
    }
}
