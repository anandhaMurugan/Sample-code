using System;
using Android.Content;
using Android.Util;

namespace Helseboka.Droid.Common.Utils
{
    public static class IntExtension
    {
        public static int ConvertToPixel(this int dp, Context context)
        {
            int pixel = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, context.Resources.DisplayMetrics);
            return pixel;
        }
    }
}
