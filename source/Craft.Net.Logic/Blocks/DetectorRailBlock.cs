using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(DetectorRailBlock.BlockId, DetectorRailBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(DetectorRailBlock.BlockId, DisplayName = DetectorRailBlock.DisplayName, Hardness = DetectorRailBlock.Hardness)]
    public static class DetectorRailBlock
    {
        public const string DisplayName = "Detector Rail";
        public const short BlockId = 28;
        public const double Hardness = 0.7;
    }
}
