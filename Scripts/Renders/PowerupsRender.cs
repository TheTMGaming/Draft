using System.Collections.Generic;
using Top_Down_shooter.Scripts.GameObjects;
using System.Drawing;
using Top_Down_shooter.Properties;
using Top_Down_shooter.Scripts.Source;
using unvell.D2DLib;

namespace Top_Down_shooter.Scripts.Renders
{
    class PowerupsRender : IRender
    {
        private readonly HashSet<Powerup> powerups;

        private readonly Bitmap bigLoot = Resources.BigPowerup;
        private readonly Bitmap smallLoot = Resources.SmallLoot;
        private readonly Bitmap hp = Resources.Heart;

        public PowerupsRender(HashSet<Powerup> powerups)
        {
            this.powerups = powerups;
        }

        public int X { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int Y { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public Size Size => throw new System.NotImplementedException();

        public void Draw(D2DGraphicsDevice device)
        {
            foreach (var powerup in powerups)
            {
                var image = bigLoot;

                if (powerup is SmallLoot)
                    image = smallLoot;

                if (powerup is HP)
                    image = hp;

                device.Graphics.DrawBitmap(device.CreateBitmap(image),
                    new D2DRect(powerup.X - image.Width / 2, powerup.Y - image.Height / 2, image.Width, image.Height));
            }
        }
    }
}
