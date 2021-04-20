using System;
using System.Drawing;

namespace Top_Down_shooter.Scripts.GameObjects
{
    enum TileTypes
    {
        Grass, Box
    }

    class TileMap
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public readonly TileTypes[,] Tiles;


        public TileMap(int width, int height)
        {
            Width = width;
            Height = height;
            Tiles = new TileTypes[Width, Height];

            CreateMap();
        }

        public void CreateMap()
        {
            var rand = new Random();

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    if (rand.NextDouble() > .95)
                        Tiles[x, y] = TileTypes.Box;
                    else
                        Tiles[x, y] = TileTypes.Grass;
                }
            }
        }

    }
}
