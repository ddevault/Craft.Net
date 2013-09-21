using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(BeaconBlock.BlockId, BeaconBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(BeaconBlock.BlockId, DisplayName = BeaconBlock.DisplayName, Hardness = BeaconBlock.Hardness)]
    public static class BeaconBlock
    {
        public const string DisplayName = "Beacon";
        public const short BlockId = 138;
        public const double Hardness = 0;
    }
}
