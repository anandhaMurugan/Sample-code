using System;
using Android.Content;
using Android.Views;

namespace Helseboka.Droid.Common.Utils
{
    public static class ViewExtension
    {
        public static void SetMargin(this View view, Context context, int left, int top, int right, int bottom)
        {
            if (view.LayoutParameters is ViewGroup.MarginLayoutParams layoutParams)
            {
                left = left.ConvertToPixel(context);
                top = top.ConvertToPixel(context);
                right = right.ConvertToPixel(context);
                bottom = bottom.ConvertToPixel(context);

                layoutParams.SetMargins(left, top, right, bottom);
            }
        }
    }
}
