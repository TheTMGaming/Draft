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
        private D2DBitmap gameOverImage;
        private D2DBitmap victoryImage;

        private readonly Point positionLableCountBullets = new Point(980, 670);
        private readonly Point positionBulletIcon = new Point(910, 660);

        private readonly Random randGenerator = new Random();

        public Form1()
        {
            Cursor = new Cursor("Cursor.cur");
            DoubleBuffered = false;
            Size = new Size(GameSettings.ScreenWidth, GameSettings.ScreenHeight);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            CenterToScreen();

            GameModel.Initialize();
            GameRender.Initialize();
         
            RunTimeInvoker(IntervalUpdateGameLoop, UpdateGameLoop);
            RunTimeInvoker(GameSettings.DelaySpawnNewMonster, GameModel.SpawnTank);
            RunTimeInvoker(GameSettings.BossCooldown, GameModel.SpawnFire);
         
            RunFunctionAsync(Controller.UpdateKeyboardHandler);
            RunFunctionAsync(Controller.UpdateMouseHandler);
            RunFunctionAsync(NavMesh.Update);
            RunFunctionAsync(Physics.Update);
            RunFunctionAsync(GameRender.PlayAnimations);
        }
        protected override void OnLoad(EventArgs e)
        {
            device = new D2DGraphicsDevice(this);
            defaultTransform = device.Graphics.GetTransform();

            bulletIcon = device.CreateBitmap(Resources.BulletIcon);
            gameOverImage = device.CreateBitmap(Resources.GameOver);
            victoryImage = device.CreateBitmap(Resources.Victory);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F && GameModel.IsEnd)
                Application.Restart();
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

            if (GameModel.IsEnd)
            {
                if (GameModel.Player.Health < 1)
                {
                    device.Graphics.DrawBitmap(gameOverImage, new D2DRect(
                         GameRender.Camera.X + GameSettings.ScreenWidth / 2 - gameOverImage.Width / 2,
                         GameRender.Camera.Y + GameSettings.ScreenHeight / 2 - gameOverImage.Height / 2,
                         gameOverImage.Width, gameOverImage.Height));
                }
                else
                {
                    device.Graphics.DrawBitmap(victoryImage, new D2DRect(
                         GameRender.Camera.X + GameSettings.ScreenWidth / 2 - victoryImage.Width / 2,
                         GameRender.Camera.Y + GameSettings.ScreenHeight / 2 - victoryImage.Height / 2,
                         victoryImage.Width, victoryImage.Height));
                }
            }
            
            device.EndRender();
        }

        private void UpdateGameLoop()
        {
            if (!GameModel.IsEnd)
            {
                UpdatePlayer();

                UpdateEnemies();

                UpdateBullets();

                UpdateFires();

                if (GameModel.Player.Health < 0 || GameModel.Boss.Health < 0)
                    GameModel.IsEnd = true;
            }

            Invalidate();
            
        }

        private void UpdatePlayer()
        {
            var mousePosition = PointToClient(MousePosition);

            GameRender.Camera.Move(GameModel.Player);

            GameModel.Player.Gun.Angle = (float)Math.Atan2(
                mousePosition.Y + GameRender.Camera.Y - GameModel.Player.Gun.Y,
                mousePosition.X + GameRender.Camera.X - GameModel.Player.Gun.X);

            var prevPos = GameModel.Player.Transform;

            GameModel.Player.MoveX();
            if (Physics.IsCollided(GameModel.Player.Collider, typeof(Box), typeof(Block), typeof(Boss)))
                GameModel.Player.SetX(prevPos.X);

            GameModel.Player.MoveY();
            if (Physics.IsCollided(GameModel.Player.Collider, typeof(Box), typeof(Block), typeof(Boss)))
                GameModel.Player.SetY(prevPos.Y);



            if (Physics.IsHit(GameModel.Player.HitBox, out var collisions))
            {
                foreach (var other in collisions.Select(collider => collider.GameObject))
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
                                Physics.RemoveFromTrackingHitBoxes(powerup.Collider);
                                GameRender.RemoveRender(powerup);
                            }
                            else GameModel.RespawnStaticPowerup(powerup);
                        }
                    }
                }
            }
        }

        private void UpdateEnemies()
        {
            lock (GameModel.LockerEnemies)
            {
                GameModel.Boss.LookAt(GameModel.Player.Transform);

                while (GameModel.NewEnemies.Count > 0)
                {
                    var enemy = GameModel.NewEnemies.Dequeue();

                    if (enemy is null)
                        continue;

                    GameModel.Enemies.Add(enemy);

                    Physics.AddToTrackingColliders(enemy.Collider);
                    Physics.AddToTrackingHitBoxes(enemy.HitBox);

                    GameRender.AddRenderFor(enemy);
                }


                foreach (var enemy in GameModel.Enemies)
                {
                    if (enemy.Health < 1)
                    {                     
                        GameModel.RemovedEnemies.Enqueue(enemy);                       
                    }

                    if (enemy is Fireman fireman)
                        GameModel.UpdateTargetFireman(fireman);

                    enemy.Move();

                    if (enemy is Tank tank && Physics.IsHit(enemy.HitBox, out var collisions))
                    {
                        foreach (var other in collisions.Select(hitBox => hitBox.GameObject))
                        {
                            if (other is Fire fire && fire.CanKick)
                            {
                                fire.CanKick = false;
                                enemy.Health -= GameSettings.FireDamage;
                            }

                            if (other is Player && tank.CanKick)
                            {
                                GameModel.Player.Health -= GameSettings.TankDamage;
                                tank.CanKick = false;
                            }
                        }
                    }
                }

                while (GameModel.RemovedEnemies.Count > 0)
                {
                    var enemy = GameModel.RemovedEnemies.Dequeue();

                    GameModel.Enemies.Remove(enemy);

                    GameRender.RemoveRender(enemy);

                    NavMesh.RemoveAgent(enemy.Agent);

                    Physics.RemoveFromTrackingColliders(enemy.Collider);
                    Physics.RemoveFromTrackingHitBoxes(enemy.HitBox);

                    enemy.Cooldown.Dispose();

                    GameModel.SpawnBigLoot(enemy);
                }
            }
        }

        private void UpdateBullets()
        {
            lock (GameModel.LockerBullets)
            {
                while (GameModel.NewBullets.Count > 0)
                {
                    var bullet = GameModel.NewBullets.Dequeue();

                    if (bullet is null)
                        continue;

                    if (bullet.Parent is Player)
                        GameModel.Player.Gun.CountBullets--;

                    GameModel.Bullets.Add(bullet);

                    GameRender.AddRenderFor(bullet);
                }


                foreach (var bullet in GameModel.Bullets)
                {
                    bullet.Move();

                    if (Physics.IsHit(bullet.Collider, out var collisions))
                    {
                        var willBeDestroyed = false;

                        foreach (var other in collisions.Select(hitBox => hitBox.GameObject))
                        {
                            if (other is Block)
                                willBeDestroyed = true;

                            if (other is Box box)
                            {
                                box.Health -= bullet.Damage;
                                if (box.Health < 1)
                                {
                                    GameModel.ChangeBoxToGrass(box);
                                    Physics.RemoveFromTrackingColliders(box.Collider);
                                    Physics.RemoveFromTrackingHitBoxes(box.Collider);
                                    GameRender.RemoveRender(box);
                                }

                                willBeDestroyed = true;
                            }

                            if (other is Player && !(bullet.Parent is Player))
                            {
                                GameModel.Player.Health -= bullet.Damage;

                                willBeDestroyed = true;
                            }

                            if (other is Enemy enemy)
                            {
                                if ((enemy is Fireman || enemy is Boss) && bullet.Parent is Fireman)
                                    continue;

                                enemy.Health -= bullet.Damage;

                                willBeDestroyed = true;
                            }
                        }

                        if (willBeDestroyed)
                        {
                            GameModel.DeletedBullets.Enqueue(bullet);
                            GameRender.RemoveRender(bullet);
                            Physics.RemoveFromTrackingHitBoxes(bullet.Collider);
                        }
                    }

                }

                while (GameModel.DeletedBullets.Count > 0)
                {
                    var removedBullet = GameModel.DeletedBullets.Dequeue();

                    GameModel.Bullets.Remove(removedBullet);
                    GameRender.RemoveRender(removedBullet);
                }
            }
        }

        private void UpdateFires()
        {
            lock (GameModel.LockerFires)
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
