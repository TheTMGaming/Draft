using System;
using System.Drawing;

namespace Top_Down_shooter
{
    class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }
        
        private readonly int sizeTile = 64;
        private readonly Sprite[,] map;

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            map = new Sprite[Width / sizeTile, Height / sizeTile];
        }

        public Sprite[,] CreateMap()
        {
            var rand = new Random();

            for (var x = sizeTile / 2; x < Width; x += sizeTile)
            {
                for (var y = sizeTile / 2; y < Height; y += sizeTile)
                {
                    map[x / sizeTile, y / sizeTile] = new Sprite()
                    {
                        X = x,
                        Y = y,
                        Image = new Bitmap("Sprites/Grass.png").Extract(new Rectangle(64 * rand.Next(0, 4), 0, 64, 64))
                    };
                }
            }

            map[0, 0] = new Sprite
            {
                X = 32,
                Y = 32,
                Image = new Bitmap("Sprite/Box.png");
            }

            return map;
        }

    }
}
