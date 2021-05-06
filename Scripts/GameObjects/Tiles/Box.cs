namespace Top_Down_shooter.Scripts.GameObjects
{
    class Box : GameObject
    { 
        public int Health { get; set; }

        public Box(int xLeft, int yTop)
        {
            Health = 10;
            X = xLeft;
            Y = yTop;
        }
    }
}
