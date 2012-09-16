using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Windows
{
    public class InventoryWindow : Window
    {
        public InventoryWindow()
        {
            WindowAreas = new[]
                {
                    new WindowArea(CraftingOutputIndex, 5), // Crafting grid
                    new WindowArea(ArmorIndex, 4), // Armor
                    new WindowArea(MainIndex, 27), // Main inventory
                    new WindowArea(HotbarIndex, 9) // Hotbar
                };
        }

        #region Variables

        public const int HotbarIndex = 36;
        public const int CraftingGridIndex = 1;
        public const int CraftingOutputIndex = 0;
        public const int ArmorIndex = 5;
        public const int MainIndex = 9;

        public override WindowArea[] WindowAreas { get; protected set; }

        #region Properties

        public WindowArea CraftingGrid 
        {
            get { return WindowAreas[0]; }
        }

        public WindowArea Armor
        {
            get { return WindowAreas[1]; }
        }

        public WindowArea MainInventory
        {
            get { return WindowAreas[2]; }
        }

        public WindowArea Hotbar
        {
            get { return WindowAreas[3]; }
        }

        #endregion

        #endregion

        protected override WindowArea GetLinkedArea(int index)
        {
            // TODO: Shift-click armor
            if (index == 0 || index == 1 || index == 3)
                return MainInventory;
            return Hotbar;
        }
    }
}
