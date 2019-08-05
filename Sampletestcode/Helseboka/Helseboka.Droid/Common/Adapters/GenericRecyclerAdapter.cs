
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.Interop;
using Java.Lang;

namespace Helseboka.Droid.Common.Adapters
{
    public interface IUniversalAdapter
    {
        int GetItemCount();
        UniversalViewHolder CreateItemView(ViewGroup parent, int viewType);
        void BindViewHolder(UniversalViewHolder holder, int position);
        void OnItemClick(int position);
    }

    public interface IMultipleItemUniversalAdapter : IUniversalAdapter
    {
        int GetItemViewType(int position);
    }

    public class GenericRecyclerAdapter : RecyclerView.Adapter
    {
        private IUniversalAdapter adapterDelegate;

        public GenericRecyclerAdapter(IUniversalAdapter adapterDelegate)
        {
            this.adapterDelegate = adapterDelegate;
        }

        public override int ItemCount
        {
            get
            {
                if (adapterDelegate != null)
                {
                    return adapterDelegate.GetItemCount();
                }

                return 0;
            }
        }

        public override int GetItemViewType(int position)
        {
            if(adapterDelegate is IMultipleItemUniversalAdapter multiItemDelegate)
            {
                return multiItemDelegate.GetItemViewType(position);
            }
            else
            {
                return base.GetItemViewType(position);
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (adapterDelegate != null && holder is UniversalViewHolder universalViewHolder)
            {
                adapterDelegate.BindViewHolder(universalViewHolder, position);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (adapterDelegate != null)
            {
                var viewHolder = adapterDelegate.CreateItemView(parent, viewType);

                if (viewHolder != null && viewHolder.ItemView != null)
                {
                    viewHolder.ItemView.Click += (sender, e) =>
                    {
                        adapterDelegate?.OnItemClick(viewHolder.LayoutPosition);
                    };
                }

                return viewHolder;
            }

            return null;
        }
    }

    public class UniversalViewHolder : RecyclerView.ViewHolder
    {
        private Dictionary<int, View> viewList;

        public UniversalViewHolder(View itemView, Dictionary<int, View> viewList) : base(itemView)
        {
            this.viewList = viewList;

        }

        public T GetView<T>(int id) where T : class
        {
            return viewList[id] as T;
        }
    }
}
