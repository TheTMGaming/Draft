using System;
using System.Collections.Generic;
using System.Drawing;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.UI;
using Top_Down_shooter.Scripts.Controllers;


namespace Top_Down_shooter
{
    static class GameModel
    {
        public static readonly Player Player;
        public static readonly Map Map;
        public static readonly HealthBar HealthBar;
        public static readonly LinkedList<Bullet> Bullets;


        static GameModel()
        {
            Player = new Player(100, 100, 5);
            HealthBar = new HealthBar(100);
            Map = new Map();
            Bullets = new LinkedList<Bullet>();
          
        }

        public static Bullet Shoot()
        {
            var newSpawn = RotatePoint(Player.Gun.SpawnBullets, Player.Gun.Angle);

            return new Bullet(
                Player.Gun.X + newSpawn.X, Player.Gun.Y + newSpawn.Y,
                20, Player.Gun.Angle);

            

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
