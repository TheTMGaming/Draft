using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Top_Down_shooter.Scripts.Components
{
    class QuadTree
    {
        public Rectangle Bounds => _bounds;

        private readonly Rectangle _bounds;
        private readonly int _depth;

        private readonly List<QuadTree> _children = new List<QuadTree>();
        private List<Rectangle> _values = new List<Rectangle>();

        private readonly int _maxValuesCount;
        private readonly int _maxDepth;

        public QuadTree(Rectangle bounds, int maxValuesCount, int maxDepth, int depthNode = 0)
        {
            _bounds = bounds;
            _depth = depthNode;
            _maxValuesCount = maxValuesCount;
            _maxDepth = maxDepth;
        }

        public List<Rectangle> FindIntersectionsWith(Rectangle rect)
        {
            var intersections = new List<Rectangle>();

            if (!rect.IntersectsWith(_bounds))
                return intersections;

            intersections.AddRange(
                _values.Where(value => value.IntersectsWith(rect))
                );

            intersections.AddRange(_children
                .Where(child => rect.IntersectsWith(child._bounds))
                .SelectMany(child => child.FindIntersectionsWith(rect))
                );

            return intersections;
        }

        public List<Tuple<Rectangle, Rectangle>> FindAllIntersections()
        {
            var intersections = new List<Tuple<Rectangle, Rectangle>>();

            for (var i = 0; i < _values.Count; i++)
            {
                for (var j = 0; j < i; j++)
                {
                    if (_values[i].IntersectsWith(_values[j]))
                        intersections.Add(Tuple.Create(_values[i], _values[j]));
                }
            }

            foreach (var child in _children)
            {
                foreach (var value in _values)
                {
                    intersections.AddRange(child.FindIntersectionsWithParent(value));
                }    
            }

            intersections.AddRange(_children.SelectMany(child => child.FindAllIntersections()));

            return intersections;
        }

        public void Insert(Rectangle rect)
        {
            if (!_bounds.Contains(rect))
                throw new ArgumentException($"Value {rect} doesn't contain in bounds {_bounds}");

            if (_children.Count == 0)
            {
                if (_depth >= _maxDepth || _values.Count < _maxValuesCount)
                {
                    _values.Add(rect);

                    return;
                }

                Split();
                Insert(rect);
            }
            else
            {
                var quadrant = GetQuadrant(rect);

                if (quadrant == Quadrant.None)
                {
                    _values.Add(rect);

                    return;
                }

                _children[(int)quadrant].Insert(rect);
            }
        }

        public void Remove(Rectangle rect) => Remove(null, rect);

        public void Clear()
        {
            _values.Clear();

            foreach (var node in _children)
                node.Clear();
        }

        private List<Tuple<Rectangle, Rectangle>> FindIntersectionsWithParent(Rectangle parentValue)
        {
            var intersections = new List<Tuple<Rectangle, Rectangle>>();

            intersections.AddRange(
                _values
                    .Where(value => parentValue.IntersectsWith(value))
                    .Select(value => Tuple.Create(parentValue, value))
                    );

            intersections.AddRange(
                _children.SelectMany(child => child.FindIntersectionsWithParent(parentValue))
                );

            return intersections;
        }

        private void Remove(QuadTree parent, Rectangle rect)
        {
            if (!_bounds.Contains(rect))
                throw new ArgumentException($"Value {rect} doesn't contain in bounds {_bounds}");

            if (_children.Count == 0)
            {
                RemoveValue(rect);

                parent?.TryMergeValues();

                return;
            }

            var quadrant = GetQuadrant(rect);
            
            if (quadrant != Quadrant.None)
            {
                _children[(int)quadrant].Remove(this, rect);

                return;
            }

            RemoveValue(rect);
        }

        private void RemoveValue(Rectangle rect)
        {
            if (_values.Count == 0)
                throw new InvalidOperationException("Collection of values is empty");


            if (_values.Count > 1)
            {
                var index = _values.IndexOf(rect);

                if (index == -1)
                    throw new ArgumentException($"Value {rect} doesn't contain in tree");

                _values[index] = _values[_values.Count - 1];
            }

            _values.RemoveAt(_values.Count - 1);
        }

        private void TryMergeValues()
        {
            if (_children.Count == 0)
                throw new InvalidOperationException("Only interior nodes can be merged");

            var nValues = _values.Count + _children.Sum(n => n._values.Count);

            if (nValues <= _maxValuesCount)
            {
                _values.Capacity = nValues;

                foreach (var child in _children)
                    _values.AddRange(child._values);

                _children.Clear();
            }
        }

        private void Split()
        {
            if (_children.Count > 0)
                throw new InvalidOperationException("Only leaves can be splitted");

            var halfWidth = _bounds.Width / 2;
            var halfHeight = _bounds.Height / 2;

            for (var i = 0; i <= 1; i++)
            {
                for (var j = 0; j <= 1; j++)
                {
                    _children.Add(new QuadTree(
                        new Rectangle(_bounds.X + halfWidth * i, _bounds.Y + halfHeight * j, halfWidth, halfHeight),
                        _maxValuesCount, _maxDepth, _depth + 1));
                }
            }

            MoveValuesToChildren();
        }

        private void MoveValuesToChildren()
        {
            var newValues = new List<Rectangle>();

            foreach (var value in _values)
            {
                var quadrant = GetQuadrant(value);

                if (quadrant == Quadrant.None)
                {
                    newValues.Add(value);

                    continue;
                }

                _children[(int)quadrant]._values.Add(value);
            }

            _values = newValues;
        }

        private Quadrant GetQuadrant(Rectangle rect)
        {
            var center = new Point(
                (_bounds.X + _bounds.Width) / 2,
                (_bounds.Y + _bounds.Height) / 2);

            if (rect.Right < center.X)
            {
                if (rect.Bottom < center.Y)
                    return Quadrant.TopLeft;

                if (rect.Top >= center.Y)
                    return Quadrant.BottomLeft;

                return Quadrant.None;
            }

            if (rect.Left >= center.X)
            {
                if (rect.Bottom < center.Y)
                    return Quadrant.TopRight;

                if (rect.Top >= center.Y)
                    return Quadrant.BottomRight;

                 return Quadrant.None;
            }

            return Quadrant.None;
        }
    }
}
