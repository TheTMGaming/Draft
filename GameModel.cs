using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Top_Down_shooter
{
    class GameModel
    {
        public readonly Character Player;
        public readonly LinkedList<Bullet> MovedBullets = new LinkedList<Bullet>();

        public GameModel()
        {
            Player = new Character(100, 100, 5);        
        }

        public void Shoot()
        {
            var newX = Player.Gun.X + (float)(Gun.SpawnBulletX * Math.Cos(Player.Gun.Angle) - Gun.SpawnBulletY * Math.Sin(Player.Gun.Angle));
            var newY = Player.Gun.Y + (float)(Gun.SpawnBulletY * Math.Cos(Player.Gun.Angle) + Gun.SpawnBulletX * Math.Sin(Player.Gun.Angle));
            MovedBullets.AddLast(new Bullet(newX, newY, 20, Player.Gun.Angle));
        }
    }
}
