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
        public Gun gun;
        public readonly List<Bullet> BulletsOnCanvas = new List<Bullet>();

        public GameModel()
        {
            gun = new Gun();
            
        }

        public void CreateBullet()
        {
            var x = 20;
            var y = -12;
            var angle = (float)(gun.Angle * Math.PI / 180);

            var newX = gun.X + (float)(x * Math.Cos(angle) - y * Math.Sin(angle));
            var newY = gun.Y + (float)(y * Math.Cos(angle) + x * Math.Sin(angle));
            BulletsOnCanvas.Add(new Bullet(newX, newY, (float)(gun.Angle * Math.PI / 180)));
        }
    }
}
