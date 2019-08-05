using System;
using Android.Support.V7.Widget;

namespace Helseboka.Droid.Common.Listners
{
    public class LoadMoreEventArgs : EventArgs
    {
        public int Page { get; set; }
        public int TotalItemsCount { get; set; }
    }

    public class LastItemVisibleEventArgs : EventArgs
    {
        public bool IsLastItemVisible { get; set; }
    }

    public class RecyclerViewReverseScrollListener : RecyclerView.OnScrollListener
    {
        // The minimum amount of items to have below your current scroll position
        // before loading more.
        private int visibleThreshold = 5;
        private LinearLayoutManager mLayoutManager;

        public event EventHandler<LoadMoreEventArgs> LoadMore;

        public RecyclerViewReverseScrollListener(LinearLayoutManager layoutManager, int visibleThreshold = 5)
        {
            this.mLayoutManager = layoutManager;
            this.visibleThreshold = visibleThreshold;
        }

        // This happens many times a second during a scroll, so be wary of the code you place here.
        // We are given a few useful parameters to help us work out if we need to load some more data,
        // but first we check if we are waiting for the previous load to finish.
        public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        {
            var lastVisibleItemPosition = mLayoutManager.FindFirstVisibleItemPosition();

            if (lastVisibleItemPosition < visibleThreshold)
            {
                LoadMore?.Invoke(this, new LoadMoreEventArgs { Page = lastVisibleItemPosition, TotalItemsCount = 0 });
            }
        }


    }

    public class RecyclerViewScrollListener : RecyclerView.OnScrollListener
    {
        // The minimum amount of items to have below your current scroll position
        // before loading more.
        private int visibleThreshold = 5;
        // The current offset index of data you have loaded
        private int currentPage = 0;
        // The total number of items in the dataset after the last load
        private int previousTotalItemCount = 0;
        // True if we are still waiting for the last set of data to load.
        private bool loading = true;
        // Sets the starting page index
        private int startingPageIndex = 0;
        private RecyclerView.LayoutManager mLayoutManager;

        public event EventHandler<LoadMoreEventArgs> LoadMore;
        public event EventHandler<LastItemVisibleEventArgs> LastItemVisibleEvent;

        public RecyclerViewScrollListener(LinearLayoutManager layoutManager, int visibleThreshold = 5)
        {
            this.mLayoutManager = layoutManager;
            this.visibleThreshold = visibleThreshold;
        }

        public RecyclerViewScrollListener(GridLayoutManager layoutManager)
        {
            this.mLayoutManager = layoutManager;
            visibleThreshold = visibleThreshold * layoutManager.SpanCount;
        }

        public RecyclerViewScrollListener(StaggeredGridLayoutManager layoutManager)
        {
            this.mLayoutManager = layoutManager;
            visibleThreshold = visibleThreshold * layoutManager.SpanCount;
        }

        public int GetLastVisibleItem(int[] lastVisibleItemPositions)
        {
            int maxSize = 0;
            for (int i = 0; i < lastVisibleItemPositions.Length; i++)
            {
                if (i == 0)
                {
                    maxSize = lastVisibleItemPositions[i];
                }
                else if (lastVisibleItemPositions[i] > maxSize)
                {
                    maxSize = lastVisibleItemPositions[i];
                }
            }
            return maxSize;
        }

        // This happens many times a second during a scroll, so be wary of the code you place here.
        // We are given a few useful parameters to help us work out if we need to load some more data,
        // but first we check if we are waiting for the previous load to finish.
        public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        {
            int lastVisibleItemPosition = 0;
            int totalItemCount = mLayoutManager.ItemCount;

            if (mLayoutManager is StaggeredGridLayoutManager staggeredGridLayoutManager) {
                int[] lastVisibleItemPositions = staggeredGridLayoutManager.FindLastVisibleItemPositions(null);
                // get maximum element within the list
                lastVisibleItemPosition = GetLastVisibleItem(lastVisibleItemPositions);
            } else if (mLayoutManager is GridLayoutManager gridLayoutManager) {
                lastVisibleItemPosition = gridLayoutManager.FindLastVisibleItemPosition();
            } else if (mLayoutManager is LinearLayoutManager linearLayoutManager) {
                lastVisibleItemPosition = linearLayoutManager.FindLastVisibleItemPosition();
            }

            // If the total item count is zero and the previous isn't, assume the
            // list is invalidated and should be reset back to initial state
            if (totalItemCount < previousTotalItemCount)
            {
                this.currentPage = this.startingPageIndex;
                this.previousTotalItemCount = totalItemCount;
                if (totalItemCount == 0)
                {
                    this.loading = true;
                }
            }
            // If it’s still loading, we check to see if the dataset count has
            // changed, if so we conclude it has finished loading and update the current page
            // number and total item count.
            if (loading && (totalItemCount > previousTotalItemCount))
            {
                loading = false;
                previousTotalItemCount = totalItemCount;
            }

            // If it isn’t currently loading, we check to see if we have breached
            // the visibleThreshold and need to reload more data.
            // If we do need to reload some more data, we execute onLoadMore to fetch the data.
            // threshold should reflect how many total columns there are too
            if (!loading && (lastVisibleItemPosition + visibleThreshold) > totalItemCount)
            {
                currentPage++;
                LoadMore?.Invoke(this, new LoadMoreEventArgs { Page = currentPage, TotalItemsCount = totalItemCount });
                loading = true;
            }

            if(lastVisibleItemPosition == totalItemCount - 1)
            {
                LastItemVisibleEvent?.Invoke(this, new LastItemVisibleEventArgs { IsLastItemVisible = true });
            }
            else
            {
                LastItemVisibleEvent?.Invoke(this, new LastItemVisibleEventArgs { IsLastItemVisible = false });
            }
        }

        // Call this method whenever performing new searches
        public void resetState()
        {
            this.currentPage = this.startingPageIndex;
            this.previousTotalItemCount = 0;
            this.loading = true;
        }

    }
}
