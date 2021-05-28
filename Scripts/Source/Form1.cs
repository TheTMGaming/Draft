using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Top_Down_shooter.Properties;
using Top_Down_shooter.Scripts.Components;
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
            RunTimer(GameSettings.DelaySpawnNewMonster, GameModel.SpawnEnemy);

            RunFunctionAsync(Controller.KeyboardHandler);
            RunFunctionAsync(Controller.MouseHandler);
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.TranslateTransform(-GameRender.Camera.X, -GameRender.Camera.Y);


            GameRender.DrawObjects(g);
            //g.FillRectangle(new SolidBrush(Color.Red), GameModel.Player.Collider.Transform);

            foreach (var c in NavMeshAgent.navMesh)
            {
                var b = new SolidBrush(Color.Blue);
                if (c.IsObstacle)
                    b = new SolidBrush(Color.Red);


                g.FillRectangle(b, c.Position.X, c.Position.Y, 3, 3);
            }

          

            g.FillRectangle(new SolidBrush(Color.Blue), new Rectangle(GameSettings.MapWidth / 2, GameSettings.MapHeight / 2, 50, 50));
            //g.FillRectangle(new SolidBrush(Color.Blue), GameModel.Player.Collider.X, GameModel.Player.Collider.Y, 5, 5);
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
            Console.WriteLine(GameModel.Player.Transform);
            if (Physics.IsCollided(GameModel.Player, out var others))
            {
                foreach (var other in others)
                {
                    if (other is Box)
                    {
                        GameModel.Player.Move(isReverse: true);
                        break;
                    }
                }
            }


            GameModel.MoveEnemies();
         

            if (Physics.IsCollided(GameModel.Player, out others))
            {
                foreach (var collision in others)
                {
                    if (collision is Block)
                    {
                        GameModel.Player.Move(isReverse: true);
                        break;
                    }
                }
            }


            for (var bullet = GameModel.Bullets.First; !(bullet is null); bullet = bullet.Next)
            {
                bullet.Value.Move();

                if (Physics.IsCollided(bullet.Value, out others))
                {
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
                        }

                        if (collision is Character character)
                        {
                            character.Health -= 1;
                            if (character.Health < 1)
                                GameModel.RespawnEnemy(character);
                        }
                    }

                    GameModel.Bullets.Remove(bullet);
                    Physics.RemoveFromTrackingCollisions(bullet.Value.Collider);
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
