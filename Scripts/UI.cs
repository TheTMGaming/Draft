
namespace Top_Down_shooter
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
