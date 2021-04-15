using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;

namespace Top_Down_shooter
{
    class Leaf
    {

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Leaf LeftChild { get; set; }
        public Leaf RightChild { get; set; }
        public Rectangle Room { get; set; }
        public List<Rectangle> Halls { get; set; }

        private readonly float aspectRation = 0.25f;
        private readonly int minLeafSize = 6;

        public Leaf(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public bool Split()
        {
            var randGenerator = new Random();

            if (!(LeftChild is null) || !(RightChild is null))
                return false;

            var isHeightSplited = randGenerator.NextDouble() > 0.5;
            if (Width > Height && Width / Height >= 1 + aspectRation)
                isHeightSplited = false;
            else if (Height > Width && Height / Width > 1 + aspectRation)
                isHeightSplited = true;

            var maxLeafSize = (isHeightSplited ? Height : Width) - minLeafSize;
            if (maxLeafSize <= minLeafSize)
                return false;

            var splitOffset = randGenerator.Next(minLeafSize, maxLeafSize);

            if (isHeightSplited)
            {
                RightChild = new Leaf(X, Y, Width, splitOffset);
                LeftChild = new Leaf(X, Y + splitOffset, Width, Height - splitOffset);
            }
            else
            {
                RightChild = new Leaf(X, Y, splitOffset, Height);
                LeftChild = new Leaf(X + splitOffset, Y, Width - splitOffset, Height);
            }

            return true;
        }
    }
}
