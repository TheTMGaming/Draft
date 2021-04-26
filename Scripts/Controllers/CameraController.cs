using System.Drawing;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Properties;

namespace Top_Down_shooter.Scripts.Controllers
{
    static class CameraController
    {
        private static Point position;

        private static readonly int screenWidth;
        private static readonly int screenHeight;
        private static readonly int mapWidth;
        private static readonly int mapHeight;

        static CameraController()
        {
            screenWidth = int.Parse(Resources.ScreenWidth);
            screenHeight = int.Parse(Resources.ScreenHeight);
            mapWidth = int.Parse(Resources.MapWidth);
            mapHeight = int.Parse(Resources.MapHeight);
        }

        public static void Move(Player player, Graphics g)
        {
            if (player.X > screenWidth / 2 && player.X < mapWidth - screenWidth / 2)
            {
                position.X = -(player.X - screenWidth / 2);

            }

            if (player.Y > screenHeight / 2 && player.Y < mapHeight - screenHeight / 2)
            {
                position.Y = -(player.Y - screenHeight / 2);
            }


            g.TranslateTransform(position.X, position.Y);
        }
    }
}
