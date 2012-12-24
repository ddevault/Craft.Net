using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class DirtBlock : Block
    {
        public override short Id
        {
            get { return 3; }
        }

        public override double Hardness
        {
            get { return 0.5; }
        }

        public override string PlacementSoundEffect
        {
            get { return SoundEffect.DigGrass; }
        }
    }
}
