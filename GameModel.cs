using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Top_Down_shooter
{
    class GameModel
    {
        public readonly Player Player;
        public readonly HealthBar HealthBar;
        public readonly LinkedList<Bullet> MovedBullets = new LinkedList<Bullet>();

        public GameModel()
        {
            Player = new Player(100, 100, 5, new Bitmap(@"Sprites\Player.png"), 4, 2);
            HealthBar = new HealthBar(140, 140, 100);
        }

        public void Shoot()
        {
            var newX = Player.Gun.X + (float)(Player.Gun.SpawnBulletX * Math.Cos(Player.Gun.Angle) - Player.Gun.SpawnBulletY * Math.Sin(Player.Gun.Angle));
            var newY = Player.Gun.Y + (float)(Player.Gun.SpawnBulletY * Math.Cos(Player.Gun.Angle) + Player.Gun.SpawnBulletX * Math.Sin(Player.Gun.Angle));
            MovedBullets.AddLast(new Bullet(newX, newY, 20, Player.Gun.Angle, new Bitmap(@"Sprites/Bullet.png")));
        }
    }
}
