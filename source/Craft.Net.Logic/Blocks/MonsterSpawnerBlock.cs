using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(MonsterSpawnerBlock.BlockId, MonsterSpawnerBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(MonsterSpawnerBlock.BlockId, DisplayName = MonsterSpawnerBlock.DisplayName, Hardness = MonsterSpawnerBlock.Hardness)]
    public static class MonsterSpawnerBlock
    {
        public const string DisplayName = "Monster Spawner";
        public const short BlockId = 52;
        public const double Hardness = 5;
    }
}
