using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation;
using Helseboka.Core.Profile.Interface;
using Helseboka.Core.Common.Extension;
using Helseboka.iOS.Common.View;
using Helseboka.iOS.Common.Extension;
using Helseboka.iOS.Profile.View.TableViewCell;
using MyTest.TableViewCellModel;
using UIKit;
using Helseboka.Core.Profile.Model;
using Helseboka.iOS.Common.Constant;
using Helseboka.iOS.Common.View.PopUpDialogs;
using Helseboka.Core.Resources.StringResources;
using Helseboka.Core.Common.Constant;
using SafariServices;
using MessageUI;
using Helseboka.Core.Common.EnumDefinitions;

namespace Helseboka.iOS.Profile.View
{
    public partial class MyProfileView : BaseView, IUITableViewDelegate, IUITableViewDataSource
    {
        private User currentUser;
        private int rowAdjustment = 0;
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

            userDetailsTableView.DataSource = this;
            userDetailsTableView.Delegate = this;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            LoadTableData().Forget();
        }

        private async Task LoadTableData()
        {
            currentUser = await Presenter.GetCurrentUserProfile();
            if (currentUser != null)
            {
                rowAdjustment = 0;
                UserNameLabel.Text = $"{currentUser.FirstName} {currentUser.LastName}";
                userTableCellModels = new List<UserTableCellModel>
                {
                    new UserTableCellModel("Profile.Home.PersonalSettings".Translate(), "", true, false),
                    new UserTableCellModel("Profile.Home.MedicalCenter".Translate(),currentUser.AssignedDoctor.OfficeName.ToNameCase(),true,true),
                    new UserTableCellModel("Profile.Home.Doctor".Translate(), currentUser.AssignedDoctor.FullName,true,true),
                    new UserTableCellModel("Profile.Home.Feedback".Translate(),"",true,false),
                    new UserTableCellModel("Profile.Home.GDPR".Translate(),"",true,false),
                    new UserTableCellModel("Profile.Home.Logout".Translate(),"",false,false, Colors.MyProfileLogoutColor),
                    new UserTableCellModel("Profile.Home.DeleteProfile".Translate(), "", false, false, Colors.MyProfileDeleteProfileColor),
                };

                var loginMode = Presenter.GetLoginMode();
                if (loginMode.HasValue && loginMode.Value == Core.Common.EnumDefinitions.LoginMode.PIN)
                {
                    userTableCellModels.Insert(4, new UserTableCellModel("Profile.Home.ChangePIN".Translate(), "Profile.Home.Change".Translate(), false, false, null, true));
                    rowAdjustment++;
                }

                userDetailsTableView.ReloadData();
            }
        }

        #region Table view Delegates
        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(UserProfileTableViewCell.Key, indexPath) as UserProfileTableViewCell;
            var cellData = userTableCellModels[indexPath.Row];
            cell.UpdateCell(cellData);
            return cell;
        }

        public nint RowsInSection(UITableView tableview, nint section)
        {
            return userTableCellModels != null ? userTableCellModels.Count : 0;
        }

        [Export("tableView:didSelectRowAtIndexPath:")]
        public void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath.Row == 0)
            {
                Presenter.ShowUserInfoView();
            }
            else if (indexPath.Row == 1 || indexPath.Row == 2)
            {
                Presenter.ShowDoctorAndOfficeDetailsView();
            }
            else if (indexPath.Row == 3)
            {
                var messageAttributedText = new NSMutableAttributedString();
                messageAttributedText.Append(new NSAttributedString(String.Format(AppResources.FeedbackPopupMessage, AppConstant.FeedbackEmailAddress), Fonts.GetNormalFont(16), Colors.TitleTextColor));
                messageAttributedText.Append(new NSAttributedString(Environment.NewLine));
                messageAttributedText.Append(new NSAttributedString(Environment.NewLine));
                messageAttributedText.Append(new NSAttributedString(AppResources.FeedbackPopupAlert, Fonts.GetNormalFont(16), UIColor.Red));

                var dialog = new YesNoDialogView(AppResources.FeedbackPopupTitle, leftButtonText: AppResources.GeneralTextContinue, rightButtonText: AppResources.GeneralTextCancel);
                dialog.LeftButtonTapped += FeedbackConfirmation_YesTapped;
                dialog.AttributedMessage = messageAttributedText;
                dialog.Show();
            }
            else
            {
                if (rowAdjustment > 0 && indexPath.Row == 4)
                {
                    Presenter.ShowPINConfirmation();
                }
                else if (indexPath.Row - rowAdjustment == 4)
                {        
                    Presenter.ShowTermsPage();
                }
                else if (indexPath.Row - rowAdjustment == 5)
                {
                    Presenter.Logout();
                }
                else
                {
                    var dialog = new YesNoDialogView(AppResources.DeleteProfileConfirmationTitle, AppResources.DeleteProfileConfirmationMessage, AppResources.DeleteProfileConfirmationButton);
                    dialog.LeftButtonTapped += DeleteProfileConfirmation_YesTapped;
                    dialog.Show();
                }

            }
        }
        #endregion

        partial void Help_Tapped(UIButton sender)
        {
            new ModalHelpViewController(HelpFAQType.ProfileHome).Show();
        }

        void DeleteProfileConfirmation_YesTapped(object sender, EventArgs e)
        {
            Presenter.DeleteProfile().Forget();
        }

        void FeedbackConfirmation_YesTapped(object sender, EventArgs e)
        {
            if (MFMailComposeViewController.CanSendMail)
            {

                var mailController = new MFMailComposeViewController();

                mailController.SetToRecipients(new string[] { AppConstant.FeedbackEmailAddress });
                mailController.SetSubject(AppResources.FeedbackMailSubject);

                mailController.Finished += MailController_Finished;

                this.PresentViewController(mailController, true, null);
            }
        }

        void MailController_Finished(object sender, MFComposeResultEventArgs e)
        {
            this.DismissModalViewController(true);
        }
    }
}

