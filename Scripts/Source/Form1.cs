using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Top_Down_shooter.Properties;
using Top_Down_shooter.Scripts.Components;
using Top_Down_shooter.Scripts.Controllers;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace Top_Down_shooter
{
    public class Form1 : Form
    {
        private readonly int IntervalUpdateGameLoop = 30;
        private readonly int IntervalUpdateAnimations = 250;

        private readonly Label countBulletsLabel;

        public Form1()
        { 
            DoubleBuffered = true;
            Size = new Size(GameSettings.ScreenWidth, GameSettings.ScreenHeight);
            //FormBorderStyle = FormBorderStyle.None;
            CenterToScreen();
         
            RunTimeInvoker(IntervalUpdateGameLoop, UpdateGameLoop);
            RunTimeInvoker(IntervalUpdateAnimations, GameRender.PlayAnimations);
            RunTimeInvoker(GameSettings.DelaySpawnNewMonster, GameModel.SpawnEnemy);

            RunFunctionAsync(Controller.KeyboardHandler);
            RunFunctionAsync(Controller.MouseHandler);

           
            var gameTimer = new Timer();
            var time = new TimeSpan(0, 5, 0);
            var timeLabel = new Label()
            {
                Size = new Size(200, 200),
                Font = new Font("Arial Rounded MT Bold", 30),
                BackColor = Color.Transparent,
                Location = new Point(1100, 20)
            };
            
            gameTimer.Tick += (sender, obj) =>
            {
                time -= TimeSpan.FromSeconds(1);
                timeLabel.Text = time.ToString(@"m\:ss");
            };
            gameTimer.Interval = 1000;
            gameTimer.Start();
            Controls.Add(timeLabel);

            countBulletsLabel = new Label()
            {
                Size = new Size(200, 200),
                Font = new Font("Arial Rounded MT Bold", 30),
                BackColor = Color.Transparent,
                Location = new Point(1160, 666)
            };
            Controls.Add(countBulletsLabel);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.TranslateTransform(-GameRender.Camera.X, -GameRender.Camera.Y);

            GameRender.DrawObjects(g);
        }

        private void UpdateGameLoop()
        {
            GameModel.Player.Gun.CountBullets -= Controller.SpawnedBullets.Count;
            while (Controller.SpawnedBullets.Count > 0)
            {
                var b = Controller.SpawnedBullets.Dequeue();
                GameModel.Bullets.AddLast(b);
            }
            countBulletsLabel.Text = GameModel.Player.Gun.CountBullets.ToString();

            var mousePosition = PointToClient(MousePosition);
            GameRender.Camera.Move(GameModel.Player);
            GameModel.Player.Gun.Angle = (float)Math.Atan2(
                mousePosition.Y + GameRender.Camera.Y - GameModel.Player.Gun.Y,
                mousePosition.X + GameRender.Camera.X - GameModel.Player.Gun.X);
            
            Physics.Update();

            GameModel.Player.Move();
            if (Physics.IsCollided(GameModel.Player, out var others))
            {
                foreach (var other in others)
                {
                    if (other is Box || other is Block)
                    {
                        GameModel.Player.Move(isReverse: true);
                        break;
                    }
                }
            }


            GameModel.MoveEnemies();
            foreach (var enemy in GameModel.Enemies)
            {
                if (Physics.IsCollided(enemy, out others))
                {
                    foreach (var collision in others)
                    {
                        if (collision is Player && enemy.CanKick)
                        {
                            GameModel.Player.Health -= GameSettings.TankDamage;
                            enemy.CanKick = false;
                        }
                    }
                }
            }

            for (var bullet = GameModel.Bullets.First; !(bullet is null); bullet = bullet.Next)
            {
                bullet.Value.Move();

                if (Physics.IsCollided(bullet.Value, out others))
                {
                    var willBeDestroyed = false;

                    foreach (var collision in others)
                    {
                        if (collision is Box box)
                        {
                            box.Health -= 1;
                            if (box.Health == 0)
                            {
                                GameModel.ChangeBoxToGrass(box);
                                Physics.RemoveFromTrackingCollisions(box.Collider);
                            }

                            willBeDestroyed = true;
                        }

                        if (collision is Tank tank)
                        {
                            tank.Health -= 1;
                            if (tank.Health < 1)
                                GameModel.RespawnEnemy(tank);

                            willBeDestroyed = true;
                        }
                    }

                    if (willBeDestroyed)
                    {
                        GameModel.Bullets.Remove(bullet);
                        Physics.RemoveFromTrackingCollisions(bullet.Value.Collider);
                    }
                }
            }

            Invalidate();
           
        }

        private void RunTimeInvoker(int interval, Action func)
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
