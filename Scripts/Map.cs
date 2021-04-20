using System;
using System.Drawing;

namespace Top_Down_shooter
{
    class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public readonly Sprite[,] Tiles;

        private readonly int sizeTile = 64;

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            Tiles = new Sprite[Width / sizeTile, Height / sizeTile];

            CreateMap();
        }

        public void CreateMap()
        {
            var rand = new Random();

            //for (var x = sizeTile / 2; x < Width; x += sizeTile)
            //{
            //    for (var y = sizeTile / 2; y < Height; y += sizeTile)
            //    {
            //        var image = new Bitmap("Sprites/Grass.png").Extract(new Rectangle(64 * rand.Next(0, 4), 0, 64, 64));

            //        if (rand.NextDouble() > 0.95)
            //            image = new Bitmap("Sprites/Box.png");

            //        Tiles[x / sizeTile, y / sizeTile] = new Sprite()
            //        {
            //            X = x,
            //            Y = y,
            //            Image = image
            //        };
            //    }
            //}
        }

    }
}
