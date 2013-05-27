using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class AirBlock : Block
    {
        public override short Id
        {
            get { return 0; }
        }

        public override Size Size
        {
            get { return new Size(0, 0, 0); }
        }

        public override double Hardness
        {
            get { return -1; }
        }

        public override bool CanHarvest(ToolItem tool)
        {
            return false;
        }

        public override Transparency Transparency
        {
            get { return Transparency.Transparent; } // TODO: More transparency options
        }

        public override byte LightReduction
        {
            get { return 0; }
        }

        public override BoundingBox? BoundingBox
        {
            get { return null; }
        }
    }
}
