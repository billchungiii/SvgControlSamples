using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SvgControlSample01
{
    public class SvgControl : SKCanvasView
    {
        public static readonly BindableProperty AspectProperty =
    BindableProperty.Create("Aspect", typeof(Aspect), typeof(SvgControl), Aspect.AspectFit);
        public Aspect Aspect
        {
            get { return (Aspect)GetValue(AspectProperty); }
            set { SetValue(AspectProperty, value); }
        }
        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            var svg = CreateSKSvg();
            ScaleCanvas(e, svg);
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

        private void ScaleCanvas(SKPaintSurfaceEventArgs e, SkiaSharp.Extended.Svg.SKSvg svg)
        {
            var rationRange = GetRatioRange(e.Info, svg);
            switch (Aspect)
            {
                case Aspect.Fill:
                    e.Surface.Canvas.Scale(rationRange.Width, rationRange.Height);
                    break;
                case Aspect.AspectFill:
                    e.Surface.Canvas.Scale(rationRange.Max);
                    break;
                default:
                    e.Surface.Canvas.Scale(rationRange.Min);
                    break;
            }
        }

        private float GetWidthScaleRatio(SKImageInfo info, SkiaSharp.Extended.Svg.SKSvg svg)
        {
            return info.Width / svg.CanvasSize.Width;
        }
        private float GetHeightScaleRatio(SKImageInfo info, SkiaSharp.Extended.Svg.SKSvg svg)
        {
            return info.Height / svg.CanvasSize.Height;
        }

        private RatioRange GetRatioRange(SKImageInfo info, SkiaSharp.Extended.Svg.SKSvg svg)
        {
            var ratioRange = new RatioRange();
            ratioRange.Width = GetWidthScaleRatio(info, svg);
            ratioRange.Height = GetHeightScaleRatio(info, svg);
            if (ratioRange.Width > ratioRange.Height)
            {
                ratioRange.Max = ratioRange.Width;
                ratioRange.Min = ratioRange.Height;
            }
            else
            {
                ratioRange.Max = ratioRange.Height;
                ratioRange.Min = ratioRange.Width;
            }
            return ratioRange;
        }
    }

    internal class RatioRange
    {
        /// <summary>
        /// 容器比較長的那一邊
        /// </summary>
        public float Max { get; set; }

        /// <summary>
        /// 容器比較短的那一邊
        /// </summary>
        public float Min { get; set; }

        /// <summary>
        /// 容器原來的寬
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// 容器原來的高
        /// </summary>
        public float Height { get; set; }
    }
}
