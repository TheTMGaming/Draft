using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.Renders
{
    class EnemiesRender : IAnimationRender
    {
        public int X { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Y { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Size Size => throw new NotImplementedException();

        private readonly List<CharacterRender> renders = new List<CharacterRender>();
        private readonly List<Enemy> enemies;
        private readonly Bitmap atlas;

        public EnemiesRender(List<Enemy> enemies, Bitmap atlas)
        {
            this.enemies = enemies;
            this.atlas = atlas;

            foreach (var enemy in enemies)
            {
                renders.Add(new CharacterRender(enemy, atlas, 4, 2));
            }
        }

        public void Draw(D2DGraphicsDevice device)
        {
            Update();
            foreach (var render in renders)
                render.Draw(device);
        }

        public void Draw(D2DGraphicsDevice device, Point startSlice, Size sizeSlice)
        {
            throw new NotImplementedException();
        }

        public void PlayAnimation()
        {
            foreach (var render in renders)
                render.PlayAnimation();
        }

        public void ChangeTypeAnimation()
        {
            foreach (var render in renders)
                render.ChangeTypeAnimation();
        }

        public void Update()
        {
            if (enemies.Count == renders.Count)
                return;

            renders.Add(new CharacterRender(enemies.Last(), atlas, 4, 2));
        }
    }
}
