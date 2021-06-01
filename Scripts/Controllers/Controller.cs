using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Top_Down_shooter.Scripts.GameObjects;

namespace Top_Down_shooter.Scripts.Controllers
{
    static class Controller
    {
        public static readonly Queue<Bullet> SpawnedBullets = new Queue<Bullet>();

        [DllImport("user32.dll")]
        private static extern short GetKeyState(Keys key);

        private static bool wasShot;

        public static void UpdateKeyboardHandler()
        {
            while (true)
            {
                if (IsKeyPressed(Keys.Up) || IsKeyPressed(Keys.W))
                    GameModel.Player.ChangeDirection(DirectionY.Up);
                else if (IsKeyPressed(Keys.Down) || IsKeyPressed(Keys.S))
                    GameModel.Player.ChangeDirection(DirectionY.Down);
                else
                    GameModel.Player.ChangeDirection(DirectionY.Idle);

                if (IsKeyPressed(Keys.Right) || IsKeyPressed(Keys.D))
                    GameModel.Player.ChangeDirection(DirectionX.Right);
                else if (IsKeyPressed(Keys.Left) || IsKeyPressed(Keys.A))
                    GameModel.Player.ChangeDirection(DirectionX.Left);
                else
                    GameModel.Player.ChangeDirection(DirectionX.Idle);


                Thread.Sleep(30);
            }
        }

        public static void UpdateMouseHandler()
        {
            while (true)
            {
                if (!IsKeyPressed(Keys.LButton))
                    wasShot = false;
                else if (!wasShot && GameModel.Player.Gun.CountBullets > 0)
                {
                    wasShot = true;

                    SpawnedBullets.Enqueue(GameModel.Shoot());
                    Thread.Sleep(GameModel.Player.Gun.Cooldown);
                }
            }
        }

        private static bool IsKeyPressed(Keys key)
        {
            var isPressed = false;

            switch (GetKeyState(key))
            {
                case 0:
                case 1:
                    isPressed = false;
                    break;

                default:
                    isPressed = true;
                    break;
            }

            return isPressed;
        }

    }
}
