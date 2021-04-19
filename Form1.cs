using System;
using System.Drawing;
using System.Windows.Forms;

namespace Top_Down_shooter
{
    public class Form1 : Form
    {
        private readonly GameModel gameModel;
        private readonly Map map;

        public Form1()
        {
            DoubleBuffered = true;
            Size = new Size(1280, 768);
            CenterToScreen();

            gameModel = new GameModel();
            map = new Map(Width, Height);
            

            var updateGameLoop = new Timer();
            updateGameLoop.Interval = 30;
            updateGameLoop.Tick += (sender, args) => UpdateGameLoop();

            var playAnimations = new Timer();
            playAnimations.Interval = 300;
            playAnimations.Tick += new EventHandler((sender, args) =>
            {
                gameModel.PlayAnimations();
            });

            playAnimations.Start();
            updateGameLoop.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            foreach (var a in map.Tiles)
                a.Draw(g);

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
            var mousePosition = PointToClient(MousePosition);

            gameModel.Player.Gun.Angle = (float)Math.Atan2(mousePosition.Y - gameModel.Player.Gun.Y, mousePosition.X - gameModel.Player.Gun.X);

            for (var node = gameModel.GameSprites.First; !(node is null); node = node.Next)
            {
                if ((node.Value.X < 0 || node.Value.X > Size.Width || node.Value.Y < 0 || node.Value.Y > Size.Height) && !(node.Value is Player))
                {
                    gameModel.GameSprites.Remove(node);
                    continue;
                }


                node.Value.Move();
            }

            Invalidate();
        }
    }
}
