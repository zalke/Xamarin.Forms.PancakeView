using System;
using Android.Graphics;
using Android.OS;
using Android.Views;

namespace Xamarin.Forms.PancakeView.Droid
{
    public class RoundedCornerOutlineProvider : ViewOutlineProvider
    {
        private readonly PancakeView _pancake;
        private readonly Func<double, float> _convertToPixels;

        public RoundedCornerOutlineProvider(PancakeView pancake, Func<double, float> convertToPixels)
        {
            _pancake = pancake;
            _convertToPixels = convertToPixels;
        }

        [Obsolete]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
        public override void GetOutline(global::Android.Views.View view, Outline outline)
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
        {
            try
            {
                if (_pancake.Sides != 4)
                {
                    var hexPath = DrawingExtensions.CreatePolygonPath(view.Width, view.Height, _pancake.Sides, _pancake.Shadow != null ? 0 : _pancake.CornerRadius.TopLeft, _pancake.OffsetAngle);
                    if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
                    {
                        outline.SetPath(hexPath);
                    }
                    else if (hexPath.IsConvex)
                    {
                        outline.SetConvexPath(hexPath);
                    }
                }
                else
                {
                    var path = DrawingExtensions.CreateRoundedRectPath(view.Width, view.Height,
                        _convertToPixels(_pancake.CornerRadius.TopLeft),
                        _convertToPixels(_pancake.CornerRadius.TopRight),
                        _convertToPixels(_pancake.CornerRadius.BottomRight),
                        _convertToPixels(_pancake.CornerRadius.BottomLeft));
                    if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
                    {
                        outline.SetPath(path);
                    }
                    else if (path.IsConvex)
                    {
                        outline.SetConvexPath(path);
                    }
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Warning(ex, "unable to get outline");
            }
        }
    }
}