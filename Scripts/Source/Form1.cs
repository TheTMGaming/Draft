using System;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Controllers;
using Top_Down_shooter.Properties;
using System.ComponentModel;

namespace Top_Down_shooter
{
    public class Form1 : Form
    {
        private readonly GameRender gameRender;
        //private readonly Map map;

        public Form1()
        {

            DoubleBuffered = true;
            Size = new Size(int.Parse(Resources.ScreenWidth), int.Parse(Resources.ScreenHeight));
            CenterToScreen();

            gameRender = new GameRender();
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

            var worker = new BackgroundWorker();
            worker.DoWork += Controller.KeyboardHandler;
            worker.RunWorkerAsync();

            var worker1 = new BackgroundWorker();
            worker1.DoWork += Controller.MouseHandler;
            worker1.RunWorkerAsync();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.TranslateTransform(-gameRender.Camera.X, -gameRender.Camera.Y);
            TileMapController.DrawTile(g);

            gameRender.DrawObjects(g);
            
        }

        private void UpdateGameLoop()
        {
            
            while (Controller.SpawnedBullets.Count > 0)
            {
                var b = Controller.SpawnedBullets.Dequeue();
                GameModel.Bullets.AddLast(b);
            }

            var mousePosition = PointToClient(MousePosition);
            gameRender.Camera.Move(GameModel.Player);
            GameModel.Player.Gun.Angle = (float)Math.Atan2(
                mousePosition.Y + gameRender.Camera.Y - GameModel.Player.Gun.Y,
                mousePosition.X + gameRender.Camera.X - GameModel.Player.Gun.X);
            
            Physics.Update();

            GameModel.Player.Move();

            if (Physics.IsCollided(GameModel.Player, out var a))
            {
                if (!(a is Bullet))
                    GameModel.Player.ComeBack();
            }


            for (var node = GameModel.Bullets.First; !(node is null); node = node.Next)
            {
                node.Value.Move();

                if (Physics.IsCollided(node.Value, out var other))
                {
                    if (other is Player || other is Bullet)
                        continue;

                    GameModel.Bullets.Remove(node);
                    Physics.RemoveFromTrackingCollisions(node.Value);
                }
            }

            Invalidate();
           
        }
    }
}
