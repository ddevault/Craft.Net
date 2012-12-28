using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Blocks;

namespace Craft.Net.Data.Items
{
    public class DyeItem : Item
    {
        public enum DyeType
        {
            InkSac = 0,
            RoseRed = 1,
            CactusGreen = 2,
            CocoaBeans = 3,
            LapisLazuli = 4,
            PurpleDye = 5,
            CyanDye = 6,
            LightGrayDye = 7,
            GrayDye = 8,
            PinkDye = 9,
            LimeDye = 10,
            DandelionYellow = 11,
            LightBlueDye = 12,
            MagentaDye = 13,
            OrangeDye = 14,
            BoneMeal = 15
        }

        public DyeType Type
        {
            get { return (DyeType)Data; }
            set { Data = (short)value; }
        }

        public override short Id
        {
            get { return 351; }
        }

        public DyeItem() { }

        public DyeItem(DyeType type)
        {
            Type = type;
        }

        public override void OnItemUsedOnBlock(World world, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entities.Entity usedBy)
        {
            if (Type == DyeType.BoneMeal)
            {
                var block = world.GetBlock(clickedBlock) as IGrowableBlock;
                if (block != null)
                {
                    if (block.Grow(world, clickedBlock, true))
                        base.OnItemUsedOnBlock(world, clickedBlock, clickedSide, cursorPosition, usedBy);
                }
            }
            else if (Type == DyeType.CocoaBeans)
            {
                var block = new CocoaPlantBlock();
                if (block.OnBlockPlaced(world, clickedBlock + clickedSide, clickedBlock, clickedSide, cursorPosition, usedBy))
                    world.SetBlock(clickedBlock + clickedSide, block);
                base.OnItemUsedOnBlock(world, clickedBlock, clickedSide, cursorPosition, usedBy);
            }
        }
    }
}
