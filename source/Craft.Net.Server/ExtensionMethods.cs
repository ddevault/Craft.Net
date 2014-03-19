using System;
using Craft.Net.Logic;
using Craft.Net.Anvil;
using System.IO;
using fNbt;
using Craft.Net.Common;

namespace Craft.Net.Server
{
    public static class ExtensionMethods
    {
        // TODO: Attempt to move these into Level proper
        public static void LoadPlayer(this Level level, RemoteClient client)
        {
            if (string.IsNullOrEmpty(level.BaseDirectory))
            {
                CreateNewPlayer(level, client);
                return;
            }
            Directory.CreateDirectory(Path.Combine(level.BaseDirectory, "players"));
            if (File.Exists(Path.Combine(level.BaseDirectory, "players", client.Username + ".dat")))
            {
                try
                {
                    var file = new NbtFile(Path.Combine(level.BaseDirectory, "players", client.Username + ".dat"));
                    // TODO: Consder trying to serialize this, maybe use AutoMapper?
                    client.Entity = new PlayerEntity(client.Username);
                    client.GameMode = (GameMode)file.RootTag["playerGameType"].IntValue;
                    client.Entity.SelectedSlot = file.RootTag["SelectedItemSlot"].ShortValue;
                    client.Entity.SpawnPoint = new Vector3(
                        file.RootTag["SpawnX"].IntValue,
                        file.RootTag["SpawnY"].IntValue,
                        file.RootTag["SpawnZ"].IntValue);
                    client.Entity.Food = (short)file.RootTag["foodLevel"].IntValue;
                    client.Entity.FoodExhaustion = file.RootTag["foodExhaustionLevel"].FloatValue;
                    client.Entity.FoodSaturation = file.RootTag["foodSaturationLevel"].FloatValue;
                    client.Entity.Health = file.RootTag["Health"].ShortValue;
                    foreach (var tag in (NbtList)file.RootTag["Inventory"])
                    {
                        client.Entity.Inventory[Level.DataSlotToNetworkSlot(tag["Slot"].ByteValue)] = 
                            new ItemStack(
                                tag["id"].ShortValue,
                                (sbyte)tag["Count"].ByteValue,
                                tag["Damage"].ShortValue,
                                tag["tag"] as NbtCompound);
                    }
                    client.Entity.Position = new Vector3(
                        file.RootTag["Pos"][0].DoubleValue,
                        file.RootTag["Pos"][1].DoubleValue,
                        file.RootTag["Pos"][2].DoubleValue);
                    client.Entity.Yaw = file.RootTag["Rotation"][0].FloatValue;
                    client.Entity.Pitch = file.RootTag["Rotation"][1].FloatValue;
                    client.Entity.Velocity = new Vector3(
                        file.RootTag["Motion"][0].DoubleValue,
                        file.RootTag["Motion"][1].DoubleValue,
                        file.RootTag["Motion"][2].DoubleValue);
                }
                catch
                {
                    CreateNewPlayer(level, client);
                }
            }
            else
                CreateNewPlayer(level, client);
        }

        public static void SavePlayer(this Level level, RemoteClient client)
        {
            if (string.IsNullOrEmpty(level.BaseDirectory))
                return;
            try
            {
                Directory.CreateDirectory(Path.Combine(level.BaseDirectory, "players"));
                var file = new NbtFile();
                file.RootTag.Add(new NbtInt("playerGameType", (int)client.GameMode));
                file.RootTag.Add(new NbtShort("SelectedItemSlot", client.Entity.SelectedSlot));
                file.RootTag.Add(new NbtInt("SpawnX", (int)client.Entity.SpawnPoint.X));
                file.RootTag.Add(new NbtInt("SpawnY", (int)client.Entity.SpawnPoint.Y));
                file.RootTag.Add(new NbtInt("SpawnZ", (int)client.Entity.SpawnPoint.Z));
                file.RootTag.Add(new NbtInt("foodLevel", client.Entity.Food));
                file.RootTag.Add(new NbtFloat("foodExhaustionLevel", client.Entity.FoodExhaustion));
                file.RootTag.Add(new NbtFloat("foodSaturationLevel", client.Entity.FoodSaturation));
                file.RootTag.Add(new NbtShort("Health", client.Entity.Health));
                var inventory = new NbtList("Inventory", NbtTagType.Compound);
                var slots = client.Entity.Inventory.GetSlots();
                for (int i = 0; i < slots.Length; i++)
                {
                    var slot = (ItemStack)slots[i].Clone();
                    slot.Index = Level.NetworkSlotToDataSlot(i);
                    if (!slot.Empty)
                        inventory.Add(slot.ToNbt());
                }
                file.RootTag.Add(inventory);
                var position = new NbtList("Pos", NbtTagType.Double);
                position.Add(new NbtDouble(client.Entity.Position.X));
                position.Add(new NbtDouble(client.Entity.Position.Y));
                position.Add(new NbtDouble(client.Entity.Position.Z));
                file.RootTag.Add(position);
                var rotation = new NbtList("Rotation", NbtTagType.Float);
                rotation.Add(new NbtFloat(client.Entity.Yaw));
                rotation.Add(new NbtFloat(client.Entity.Pitch));
                file.RootTag.Add(rotation);
                var velocity = new NbtList("Motion", NbtTagType.Double);
                velocity.Add(new NbtDouble(client.Entity.Velocity.X));
                velocity.Add(new NbtDouble(client.Entity.Velocity.Y));
                velocity.Add(new NbtDouble(client.Entity.Velocity.Z));
                file.RootTag.Add(velocity);
                file.SaveToFile(Path.Combine(level.BaseDirectory, "players", client.Username + ".dat"), NbtCompression.None);
            }
            catch { } // If exceptions happen here, the entire server dies
        }

        private static void CreateNewPlayer(Level level, RemoteClient client)
        {
            client.Entity = new PlayerEntity(client.Username);
            client.Entity.Position = level.Spawn;
            client.Entity.SpawnPoint = level.Spawn;
            client.GameMode = level.GameMode;
        }
    }
}

