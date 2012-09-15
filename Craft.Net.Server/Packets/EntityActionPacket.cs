using System;
using Craft.Net.Data;

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
        public EntityAction Action;
        public int EntityId;

        public override byte PacketId
        {
            get { return 0x13; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            int offset = 1;
            byte action = 0;
            if (!DataUtility.TryReadInt32(buffer, ref offset, out EntityId))
                return -1;
            if (!DataUtility.TryReadByte(buffer, ref offset, out action))
                return -1;
            Action = (EntityAction)action;
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, MinecraftClient client)
        {
            switch (Action)
            {
                case EntityAction.Crouch:
                    client.IsCrouching = true;
                    break;
                case EntityAction.Uncrouch:
                    client.IsCrouching = false;
                    break;
                case EntityAction.StartSprinting:
                    client.IsSprinting = true;
                    break;
                case EntityAction.StopSprinting:
                    client.IsSprinting = false;
                    break;
                case EntityAction.LeaveBed:
                    client.Entity.LeaveBed();
                    break;
            }
            if (Action != EntityAction.LeaveBed) // NOTE: Does this matter?
            {
                // TODO ?
            }
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            throw new NotImplementedException();
        }
    }
}