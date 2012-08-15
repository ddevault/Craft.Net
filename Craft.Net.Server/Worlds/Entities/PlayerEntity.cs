namespace Craft.Net.Server.Worlds.Entities
{
    public class PlayerEntity : Entity
    {
        public MinecraftClient Client;

        public PlayerEntity()
        {
        }

        public PlayerEntity(MinecraftClient client)
        {
            this.Client = client;
        }

        #region Properties

        public override Size Size
        {
            get { return new Size(0.6, 1.8, 0.6); }
        }

        public static double Width
        {
            get { return 0.6; }
        }

        public static double Height
        {
            get { return 1.8; }
        }

        public static double Depth
        {
            get { return 0.6; }
        }

        #endregion
    }
}