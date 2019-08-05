using System;
using UIKit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.Common.Constant;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.iOS.Common.Extension;
using Helseboka.iOS.Common.Constant;
using Helseboka.iOS.Common.View;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.MedicineModule.Model;
using Foundation;
using Helseboka.iOS.Medisiner.View.TableViewCell;
using CoreGraphics;
using System.Diagnostics;
using Helseboka.iOS.Common.View.DateTimePicker;
using Helseboka.Core.MedicineModule.Interface;
using Helseboka.iOS.Common.View.PopUpDialogs;
using SafariServices;

namespace Helseboka.iOS.Medisiner.View
{
    public partial class MedicineOverview : BaseView
    {
        public IMedicineHomePresenter Presenter
        {
            get => presenter as IMedicineHomePresenter;
            set => presenter = value;
        }

        public MedicineReminder MedicineDetails { get; set; }

        public Boolean IsFromSearch { get; set; }

        public MedicineOverview() : base() { }

        public MedicineOverview(IntPtr handler) : base(handler) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            AlarmView.AddGestureRecognizer(new UITapGestureRecognizer(AlarmView_Tapped));
            ReadMoreView.AddGestureRecognizer(new UITapGestureRecognizer(ReadMore_Tapped));
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            MedicineNameLabel.Text = MedicineDetails.Medicine.Name;
            MedicineDoseLabel.Text = MedicineDetails.Medicine.Strength;

            if (MedicineDetails.HasReminder && MedicineDetails.Reminder != null && MedicineDetails.Reminder.Days != null && MedicineDetails.Reminder.FrequencyPerDay != null)
            {
                AlarmTextLabel.Text = "Medicine.Overview.AlarmLabel.Edit.Title".Translate();

                var localizedList = MedicineDetails.Reminder.Days.Select(x => $"Medicine.Overview.{x}".Translate()).ToList();

                var attributedText = new NSMutableAttributedString();
                attributedText.Append(new NSAttributedString(String.Join(", ", localizedList)));
                attributedText.Append(new NSAttributedString($" {"General.View.TimePrefix".Translate()} ", Fonts.GetMediumFont(15), Colors.TitleTextColor));
                attributedText.Append(new NSAttributedString(String.Join(" ", MedicineDetails.Reminder.FrequencyPerDay), Fonts.GetMediumFont(15), Colors.TitleTextColor));
                AlarmDetailsLabel.AttributedText = attributedText;
            }
            else
            {
                AlarmTextLabel.Text = "Medicine.Overview.AlarmLabel.Add.Title".Translate();
                AlarmDetailsLabel.Hidden = true;
                AlarmDetailsTopConstraint.Constant = 0;
            }

            if (Presenter.IsMedicineExistInProfile(MedicineDetails.Medicine))
            {
                AddToMyProfileButton.Hidden = true;
                DeleteButton.Hidden = false;
            }
            else
            {
                AddToMyProfileButton.Hidden = false;
                DeleteButton.Hidden = true;
            }

        }

        partial void Back_Tapped(UIButton sender)
        {
            Presenter.GoBack();
        }

        partial void AddToMyProfile_Tapped(PrimaryActionButton sender)
        {
            Presenter.AddMedicineToProfile(MedicineDetails).Forget();
        }

        partial void Delete_Tapped(UIButton sender)
        {
            var view = new YesNoDialogView();
            view.LeftButtonTapped += Dialog_OkTapped;
            view.Show();

        }

        private void AlarmView_Tapped()
        {
            Presenter.NavigateToMedicineAlarmView(MedicineDetails);
        }

        private void Dialog_OkTapped(object sender, EventArgs e)
        {
            Presenter.DeleteMedicine(MedicineDetails).Forget();
        }


        private void ReadMore_Tapped()
        {
            NSUrl url;
            try
            {
                url = new NSUrl(String.Format(AppConstant.ReadMoreUrlFormat, MedicineDetails.Medicine.Name.ToURLEncodedString()));

            }
            catch
            {
                url = new NSUrl(AppConstant.ReadMoreUrl);
            }

            var view = new SFSafariViewController(url);
            PresentViewController(view, true, null);
        }

    }
}

