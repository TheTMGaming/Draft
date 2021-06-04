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
using unvell.D2DLib;

namespace Top_Down_shooter
{
    public class Form1 : Form
    {
        private readonly int IntervalUpdateGameLoop = 30;

        private D2DGraphicsDevice device;
        private D2DMatrix3x2 defaultTransform;

        private D2DBitmap bulletIcon;
        private readonly Point positionLableCountBullets = new Point(980, 670);
        private readonly Point positionBulletIcon = new Point(910, 660);

        public Form1()
        { 
            DoubleBuffered = false;
            Size = new Size(GameSettings.ScreenWidth, GameSettings.ScreenHeight);
            //FormBorderStyle = FormBorderStyle.None;
            CenterToScreen();

            GameModel.Initialize();
            GameRender.Initialize();
         
            RunTimeInvoker(IntervalUpdateGameLoop, UpdateGameLoop);
            RunTimeInvoker(GameSettings.DelaySpawnNewMonster, GameModel.SpawnEnemy);
            RunTimeInvoker(GameSettings.BosCooldown, GameModel.SpawnFire);

            RunFunctionAsync(Controller.UpdateKeyboardHandler);
            RunFunctionAsync(Controller.UpdateMouseHandler);
            RunFunctionAsync(NavMesh.Update);
            RunFunctionAsync(GameRender.PlayAnimations);
            RunFunctionAsync(Physics.Update);
        }
        protected override void OnLoad(EventArgs e)
        {
            device = new D2DGraphicsDevice(this);
            defaultTransform = device.Graphics.GetTransform();

            bulletIcon = device.CreateBitmap(Resources.BulletIcon);
        }

        protected override void OnPaintBackground(PaintEventArgs e) { }

        protected override void OnPaint(PaintEventArgs e)
        {
            device.BeginRender();

            device.Graphics.SetTransform(defaultTransform);
            device.Graphics.TranslateTransform(-GameRender.Camera.X, -GameRender.Camera.Y);

            GameRender.DrawScene(device);
          
            device.Graphics.DrawText(
                GameModel.Player.Gun.CountBullets.ToString(), 
                D2DColor.Black, "Intro", 35,
                GameRender.Camera.X + positionLableCountBullets.X, GameRender.Camera.Y + positionLableCountBullets.Y);

            device.Graphics.DrawBitmap(bulletIcon, new D2DRect(
                GameRender.Camera.X + positionBulletIcon.X, 
                GameRender.Camera.Y + positionBulletIcon.Y, 
                bulletIcon.Size.width, bulletIcon.Size.height));
            
            device.EndRender();
        }

        private void UpdateGameLoop()
        {           
            UpdatePlayer();

            UpdateEnemies();

            UpdateBullets();

            UpdateFires();

            Invalidate();
            
        }

        private void UpdatePlayer()
        {
            var mousePosition = PointToClient(MousePosition);

            GameRender.Camera.Move(GameModel.Player);

            GameModel.Player.Gun.Angle = (float)Math.Atan2(
                mousePosition.Y + GameRender.Camera.Y - GameModel.Player.Gun.Y,
                mousePosition.X + GameRender.Camera.X - GameModel.Player.Gun.X);

            GameModel.Player.Move();
            if (Physics.IsCollided(GameModel.Player, out var collisions))
            {
                foreach (var other in collisions)
                {
                    if (other is Fire fire && fire.CanKick)
                    {
                        fire.CanKick = false;
                        GameModel.Player.Health -= GameSettings.FireDamage;

                        continue;
                    }
                    if (other is Powerup powerup)
                    {
                        if (powerup is HP)
                        {
                            if (GameModel.Player.Health < GameSettings.PlayerHealth)
                            {
                                GameModel.Player.Health += powerup.Boost;
                                GameModel.RespawnStaticPowerup(powerup);
                            }
                        }
                        else
                        {
                            GameModel.Player.Gun.CountBullets += powerup.Boost;

                            if (powerup is BigLoot)
                            {
                                GameModel.Powerups.Remove(powerup);
                                Physics.RemoveFromTrackingCollisions(powerup.Collider);
                                GameRender.RemoveDynamicRenderFrom(powerup);
                            }
                            else GameModel.RespawnStaticPowerup(powerup);
                        }

                        continue;
                    }

                    if (other is Box || other is Block || other is Boss)
                    {
                        GameModel.Player.Move(isReverse: true);

                        GameModel.Player.MoveX();
                        if (GameModel.Player.Collider.IntersectsWith(other.Collider))
                            GameModel.Player.MoveX(isReverse: true);

                        GameModel.Player.MoveY();
                        if (GameModel.Player.Collider.IntersectsWith(other.Collider))
                            GameModel.Player.MoveY(isReverse: true);

                    }
                }
            }
        }

        private void UpdateEnemies()
        {
            GameModel.Boss.LookAt(GameModel.Player.Transform);

            while (GameModel.NewEnemies.Count > 0)
                GameModel.Enemies.Add(GameModel.NewEnemies.Dequeue());

            foreach (var enemy in GameModel.Enemies)
            {
                if (enemy is Fireman fireman)
                    GameModel.UpdateTargetFireman(fireman);

                enemy.Move();

                if (enemy is Tank tank && Physics.IsCollided(enemy, out var collisions))
                {
                    foreach (var other in collisions)
                    {
                        if (other is Fire fire && fire.CanKick)
                        {
                            fire.CanKick = false;
                            enemy.Health -= GameSettings.FireDamage;
                            if (enemy.Health < 1)
                                GameModel.RespawnEnemy(enemy);
                        }

                        if (other is Player && tank.CanKick)
                        {
                            GameModel.Player.Health -= GameSettings.TankDamage;
                            tank.CanKick = false;
                        }
                    }
                }
            }
        }

        private void UpdateBullets()
        {
            GameModel.Player.Gun.CountBullets -= GameModel.NewBullets.Count;
            while (GameModel.NewBullets.Count > 0)
            {
                var bullet = GameModel.NewBullets.Dequeue();

                GameModel.Bullets.AddLast(bullet);

                GameRender.AddDynamicRenderFor(bullet);
            }

            for (var bullet = GameModel.Bullets.First; !(bullet is null); bullet = bullet.Next)
            {
                bullet.Value.Move();

                if (Physics.IsCollided(bullet.Value, out var collisions))
                {
                    var willBeDestroyed = false;

                    foreach (var other in collisions)
                    {
                        if (other is Block)
                            willBeDestroyed = true;

                        if (other is Box box)
                        {
                            box.Health -= bullet.Value.Damage;
                            if (box.Health == 0)
                            {
                                GameModel.ChangeBoxToGrass(box);
                                Physics.RemoveFromTrackingCollisions(box.Collider);
                                GameRender.RemoveDynamicRenderFrom(box);
                            }

                            willBeDestroyed = true;
                        }

                        if (other is Enemy enemy)
                        {
                            if (other is Fireman && bullet.Value.Parent is Fireman)
                                continue;

                            enemy.Health -= bullet.Value.Damage;
                            if (!(enemy is Boss && enemy is Fireman) && enemy.Health < 1)
                                GameModel.RespawnEnemy(enemy);

                            willBeDestroyed = true;
                        }
                    }

                    if (willBeDestroyed)
                    {
                        GameModel.Bullets.Remove(bullet);
                        GameRender.RemoveDynamicRenderFrom(bullet.Value);
                        Physics.RemoveFromTrackingCollisions(bullet.Value.Collider);
                    }
                }
            }
        }

        private void UpdateFires()
        {
            while (GameModel.NewFires.Count > 0)
            {
                var fire = GameModel.NewFires.Dequeue();

                GameModel.Fires.Add(fire);
                GameModel.MovingFires.AddLast(fire);
            }

            for (var fire = GameModel.MovingFires.First; !(fire is null); fire = fire.Next)
            {
                fire.Value.Move();

                if (fire.Value.IsCompleteMoving)
                {
                    GameModel.MovingFires.Remove(fire);
                }
            }
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
