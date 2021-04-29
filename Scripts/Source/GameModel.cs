using System;
using System.Collections.Generic;
using System.Drawing;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.UI;
using Top_Down_shooter.Scripts.Controllers;


namespace Top_Down_shooter
{
    class GameModel
    {
        public readonly Player Player;
        public readonly HealthBar HealthBar;
        public readonly LinkedList<Bullet> Bullets;
        

        public GameModel()
        {
            Player = new Player(100, 100, 5);
            HealthBar = new HealthBar(100);

            Bullets = new LinkedList<Bullet>();
          
        }

        public void Shoot()
        {
            var newSpawn = RotatePoint(Player.Gun.SpawnBullets, Player.Gun.Angle);

            var bullet = new Bullet(
                Player.Gun.X + newSpawn.X, Player.Gun.Y + newSpawn.Y,
                20, Player.Gun.Angle);

            Bullets.AddLast(bullet);
            Physics.AddToTrackingCollisions(bullet);

        }

        private Point RotatePoint(Point point, float angleInRadian)
        {
            return new Point(
                (int)(point.X * Math.Cos(angleInRadian) - point.Y * Math.Sin(angleInRadian)),
                (int)(point.Y * Math.Cos(angleInRadian) + point.X * Math.Sin(angleInRadian))
                );
        }
    }
}
