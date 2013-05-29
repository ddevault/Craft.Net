using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(DaylightSensorBlock.BlockId, DaylightSensorBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(DaylightSensorBlock.BlockId, DisplayName = DaylightSensorBlock.DisplayName, Hardness = DaylightSensorBlock.Hardness)]
    public static class DaylightSensorBlock
    {
        public const string DisplayName = "Daylight Sensor";
        public const short BlockId = 151;
		public const double Hardness = 0.2;
    }
}
