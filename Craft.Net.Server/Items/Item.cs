using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Items
{
    public abstract class Item
    {
        #region Constant Items

        private static Item[] Items = new Item[]
        {
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
            new MushroomSoupItem(),
            new GoldSwordItem(),
            new GoldShovelItem(),
            new GoldPickaxeItem(),
            new GoldAxeItem(),
            new StringItem(),
            new FeatherItem(),
            new SulphurItem(),
            new WoodenHoeItem(),
            new StoneHoeItem(),
            new IronHoeItem(),
            new DiamondHoeItem(),
            new GoldHoeItem(),
            new SeedsItem(),
            new WheatItem(),
            new BreadItem(),
            new LeatherHelmetItem(),
            new LeatherChestplateItem(),
            new LeatherLeggingsItem(),
            new LeatherBootsItem(),
            new ChainmailHelmetItem(),
            new ChainmailChestplateItem(),
            new ChainmailLeggingsItem(),
            new ChainmailBootsItem(),
            new IronHelmetItem(),
            new IronChestplateItem(),
            new IronLeggingsItem(),
            new IronBootsItem(),
            new DiamondHelmetItem(),
            new DiamondChestplateItem(),
            new DiamondLeggingsItem(),
            new DiamondBootsItem(),
            new GoldHelmetItem(),
            new GoldChestplateItem(),
            new GoldLeggingsItem(),
            new GoldBootsItem(),
            new FlintItem(),
            new RawPorkchopItem(),
            new CookedPorkchopItem(),
            new PaintingItem(),
            new GoldenAppleItem(),
            new SignPostItem(),
            new WoodenDoorItem(),
            new BucketItem(),
            new WaterBucketItem(),
            new LavaBucketItem(),
            new MinecartItem(),
            new SaddleItem(),
            new IronDoorItem(),
            new RedstoneDustItem(),
            new SnowballItem(),
            new BoatItem(),
            new LeatherItem(),
            new MilkBucketItem(),
            new ClayBrickItem(),
            new ClayBallsItem(),
            new SugarcaneItem(),
            new PaperItem(),
            new BookItem(),
            new SlimeballItem(),
            new StorageMinecartItem(),
            new PoweredMinecartItem(),
            new EggItem(),
            new CompassItem(),
            new FishingRodItem(),
            new ClockItem(),
            new GlowstoneDustItem(),
            new RawFishItem(),
            new CookedFishItem(),
            new InkSackItem(),
            new BoneItem(),
            new SugarItem(),
            new CakeItem(),
            new BedItem(),
            new RedstoneRepeaterItem(),
            new CookieItem(),
            new MapItem(),
            new ShearsItem(),
            new MelonItem(),
            new MelonSeedsItem(),
            new PumpkinSeedsItem(),
            new BeefItem(),
            new CookedBeefItem(),
            new RawChickenItem(),
            new CookedChickenItem(),
            new RottenFleshItem(),
            new EnderPearlItem(),
            new BlazeRodItem(),
            new GhastTearItem(),
            new GoldNuggetItem(),
            new NetherWartItem(),
            new PotionItem(),
            new BottleItem(),
            new SpiderEyeItem(),
            new FermentedSpiderEyeItem(),
            new BlazePowderItem(),
            new MagmaCreamItem(),
            new BrewingStandItem(),
            new CauldronItem(),
            new EyeOfEnderItem(),
            new GlisteringMelonItem(),
            new MonsterSpawningEggItem(),
            new BottleOEnchantingItem(),
            new FireChargeItem(),
            new ThirteenMusicDiscItem(),
            new CatMusicDiscItem(),
            new BlocksMusicDiscItem(),
            new ChirpMusicDiscItem(),
            new FarMusicDiscItem(),
            new MallMusicDiscItem(),
            new MellohiMusicDiscItem(),
            new StalMusicDiscItem(),
            new StradMusicDiscItem(),
            new WardMusicDiscItem(),
            new ElevenMusicDiscItem(),
        };

        #endregion

        /// <summary>
        /// The ID for this Item
        /// </summary>
        /// <remarks></remarks>
        public abstract short ItemID { get; }

        public virtual byte Metadata { get; set; }

        public virtual short Damage { get; set; }

        public virtual bool IsStackable
        {
            get
            {
                return false;
            }
        }

        public static implicit operator short(Item i)
        {
            return i.ItemID;
        }

        public static implicit operator Item(short s)
        {
            return Items[s - 256];
        }

        /// <summary>
        /// Override an existing item
        /// </summary>
        /// <param name="item">The item to replace the old item with</param>
        public static void OverrideItem(Item item)
        {
            Items[item.ItemID - 256] = item;
        }
    }
}
