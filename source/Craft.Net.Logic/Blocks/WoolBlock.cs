using System;

namespace Craft.Net.Logic.Blocks
{
	[Item(WoolBlock.BlockId, WoolBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(WoolBlock.BlockId, DisplayName = WoolBlock.DisplayName, Hardness = WoolBlock.Hardness)]
	public static class WoolBlock
	{
		public const short BlockId = 35;
		public const string DisplayName = "Wool";
		public const double Hardness = 0.8;
	}
}

