using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Blocks;

namespace Craft.Net.Data.Entities
{
    public class PaintingEntity : Entity
    {
        /// <summary>
        /// Creates an entity that will fit into the available space.
        /// </summary>
        public static PaintingEntity CreateEntity(World world, Vector3 clickedBlock, Vector3 clickedSide)
        {
            // Get horizontal for the space search
            // We need to move up and to the right to search. "To the right" changes
            // based on orientation.
            PaintingDirection direction;
            Vector3 horizontal;
            if (clickedSide == Vector3.East)
            {
                horizontal = Vector3.North;
                direction = PaintingDirection.East;
            }
            else if (clickedSide == Vector3.West)
            {
                horizontal = Vector3.South;
                direction = PaintingDirection.West;
            }
            else if (clickedSide == Vector3.North)
            {
                horizontal = Vector3.West;
                direction = PaintingDirection.North;
            }
            else if (clickedSide == Vector3.South)
            {
                horizontal = Vector3.East;
                direction = PaintingDirection.South;
            }
            else
                return null;
            Vector3 position = clickedBlock + clickedSide;
            // Get the size of the largest painting
            int largestWidth = -1, largestHeight = -1;
            foreach (var painting in Painting.Paintings)
            {
                if (painting.Width > largestWidth)
                    largestWidth = painting.Width;
                if (painting.Height > largestHeight)
                    largestHeight = painting.Height;
            }
            // Get the available space
            int maxWidth = int.MaxValue, maxHeight = 0;
            for (int y = 0; y < largestHeight; y++)
            {
                int width = 0;
                bool any = false;
                for (int x = 0; x < largestWidth; x++)
                {
                    // Check for air
                    var block = world.GetBlock(position + (horizontal * x) + new Vector3(0, y, 0));
                    if (!(block is AirBlock))
                        break;
                    // Check for support
                    var support = world.GetBlock(position + (horizontal * x) + new Vector3(0, y, 0) - clickedSide);
                    if (support is AirBlock)
                        break;
                    any = true;
                    width++;
                }
                if (any)
                {
                    maxHeight++;
                    if (width < maxWidth)
                        maxWidth = width;
                }
            }
            // Get all possible paintings that fit in that space
            var paintings = Painting.Paintings.Where(p => p.Width <= maxWidth && p.Height <= maxHeight).ToArray();
            if (paintings.Length == 0) return null;
            // Create entity
            var selectedPainting = paintings[MathHelper.Random.Next(paintings.Length)];
            // Place against block, upper left
            position -= clickedSide;
            position += new Vector3(0, selectedPainting.Height - 1, 0);
            return new PaintingEntity(selectedPainting, direction, position);
        }

        public PaintingEntity(Painting painting, PaintingDirection direction, Vector3 position)
        {
            Position = position;
            Painting = painting;
            Direction = direction;
        }

        public Painting Painting { get; set; }
        public PaintingDirection Direction { get; set; }

        public override Size Size
        {
            get 
            {
                switch (Direction)
                {
                    case PaintingDirection.North:
                    case PaintingDirection.South:
                        return new Size(Painting.Width, Painting.Height, 0.1);
                    case PaintingDirection.West:
                    case PaintingDirection.East:
                    default:
                        return new Size(0.1, Painting.Height, Painting.Width);
                }
            }
        }

        public Vector3 Center
        {
            get
            {
                switch (Direction)
                {
                    case PaintingDirection.North:
                    case PaintingDirection.South:
                        return Position - new Vector3((int)Math.Max(0, Painting.Width / 2 - 1), (int)(Painting.Height / 2), 0);
                    case PaintingDirection.West:
                    case PaintingDirection.East:
                    default:
                        return Position - new Vector3(0, (int)(Painting.Height / 2), (int)Math.Max(0, Painting.Width / 2 - 1));
                }
            }
        }

        public override bool IncludeMetadataOnClient
        {
            get { return true; }
        }

        public override void PhysicsUpdate(World world)
        {
            // Paintings don't recieve physics updates
        }

        public override void UsedByEntity(World world, bool leftClick, LivingEntity usedBy)
        {
            if (leftClick)
                world.OnDestroyEntity(this);
        }

        public enum PaintingDirection
        {
            North = 2,
            West = 1,
            South = 0,
            East = 3
        }
    }

    public class Painting
    {
        static Painting()
        {
            Paintings = new List<Painting>();
            Paintings.Add(new Painting("Kebab", 1, 1));
            Paintings.Add(new Painting("Aztec", 1, 1));
            Paintings.Add(new Painting("Alban", 1, 1));
            Paintings.Add(new Painting("Aztec2", 1, 1));
            Paintings.Add(new Painting("Bomb", 1, 1));
            Paintings.Add(new Painting("Plant", 1, 1));
            Paintings.Add(new Painting("Wasteland", 1, 1));
            Paintings.Add(new Painting("Wanderer", 1, 2));
            Paintings.Add(new Painting("Graham", 1, 2));
            Paintings.Add(new Painting("Pool", 2, 1));
            Paintings.Add(new Painting("Courbet", 2, 1));
            Paintings.Add(new Painting("Sunset", 2, 1));
            Paintings.Add(new Painting("Sea", 2, 1));
            Paintings.Add(new Painting("Creebet", 2, 1));
            Paintings.Add(new Painting("Match", 2, 2));
            Paintings.Add(new Painting("Bust", 2, 2));
            Paintings.Add(new Painting("Stage", 2, 2));
            Paintings.Add(new Painting("Void", 2, 2));
            Paintings.Add(new Painting("SkullAndRoses", 2, 2));
            Paintings.Add(new Painting("Wither", 2, 2));
            Paintings.Add(new Painting("Fighters", 4, 2));
            Paintings.Add(new Painting("Skeleton", 4, 3));
            Paintings.Add(new Painting("DonkeyKong", 4, 3));
            Paintings.Add(new Painting("Pointer", 4, 4));
            Paintings.Add(new Painting("Pigscene", 4, 4));
            //Paintings.Add(new Painting("FlamingSkull", 4, 4));
        }

        public static List<Painting> Paintings { get; set; }

        public Painting(string name, int width, int height)
        {
            Name = name;
            Width = width;
            Height = height;
        }

        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
