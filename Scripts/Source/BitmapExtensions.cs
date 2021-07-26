using System.Drawing;

namespace Top_Down_shooter
{
    static class BitmapExtensions
    {
        public static Bitmap Extract(this Bitmap source, Rectangle section)
        {
            var bmp = new Bitmap(section.Width, section.Height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);
            }
            return bmp;
        }

        public static Bitmap Blackout(this Bitmap source, float percent)
        {
            var bmp = new Bitmap(source);

            if ((int)(255 * percent) > -1 && (int)(255 * percent) < 256)
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    var darken = Color.FromArgb((int)(255 * percent), Color.Black);

                    using (var brush = new SolidBrush(darken))
                        g.FillRectangle(brush, new Rectangle(0, 0, bmp.Width, bmp.Height));
                }
            }

            return bmp;
        }
    }
}
