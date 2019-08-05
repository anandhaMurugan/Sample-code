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

namespace Helseboka.iOS.Profile.View
{
    public partial class LegeDetailsViewController : BaseView
    {
        private User currentUser;
        public static readonly String Identifier = "LegeDetailsViewController";
        public IProfilePresenter Presenter
        {
            get => presenter as IProfilePresenter;
            set => presenter = value;
        }


        public LegeDetailsViewController() { }

        protected LegeDetailsViewController(IntPtr handle) : base(handle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            LoadTableData().Forget();
        }

        partial void BackButton_TouchUpInside(UIButton sender)
		{
            Presenter.GoBackToHome();
		}

        private async Task LoadTableData()
        {
            currentUser = await Presenter.GetCurrentUserProfile();
            if (currentUser != null)
            {
                DoctorOfficeLabel.Text = currentUser.AssignedDoctor.OfficeName.ToNameCase();
                DoctorNameLabel.Text = $"{currentUser.AssignedDoctor.FullName}";
                AddressLabel.Text = $"{currentUser.AssignedDoctor.OfficeStreet}\n{currentUser.AssignedDoctor.OfficeZip} {currentUser.AssignedDoctor.OfficeCity}";
                MobileLabel.Text = $"{"Profile.Doctor.Details.Mobile".Translate()}: {String.Format("{0:## ### ###}", currentUser.AssignedDoctor.PhoneNumber)}";
            }
        }

        partial void ChangeDoctor_Tapped(UIButton sender)
        {
            Presenter.ShowDoctorSelectionView();
        }
    }
}

