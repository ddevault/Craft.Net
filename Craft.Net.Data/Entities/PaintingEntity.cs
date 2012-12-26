using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Entities
{
    public class PaintingEntity : Entity
    {
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

        public override void PhysicsUpdate(World world)
        {
            // Paintings don't recieve physics updates
        }

        public enum PaintingDirection
        {
            North = 0,
            West = 1,
            South = 2,
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
            Paintings.Add(new Painting("Flaming Skull", 4, 4));
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
