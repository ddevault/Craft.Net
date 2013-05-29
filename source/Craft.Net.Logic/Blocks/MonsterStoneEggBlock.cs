using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(MonsterStoneEggBlock.BlockId, MonsterStoneEggBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(MonsterStoneEggBlock.BlockId, DisplayName = MonsterStoneEggBlock.DisplayName, Hardness = MonsterStoneEggBlock.Hardness)]
    public static class MonsterStoneEggBlock
    {
        public const string DisplayName = "MonsterStoneEgg";
        public const short BlockId = 97;
		public const double Hardness = 0.75;
    }
}
