using System.Collections.Generic;
using Top_Down_shooter.Scripts.GameObjects;
using System.Drawing;
using Top_Down_shooter.Properties;

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

        public void Draw(Graphics g)
        {
            foreach (var powerup in powerups)
            {
                var image = Resources.BigPowerup;

                if (powerup is SmallLoot)
                    image = Resources.SmallLoot;

                if (powerup is HP)
                    image = Resources.Heart;

                g.DrawImage(image,
                    powerup.X - image.Width / 2, powerup.Y - image.Height / 2,
                    new Rectangle(0, 0, image.Width, image.Width),
                    GraphicsUnit.Pixel);              
            }
        }
    }
}
