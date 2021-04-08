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
        private int a;

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

            
            g.TranslateTransform(gun.X, gun.Y);
            g.RotateTransform(gun.Angle);
            g.TranslateTransform(-gun.X, -gun.Y);
            g.DrawImage(
                gun.AtlasAnimations,
                gun.X - gun.Scale.Width / 2, gun.Y - gun.Scale.Width / 2,
                new Rectangle(new Point(0, 0), gun.Scale),
                GraphicsUnit.Pixel);
            g.ResetTransform();

            foreach (var bullet in gameModel.BulletsOnCanvas)
            {
                g.TranslateTransform(bullet.X, bullet.Y);
                g.RotateTransform((float)(bullet.Angle * 180 / Math.PI));
                g.TranslateTransform(-bullet.X, -bullet.Y);
                g.DrawImage(Bullet.Image,
                    bullet.X, bullet.Y,
                    new Rectangle(new Point(0, 0), Bullet.Image.Size),
                    GraphicsUnit.Pixel);
                g.ResetTransform();
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                gameModel.CreateBullet();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            
        }

        public void UpdateGameLoop(object sender, EventArgs args)
        {
            var mousePos = PointToClient(MousePosition);
            gameModel.gun.Angle = (float)(Math.Atan2(-gameModel.gun.Y + mousePos.Y, -gameModel.gun.X + mousePos.X) * 180 / Math.PI);
            foreach (var bullet in gameModel.BulletsOnCanvas)
                bullet.Move();
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
