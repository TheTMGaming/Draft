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
        public readonly Gun Gun;
        public readonly LinkedList<Bullet> MovedBullets = new LinkedList<Bullet>();

        public GameModel()
        {
            Gun = new Gun(140, 140);           
        }

        public void Shoot()
        {
            var newX = Gun.X + (float)(Gun.SpawnBulletX * Math.Cos(Gun.Angle) - Gun.SpawnBulletY * Math.Sin(Gun.Angle));
            var newY = Gun.Y + (float)(Gun.SpawnBulletY * Math.Cos(Gun.Angle) + Gun.SpawnBulletX * Math.Sin(Gun.Angle));
            MovedBullets.AddLast(new Bullet(newX, newY, 20, Gun.Angle));
        }
    }
}
