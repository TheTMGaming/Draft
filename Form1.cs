using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Top_Down_shooter
{
    enum AnimationTypes
    {
        IdleRight,
        IdleLeft,
        RunRight,
        RunLeft
    }

    public class Form1 : Form
    {
        private GameModel gameModel;
        private int currentFrameAnimation;

        public Form1()
        {
            DoubleBuffered = true;

            gameModel = new GameModel();

            var timerGameLoop = new Timer();
            timerGameLoop.Interval = 30;
            timerGameLoop.Tick += new EventHandler(UpdateGameLoop);
            timerGameLoop.Start();

            var timerChangeAnimationFrame = new Timer();
            timerChangeAnimationFrame.Interval = 300;
            timerChangeAnimationFrame.Tick += new EventHandler((sender, args) =>
            {
                currentFrameAnimation = ++currentFrameAnimation % 2;
            });
            timerChangeAnimationFrame.Start();
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            var gun = gameModel.gun;

            
            var angle = Math.Atan2(-gun.Y + MousePosition.Y, -gun.X + MousePosition.X) * 180 / Math.PI;
            g.TranslateTransform(gun.X, gun.Y);
            g.RotateTransform((float)angle);
            g.TranslateTransform(-gun.X, -gun.Y);
            g.DrawImage(
                gun.AtlasAnimations,
                gun.X - gun.Scale.Width / 2, gun.Y - gun.Scale.Width / 2,
                new Rectangle(new Point(0, 0), gun.Scale),
                GraphicsUnit.Pixel);
           
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            
        }

        public void UpdateGameLoop(object sender, EventArgs args)
        {
            
            Invalidate();
        }

        private AnimationTypes GetAnimationType(DirectionX directionX, DirectionY directionY, Sight sight)
        {
            if (directionX == DirectionX.Idle && directionY == DirectionY.Idle)
                return sight == Sight.Left ? AnimationTypes.IdleLeft : AnimationTypes.IdleRight;

            return sight == Sight.Left ? AnimationTypes.RunLeft : AnimationTypes.RunRight;
        }
    }
}
