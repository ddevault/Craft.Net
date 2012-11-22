using System;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public class UpdateHealthPacket : Packet
    {
        public short Health, Food;
        public float FoodSaturation;

        public UpdateHealthPacket()
        {
        }

        public UpdateHealthPacket(short health, short food, float foodSaturation)
        {
            Health = health;
            Food = food;
            FoodSaturation = foodSaturation;
        }

        public override byte PacketId
        {
            get { return 0x08; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            throw new InvalidOperationException();
        }

        public override void HandlePacket(MinecraftServer server, MinecraftClient client)
        {
            throw new InvalidOperationException();
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            client.SendData(CreateBuffer(
                DataUtility.CreateInt16(Health),
                DataUtility.CreateInt16(Food),
                DataUtility.CreateFloat(FoodSaturation)));
            }
        }
}