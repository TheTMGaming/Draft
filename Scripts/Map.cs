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

            for (var row = sizeTile / 2; row <= Width; row += sizeTile)
            {
                for (var col = sizeTile / 2; col <= Height; col += sizeTile)
                {
                    map[row / sizeTile, col / sizeTile] = new Sprite()
                    {
                        X = col,
                        Y = row,
                        Image = new Bitmap("Sprites/Grass.png")
                    };
                }
            }

            return map;
        }

    }
}
