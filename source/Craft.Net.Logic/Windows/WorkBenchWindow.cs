using Craft.Net.Common;
using System;

namespace Craft.Net.Logic.Windows
{
    public class WorkBenchWindow : Window
    {
        public const int CraftingOutput = 0;
        public const int CraftingIndex = 0;
        public const int MainInventoryIndex = 10;
        public const int HotBarIndex = 37;

        public WorkBenchWindow()
        {
            Id = (byte)MathHelper.Random.Next();
            WindowAreas = new[]
            {
                new WindowArea(CraftingIndex,10),
                new WindowArea(MainInventoryIndex,27),
                new WindowArea(HotBarIndex,9)
            };
            foreach (var area in WindowAreas)
                area.WindowChange += (s, e) => OnWindowChange(new WindowChangeEventArgs(
                    (s as WindowArea).StartIndex + e.SlotIndex, e.Value));
        }

        public void LoadWorkBenchItem(ItemStack[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                this[i] = items[i];
            }
        }

        public WindowArea CraftingGrid
        {
            get { return WindowAreas[0]; }
        }


        public override WindowArea[] WindowAreas { get; protected set; }

        public override byte Id { get; protected set; }

        protected override WindowArea GetLinkedArea(int index, Common.ItemStack slot)
        {
            // TODO : Not tested
            if (index == 0 || index == 1 || index == 3)
                return WindowAreas[1];
            return WindowAreas[2];
        }

    }
}
