using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Controllers;
using Top_Down_shooter.Scripts.Source;
using Top_Down_shooter.Properties;

namespace Top_Down_shooter
{
    public class Form1 : Form
    {
        private readonly GameModel gameModel;
        private readonly GameRender gameRender;
        //private readonly Map map;

        public Form1()
        {
            

            DoubleBuffered = true;
            Size = new Size(int.Parse(Resources.ScreenWidth), int.Parse(Resources.ScreenHeight));
            CenterToScreen();

            gameModel = new GameModel();
            gameRender = new GameRender(gameModel);
            TileMapController.CreateTile();

            var updateGameLoop = new System.Windows.Forms.Timer();
            updateGameLoop.Interval = 30;
            updateGameLoop.Tick += (sender, args) => UpdateGameLoop();

            var playAnimations = new System.Windows.Forms.Timer();
            playAnimations.Interval = 250;
            playAnimations.Tick += new EventHandler((sender, args) =>
            {
                gameRender.PlayAnimations();
            });

            playAnimations.Start();
            updateGameLoop.Start();

            var thread = new Thread(new ThreadStart(A));
            thread.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.TranslateTransform(-gameRender.Camera.X, -gameRender.Camera.Y);
            TileMapController.DrawTile(g);

            gameRender.DrawObjects(g);
            
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                gameModel.Shoot();
        }

        private void A()
        {
           while (true)
            {
                Console.WriteLine(1);
                if (Keyboard.IsKeyPressed(Keyboard.VirtualKeyStates.VK_UP))
                    gameModel.Player.ChangeDirection(DirectionY.Up);
                else if (Keyboard.IsKeyPressed(Keyboard.VirtualKeyStates.VK_DOWN))
                    gameModel.Player.ChangeDirection(DirectionY.Down);
                else
                    gameModel.Player.ChangeDirection(DirectionY.Idle);

                if (Keyboard.IsKeyPressed(Keyboard.VirtualKeyStates.VK_RIGHT))
                    gameModel.Player.ChangeDirection(DirectionX.Right);
                else if (Keyboard.IsKeyPressed(Keyboard.VirtualKeyStates.VK_LEFT))
                    gameModel.Player.ChangeDirection(DirectionX.Left);
                else
                    gameModel.Player.ChangeDirection(DirectionX.Idle);

            }
        }

        //protected override void OnKeyDown(KeyEventArgs e)
        //{
        //    switch (e.KeyCode)
        //    {
        //        case Keys.W:
        //            gameModel.Player.ChangeDirection(DirectionY.Up);
        //            break;
        //        case Keys.A:
        //            gameModel.Player.ChangeDirection(DirectionX.Left);
        //            break;
        //        case Keys.S:
        //            gameModel.Player.ChangeDirection(DirectionY.Down);
        //            break;
        //        case Keys.D:
        //            gameModel.Player.ChangeDirection(DirectionX.Right);
        //            break;
        //    }
        //}


        //protected override void OnKeyUp(KeyEventArgs e)
        //{
        //    switch (e.KeyCode)
        //    {
        //        case Keys.W:
        //        case Keys.S:
        //            gameModel.Player.ChangeDirection(DirectionY.Idle);
        //            break;
        //        case Keys.A:
        //        case Keys.D:
        //            gameModel.Player.ChangeDirection(DirectionX.Idle);
        //            break;
        //    }
        //}

        private void UpdateGameLoop()
        {
            var mousePosition = PointToClient(MousePosition);
            gameRender.Camera.Move(gameModel.Player);
            gameModel.Player.Gun.Angle = (float)Math.Atan2(mousePosition.Y + gameRender.Camera.Y - gameModel.Player.Gun.Y, mousePosition.X + gameRender.Camera.X - gameModel.Player.Gun.X);

            Physics.Update();

            gameModel.Player.Move();

            if (Physics.IsCollided(gameModel.Player, out var a))
            {
                if (!(a is Bullet))
                gameModel.Player.ComeBack();
            }
                
           
            for (var node = gameModel.Bullets.First; !(node is null); node = node.Next)
            {
                node.Value.Move();

                if (Physics.IsCollided(node.Value, out var other))
                {
                    if (other is Player || other is Bullet)
                        continue;

                    gameModel.Bullets.Remove(node);
                    Physics.RemoveFromTrackingCollisions(node.Value);
                }             
            }

            Invalidate();
        }
    }
}
