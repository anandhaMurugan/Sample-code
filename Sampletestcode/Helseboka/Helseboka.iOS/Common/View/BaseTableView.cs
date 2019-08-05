using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace Helseboka.iOS.Common.View
{
    [Register("BaseTableView"), DesignTimeVisible(true)]
    public class BaseTableView : UITableView
    {
        public UIActivityIndicatorView ActivityIndicator { get; set; }
        public bool IsLoading { get; set; } = false;

        public BaseTableView() : base()
        {
            CommontInitialization();
        }

        public BaseTableView(IntPtr handler) : base(handler)
        {
            
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            CommontInitialization();
        }

        public void AddPullToRefresh(Func<Task> refreshAction)
        {
            RefreshControl = new UIRefreshControl();
            RefreshControl.ValueChanged += async (sender, e) => {
                if (refreshAction != null)
                {
                    if(!IsLoading)
                    {
                        RefreshControl.BeginRefreshing();
                        await refreshAction();
                        RefreshControl.EndRefreshing();
                    }
                }
            };
            this.Add(RefreshControl);
        }

        protected void CommontInitialization()
        {
            ActivityIndicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray);
            BackgroundView = ActivityIndicator;
        }

        public void ShowLoader()
        {
            ActivityIndicator.StartAnimating();
        }

        public void HideLoader()
        {
            ActivityIndicator.StopAnimating();
        }
    }
}
