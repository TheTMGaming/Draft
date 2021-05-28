using System.Drawing;

namespace Top_Down_shooter.Scripts.Renders
{
    interface IAnimationRender : IRender
    {
        void Draw(Graphics g, Point startSlice, Size sizeSlice);

        void PlayAnimation();

        void ChangeTypeAnimation();
    }
}
