using System.Collections.Generic;
using System.Drawing;
using Top_Down_shooter.Properties;

namespace Top_Down_shooter.Scripts.Source
{
    static class GameImages
    {
        public static readonly Bitmap Player = Resources.Player;
        public static readonly Bitmap Gun = Resources.Gun;
        public static readonly Bitmap Tank = Resources.Tank;
        public static readonly Bitmap Fireman = Resources.Fireman;
        public static readonly Bitmap Boss = Resources.Boss;

        public static readonly Bitmap Fire = Resources.Fire;

        public static readonly Bitmap PlayerBullet = Resources.Bullet;
        public static readonly Bitmap FiremanBullet = Resources.Fireball;

        public static readonly Bitmap Heart = Resources.Heart;
        public static readonly Bitmap BigLoot = Resources.BigPowerup;
        public static readonly Bitmap SmallLoot = Resources.SmallLoot;

        public static readonly List<Bitmap> Grass = new List<Bitmap>();
        public static readonly Bitmap Box = Resources.Box;
        public static readonly Bitmap Block = Resources.Block;

        public static readonly Bitmap HealthBarCross = Resources.Cross;
        public static readonly Bitmap HealthBarPlayer = Resources.HealthBar;
        public static readonly Bitmap HealthBarBackgroundPlayer = Resources.BackgroundHealthBar;

        public static readonly Bitmap HealthBarEnemy = new Bitmap(
            Resources.HealthBar, new Size(Resources.HealthBar.Width / 2, Resources.HealthBar.Height / 2 - 1));
        public static readonly Bitmap HealthBarBackgroundEnemy = new Bitmap(
            Resources.BackgroundHealthBar, new Size(Resources.BackgroundHealthBar.Width / 2 - 1, Resources.BackgroundHealthBar.Height / 2));

        public static readonly Bitmap HealthBarBoss = new Bitmap(
            Resources.HealthBar, new Size(Resources.HealthBar.Width, Resources.HealthBar.Height));
        public static readonly Bitmap HealthBarBackgroundBoss = new Bitmap(
            Resources.BackgroundHealthBar, new Size(Resources.BackgroundHealthBar.Width, Resources.BackgroundHealthBar.Height));
    }
}
