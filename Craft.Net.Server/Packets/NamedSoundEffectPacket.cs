using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public class NamedSoundEffectPacket : Packet
    {
        public string SoundName;
        public Vector3 Position;
        public float Volume;
        public byte Pitch;

        public NamedSoundEffectPacket()
        {
        }

        public NamedSoundEffectPacket(string soundName, Vector3 position)
        {
            SoundName = soundName;
            Position = position;
            Volume = 1;
            Pitch = 63;
        }

        public NamedSoundEffectPacket(string soundName, Vector3 position, byte pitch)
        {
            SoundName = soundName;
            Position = position;
            Volume = 1;
            Pitch = pitch;
        }

        public override byte PacketId
        {
            get { return 0x3E; }
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
                DataUtility.CreateString(SoundName),
                DataUtility.CreateInt32((int)(Position.X * 8)),
                DataUtility.CreateInt32((int)(Position.Y * 8)),
                DataUtility.CreateInt32((int)(Position.Z * 8)),
                DataUtility.CreateFloat(Volume),
                new[] { Pitch }));
        }
    }
}
