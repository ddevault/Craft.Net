using System.Linq;

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

        public override byte PacketID
        {
            get { return 0x12; }
        }

        public override int TryReadPacket(byte[] Buffer, int Length)
        {
            int offset = 1;
            byte animation = 0;
            if (!TryReadInt(Buffer, ref offset, out EntityId))
                return -1;
            if (!TryReadByte(Buffer, ref offset, out animation))
                return -1;
            Animation = (Animation) animation;
            return offset;
        }

        public override void HandlePacket(MinecraftServer Server, ref MinecraftClient Client)
        {
            EntityId = Client.Entity.Id;
            for (int i = 0;
                 i <
                 Server.GetClientsInWorld(Server.GetClientWorld(Client)).Count();
                 i++) // TODO: Better way
            {
                if (Server.Clients[i] != Client)
                    Server.Clients[i].SendPacket(this);
            }
            Server.ProcessSendQueue();
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            return;
            byte[] data = new byte[] {PacketID}
                .Concat(CreateInt(EntityId))
                .Concat(new byte[] {(byte) Animation}).ToArray();
            Client.SendData(data);
        }
    }
}