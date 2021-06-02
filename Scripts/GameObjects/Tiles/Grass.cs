using System;
using System.Drawing;
using Top_Down_shooter.Properties;
using Top_Down_shooter.Scripts.Source;
using Top_Down_shooter.Scripts.Components;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Grass : GameObject
    {
        public int ID { get; set; }

        private static Random randGenerator = new Random();

        public Grass(int x, int y)
        {
            Collider = new Collider(this, 0, 0, GameSettings.TileSize, GameSettings.TileSize);
            ID = randGenerator.Next(0, 4);
            X = x;
            Y = y;
        }
    }
}
