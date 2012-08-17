using System;
using System.Collections.Generic;
using System.Linq;
using Craft.Net.Server.Packets;
using Craft.Net.Data;

namespace Craft.Net.Server
{
    public static class PacketReader
    {
        #region Packet type array

        private static readonly Type[] PacketTypes =
            {
                typeof (KeepAlivePacket), // 0x0
                typeof (LoginPacket), // 0x1
                typeof (HandshakePacket), // 0x2
                typeof (ChatMessagePacket), // 0x3
                null, // 0x4
                null, // 0x5
                null, // 0x6
                null, // 0x7
                null, // 0x8
                null, // 0x9
                typeof (PlayerPacket), // 0xa
                typeof (PlayerPositionPacket), // 0xb
                typeof (PlayerLookPacket), // 0xc
                typeof (PlayerPositionAndLookPacket), // 0xd
                typeof (PlayerDiggingPacket), // 0xe
                typeof (BlockPlacementPacket), // 0xf
                typeof (HeldItemChangePacket), // 0x10
                null, // 0x11
                typeof (AnimationPacket), // 0x12
                typeof (EntityActionPacket), // 0x13
                null, // 0x14
                null, // 0x15
                null, // 0x16
                null, // 0x17
                null, // 0x18
                null, // 0x19
                null, // 0x1a
                null, // 0x1b
                null, // 0x1c
                null, // 0x1d
                null, // 0x1e
                null, // 0x1f
                null, // 0x20
                null, // 0x21
                null, // 0x22
                null, // 0x23
                null, // 0x24
                null, // 0x25
                null, // 0x26
                null, // 0x27
                null, // 0x28
                null, // 0x29
                null, // 0x2a
                null, // 0x2b
                null, // 0x2c
                null, // 0x2d
                null, // 0x2e
                null, // 0x2f
                null, // 0x30
                null, // 0x31
                null, // 0x32
                typeof (ChunkDataPacket), // 0x33
                null, // 0x34
                null, // 0x35
                null, // 0x36
                null, // 0x37
                null, // 0x38
                null, // 0x39
                null, // 0x3a
                null, // 0x3b
                null, // 0x3c
                null, // 0x3d
                null, // 0x3e
                null, // 0x3f
                null, // 0x40
                null, // 0x41
                null, // 0x42
                null, // 0x43
                null, // 0x44
                null, // 0x45
                null, // 0x46
                null, // 0x47
                null, // 0x48
                null, // 0x49
                null, // 0x4a
                null, // 0x4b
                null, // 0x4c
                null, // 0x4d
                null, // 0x4e
                null, // 0x4f
                null, // 0x50
                null, // 0x51
                null, // 0x52
                null, // 0x53
                null, // 0x54
                null, // 0x55
                null, // 0x56
                null, // 0x57
                null, // 0x58
                null, // 0x59
                null, // 0x5a
                null, // 0x5b
                null, // 0x5c
                null, // 0x5d
                null, // 0x5e
                null, // 0x5f
                null, // 0x60
                null, // 0x61
                null, // 0x62
                null, // 0x63
                null, // 0x64
                typeof (CloseWindowPacket), // 0x65
                null, // 0x66
                null, // 0x67
                null, // 0x68
                null, // 0x69
                null, // 0x6a
                typeof (CreativeInventoryActionPacket), // 0x6b
                null, // 0x6c
                null, // 0x6d
                null, // 0x6e
                null, // 0x6f
                null, // 0x70
                null, // 0x71
                null, // 0x72
                null, // 0x73
                null, // 0x74
                null, // 0x75
                null, // 0x76
                null, // 0x77
                null, // 0x78
                null, // 0x79
                null, // 0x7a
                null, // 0x7b
                null, // 0x7c
                null, // 0x7d
                null, // 0x7e
                null, // 0x7f
                null, // 0x80
                null, // 0x81
                null, // 0x82
                null, // 0x83
                null, // 0x84
                null, // 0x85
                null, // 0x86
                null, // 0x87
                null, // 0x88
                null, // 0x89
                null, // 0x8a
                null, // 0x8b
                null, // 0x8c
                null, // 0x8d
                null, // 0x8e
                null, // 0x8f
                null, // 0x90
                null, // 0x91
                null, // 0x92
                null, // 0x93
                null, // 0x94
                null, // 0x95
                null, // 0x96
                null, // 0x97
                null, // 0x98
                null, // 0x99
                null, // 0x9a
                null, // 0x9b
                null, // 0x9c
                null, // 0x9d
                null, // 0x9e
                null, // 0x9f
                null, // 0xa0
                null, // 0xa1
                null, // 0xa2
                null, // 0xa3
                null, // 0xa4
                null, // 0xa5
                null, // 0xa6
                null, // 0xa7
                null, // 0xa8
                null, // 0xa9
                null, // 0xaa
                null, // 0xab
                null, // 0xac
                null, // 0xad
                null, // 0xae
                null, // 0xaf
                null, // 0xb0
                null, // 0xb1
                null, // 0xb2
                null, // 0xb3
                null, // 0xb4
                null, // 0xb5
                null, // 0xb6
                null, // 0xb7
                null, // 0xb8
                null, // 0xb9
                null, // 0xba
                null, // 0xbb
                null, // 0xbc
                null, // 0xbd
                null, // 0xbe
                null, // 0xbf
                null, // 0xc0
                null, // 0xc1
                null, // 0xc2
                null, // 0xc3
                null, // 0xc4
                null, // 0xc5
                null, // 0xc6
                null, // 0xc7
                null, // 0xc8
                null, // 0xc9
                typeof (PlayerAbilitiesPacket), // 0xca
                null, // 0xcb
                typeof (LocaleAndViewDistancePacket), // 0xcc
                typeof (ClientStatusPacket), // 0xcd
                null, // 0xce
                null, // 0xcf
                null, // 0xd0
                null, // 0xd1
                null, // 0xd2
                null, // 0xd3
                null, // 0xd4
                null, // 0xd5
                null, // 0xd6
                null, // 0xd7
                null, // 0xd8
                null, // 0xd9
                null, // 0xda
                null, // 0xdb
                null, // 0xdc
                null, // 0xdd
                null, // 0xde
                null, // 0xdf
                null, // 0xe0
                null, // 0xe1
                null, // 0xe2
                null, // 0xe3
                null, // 0xe4
                null, // 0xe5
                null, // 0xe6
                null, // 0xe7
                null, // 0xe8
                null, // 0xe9
                null, // 0xea
                null, // 0xeb
                null, // 0xec
                null, // 0xed
                null, // 0xee
                null, // 0xef
                null, // 0xf0
                null, // 0xf1
                null, // 0xf2
                null, // 0xf3
                null, // 0xf4
                null, // 0xf5
                null, // 0xf6
                null, // 0xf7
                null, // 0xf8
                null, // 0xf9
                typeof (PluginMessagePacket), // 0xfa
                null, // 0xfb
                typeof (EncryptionKeyResponsePacket), // 0xfc
                typeof (EncryptionKeyRequestPacket), // 0xfd
                typeof (ServerListPingPacket), // 0xfe
                typeof (DisconnectPacket) // 0xff
            };

        #endregion

        /// <summary>
        /// Attempts to parse all packets in the given client and update
        /// its buffer.
        /// </summary>
        /// <returns>
        /// The read packets.
        /// </returns>
        public static IEnumerable<Packet> TryReadPackets(ref MinecraftClient client, int length)
        {
            var results = new List<Packet>();
            // Get a buffer to parse that is the length of the recieved data
            byte[] buffer = client.RecieveBuffer.Take(length).ToArray();
            // Decrypt the buffer if needed
            if (client.EncryptionEnabled)
                buffer = client.Decrypter.ProcessBytes(buffer);

            while (buffer.Length > 0)
            {
                Type packetType = PacketTypes[buffer[0]]; // Get the correct type to parse this packet
                if (packetType == null)
                {
                    throw new InvalidOperationException("Invalid packet ID 0x" +
                                                        buffer[0].ToString("x").ToUpper());
                }
                var workingPacket = (Packet)Activator.CreateInstance(packetType);
                workingPacket.PacketContext = PacketContext.ClientToServer;
                // Attempt to read the packet
                int workingLength = workingPacket.TryReadPacket(buffer, length);
                if (workingLength == -1) // Incomplete packet
                {
                    // Copy the incomplete packet into the recieve buffer and recieve more data
                    // TODO: Test if this can be avoided
                    Array.Copy(buffer, client.RecieveBuffer, buffer.Length);
                    client.RecieveBufferIndex = buffer.Length;
                    client.Socket.ReceiveTimeout = 500;
                    return results;
                }
                // Log the packet
                client.Server.Log("[CLIENT->SERVER] " + client.Socket.RemoteEndPoint, LogImportance.Low);
                client.Server.Log("Raw: " + DataUtility.DumpArray(buffer.Take(workingLength).ToArray()), LogImportance.Low);
                client.Server.Log(workingPacket.ToString(), LogImportance.Low);

                client.Socket.ReceiveTimeout = 30000;
                // Add this packet to the results
                results.Add(workingPacket);
                // Shift the buffer over and remove the packet just parsed
                buffer = buffer.Skip(workingLength).ToArray();
            }

            return results;
        }
    }
}