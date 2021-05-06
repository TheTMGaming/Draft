

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Grass : GameObject
    {
        public Grass(int xLeft, int yTop)
        {
            IsTrigger = true;
            X = xLeft;
            Y = yTop;
        }
    }
}
