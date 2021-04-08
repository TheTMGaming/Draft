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
            BulletsOnCanvas.Add(new Bullet(gun.X, gun.Y, (float)(gun.Angle * Math.PI / 180)));
        }
    }
}
