using Top_Down_shooter.Scripts.Components;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.GameObjects
{
    enum TypesPowerup
    { 
        BigLoot, SmallLoot
    }

    class Powerup : GameObject
    {
        public readonly TypesPowerup Type;
        public readonly int Cost;

        public Powerup(int x, int y, TypesPowerup type)
        {
            X = x;
            Y = y;

            Type = type;
            if (type == TypesPowerup.SmallLoot)
                Cost = GameSettings.SmallLoot;
            else
                Cost = GameSettings.BigLoot;

            Collider = new Collider(this, 0, 0, 60, 60, isTrigger: true);
        }
    }
}
