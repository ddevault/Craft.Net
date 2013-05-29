using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fNbt.Serialization;

namespace Craft.Net.Common
{
    public class GameRules
    {
        public GameRules()
        {
            CommandBlockOutput = true;
            DoFireTick = true;
            DoMobLoot = true;
            DoMobSpawning = true;
            DoTileDrops = true;
            KeepInventory = false;
            MobGriefing = true;
        }

        private string commandBlockOutput;
        /// <summary>
        /// Determines if command blocks will have their results output to chat.
        /// </summary>
        [NbtIgnore]
        public bool CommandBlockOutput
        {
            get { return bool.Parse(commandBlockOutput);  }
            set { commandBlockOutput = value.ToString(); }
        }

        private string doFireTick;
        /// <summary>
        /// Determines if fire may spread.
        /// </summary>
        [NbtIgnore]
        public bool DoFireTick
        {
            get { return bool.Parse(doFireTick); }
            set { doFireTick = value.ToString(); }
        }

        private string doMobLoot;
        /// <summary>
        /// Determines if killing mods drops items.
        /// </summary>
        [NbtIgnore]
        public bool DoMobLoot
        {
            get { return bool.Parse(doMobLoot); }
            set { doMobLoot = value.ToString(); }
        }

        private string doMobSpawning;
        /// <summary>
        /// Determines if mobs will be allowed to spawn.
        /// </summary>
        [NbtIgnore]
        public bool DoMobSpawning
        {
            get { return bool.Parse(doMobSpawning); }
            set { doMobSpawning = value.ToString(); }
        }

        private string doTileDrops;
        /// <summary>
        /// Determines if breaking blocks with tile entities will drop items within.
        /// </summary>
        [NbtIgnore]
        public bool DoTileDrops
        {
            get { return bool.Parse(doTileDrops); }
            set { doTileDrops = value.ToString(); }
        }

        private string keepInventory;
        /// <summary>
        /// True if a player's death does not remove their inventory.
        /// </summary>
        [NbtIgnore]
        public bool KeepInventory
        {
            get { return bool.Parse(keepInventory); }
            set { keepInventory = value.ToString(); }
        }

        private string mobGriefing;
        /// <summary>
        /// True to allow mob effects to modify terrain.
        /// </summary>
        [NbtIgnore]
        public bool MobGriefing
        {
            get { return bool.Parse(mobGriefing); }
            set { mobGriefing = value.ToString(); }
        }
    }
}
