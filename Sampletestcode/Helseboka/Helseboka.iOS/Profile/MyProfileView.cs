using System;
using System.Collections.Generic;
using Foundation;
using Helseboka.Core.Profile.Interface;
using Helseboka.iOS.Common.View;
using Helseboka.iOS.Profil.View.TableViewCell;
using MyTest.TableViewCellModel;
using UIKit;

namespace Helseboka.iOS.Profil.View
{
    public partial class MyProfileView : BaseView, IUITableViewDelegate, IUITableViewDataSource
    {
        public static readonly String Identifier = "MyProfileView";
        public IProfilePresenter Presenter
        {
            get => presenter as IProfilePresenter;
            set => presenter = value;
        }

        List<UserTableCellModel> userTableCellModels;

        public MyProfileView() { }

        protected MyProfileView(IntPtr handle) : base(handle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            LoadTableData();

            userDetailsTableView.DataSource = this;
            userDetailsTableView.Delegate = this;
            userDetailsTableView.RowHeight = UITableView.AutomaticDimension;
            userDetailsTableView.EstimatedRowHeight = 40f;
            userDetailsTableView.ReloadData();

            // Remove unnecessary rows at last .......
            userDetailsTableView.TableFooterView = new UIView(System.Drawing.Rectangle.Empty);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private void LoadTableData()
        {
            userTableCellModels = new List<UserTableCellModel>
            {
                new UserTableCellModel("Dine personlige innstillinger","",true,false),

                new UserTableCellModel ("Legekontor","Gildheim Legesenter",true,true),

                new UserTableCellModel ("Lege","Anne Jacobsen",true,true),

                new UserTableCellModel ("PIN-kode/Touch ID","Endre",false,false,false,true),

                new UserTableCellModel ("Vilkår/GDPR","",true,false),

                new UserTableCellModel ("Logg ut","",false,false,true)

            };
        }

        #region Table view Delegates
        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = (UserProfileTableViewCell)tableView.DequeueReusableCell("UserProfileTableCell", indexPath);
            var cellData = userTableCellModels[indexPath.Row];
            cell.UpdateCell(cellData);
            return cell;
        }

        public nint RowsInSection(UITableView tableview, nint section)
        {
            return userTableCellModels.Count;
        }

        [Export("tableView:heightForRowAtIndexPath:")]
        public nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath.Row == 0 || indexPath.Row == 1 || indexPath.Row == 3)
            {
                return 100;
            }
            else
            {
                return 60;
            }
        }

        [Export("tableView:didSelectRowAtIndexPath:")]
        public void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath.Row == 0)   //LegeDetailsViewController
            {
                var story = UIStoryboard.FromName("Main", null);
                var controller = story.InstantiateViewController("UserInfoViewController");
                PresentViewController(controller, true, null);
            }

            if (indexPath.Row == 1 || indexPath.Row == 2)   //LegeDetailsViewController
            {
                var story = UIStoryboard.FromName("Main", null);
                var controller = story.InstantiateViewController("LegeDetailsViewController");
                PresentViewController(controller, true, null);
            }
        }
        #endregion
    }
}

