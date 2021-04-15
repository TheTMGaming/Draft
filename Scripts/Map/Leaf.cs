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
        private readonly int indent = 3;
        private readonly Random randGenerator;

        public Leaf(int x, int y, int width, int height, int minLeafSize)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            this.minLeafSize = minLeafSize;
            randGenerator = new Random();
        }

        public bool Split()
        {
            if (!(LeftChild is null) || !(RightChild is null))
                return false;

            var isHeightSplited = randGenerator.NextDouble() > 0.5;
            if (Width > Height && Width / Height >= 1 + aspectRation)
                isHeightSplited = false;
            else if (Height > Width && Height / Width >= 1 + aspectRation)
                isHeightSplited = true;

            var maxLeafSize = (isHeightSplited ? Height : Width) - minLeafSize;
            if (maxLeafSize <= minLeafSize)
                return false;

            var splitOffset = randGenerator.Next(minLeafSize, maxLeafSize);

            if (isHeightSplited)
            {
                RightChild = new Leaf(X, Y, Width, splitOffset, minLeafSize);
                LeftChild = new Leaf(X, Y + splitOffset, Width, Height - splitOffset, minLeafSize);
            }
            else
            {
                RightChild = new Leaf(X, Y, splitOffset, Height, minLeafSize);
                LeftChild = new Leaf(X + splitOffset, Y, Width - splitOffset, Height, minLeafSize);
            }

            return true;
        }

        public void CreateRooms()
        {
            if (!(LeftChild is null) || !(RightChild is null))
            {
                if (!(LeftChild is null)) LeftChild.CreateRooms();
                if (!(RightChild is null)) RightChild.CreateRooms();
            }
            else
            {
                var roomSize = new Size(
                    randGenerator.Next(1, Width - 1),
                    randGenerator.Next(1, Height - 1)
                    );
                var roomPos = new Point(
                    randGenerator.Next(1, Width - roomSize.Width - 1),
                    randGenerator.Next(1, Height - roomSize.Height - 1)
                    );

                Room = new Rectangle(X + roomPos.X, Y + roomPos.Y, roomSize.Width, roomSize.Height);
            }
            
        }
    }
}
