using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using Helseboka.Core.Profile.Interface;
using Helseboka.Core.Common.Extension;
using Helseboka.iOS.Common.View;
using Helseboka.iOS.Common.Extension;
using Helseboka.iOS.Profile.View.TableViewCell;
using MyTest.TableViewCellModel;
using UIKit;
using Helseboka.Core.Profile.Model;
using System.Linq;
using Helseboka.iOS.Common.Constant;

namespace Helseboka.iOS.Profile.View
{
    public interface IUserDataSourceDelegate
    {
        void ErrorTextLogics(bool emptyCheck, bool regexCheck);
    }

    public partial class UserInfoViewController : BaseView, IUITableViewDelegate, IUITableViewDataSource, IUserDataSourceDelegate
    {
        private User currentUser;
        private NSObject keyBoardWillShow;
        private NSObject keyBoardWillHide;
        public static readonly String Identifier = "UserInfoViewController";
        private bool isKeyboardVisible = false;

        public IProfilePresenter Presenter
        {
            get => presenter as IProfilePresenter;
            set => presenter = value;
        }

        public UserInfoViewController() { }

        protected UserInfoViewController(IntPtr handle) : base(handle) { }

		List<UserPersonalInfoCellModel> UserInfoCellModel;

        public override void ViewDidLoad()
		{
			base.ViewDidLoad();
            LoadTableData().Forget();

            UserInfoTableView.DataSource = this;
			UserInfoTableView.Delegate = this;
			UserInfoTableView.RowHeight = UITableView.AutomaticDimension;
			UserInfoTableView.EstimatedRowHeight = 40f;
		}
		
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            keyBoardWillShow = UIKeyboard.Notifications.ObserveWillShow(KeyboardWillShow);
            keyBoardWillHide = UIKeyboard.Notifications.ObserveWillHide(KeyboardWillHide);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            keyBoardWillShow.Dispose();
            keyBoardWillHide.Dispose();
        }

        private void KeyboardWillShow(object sender, UIKeyboardEventArgs args)
        {
            isKeyboardVisible = true;
            float bottompadding = 0;
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                bottompadding = (float)View.SafeAreaInsets.Bottom;
            }

            var current = UserInfoTableView.IndexPathsForVisibleRows.Last();

            UIView.Animate(args.AnimationDuration, 0, args.GetAnimationOptions(), () =>
            {
                SaveButtonBottomConstraint.Constant = -(args.GetKeyboardHeight() - bottompadding);
                View.LayoutIfNeeded();
            }, () => UserInfoTableView.ScrollToRow(NSIndexPath.FromRowSection(5, 0), UITableViewScrollPosition.Bottom, true));
        }

        private void KeyboardWillHide(object sender, UIKeyboardEventArgs args)
        {
            isKeyboardVisible = false;
            UIView.Animate(args.AnimationDuration, 0, args.GetAnimationOptions(), () =>
            {
                SaveButtonBottomConstraint.Constant = -10;
                View.LayoutIfNeeded();
            }, null);
        }

        private async Task LoadTableData()
		{
            currentUser = await Presenter.GetCurrentUserProfile();
            if (currentUser != null)
            {
                NameLabel.Text = $"{currentUser.FirstName} {currentUser.LastName}";
                UserInfoCellModel = new List<UserPersonalInfoCellModel>
                {
                    new UserPersonalInfoCellModel ("Profile.PersonalSettings.DOB".Translate(), currentUser.DateOfBirth),
                    new UserPersonalInfoCellModel ("Profile.PersonalSettings.Gender".Translate(), currentUser.Gender),
                    new UserPersonalInfoCellModel ("Profile.PersonalSettings.Address".Translate(), currentUser.Address, true),
                    new UserPersonalInfoCellModel ("Profile.PersonalSettings.Telefone".Translate(), currentUser.Phone, true, UIKeyboardType.PhonePad),
                    new UserPersonalInfoCellModel ("Profile.PersonalSettings.ErrorMessage".Translate(), "", false, UIKeyboardType.Default, Colors.MyProfileDeleteProfileColor),
                    new UserPersonalInfoCellModel ("Profile.PersonalSettings.ErrorMessageText".Translate(), "", false, UIKeyboardType.Default, Colors.MyProfileDeleteProfileColor)
                };
                UserInfoTableView.ReloadData();
            }
		}

		#region Table view Delegates
		public nint RowsInSection(UITableView tableView, nint section)
		{
            return UserInfoCellModel != null ? UserInfoCellModel.Count : 0;
		}

		public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
            var cell = tableView.DequeueReusableCell(UserPersonalInfoTableCell.Key, indexPath) as UserPersonalInfoTableCell;
            cell.Delegate = this;
            var cellData = UserInfoCellModel[indexPath.Row];
			cell.UpdateCell(cellData);

            if (indexPath.Row == 4 || indexPath.Row == 5)
            {
                cell.Hidden = true;
            }

            return cell;
		}

        [Export("tableView:heightForRowAtIndexPath:")]
        public nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return UITableView.AutomaticDimension;
        }
        #endregion

        #region UI Events

        partial void Save_Tapped(UIButton sender)
        {
            var errorText = UserInfoTableView.CellAt(NSIndexPath.FromRowSection(4, 0)) as UserPersonalInfoTableCell;
            var errorTextMessage = UserInfoTableView.CellAt(NSIndexPath.FromRowSection(5, 0)) as UserPersonalInfoTableCell;
    
            if(errorText.Hidden && errorTextMessage.Hidden)
            {
                View.EndEditing(true);
                Save().Forget();
            }
        }

        partial void BackButton_TouchUpInside(UIButton sender)
        {
            var errorText = UserInfoTableView.CellAt(NSIndexPath.FromRowSection(4, 0)) as UserPersonalInfoTableCell;
            var errorTextMessage = UserInfoTableView.CellAt(NSIndexPath.FromRowSection(5, 0)) as UserPersonalInfoTableCell;

            if (errorText != null && errorTextMessage != null)
            {
                if (errorText.Hidden && errorTextMessage.Hidden)
                {
                    if (isKeyboardVisible)
                    {
                        View.EndEditing(true);
                        UserInfoTableView.ReloadData();
                    }
                    else
                    {
                        Presenter.GoBackToHome();
                    }
                }
            }
        }
        #endregion

        private async Task Save()
        {
            var mobileCell = UserInfoTableView.CellAt(NSIndexPath.FromRowSection(3, 0)) as UserPersonalInfoTableCell;
            var addressCell = UserInfoTableView.CellAt(NSIndexPath.FromRowSection(2, 0)) as UserPersonalInfoTableCell;
            if (mobileCell != null && addressCell != null)
            {
                var response = await Presenter.UpdateMobile(mobileCell.Value, addressCell.Value);
                if(response.IsSuccess)
                {
                    await LoadTableData();
                }
            }
        }

        public void ErrorTextLogics(bool emptyCheck, bool regexCheck)
        {
            var errorText = UserInfoTableView.CellAt(NSIndexPath.FromRowSection(4, 0)) as UserPersonalInfoTableCell;
            var errorTextMessage = UserInfoTableView.CellAt(NSIndexPath.FromRowSection(5, 0)) as UserPersonalInfoTableCell;

            if (errorText != null && errorTextMessage != null)
            {
                if (emptyCheck)
                {
                    errorText.Hidden = false;
                }
                else
                {
                    errorText.Hidden = true;
                }

                if (regexCheck)
                {
                    errorTextMessage.Hidden = false;
                }
                else
                {
                    errorTextMessage.Hidden = true;
                }
            }
            
        }
    }
}