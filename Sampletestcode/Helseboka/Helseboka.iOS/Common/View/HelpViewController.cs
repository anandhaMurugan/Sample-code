using System;
using System.Collections.Generic;
using Foundation;
using Helseboka.Core.Common.Model;
using Helseboka.iOS.Common.TableViewCell;
using UIKit;
using Helseboka.iOS.Common.Extension;
using Helseboka.Core.Common.Extension;
using System.Linq;
using Helseboka.Core.Common.EnumDefinitions;
using System.Threading.Tasks;
using Helseboka.Core.HelpAndFAQ.Model;
using SafariServices;

namespace Helseboka.iOS.Common.View
{
    public class HelpViewController : BaseView, IUITableViewDataSource, IUITableViewDelegate
    {
        private BaseTableView tableView;
        private List<HelpFAQDataModel> helpDataList;
        private UIImage topImage;
        private String topText;
        private HelpFAQType helpType;
        private int extraRow = 0;

        public HelpViewController(HelpFAQType helpType, UIImage topImage = null, String topText = null) : base()
        {
            this.topImage = topImage;
            this.topText = topText;
            this.helpType = helpType;

            if (topImage != null)
            {
                extraRow++;
            }

            if (topText != null)
            {
                extraRow++;
            }
        }

        public HelpViewController(IntPtr handler) : base(handler) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;
            tableView = new BaseTableView();
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            tableView.TableFooterView = new UIView();
            tableView.RowHeight = UITableView.AutomaticDimension;
            tableView.EstimatedRowHeight = 500;
            View.AddSubview(tableView);

            tableView.AllEdgesToSuperView();

            View.TranslatesAutoresizingMaskIntoConstraints = true;

            tableView.DataSource = this;
            tableView.Delegate = this;

            LoadData().Forget();
        }

        private async Task LoadData()
        {
            tableView.ShowLoader();
            var dataList = await new HelpFAQManager().GetHelpFAQList(helpType);
            tableView.HideLoader();
            if (dataList != null)
            {
                helpDataList = dataList;
                tableView.ReloadData();
            }
        }

        public nint RowsInSection(UITableView tableView, nint section)
        {            
            int rowCount = helpDataList != null ? helpDataList.Count : 0;

            rowCount += extraRow;

            return rowCount;
        }

        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath.Row == 0)
            {
                if (topImage != null)
                {
                    var imageCell = (HelpTableViewImageCell)tableView.DequeueReusableCell(HelpTableViewImageCell.Key);
                    if (imageCell == null)
                    {
                        imageCell = HelpTableViewImageCell.Create();
                    }
                    imageCell.Configure(topImage);
                    return imageCell;
                }
                else if (topText != null)
                {
                    var labelCell = (TableViewLabelCell)tableView.DequeueReusableCell(TableViewLabelCell.Key);
                    if (labelCell == null)
                    {
                        labelCell = TableViewLabelCell.Create();
                    }
                    labelCell.Configure(topText);
                    return labelCell;
                }
            }
            else if (indexPath.Row == 1)
            {
                if (topImage != null && topText != null)
                {
                    var labelCell = (TableViewLabelCell)tableView.DequeueReusableCell(TableViewLabelCell.Key);
                    if (labelCell == null)
                    {
                        labelCell = TableViewLabelCell.Create();
                    }
                    labelCell.Configure(topText);
                    return labelCell;
                }
            }

            var helperData = helpDataList[indexPath.Row - extraRow];

            var cell = (HelpTableViewCell)tableView.DequeueReusableCell(HelpTableViewCell.Key);
            if (cell == null)
            {
                cell = HelpTableViewCell.Create();
            }
            cell.Configure(helperData.Title, helperData.Description, Link_Tapped);
            if (helperData.IsExpanded)
            {
                cell.Expand();
            }
            else
            {
                cell.Collapse();
            }

            return cell;
        }

        [Export("tableView:didSelectRowAtIndexPath:")]
        public void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath.Row == 0 && (topImage != null || !String.IsNullOrEmpty(topText)))
            {
                return;
            }

            if (indexPath.Row == 1 && topImage != null && !String.IsNullOrEmpty(topText))
            {
                return;
            }

            var row = indexPath.Row - extraRow;

            if (helpDataList != null && helpDataList.Count > row)
            {
                helpDataList[row].IsExpanded = !helpDataList[row].IsExpanded;
                tableView.ReloadRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Automatic);
            }
        }

        void Link_Tapped(NSUrl obj)
        {
            var safari = new SFSafariViewController(obj);
            PresentViewController(safari, true, null);
        }
    }
}

