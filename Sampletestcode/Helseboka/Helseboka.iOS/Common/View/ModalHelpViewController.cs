using System;
using System.Collections.Generic;
using Foundation;
using Helseboka.Core.Common.Model;
using Helseboka.iOS.Common.TableViewCell;
using UIKit;
using Helseboka.iOS.Common.Extension;
using Helseboka.Core.Common.Extension;
using System.Linq;
using System.Threading.Tasks;
using Helseboka.Core.HelpAndFAQ.Model;
using Helseboka.Core.Common.EnumDefinitions;
using SafariServices;

namespace Helseboka.iOS.Common.View
{
    public partial class ModalHelpViewController : BaseModalViewController, IUITableViewDataSource, IUITableViewDelegate
    {
        private BaseTableView tableView;
        private UIButton closeButton;
        private List<HelpFAQDataModel> helpDataList = new List<HelpFAQDataModel>();
        private HelpFAQType helpType;

        public ModalHelpViewController(HelpFAQType helpType) : base()
        {
            this.helpType = helpType;
        }

        public ModalHelpViewController(IntPtr handler) : base(handler) { }



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

            closeButton = new UIButton(UIButtonType.Custom);
            closeButton.SetImage(UIImage.FromBundle("Modal-close-icon"), UIControlState.Normal);
            closeButton.ContentMode = UIViewContentMode.ScaleAspectFit;
            closeButton.TouchUpInside += CloseButton_TouchUpInside;
            View.AddSubview(closeButton);

            closeButton.TrailToSuperView(-20, true);
            closeButton.TopToSuperView(15, true);

            closeButton.HeightEquals(35);
            closeButton.WidthEquals(35);

            tableView.TopToBottom(closeButton, 20);
            tableView.EdgesToSuperview(30, true);
            tableView.BottomToSuperView();

            helpDataList.ForEach((obj) => obj.IsExpanded = true);

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

        private void CloseButton_TouchUpInside(object sender, EventArgs e)
        {
            Close();
        }

        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var helperData = helpDataList[indexPath.Row];
            var cell = (ModalHelpTableViewCell)tableView.DequeueReusableCell(ModalHelpTableViewCell.Key);
            if (cell == null)
            {
                cell = ModalHelpTableViewCell.Create();
            }
            cell.Configure(helperData.Title, helperData.Description, Link_Tapped);
            return cell;
        }

        public nint RowsInSection(UITableView tableView, nint section)
        {
            return helpDataList != null ? helpDataList.Count : 0;
        }

        [Export("tableView:heightForRowAtIndexPath:")]
        public nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return UITableView.AutomaticDimension;
        }

        void Link_Tapped(NSUrl obj)
        {
            var safari = new SFSafariViewController(obj);
            PresentViewController(safari, true, null);
        }

    }
}
