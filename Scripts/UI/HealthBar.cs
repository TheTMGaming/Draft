namespace Top_Down_shooter.Scripts.UI
{
    class HealthBar
    {
        public int Percent
        {
            get { return percent; }
            set
            {
                percent = value;
                if (percent < 0) percent = 0;
                if (percent > 100) percent = 100;
            }
        }

        private int percent;

        public HealthBar(int percent)
        {
            Percent = percent;
        }
    }
}
