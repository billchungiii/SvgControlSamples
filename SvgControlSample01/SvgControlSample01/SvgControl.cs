using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvgControlSample01
{
    public class SvgControl : SKCanvasView
    {
        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            var svg = CreateSKSvg();

            using (SKPaint paint = new SKPaint())
            {
                e.Surface.Canvas.Clear();
                e.Surface.Canvas.DrawPicture(svg.Picture, paint);
            }
        }

        private SkiaSharp.Extended.Svg.SKSvg CreateSKSvg()
        {
            string svgText = "<?xml version='1.0' encoding='utf-8'?>" +
                             "<svg xmlns='http://www.w3.org/2000/svg' height='128' width='128' viewBox='0 0 128 128'>" +
                             "<g>" +
                             "<path id='path1' transform='rotate(0,64,64) translate(26.4105767058103,0) scale(3.9988749808127,3.9988749808127)  ' Fill='#FFFFFF' " +
                             "d='M16.599986,4.4019985L3.0000018,17.004999 16.599986,27.808001z M18.8,0L18.599986,32.007996 18.599986,32.009003 0,17.205z' />" +
                             "</g>" +
                             "</svg>";

            byte[] bytes = Encoding.UTF8.GetBytes(svgText);
            MemoryStream stream = new MemoryStream(bytes);
            var svg = new SkiaSharp.Extended.Svg.SKSvg();
            svg.Load(stream);
            return svg;
        }
    }
}
