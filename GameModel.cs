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

        public void CreateBullet()
        {
            var x = 20;
            var y = -12;

            var newX = Gun.X + (float)(x * Math.Cos(Gun.Angle) - y * Math.Sin(Gun.Angle));
            var newY = Gun.Y + (float)(y * Math.Cos(Gun.Angle) + x * Math.Sin(Gun.Angle));
            MovedBullets.AddLast(new Bullet(newX, newY, 20, Gun.Angle));
        }
    }
}
