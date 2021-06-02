using System.Drawing;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.Renders
{
    interface IAnimationRender : IRender
    {
        void Draw(D2DGraphicsDevice device, Point startSlice, Size sizeSlice);

        void PlayAnimation();

        void ChangeTypeAnimation();
    }
}
