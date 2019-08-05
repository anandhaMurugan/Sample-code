
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Helseboka.Droid.Common.Utils;

namespace Helseboka.Droid.Common.Views
{
    public class MaxHeightRecyclerView : RecyclerView
    {
        private Context context;
        private int maxHeight;

        public MaxHeightRecyclerView(Context context) :
            base(context)
        {
            Initialize(context);
        }

        public MaxHeightRecyclerView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize(context, attrs);
        }

        public MaxHeightRecyclerView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize(context, attrs);
        }

        void Initialize(Context context, IAttributeSet attrs = null)
        {
            this.context = context;
            maxHeight = -1;
            if(attrs != null)
            {
                var array = context.ObtainStyledAttributes(attrs, Resource.Styleable.MaxHeightRecyclerView);
                maxHeight = array.GetLayoutDimension(Resource.Styleable.MaxHeightRecyclerView_maxHeight, maxHeight);
                array.Recycle();
            }
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            if(maxHeight > 0)
            {
                heightMeasureSpec = MeasureSpec.MakeMeasureSpec(maxHeight, MeasureSpecMode.AtMost);
            }

            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
        }
    }
}
