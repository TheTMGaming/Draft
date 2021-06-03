using Top_Down_shooter.Scripts.Components;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Powerup : GameObject
    {
        public int Boost { get; set; }

        public Powerup(int x, int y)
        {
            X = x;
            Y = y;

            Collider = new Collider(this, 0, 0, 60, 60, isTrigger: true, isIgnoreNavMesh: true);
        }
    }

    class BigLoot : Powerup
    {
        private readonly Powerup powerup;

        public BigLoot(Powerup powerup) : base(powerup.X, powerup.Y)
        {
            this.powerup = powerup;
            Boost = GameSettings.BigLoot;
        }
    }

    class SmallLoot : Powerup
    {
        private readonly Powerup powerup;

        public SmallLoot(Powerup powerup) : base(powerup.X, powerup.Y)
        {
            this.powerup = powerup;
            Boost = GameSettings.SmallLoot;
        }
    }

    class HP : Powerup
    {
        private readonly Powerup powerup;

        public HP(Powerup powerup) : base(powerup.X, powerup.Y)
        {
            this.powerup = powerup;
            Boost = GameSettings.HPUp;
        }
    }
}
