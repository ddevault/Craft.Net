using System;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
   public enum GameState
   {
      InvalidBed = 0, // wat
      BeginRaining = 1,
      EndRaining = 2,
      ChangeGameMode = 3,
      EnterCredits = 4
   }

   public class ChangeGameStatePacket : Packet
   {
      public GameState GameState;
      public GameMode GameMode;

      public ChangeGameStatePacket()
      {
      }

      public ChangeGameStatePacket(GameState gameState) : this(gameState, GameMode.Survival)
      {
      }

      public ChangeGameStatePacket(GameState gameState, GameMode gameMode)
      {
         GameState = gameState;
         GameMode = gameMode;
      }

      public override byte PacketId
      {
         get { return 0x46; }
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
         byte[] payload = new byte[]
         {
            PacketId,
            (byte)GameState,
            (byte)GameMode
         };
         client.SendData(payload);
      }
   }
}