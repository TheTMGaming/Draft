using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using unvell.D2DLib;

namespace Top_Down_shooter.Scripts.Source
{
    class D2DGraphicsDevice
    {
        public D2DGraphics Graphics { get; }

        private D2DDevice D2DDevice { get; }

        private Form Form { get; }

        private readonly Dictionary<Bitmap, D2DBitmap> cache = new Dictionary<Bitmap, D2DBitmap>();

        public D2DGraphicsDevice(Form form)
        {
            Form = form;
            D2DDevice = D2DDevice.FromHwnd(Form.Handle);

            D2DDevice.Resize();
            Form.Resize += (sender, args) => D2DDevice.Resize();
            Form.HandleDestroyed += (sender, args) => D2DDevice.Dispose();
            Graphics = new D2DGraphics(D2DDevice);
        }

        public D2DBitmap CreateBitmap(Bitmap bitmap)
        {
            if (cache.TryGetValue(bitmap, out var result))
                return result;

            return cache[bitmap] = D2DDevice.CreateBitmapFromGDIBitmap(bitmap);
        }

        public void BeginRender()
        {
            Graphics.BeginRender(D2DColor.FromGDIColor(Form.BackColor));
        }

        public void EndRender()
        {
            Graphics.EndRender();
        }
    }
}
