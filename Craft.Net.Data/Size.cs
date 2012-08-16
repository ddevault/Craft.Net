namespace Craft.Net.Data
{
    public struct Size
    {
        public double Depth;
        public double Height;
        public double Width;

        public Size(double width, double height, double depth)
        {
            this.Width = width;
            this.Height = height;
            this.Depth = depth;
        }
    }
}