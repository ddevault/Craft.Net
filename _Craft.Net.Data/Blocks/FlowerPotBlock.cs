using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Entities;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class FlowerPotBlock : Block
    {
        private short[] validPlantIds = new short[] { 6, 31, 32, 37, 38, 39, 40, 81 };
        public enum FlowerPotPlant
        {
            Empty = 0,
            Rose = 1,
            Dandelion = 2,
            OakSapling = 3,
            SpruceSapling = 4,
            BirchSapling = 5,
            JungleTreeSapling = 6,
            RedMushroom = 7,
            BrownMushroom = 8,
            Cactus = 9,
            DeadBush = 10,
            Fern = 11
        }

        public FlowerPotBlock() { }

        public FlowerPotBlock(FlowerPotPlant plant)
        {
            Plant = plant;
        }

        public FlowerPotPlant Plant
        {
            get { return (FlowerPotPlant)Metadata; }
            set { Metadata = (byte)value; }
        }

        public override short Id
        {
            get { return 140; }
        }

        public override bool OnBlockRightClicked(Vector3 clickedBlock, Vector3 clickedSide, 
            Vector3 cursorPosition, World world, Entity usedBy)
        {
            var player = usedBy as PlayerEntity;
            if (player == null) return true;

            var item = player.SelectedItem.AsItem();
            if (!validPlantIds.Contains(item.Id))
                return true;
            if (Plant != FlowerPotPlant.Empty)
                return false;

            // Apply item
            if (item is RedFlowerBlock)
                Plant = FlowerPotPlant.Rose;
            else if (item is YellowFlowerBlock)
                Plant = FlowerPotPlant.Dandelion;
            else if (item is RedMushroomBlock)
                Plant = FlowerPotPlant.RedMushroom;
            else if (item is BrownMushroomBlock)
                Plant = FlowerPotPlant.BrownMushroom;
            else if (item is CactusBlock)
                Plant = FlowerPotPlant.Cactus;
            else if (item is DeadBushBlock)
                Plant = FlowerPotPlant.DeadBush;
            else if (item is TallGrassBlock)
            {
                var grass = item as TallGrassBlock;
                if (grass.Metadata == 0)
                    Plant = FlowerPotPlant.DeadBush;
                else if (grass.Metadata == 2)
                    Plant = FlowerPotPlant.Fern;
                else
                    return false;
            }
            else if (item is SaplingBlock)
            {
                var sapling = item as SaplingBlock;
                if (sapling.Type == SaplingBlock.SaplingType.Oak)
                    Plant = FlowerPotPlant.OakSapling;
                else if (sapling.Type == SaplingBlock.SaplingType.Spruce)
                    Plant = FlowerPotPlant.SpruceSapling;
                else if (sapling.Type == SaplingBlock.SaplingType.Birch)
                    Plant = FlowerPotPlant.BirchSapling;
                else
                    Plant = FlowerPotPlant.JungleTreeSapling;
            }
            world.SetBlock(clickedBlock, this);
            if (player.GameMode != GameMode.Creative)
            {
                var slot = player.Inventory[player.SelectedSlot];
                slot.Count--;
                player.Inventory[player.SelectedSlot] = slot;
            }
            return false;
        }

        public override bool GetDrop(ToolItem tool, out ItemStack[] drop)
        {
            if (Plant == FlowerPotPlant.Empty)
            {
                drop = new[] { new ItemStack(new FlowerPotItem()) };
                return true;
            }
            Block plant;
            switch (Plant)
            {
                case FlowerPotPlant.Fern:
                    plant = new TallGrassBlock();
                    plant.Metadata = 2;
                    break;
                case FlowerPotPlant.Rose:
                    plant = new RedFlowerBlock();
                    break;
                case FlowerPotPlant.Dandelion:
                    plant = new YellowFlowerBlock();
                    break;
                case FlowerPotPlant.OakSapling:
                    plant = new SaplingBlock(SaplingBlock.SaplingType.Oak);
                    break;
                case FlowerPotPlant.SpruceSapling:
                    plant = new SaplingBlock(SaplingBlock.SaplingType.Spruce);
                    break;
                case FlowerPotPlant.BirchSapling:
                    plant = new SaplingBlock(SaplingBlock.SaplingType.Birch);
                    break;
                case FlowerPotPlant.DeadBush:
                    plant = new DeadBushBlock();
                    break;
                case FlowerPotPlant.JungleTreeSapling:
                    plant = new SaplingBlock(SaplingBlock.SaplingType.Jungle);
                    break;
                case FlowerPotPlant.RedMushroom:
                    plant = new RedMushroomBlock();
                    break;
                case FlowerPotPlant.BrownMushroom:
                    plant = new BrownMushroomBlock();
                    break;
                default:
                    plant = new CactusBlock();
                    break;
            }
            drop = new[]
            {
                new ItemStack(new FlowerPotItem()),
                new ItemStack(plant, 1, plant.Metadata)
            };
            return true;
        }
    }
}
