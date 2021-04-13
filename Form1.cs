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
    public class Form1 : Form
    {
        private GameModel gameModel;

        public Form1()
        {
            DoubleBuffered = true;
            Size = new Size(1280, 768);
            CenterToScreen();

            gameModel = new GameModel();

            var timerGameLoop = new Timer();
            timerGameLoop.Interval = 30;
            timerGameLoop.Tick += new EventHandler(UpdateGameLoop);
            timerGameLoop.Start();

            var timerChangeAnimationFrame = new Timer();
            timerChangeAnimationFrame.Interval = 300;
            timerChangeAnimationFrame.Tick += new EventHandler((sender, args) =>
            {
                gameModel.Player.PlayAnimation();
            });
            timerChangeAnimationFrame.Start();
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            gameModel.Player.Draw(g);

            g.TranslateTransform(gameModel.Player.Gun.X, gameModel.Player.Gun.Y);
            g.RotateTransform((float)(gameModel.Player.Gun.Angle * 180 / Math.PI));
            g.TranslateTransform(-gameModel.Player.Gun.X, -gameModel.Player.Gun.Y);
            

            gameModel.Player.Gun.Draw(g);
            g.ResetTransform();

            foreach (var bullet in this.gameModel.MovedBullets)
            {
                g.TranslateTransform(bullet.X, bullet.Y);
                g.RotateTransform((float)(bullet.Angle * 180 / Math.PI));
                g.TranslateTransform(-bullet.X, -bullet.Y);
                g.DrawImage(bullet.Image,
                    bullet.X, bullet.Y,
                    new Rectangle(new Point(0, 0), bullet.Image.Size),
                    GraphicsUnit.Pixel);
                g.ResetTransform();
            }

            //gameModel.HealthBar.Draw(g);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                gameModel.Shoot();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    gameModel.Player.ChangeDirection(DirectionY.Up);
                    break;
                case Keys.A:
                    gameModel.Player.ChangeDirection(DirectionX.Left);
                    break;
                case Keys.S:
                    gameModel.Player.ChangeDirection(DirectionY.Down);
                    break;
                case Keys.D:
                    gameModel.Player.ChangeDirection(DirectionX.Right);
                    break;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                case Keys.S:
                    gameModel.Player.ChangeDirection(DirectionY.Idle);
                    break;
                case Keys.A:
                case Keys.D:
                    gameModel.Player.ChangeDirection(DirectionX.Idle);
                    break;
            }
        }

        public void UpdateGameLoop(object sender, EventArgs args)
        {
            var mousePos = PointToClient(MousePosition);
            gameModel.Player.Gun.Angle = (float)Math.Atan2(-gameModel.Player.Gun.Y + mousePos.Y, -gameModel.Player.Gun.X + mousePos.X);
            gameModel.Player.Move();


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
    }
}
