using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Top_Down_shooter.Properties;
using Top_Down_shooter.Scripts.Controllers;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter
{
    public class Form1 : Form
    {
        private readonly int IntervalUpdateGameLoop = 30;
        private readonly int IntervalUpdateAnimations = 250;

        public Form1()
        {
            DoubleBuffered = true;
            Size = new Size(GameSettings.ScreenWidth, GameSettings.ScreenHeight);
            //FormBorderStyle = FormBorderStyle.None;
            CenterToScreen();
         
            RunTimer(IntervalUpdateGameLoop, UpdateGameLoop);
            RunTimer(IntervalUpdateAnimations, GameRender.PlayAnimations);

            RunFunctionAsync(Controller.KeyboardHandler);
            RunFunctionAsync(Controller.MouseHandler);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.TranslateTransform(-GameRender.Camera.X, -GameRender.Camera.Y);

            
            GameRender.DrawObjects(g);
            g.FillRectangle(new SolidBrush(Color.Blue), new Rectangle(GameSettings.MapWidth / 2, GameSettings.MapHeight / 2, 50, 50));

            
        }

        private void UpdateGameLoop()
        {
            
            while (Controller.SpawnedBullets.Count > 0)
            {
                var b = Controller.SpawnedBullets.Dequeue();
                GameModel.Bullets.AddLast(b);
            }

            var mousePosition = PointToClient(MousePosition);
            GameRender.Camera.Move(GameModel.Player);
            GameModel.Player.Gun.Angle = (float)Math.Atan2(
                mousePosition.Y + GameRender.Camera.Y - GameModel.Player.Gun.Y,
                mousePosition.X + GameRender.Camera.X - GameModel.Player.Gun.X);
            
            Physics.Update();

            GameModel.Player.Move();

            if (Physics.IsCollided(GameModel.Player, out var a))
            {
                if (!(a is Bullet))
                    GameModel.Player.Move(isReverse: true);
            }


            for (var node = GameModel.Bullets.First; !(node is null); node = node.Next)
            {
                node.Value.Move();

                if (Physics.IsCollided(node.Value, out var other))
                {
                    if (other is Player || other is Bullet)
                        continue;
                    if (other is Box box)
                    {
                        box.DoDamage(1);
                        if (box.Health == 0)
                        {
                            GameModel.ChangeBoxToGrass(box);
                            Physics.RemoveFromTrackingCollisions(box);
                        }
                    }

                    GameModel.Bullets.Remove(node);
                    Physics.RemoveFromTrackingCollisions(node.Value);
                }
            }

            Invalidate();
           
        }

        private void RunTimer(int interval, Action func)
        {
            var timer = new System.Windows.Forms.Timer();

            timer.Interval = interval;
            timer.Tick += (sender, args) => func();

            timer.Start();
        }

        private void RunFunctionAsync(Action func)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (sender, args) => func();
            worker.RunWorkerAsync();
        }
    }
}
