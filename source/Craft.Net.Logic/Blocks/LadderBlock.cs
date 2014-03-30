using System;
using Craft.Net.Anvil;
using Craft.Net.Common;

namespace Craft.Net.Logic.Blocks
{
    public class LadderBlock : Block
    {
        public enum Orientation
        {
            FacingNorth = 0x2,
            FacingSouth = 0x3,
            FacingWest = 0x4,
            FacingEast = 0x5
        }
        
        public static readonly short Id = 65;
        public override short BlockId { get { return Id; } }
        
        public LadderBlock() : base("minecraft:ladder")
        {
            base.SetPlacementSoundEffect(SoundEffect.DigWood);
            base.SetBoundingBoxHandler(BoundingBox);
            base.SetItemUsedOnBlockHandler(ItemUsedOnBlock);
        }
        
        private BoundingBox? BoundingBox(BlockInfo info)
        {
            switch (info.Metadata)
            {
                case (byte)Orientation.FacingNorth:
                    return new BoundingBox(new Vector3(0,0,1-0.125), Vector3.One);
                case (byte)Orientation.FacingSouth:
                    return new BoundingBox(Vector3.Zero, new Vector3(1,1,0.125));
                case (byte)Orientation.FacingWest:
                    return new BoundingBox(new Vector3(1-0.125,0,0), Vector3.One);
                case (byte)Orientation.FacingEast:
                    return new BoundingBox(Vector3.Zero, new Vector3(0.125,1,1));
                default:
                    return null;
            }
        }
        
        private void ItemUsedOnBlock(World world, Coordinates3D coordinates, BlockFace face, Coordinates3D cursor, ItemInfo item)
        {
            var info = world.GetBlockInfo(coordinates);
            if (Block.GetIsSolidOnFace(info, face) == false)
                return;
            
            coordinates += MathHelper.BlockFaceToCoordinates(face);

            switch (face)
            {
                case BlockFace.NegativeZ:
                    world.SetBlockId(coordinates, item.ItemId);
                    world.SetMetadata(coordinates, (byte)Orientation.FacingNorth);
                    break;
                case BlockFace.PositiveZ:
                    world.SetBlockId(coordinates, item.ItemId);
                    world.SetMetadata(coordinates, (byte)Orientation.FacingSouth);
                    break;
                case BlockFace.NegativeX:
                    world.SetBlockId(coordinates, item.ItemId);
                    world.SetMetadata(coordinates, (byte)Orientation.FacingWest);
                    break;
                case BlockFace.PositiveX:
                    world.SetBlockId(coordinates, item.ItemId);
                    world.SetMetadata(coordinates, (byte)Orientation.FacingEast);
                    break;
                default:
                    // Ladders can't be placed lying flat.
                    break;
            }
        }
        
        // TODO - Once there is a mechanism for neighbor block updates, destroy this and drop a (1) ladder stack upon destruction of the neighbor opposite the ladder's face (the block the ladder is attached to).
    }
}
