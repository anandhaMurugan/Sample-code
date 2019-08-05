using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace Helseboka.iOS.Common.TableViewDelegates
{
    public abstract class BaseTableViewSource<T> : UITableViewSource where T : class, new()
    {
        public List<T> DataList { get; protected set; } = new List<T>();

        public event EventHandler<UIScrollView> DidScroll;
        public event EventHandler<T> DidSelect;

        public void UpdateList(List<T> newData, bool isReverse = false)
        {
            if (isReverse)
            {
                DataList.InsertRange(0, newData);
            }
            else
            {
                DataList.AddRange(newData);
            }

        }

        public void Clear()
        {
            DataList.Clear();
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return DataList.Count;
        }

        public override void Scrolled(UIScrollView scrollView)
        {
            if (DataList != null && DataList.Count > 0)
            {
                DidScroll?.Invoke(this, scrollView);
            }
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var selectedDoctor = DataList[indexPath.Row];
            DidSelect?.Invoke(this, selectedDoctor);
        }
    }
}
