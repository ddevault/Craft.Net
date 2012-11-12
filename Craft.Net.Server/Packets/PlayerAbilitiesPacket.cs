using Craft.Net.Data;
namespace Craft.Net.Server.Packets
{
    public class PlayerAbilitiesPacket : Packet
    {
        public PlayerAbilities Abilities;

        public PlayerAbilitiesPacket()
        {
        }

        public PlayerAbilitiesPacket(PlayerAbilities abilities)
        {
            Abilities = abilities;
        }

        public override byte PacketId
        {
            get { return 0xCA; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            byte flags;
            byte walkingSpeed, flyingSpeed;
            int offset = 1;
            if (!DataUtility.TryReadByte(buffer, ref offset, out flags))
                return -1;
            if (!DataUtility.TryReadByte(buffer, ref offset, out walkingSpeed))
                return -1;
            if (!DataUtility.TryReadByte(buffer, ref offset, out flyingSpeed))
                return -1;
            Abilities = new PlayerAbilities(null);
            Abilities.WalkingSpeed = walkingSpeed;
            Abilities.FlyingSpeed = flyingSpeed;
            Abilities.Invulnerable = (flags & 1) == 1;
            Abilities.IsFlying = (flags & 2) == 2;
            Abilities.MayFly = (flags & 4) == 4;
            Abilities.InstantMine = (flags & 8) == 8;
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, MinecraftClient client)
        {
            if (client.Entity.GameMode == GameMode.Creative)
                client.Entity.Abilities.IsFlying = Abilities.IsFlying;
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            client.SendData(CreateBuffer(
                new[] { (byte)(
                        (Abilities.Invulnerable ? 1 : 0) | 
                        (Abilities.IsFlying ? 2 : 0) | 
                        (Abilities.MayFly? 4 : 0) | 
                        (Abilities.InstantMine ? 8 : 0)
                        ), Abilities.WalkingSpeed, Abilities.FlyingSpeed }
                ));
        }
    }
}