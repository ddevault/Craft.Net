using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data;
using Craft.Net.Data.Entities;
using Craft.Net.Data.Items;
using Craft.Net.Server;

namespace TestServer
{
    public class CustomLeatherItem : LeatherItem
    {
        public static MinecraftServer Server { get; set; }

        public override void OnItemUsedOnBlock(World world, Vector3 clickedBlock, Vector3 clickedSide,
            Vector3 cursorPosition, Entity usedBy)
        {
            var block = world.GetBlock(clickedBlock);
            var player = usedBy as PlayerEntity;
            var client = Server.EntityManager.GetClient(player);
            client.SendChat(block.GetType().Name + ": " + block.Id + " (" + block.Metadata + ")");
        }
    }
}
