using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.Controllers
{
    static class Controller
    {
        [DllImport("user32.dll")]
        private static extern short GetKeyState(Keys key);

        private static bool wasShot;

        public static void UpdateKeyboardHandler()
        {
            while (!GameModel.IsEnd)
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
                    lock (GameModel.LockerBullets)
                    {
                        wasShot = true;

                        var newSpawn = RotatePoint(GameModel.Player.Gun.SpawnBullets, GameModel.Player.Gun.Angle);

                        GameModel.NewBullets.Enqueue(new Bullet(GameModel.Player,
                                GameModel.Player.Gun.X + newSpawn.X, GameModel.Player.Gun.Y + newSpawn.Y,
                                GameSettings.PlayerBulletSpeed, GameModel.Player.Gun.Angle, GameSettings.PlayerDamage));
                    }
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

        private static Point RotatePoint(Point point, float angleInRadian)
        {
            return new Point(
                (int)(point.X * Math.Cos(angleInRadian) - point.Y * Math.Sin(angleInRadian)),
                (int)(point.Y * Math.Cos(angleInRadian) + point.X * Math.Sin(angleInRadian))
                );
        }
    }
}
