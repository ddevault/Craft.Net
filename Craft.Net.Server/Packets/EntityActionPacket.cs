using System;
using System.Linq;

namespace Craft.Net.Server.Packets
{
    public enum EntityAction
    {
        Crouch = 1,
        Uncrouch = 2,
        LeaveBed = 3,
        StartSprinting = 4,
        StopSprinting = 5
    }

    public class EntityActionPacket : Packet
    {
        public int EntityId;
        public EntityAction Action;

        public EntityActionPacket()
        {
        }

        public override byte PacketID
        {
            get
            {
                return 0x13;
            }
        }

        public override int TryReadPacket(byte[] Buffer, int Length)
        {
            int offset = 1;
            byte action = 0;
            if (!TryReadInt(Buffer, ref offset, out EntityId))
                return -1;
            if (!TryReadByte(Buffer, ref offset, out action))
                return -1;
            this.Action = (EntityAction)action;
            return offset;
        }

        public override void HandlePacket(MinecraftServer Server, ref MinecraftClient Client)
        {
            switch (Action)
            {
                case EntityAction.Crouch:
                    Client.IsCrouching = true;
                    break;
                case EntityAction.Uncrouch:
                    Client.IsCrouching = false;
                    break;
                case EntityAction.StartSprinting:
                    Client.IsSprinting = true;
                    break;
                case EntityAction.StopSprinting:
                    Client.IsSprinting = false;
                    break;
            }
            if (Action != EntityAction.LeaveBed) // NOTE: Does this matter?
            {
                this.EntityId = Client.Entity.Id;
                for (int i = 0; i < 
                     Server.GetClientsInWorld(Server.GetClientWorld(Client)).Count(); i++)
                {
                    if (Server.Clients [i] != Client)
                        Server.Clients [i].SendPacket(this);
                }
                Server.ProcessSendQueue();
            }
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            throw new InvalidOperationException();
        }
    }
}

