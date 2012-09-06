using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            get { return 0x8; }
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
            byte[] payload = new byte[] {PacketId}
                .Concat(DataUtility.CreateInt16(Health))
                .Concat(DataUtility.CreateInt16(Food))
                .Concat(DataUtility.CreateFloat(FoodSaturation))
                .ToArray();
            client.SendData(payload);
        }
    }
}
