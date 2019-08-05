
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Helseboka.Droid.Common.Views
{
    public class RoundRelativeLayout : RelativeLayout
    {
        private RectF rect;
        private Paint fillPaint;
        private Paint strokePaint;

        protected RoundRelativeLayout(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public RoundRelativeLayout(Context context) :
            base(context)
        {
            Initialize();
        }

        public RoundRelativeLayout(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public RoundRelativeLayout(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        void Initialize()
        {
            

            fillPaint = new Paint();
            fillPaint.Color = Color.ParseColor("#E7FFFF");
            fillPaint.SetStyle(Paint.Style.Fill);

            strokePaint = new Paint();
            strokePaint.Color = Color.ParseColor("#E2E2E2");
            strokePaint.StrokeWidth = 1;

            strokePaint.SetShadowLayer(20, 0, 2, Color.ParseColor("#80D2D2D2"));
                
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            rect = new RectF(0.0f, 0.0f, Width, Height);
            canvas.DrawRoundRect(rect, 20, 20, fillPaint);
            canvas.DrawRoundRect(rect, 20, 20, strokePaint);
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            int parentWidth = MeasureSpec.GetSize(widthMeasureSpec);
            int parentHeight = MeasureSpec.GetSize(heightMeasureSpec);
            this.SetMeasuredDimension(parentWidth / 2, parentHeight);
        }
    }
}
