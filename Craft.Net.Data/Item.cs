using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Blocks;
using Craft.Net.Data.Entities;
using Craft.Net.Data.Items;

namespace Craft.Net.Data
{
    /// <summary>
    /// Represents an item in Minecraft.
    /// </summary>
    public abstract class Item : IComparable
    {
        /// <summary>
        /// This item's ID.
        /// </summary>
        public abstract ushort Id { get; }
        /// <summary>
        /// The metadata or durability of this item.
        /// </summary>
        public virtual ushort Data { get; set; }

        /// <summary>
        /// The amount of damage hitting a living entity with this item
        /// will do.
        /// </summary>
        public virtual int AttackDamage
        {
            get { return 1; }
        }

        public virtual byte MaximumStack
        {
            get { return 64; }
        }

        /// <summary>
        /// Called when this item is used by a player.
        /// </summary>
        public virtual void OnItemUsedOnBlock(World world, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entity usedBy)
        {
        }

        /// <summary>
        /// Called when this item is used, even when a block is not targeted.
        /// </summary>
        public virtual void OnItemUsed(World world, Entity usedBy)
        {
            var player = usedBy as PlayerEntity;
            if (player != null && player.GameMode != GameMode.Creative)
                player.Inventory[player.SelectedSlot].Count--;
        }

        #region Items Conversion

        public static implicit operator ushort(Item i)
        {
            return i.Id;
        }

        public static implicit operator Item(ushort u)
        {
            // Binary search through items
            int index = GetItemIndex(u);
            if (index == -1)
                return null;
            return (Item)Activator.CreateInstance(Items[index].GetType());
        }

        private static int GetItemIndex(ushort item)
        {
            // Binary search for items
            if (item > Items[Items.Count - 1].Id)
                return -1;
            int a = 0, b = Items.Count;
            while (b >= a)
            {
                int c = (a + b) / 2;
                if (item > Items[c].Id)
                    a = c + 1;
                else if (item < Items[c].Id)
                    b = c - 1;
                else
                    return c;
            }
            return -1;
        }

        /// <summary>
        /// Use this method to override the implementation of
        /// default items, or to add your own.
        /// </summary>
        public static void SetItemClass(Item item)
        {
            int index = GetItemIndex(item.Id);
            if (index != -1)
                Items[index] = item;
            else
            {
                Items.Add(item);
                Items.Sort();
            }
        }

        public static List<Item> Items = new List<Item>(new Item[]
        {
            new AirBlock(),
            new StoneBlock(),
            new GrassBlock(),
            new DirtBlock(),
            new CobblestoneBlock(),
            new WoodenPlanksBlock(),
            new SaplingBlock(),
            new BedrockBlock(),
            new WaterFlowingBlock(),
            new WaterStillBlock(),
            new LavaFlowingBlock(),
            new LavaStillBlock(),
            new SandBlock(),
            new GravelBlock(),
            new GoldOreBlock(),
            new IronOreBlock(),
            new CoalOreBlock(),
            new WoodBlock(),
            new LeavesBlock(),
            new SpongeBlock(),
            new GlassBlock(),
            new LapisLazuliOreBlock(),
            new LapisLazuliBlock(),
            new DispenserBlock(),
            new SandstoneBlock(),
            new NoteBlock(),
            new BedBlock(),
            new PoweredRailBlock(),
            new DetectorRailBlock(),
            new StickyPistonBlock(),
            new CobwebBlock(),
            new TallGrassBlock(),
            new DeadBushBlock(),
            new PistonBlock(),
            new PistonHeadBlock(),
            new WoolBlock(),
            new PistonMovementBlock(),
            new YellowFlowerBlock(),
            new RedFlowerBlock(),
            new BrownMushroomBlock(),
            new RedMushroomBlock(),
            new GoldBlock(),
            new IronBlock(),
            new DoubleSlabBlock(),
            new SlabBlock(),
            new BrickBlock(),
            new TNTBlock(),
            new BookshelfBlock(),
            new MossyCobblestoneBlock(),
            new ObsidianBlock(),
            new TorchBlock(),
            new FireBlock(),
            new MobSpawnerBlock(),
            new OakStairBlock(),
            new ChestBlock(),
            new RedstoneWireBlock(),
            new DiamondOreBlock(),
            new DiamondBlock(),
            new CraftingTableBlock(),
            new SeedsBlock(),
            new FarmlandBlock(),
            new FurnaceBlock(),
            new FurnaceActiveBlock(),
            new SignPostBlock(),
            new WoodenDoorBlock(),
            new LadderBlock(),
            new RailBlock(),
            new CobblestoneStairBlock(),
            new WallSignBlock(),
            new LeverBlock(),
            new StonePressurePlateBlock(),
            new IronDoorBlock(),
            new WoodenPressurePlateBlock(),
            new RedstoneOreBlock(),
            new RedstoneOreActiveBlock(),
            new RedstoneTorchBlock(),
            new RedstoneTorchActiveBlock(),
            new StoneButtonBlock(),
            new SnowfallBlock(),
            new IceBlock(),
            new SnowBlock(),
            new CactusBlock(),
            new ClayBlock(),
            new SugarCaneBlock(),
            new JukeboxBlock(),
            new FenceBlock(),
            new PumpkinBlock(),
            new NetherrackBlock(),
            new SoulSandBlock(),
            new GlowstoneBlock(),
            new NetherPortalBlock(),
            new JackOLanternBlock(),
            new CakeBlock(),
            new RedstoneRepeaterBlock(),
            new RedstoneRepeaterActiveBlock(),
            new LockedChestBlock(),
            new TrapDoorBlock(),
            new SilverfishStoneBlock(),
            new StoneBrickBlock(),
            new HugeBrownMushroomBlock(),
            new HugeRedMushroomBlock(),
            new IronBarBlock(),
            new GlassPaneBlock(),
            new MelonBlock(),
            new PumpkinStemBlock(),
            new MelonStemBlock(),
            new VineBlock(),
            new FenceGateBlock(),
            new BrickStairBlock(),
            new StoneBrickStairBlock(),
            new MyceliumBlock(),
            new LilyPadBlock(),
            new NetherBrickBlock(),
            new NetherBrickFenceBlock(),
            new NetherBrickStairBlock(),
            new NetherWartBlock(),
            new EnchantmentTableBlock(),
            new BrewingStandBlock(),
            new CauldronBlock(),
            new EndPortalBlock(),
            new EndPortalFrameBlock(),
            new EndStoneBlock(),
            new DragonEggBlock(),
            new RedstoneLampBlock(),
            new RedstoneLampActiveBlock(),
            new WoodenDoubleSlabBlock(),
            new WoodenSlabBlock(),
            new CocoaPlantBlock(),
            new SandstoneStairBlock(),
            new EmeraldOreBlock(),
            new EnderChestBlock(),
            new TripwireHookBlock(),
            new TripwireBlock(),
            new EmeraldBlock(),
            new SpruceWoodStairBlock(),
            new BirchWoodStairBlock(),
            new JungleWoodStairBlock(),
            new CommandBlock(),
            new BeaconBlock(),
            new CobblestoneWallBlock(),
            new FlowerPotBlock(),
            new CarrotBlock(),
            new PotatoBlock(),
            new WoodenButtonBlock(),
            new SkullBlock(),
            new AnvilBlock(),
            new IronShovelItem(),
            new IronPickaxeItem(),
            new IronAxeItem(),
            new FlintAndSteelItem(),
            new AppleItem(),
            new BowItem(),
            new ArrowItem(),
            new CoalItem(),
            new DiamondItem(),
            new IronIngotItem(),
            new GoldIngotItem(),
            new IronSwordItem(),
            new WoodenSwordItem(),
            new WoodenShovelItem(),
            new WoodenPickaxeItem(),
            new WoodenAxeItem(),
            new StoneSwordItem(),
            new StoneShovelItem(),
            new StonePickaxeItem(),
            new StoneAxeItem(),
            new DiamondSwordItem(),
            new DiamondShovelItem(),
            new DiamondPickaxeItem(),
            new DiamondAxeItem(),
            new StickItem(),
            new BowlItem(),
            new MushroomStewItem(),
            new GoldenSwordItem(),
            new GoldenShovelItem(),
            new GoldenPickaxeItem(),
            new GoldenAxeItem(),
            new StringItem(),
            new FeatherItem(),
            new GunpowderItem(),
            new WoodenHoeItem(),
            new StoneHoeItem(),
            new IronHoeItem(),
            new DiamondHoeItem(),
            new GoldenHoeItem(),
            new SeedsItem(),
            new WheatItem(),
            new BreadItem(),
            new LeatherHelmetItem(),
            new LeatherChestplateItem(),
            new LeatherLeggingsItem(),
            new LeatherBootsItem(),
            new ChainHelmetItem(),
            new ChainChestplateItem(),
            new ChainLeggingsItem(),
            new ChainBootsItem(),
            new IronHelmetItem(),
            new IronChestplateItem(),
            new IronLeggingsItem(),
            new IronBootsItem(),
            new DiamondHelmetItem(),
            new DiamondChestplateItem(),
            new DiamondLeggingsItem(),
            new DiamondBootsItem(),
            new GoldenHelmetItem(),
            new GoldenChestplateItem(),
            new GoldenLeggingsItem(),
            new GoldenBootsItem(),
            new FlintItem(),
            new RawPorkchopItem(),
            new CookedPorkchopItem(),
            new PaintingItem(),
            new GoldenAppleItem(),
            new SignItem(),
            new WoodenDoorItem(),
            new BucketItem(),
            new WaterBucketItem(),
            new LavaBucketItem(),
            new MinecartItem(),
            new SaddleItem(),
            new IronDoorItem(),
            new RedstoneItem(),
            new SnowballItem(),
            new BoatItem(),
            new LeatherItem(),
            new MilkItem(),
            new BrickItem(),
            new ClayItem(),
            new SugarCanesItem(),
            new PaperItem(),
            new BookItem(),
            new SlimeballItem(),
            new MinecartWithChestItem(),
            new MinecartWithFurnaceItem(),
            new EggItem(),
            new CompassItem(),
            new FishingRodItem(),
            new ClockItem(),
            new GlowstoneDustItem(),
            new RawFishItem(),
            new CookedFishItem(),
            new BoneItem(),
            new SugarItem(),
            new CakeItem(),
            new BedItem(),
            new RedstoneRepeaterItem(),
            new CookieItem(),
            new MapItem(),
            new ShearsItem(),
            new MelonItem(),
            new PumpkinSeedsItem(),
            new MelonSeedsItem(),
            new RawBeefItem(),
            new SteakItem(),
            new RawChickenItem(),
            new CookedChickenItem(),
            new RottenFleshItem(),
            new EnderPearlItem(),
            new BlazeRodItem(),
            new GhastTearItem(),
            new GoldNuggetItem(),
            new NetherWartItem(),
            new PotionItem(),
            new GlassBottleItem(),
            new SpiderEyeItem(),
            new FermentedSpiderEyeItem(),
            new BlazePowderItem(),
            new MagmaCreamItem(),
            new BrewingStandItem(),
            new CauldronItem(),
            new EyeOfEnderItem(),
            new GlisteringMelonItem(),
            new MobEggItem(),
            new BottleOEnchantingItem(),
            new FireChargeItem(),
            new BookAndQuillItem(),
            new WrittenBookItem(),
            new EmeraldItem(),
            new ItemFrameItem(),
            new FlowerPotItem(),
            new CarrotItem(),
            new PotatoItem(),
            new BakedPotatoItem(),
            new PoisonousPotatoItem(),
            new GoldenCarrotItem(),
            new SkullItem(),
            new CarrotOnStickItem(),
            new NetherStarItem(),
            new PumpkinPieItem(),
            new MusicDiscItem()
        });

        #endregion

        #region IComparable

        public int CompareTo(object obj)
        {
            return this.Id - ((Item)obj).Id;
        }

        #endregion
    }
}
