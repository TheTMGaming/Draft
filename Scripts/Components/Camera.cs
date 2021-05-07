using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.Controllers
{
    class Camera
    {
        public int X { get; set; }
        public int Y { get; set; }

        public void Move(Player player)
        {
            if (player.X > GameSettings.ScreenWidth / 2 && player.X < GameSettings.MapWidth - GameSettings.ScreenWidth / 2)
            {
                X = player.X - GameSettings.ScreenWidth / 2;

            }

            if (player.Y > GameSettings.ScreenHeight / 2 && player.Y < GameSettings.MapHeight - GameSettings.ScreenHeight / 2)
            {
                Y = player.Y - GameSettings.ScreenHeight / 2;
            }
        }
    }
}
