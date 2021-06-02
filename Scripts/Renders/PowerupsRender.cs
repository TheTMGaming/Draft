using System.Collections.Generic;
using Top_Down_shooter.Scripts.GameObjects;
using System.Drawing;
using Top_Down_shooter.Properties;
using unvell.D2DLib;

namespace Top_Down_shooter.Scripts.Renders
{
    class PowerupsRender : IRender
    {
        private readonly HashSet<Powerup> powerups;

        public PowerupsRender(HashSet<Powerup> powerups)
        {
            this.powerups = powerups;
        }

        public int X { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int Y { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public Size Size => throw new System.NotImplementedException();

        public void Draw(D2DGraphics g)
        {
            foreach (var powerup in powerups)
            {
                var image = Resources.BigPowerup;

                if (powerup is SmallLoot)
                    image = Resources.SmallLoot;

                if (powerup is HP)
                    image = Resources.Heart;

                g.DrawBitmap(g.Device.CreateBitmapFromGDIBitmap(image),
                    new D2DRect(powerup.X - image.Width / 2, powerup.Y - image.Height / 2, image.Width, image.Height));
                      
            }
        }
    }
}
