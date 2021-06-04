using Top_Down_shooter.Scripts.GameObjects;

namespace Top_Down_shooter.Scripts.UI
{
    class HealthBar : GameObject
    {
        public int Percent
        {
            get { return (int)(character.Health / startHealth * 100); }
            set
            {
                percent = value;
                if (percent < 0) percent = 0;
                if (percent > 100) percent = 100;
            }
        }

        private int percent;

        private readonly Character character;
        private readonly float startHealth;

        public HealthBar(Character character)
        {
            this.character = character;
            startHealth = character.Health;
            Percent = percent;
        }
    }
}
