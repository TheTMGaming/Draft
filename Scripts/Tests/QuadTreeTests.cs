using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Top_Down_shooter.Scripts.Components;

namespace Top_Down_shooter.Scripts.Tests
{
    [TestFixture]
    class QuadTreeTests
    {
        private QuadTree _tree;

        private readonly Rectangle _bounds = new Rectangle(0, 0, 200, 200);
        private readonly int _maxValuesCount = 10;
        private readonly int _maxDepth = 4;

        private readonly Random _randGenerator = new Random();

        [SetUp]
        public void Initialize()
        {
            _tree = new QuadTree(_bounds, _maxValuesCount, _maxDepth);
        }

        [Test]
        public void FindAllIntersecttions(int countObjects)
        {
            var rects = new List<Rectangle>();

            for (int i = 0; i < countObjects; i++)
            {
                var x = _randGenerator.Next(0, 201);
                var y = _randGenerator.Next(0, 201);

                var width = _randGenerator.Next(0, 201 - x);
                var height = _randGenerator.Next(0, 201 - y);

                var rect = new Rectangle(x, y, width, height);
                rects.Add(rect);

                _tree.Insert(rect);
            }

            var excepted = LinearSearchCollisions(rects);
            var actual = _tree.FindAllIntersections();

            Assert.AreEqual(excepted.Count, actual.Count);
        }

        private List<Tuple<Rectangle, Rectangle>> LinearSearchCollisions(List<Rectangle> rects)
        {
            var pairs = new List<Tuple<Rectangle, Rectangle>>();

            for (var i = 0; i < rects.Count; i++)
            {
                for (var j = 0; j < i; j++)
                {
                    if (rects[i].IntersectsWith(rects[j]))
                        pairs.Add(Tuple.Create(rects[i], rects[j]));
                }
            }

            return pairs;
        }
    }
}
