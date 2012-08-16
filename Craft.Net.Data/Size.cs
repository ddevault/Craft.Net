namespace Craft.Net.Data
{
    /// <summary>
    /// Represents the size of an object in 3D space.
    /// </summary>
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