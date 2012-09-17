using System;
using System.ComponentModel;
using System.Threading;
using Craft.Net.Data.Events;
using Craft.Net.Data.Windows;
namespace Craft.Net.Data.Entities
{
    public class PlayerEntity : LivingEntity
    {
        public PlayerEntity(Difficulty difficulty)
        {
            Inventory = new InventoryWindow();
            SelectedSlot = InventoryWindow.HotbarIndex;
            bedUseTimer = new Timer(state =>
            {
                if (BedTimerExpired != null)
                    BedTimerExpired(this, null);
            });
            BedPosition = -Vector3.One;
            Health = 20;
            Food = 20;
            Abilities = new PlayerAbilities(this);
            Difficulty = difficulty;
            FoodTickTimer = new Timer(discarded =>
                {
                    if (Food > 17 && Health < 20 && Health != 0) // TODO: HealthMax constant?
                        Health++;
                    if (Food == 0)
                    {
                        switch (Difficulty)
                        {
                            case Difficulty.Hard:
                                if (Health > 0)
                                    Health--;
                                break;
                            case Difficulty.Normal:
                                if (Health > 1)
                                    Health--;
                                break;
                            default:
                                if (Health > 10)
                                    Health--;
                                break;
                        }
                    }
                }, null, 80 * Level.TickLength, 80 * Level.TickLength);
        }

        #region Properties

        #region Constants

        public override Size Size
        {
            get { return new Size(0.6, 1.8, 0.6); }
        }

        public override short MaxHealth
        {
            get { return 20; }
        }

        public static double Width
        {
            get { return 0.6; }
        }

        public static double Height
        {
            get { return 1.8; }
        }

        public static double Depth
        {
            get { return 0.6; }
        }

        public override float AccelerationDueToGravity
        {
            get { return 1.6f; }
        }

        public override float TerminalVelocity
        {
            get
            {
                return 78.4f;
            }
        }

        #endregion

        #region Food & XP

        public short Food
        {
            get { return food; }
            set
            {
                food = value;
                OnPropertyChanged("Food");
            }
        }

        public float FoodSaturation
        {
            get { return foodSaturation; }
            set
            {
                foodSaturation = value;
                if (value == 0)
                    OnPropertyChanged("FoodSaturation");
            }
        }

        public float FoodExhaustion
        {
            get { return foodExhaustion; }
            set
            {
                if (Difficulty == Difficulty.Peaceful)
                    return;
                foodExhaustion = value;
                if (foodExhaustion > 4)
                {
                    if (FoodSaturation > 0)
                        FoodSaturation--;
                    else
                    {
                        if (Food > 0)
                            Food--;
                    }
                    foodExhaustion -= 4;
                }
                OnPropertyChanged("FoodExhaustion");
            }
        }

        protected Timer FoodTickTimer { get; set; }

        public int XpLevel
        {
            get { return xpLevel; }
            set
            {
                xpLevel = value;
                OnPropertyChanged("XpLevel");
            }
        }

        public int XpTotal
        {
            get { return xpTotal; }
            set
            {
                xpTotal = value;
                OnPropertyChanged("XpTotal");
            }
        }

        public float XpProgress
        {
            get { return xpProgress; }
            set
            {
                xpProgress = value;
                OnPropertyChanged("XpProgress");
            }
        }

        #endregion

        public string Username { get; set; }
        /// <summary>
        /// The client's current inventory.
        /// </summary>
        public InventoryWindow Inventory { get; set; }
        public short SelectedSlot
        {
            get { return selectedSlot; }
            set
            {
                selectedSlot = value;
                OnPropertyChanged("SelectedSlot");
            }
        }

        public Slot SelectedItem
        {
            get { return Inventory[SelectedSlot]; }
        }

        /// <summary>
        /// Set to -Vector3.One if the player is not in a bed.
        /// </summary>
        public Vector3 BedPosition
        {
            get { return bedPosition; }
            set
            {
                bedPosition = value;
                OnPropertyChanged("BedPosition");
            }
        }

        public GameMode GameMode
        {
            get { return gameMode; }
            set
            {
                gameMode = value;
                Abilities.FirePropertyChanged = false;
                if (value == GameMode.Creative)
                {
                    Abilities.InstantMine = true;
                    Abilities.Invulnerable = true;
                    Abilities.MayFly = true;
                }
                else
                {
                    Abilities.InstantMine = false;
                    Abilities.Invulnerable = false;
                    Abilities.MayFly = false;
                    Abilities.IsFlying = false;
                }
                Abilities.FirePropertyChanged = true;
                OnPropertyChanged("GameMode");
            }
        }

        public Difficulty Difficulty { get; set; }

        public Vector3 SpawnPoint
        {
            get { return spawnPoint; }
            set
            {
                spawnPoint = value;
                OnPropertyChanged("SpawnPoint");
            }
        }

        public PlayerAbilities Abilities { get; set; }

        /// <summary>
        /// When moving items around in a survival inventory,
        /// this represents the item the player is holding.
        /// </summary>
        public Slot ItemInMouse { get; set; }

        public override bool Invulnerable
        {
            get { return Abilities.Invulnerable; }
        }

        /// <summary>
        /// The position provided by the client.
        /// </summary>
        public Vector3 GivenPosition
        {
            get { return givenPosition; }
            set
            {
                givenPosition = value;
                LastGivenPositionUpdate = DateTime.Now;
                OnPropertyChanged("GivenPosition");
            }
        }
        public DateTime LastGivenPositionUpdate { get; set; }

        //public override CollisionTests TestsToPerform
        //{
        //    get { return CollisionTests.None; } // TODO: Is this a good idea?
        //}

        /// <summary>
        /// The last entity that attacked the player, used to determine
        /// the killer.
        /// </summary>
        public Entity LastAttackingEntity { get; set; }

        /// <summary>
        /// The type of damage last recieved.
        /// </summary>
        public DamageType LastDamageType { get; set; }

        private Timer bedUseTimer;
        private short food;
        private float foodSaturation;
        private float foodExhaustion;
        private int xpLevel;
        private int xpTotal;
        private float xpProgress;
        private GameMode gameMode;
        private Vector3 bedPosition;
        private short selectedSlot;
        private Vector3 spawnPoint;
        private Vector3 givenPosition;

        public event EventHandler BedStateChanged, BedTimerExpired, StartEating;
        /// <summary>
        /// Note: Only fired when the inventory is changed via SetSlot.
        /// </summary>
        public event EventHandler<InventoryChangedEventArgs> InventoryChanged;

        #endregion

        public void SetSlot(short index, Slot slot)
        {
            if (InventoryChanged != null)
                InventoryChanged(this, new InventoryChangedEventArgs()
                {
                    Index = index,
                    OldValue = Inventory[index],
                    NewValue = slot
                });
            Inventory[index] = slot;
        }

        public void EnterBed(Vector3 position)
        {
            BedPosition = position;
            bedUseTimer.Change(5000, Timeout.Infinite);
            if (BedStateChanged != null)
                BedStateChanged(this, null);
        }

        public void LeaveBed()
        {
            BedPosition = -Vector3.One;
            if (BedStateChanged != null)
                BedStateChanged(this, null);
        }

        public override void Kill()
        {
            deathTimer.Change(3000, Timeout.Infinite);
            for (int i = 0; i < Inventory.Length; i++)
                Inventory[i] = new Slot(0xFFFF, 0);
        }

        protected internal virtual void OnStartEating()
        {
            if (StartEating != null)
                StartEating(this, new EventArgs());
        }

        public override void Damage(int damage, bool accountForArmor = true)
        {
            FoodExhaustion += 0.3f;
            base.Damage(damage, accountForArmor);
        }
    }
}