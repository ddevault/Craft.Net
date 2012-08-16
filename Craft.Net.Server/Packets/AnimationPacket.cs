using System.Linq;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public enum Animation
    {
        NoAnimation = 0,
        SwingArm = 1,
        Damage = 2,
        LeaveBed = 3,
        EatFood = 4,
        Crouch = 104,
        Uncrouch = 105
    }

    public class AnimationPacket : Packet
    {
        public Animation Animation;
        public int EntityId;

        public override byte PacketId
        {
            get { return 0x12; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            int offset = 1;
            byte animation = 0;
            if (!DataUtility.TryReadInt32(buffer, ref offset, out EntityId))
                return -1;
            if (!DataUtility.TryReadByte(buffer, ref offset, out animation))
                return -1;
            Animation = (Animation)animation;
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, ref MinecraftClient client)
        {
            EntityId = client.Entity.Id;
            for (int i = 0;
                 i <
                 server.GetClientsInWorld(server.GetClientWorld(client)).Count();
                 i++) // TODO: Better way
            {
                if (server.Clients[i] != client)
                    server.Clients[i].SendPacket(this);
            }
            server.ProcessSendQueue();
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            byte[] data = new byte[] {PacketId}
                .Concat(DataUtility.CreateInt32(EntityId))
                .Concat(new byte[] {(byte)Animation}).ToArray();
            client.SendData(data);
        }
    }
}