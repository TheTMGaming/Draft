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
        private readonly GameModel gameModel;

        public Form1()
        {
            DoubleBuffered = true;
            Size = new Size(1280, 768);
            CenterToScreen();

            gameModel = new GameModel(Size);
            gameModel.UpdateGameLoop += (sender, args) => Invalidate();

            var playAnimations = new Timer();
            playAnimations.Interval = 300;
            playAnimations.Tick += new EventHandler((sender, args) =>
            {
                gameModel.PlayAnimations();
            });

            gameModel.Start();
            playAnimations.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            foreach (var sprite in gameModel.GameSprites)
                DrawSprite(g, sprite);

            foreach (var ui in gameModel.UI)
                ui.Draw(g);
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
                    gameModel.ChangeDirectionPlayer(DirectionY.Up);
                    break;
                case Keys.A:
                    gameModel.ChangeDirectionPlayer(DirectionX.Left);
                    break;
                case Keys.S:
                    gameModel.ChangeDirectionPlayer(DirectionY.Down);
                    break;
                case Keys.D:
                    gameModel.ChangeDirectionPlayer(DirectionX.Right);
                    break;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                case Keys.S:
                    gameModel.ChangeDirectionPlayer(DirectionY.Idle);
                    break;
                case Keys.A:
                case Keys.D:
                    gameModel.ChangeDirectionPlayer(DirectionX.Idle);
                    break;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e) => gameModel.UpdateMousePosition(e.Location);    

        private void DrawSprite(Graphics g, Sprite sprite)
        {
            g.TranslateTransform(sprite.X, sprite.Y);
            g.RotateTransform((float)(sprite.Angle * 180 / Math.PI));
            g.TranslateTransform(-sprite.X, -sprite.Y);

            sprite.Draw(g);

            g.ResetTransform();
        }

        private void UpdateGameLoop()
        {
            Player.Gun.Angle = (float)Math.Atan2(mousePosition.Y - Player.Gun.Y, mousePosition.X - Player.Gun.X);

            for (var node = GameSprites.First; !(node is null); node = node.Next)
            {
                if (node.Value.X < 0 || node.Value.X > sizeForm.Width || node.Value.Y < 0 || node.Value.Y > sizeForm.Height)
                {
                    GameSprites.Remove(node);
                    continue;
                }

                node.Value.Move();
            }
        }
    }
}
