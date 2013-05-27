using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Entities;

namespace Craft.Net.Data.Blocks
{
    public class SaplingBlock : Block, IGrowableBlock
    {
        public enum SaplingType
        {
            Oak = 0,
            Spruce = 1,
            Birch = 2,
            Jungle = 3
        }

        public SaplingBlock() { }

        public SaplingBlock(SaplingType type)
        {
            Type = type;
        }

        public SaplingType Type
        {
            get { return (SaplingType)Metadata; }
            set { Metadata = (byte)value; }
        }

        public override short Id
        {
            get { return 6; }
        }

        public override BoundingBox? BoundingBox
        {
            get { return null; }
        }

        public bool Grow(World world, Vector3 position, bool instant)
        {
            // TODO
            return false;
        }
    }
}
