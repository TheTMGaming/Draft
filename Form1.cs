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

            var gun = gameModel.Gun;

            
            g.TranslateTransform(gun.X, gun.Y);
            g.RotateTransform((float)(gun.Angle * 180 / Math.PI));
            g.TranslateTransform(-gun.X, -gun.Y);
            g.DrawImage(
                Gun.Image,
                gun.X - Gun.Image.Width / 4, gun.Y - Gun.Image.Width / 4,
                new Rectangle(new Point(0, 0), new Size(Gun.Image.Size.Width / 2, Gun.Image.Size.Height / 2)),
                GraphicsUnit.Pixel);
            g.ResetTransform();

            foreach (var bullet in gameModel.MovedBullets)
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
                gameModel.Shoot();
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
            gameModel.Gun.Angle = (float)Math.Atan2(-gameModel.Gun.Y + mousePos.Y, -gameModel.Gun.X + mousePos.X);


            for (var node = gameModel.MovedBullets.First; !(node is null); node = node.Next)
            {
                if (node.Value.X < 0 || node.Value.X > Size.Width || node.Value.Y < 0 || node.Value.Y > Size.Height)
                {
                    gameModel.MovedBullets.Remove(node);
                    continue;
                }

                node.Value.Move();
            }

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
