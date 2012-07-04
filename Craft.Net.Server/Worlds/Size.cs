using System;

namespace Craft.Net.Server.Worlds
{
    public struct Size
    {
        public double Width, Height, Depth;

        public Size(double Width, double Height, double Depth)
        {
            this.Width = Width;
            this.Height = Height;
            this.Depth = Depth;
        }
    }
}

