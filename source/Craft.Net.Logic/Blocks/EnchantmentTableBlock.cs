using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(EnchantmentTableBlock.BlockId, EnchantmentTableBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(EnchantmentTableBlock.BlockId, DisplayName = EnchantmentTableBlock.DisplayName, Hardness = EnchantmentTableBlock.Hardness)]
    public static class EnchantmentTableBlock
    {
        public const string DisplayName = "Enchantment Table";
        public const short BlockId = 116;
        public const double Hardness = 5;
    }
}
