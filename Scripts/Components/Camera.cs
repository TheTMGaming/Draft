using System.Drawing;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Properties;

namespace Top_Down_shooter.Scripts.Controllers
{
    class Camera
    {
        public int X { get; set; }
        public int Y { get; set; }

        private static readonly int screenWidth;
        private static readonly int screenHeight;
        private static readonly int mapWidth;
        private static readonly int mapHeight;

        static Camera()
        {
            screenWidth = int.Parse(Resources.ScreenWidth);
            screenHeight = int.Parse(Resources.ScreenHeight);
            mapWidth = int.Parse(Resources.MapWidth);
            mapHeight = int.Parse(Resources.MapHeight);
        }

        public void Move(Player player)
        {
            if (player.X > screenWidth / 2 && player.X < mapWidth - screenWidth / 2)
            {
                X = player.X - screenWidth / 2;

            }

            if (player.Y > screenHeight / 2 && player.Y < mapHeight - screenHeight / 2)
            {
                Y = player.Y - screenHeight / 2;
            }
        }
    }
}
