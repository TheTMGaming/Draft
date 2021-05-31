using System.Drawing;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Gun : GameObject
    {
        public int Cooldown { get; set; }
        public int CountBullets { get; set; }

        public float Angle { get; set; }

        public readonly Point SpawnBullets = new Point(10, -10);

        public Gun(int x, int y)
        {
            Cooldown = GameSettings.PlayerCooldown;
            CountBullets = GameSettings.StartCountBullets;

            X = x;
            Y = y;
        }

        public void Move(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
