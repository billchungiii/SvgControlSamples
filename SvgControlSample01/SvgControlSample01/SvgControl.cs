using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SvgControlSample01
{
    public class SvgControl : SKCanvasView
    {
        public static readonly BindableProperty EmbeddedResourceProperty =
  BindableProperty.Create("EmbeddedResource", typeof(string), typeof(SvgControl), default(string));
        public string EmbeddedResource
        {
            get { return (string)GetValue(EmbeddedResourceProperty); }
            set { SetValue(EmbeddedResourceProperty, value); }
        }

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
    var stream = GetEmbeddedResourceStream();
    var svg = new SkiaSharp.Extended.Svg.SKSvg();
    svg.Load(stream);
    return svg;
}

        private Stream GetEmbeddedResourceStream()
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var resourceNames = assembly.GetManifestResourceNames();
            var resourcePaths = resourceNames.FirstOrDefault((x) => x == EmbeddedResource);
            if (resourcePaths == null)
            {
                throw new Exception(string.Format("Embedde resource {0} not found.", EmbeddedResource));
            }

            return assembly.GetManifestResourceStream(resourcePaths);
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
