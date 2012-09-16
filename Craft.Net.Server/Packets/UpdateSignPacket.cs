using Craft.Net.Data;
using Craft.Net.Data.Blocks;

namespace Craft.Net.Server.Packets
{
    public class UpdateSignPacket : Packet
    {
        public Vector3 Position;
        public string Text1;
        public string Text2;
        public string Text3;
        public string Text4;

        public UpdateSignPacket()
        {
        }

        public UpdateSignPacket(Vector3 position, SignTileEntity data)
        {
            Position = position;
            Text1 = data.Text1;
            Text2 = data.Text2;
            Text3 = data.Text3;
            Text4 = data.Text4;
        }

        public override byte PacketId
        {
            get { return 0x82; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            int offset = 1;
            int x, z;
            short y;
            if (!DataUtility.TryReadInt32(buffer, ref offset, out x))
                return -1;
            if (!DataUtility.TryReadInt16(buffer, ref offset, out y))
                return -1;
            if (!DataUtility.TryReadInt32(buffer, ref offset, out z))
                return -1;
            if (!DataUtility.TryReadString(buffer, ref offset, out Text1))
                return -1;
            if (!DataUtility.TryReadString(buffer, ref offset, out Text2))
                return -1;
            if (!DataUtility.TryReadString(buffer, ref offset, out Text3))
                return -1;
            if (!DataUtility.TryReadString(buffer, ref offset, out Text4))
                return -1;
            Position = new Vector3(x, y, z);
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, MinecraftClient client)
        {
            if (Position.DistanceTo(client.Entity.Position) > client.Reach)
                return;
            var block = client.World.GetBlock(Position);
            if (!(block is SignBlock))
                return;
            var sign = (SignBlock)block;
            sign.SignData.Text1 = Text1;
            sign.SignData.Text2 = Text2;
            sign.SignData.Text3 = Text3;
            sign.SignData.Text4 = Text4;
            client.World.SetBlock(Position, sign);
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            client.SendData(CreateBuffer(
                DataUtility.CreateInt32((int)Position.X),
                DataUtility.CreateInt16((short)Position.Y),
                DataUtility.CreateInt32((int)Position.Z),
                DataUtility.CreateString(Text1),
                DataUtility.CreateString(Text2),
                DataUtility.CreateString(Text3),
                DataUtility.CreateString(Text4)));
        }
    }
}
