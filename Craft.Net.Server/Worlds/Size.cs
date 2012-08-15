namespace Craft.Net.Server.Worlds
{
    public struct Size
    {
        public double Depth;
        public double Height;
        public double Width;

        public Size(double Width, double Height, double Depth)
        {
            this.Width = Width;
            this.Height = Height;
            this.Depth = Depth;
        }
    }
}