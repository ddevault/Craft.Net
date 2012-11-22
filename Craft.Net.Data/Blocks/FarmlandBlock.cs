using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class FarmlandBlock : Block
    {
        public override ushort Id
        {
            get { return 60; }
        }

        public override double Hardness
        {
            get { return 0.6; }
        }

        public override bool GetDrop(Items.ToolItem tool, out Slot[] drop)
        {
            drop = new[] { new Slot((ushort)new DirtBlock(), 1) };
            return true;
        }

        public override void OnBlockWalkedOn(World world, Vector3 position, Entities.Entity entity)
        {
            if (entity.Velocity.Y < -0.25)
                Console.WriteLine("Entity " + entity.Id + " jumped onto farmland with velocity " + entity.Velocity);
            }
        }
}