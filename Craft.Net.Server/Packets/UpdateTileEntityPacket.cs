using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Craft.Net.Data;
using LibNbt;
using LibNbt.Tags;

namespace Craft.Net.Server.Packets
{
    public enum TileEntityAction
    {
        MobSpawner = 1,
        UpdateSign = 6 // Unverified
    }

    public class UpdateTileEntityPacket : Packet
    {
        public Vector3 Position;
        public TileEntityAction Action;
        public NbtFile Data;

        public UpdateTileEntityPacket()
        {
        }

        public UpdateTileEntityPacket(Vector3 position, TileEntityAction action, NbtCompound data)
        {
            Data = new NbtFile();
            Data.RootTag = data;
            Position = position;
            Action = action;
        }

        public override byte PacketId
        {
            get { return 0x84; }
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
            MemoryStream stream = new MemoryStream();
            Data.SaveFile(stream, true);
            byte[] nbt = stream.GetBuffer();

            var payload = new byte[] {PacketId}
                .Concat(DataUtility.CreateInt32((int)Position.X))
                .Concat(DataUtility.CreateInt16((short)Position.Y))
                .Concat(DataUtility.CreateInt32((int)Position.Z))
                .Concat(new byte[] {(byte)Action});
            // TODO: Empty NBT (is this ever useful?)
            payload = payload.Concat(DataUtility.CreateInt16((short)nbt.Length)).Concat(nbt);
            client.SendData(payload.ToArray());
        }
    }
}
