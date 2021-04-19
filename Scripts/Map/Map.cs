using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Top_Down_shooter
{
    class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }

        //private readonly int maxLeafSize = 20;
        private readonly int minLeafSize = 20;

        private Queue<Leaf> leaves = new Queue<Leaf>();

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public IEnumerable<Leaf> CreateRandomMap()
        {
            var randGenerator = new Random();
            
            var root = new Leaf(0, 0, Width, Height, minLeafSize);
            leaves.Enqueue(root);

            while (leaves.Count > 0)
            {
                var leaf = leaves.Dequeue();

                if (leaf.Width > minLeafSize && leaf.Height > minLeafSize)
                {
                    if (leaf.Split())
                    {
                        leaves.Enqueue(leaf.RightChild);
                        leaves.Enqueue(leaf.LeftChild);
                    }
                    
                }

                yield return leaf;
            }

            //root.CreateRooms();
        }
    }
}
