using System;
using System.IO;
using Craft.Net.Data;
using LibNbt;
using LibNbt.Tags;

namespace Craft.Net.Server.Packets
{
    public enum TileEntityAction
    {
        MobSpawner = 1,
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

            // TODO: Empty NBT (is this ever useful?)
            client.SendData(CreateBuffer(
                DataUtility.CreateInt32((int)Position.X),
                DataUtility.CreateInt16((short)Position.Y),
                DataUtility.CreateInt32((int)Position.Z),
                new[] {(byte)Action},
                DataUtility.CreateInt16((short)nbt.Length),
                nbt));
            }
        }
}