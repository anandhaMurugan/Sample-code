using System;
using Helseboka.Core.Startup.Interface;
using Helseboka.iOS.Common.View;
using Helseboka.iOS.Common.Extension;
using UIKit;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using Helseboka.Core.Common.Constant;
using Helseboka.Core.Common.Extension;
using Foundation;
using System.Collections.Generic;
using Helseboka.Core.Common.Model;
using Helseboka.iOS.Startup.View.TableViewCell;
using Helseboka.iOS.Common.TableViewDelegates;
using Helseboka.iOS.Common.Utilities;
using Helseboka.iOS.Common.PlatformEnums;
using Helseboka.Core.Resources.StringResources;

namespace Helseboka.iOS.Startup.View
{
    public partial class DoctorSelectionView : BaseView
    {
        public IDoctorSelectionPresenter Presenter
        {
            get => presenter as IDoctorSelectionPresenter;
            set => presenter = value;
        }

        private System.Timers.Timer timer = new System.Timers.Timer(AppConstant.SearchDelay);
        private SearchDoctorTableviewSource tableviewSource = new SearchDoctorTableviewSource();
        private const int cellBuffer = 100;
        private NSObject keyBoardWillShow;
        private NSObject keyBoardWillHide;
        private Doctor selectedDoctor;
        private bool isloading = false;

		public DoctorSelectionView() { }

		public DoctorSelectionView(IntPtr handler) : base(handler) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (Device.DeviceType == DeviceType.iPhones_5_5s_5c_SE || Device.DeviceType == DeviceType.iPhones_6_6s_7_8)
            {
                OkButtonBottomConstraint.Constant = -10;
                PageTitleTopConstraint.Constant = 10;
                PageSubtitleTopConstraint.Constant = 10;
                SelectionLabelTopConstraint.Constant = 15;
                SelectionlabelLeadingConstraint.Constant = 15;
                SelectionLabelTrailingConstraint.Constant = -15;
            }
            else if (Device.DeviceType == DeviceType.iPhones_6Plus_6sPlus_7Plus_8Plus)
            {
                OkButtonBottomConstraint.Constant = -10;
                //PageTitleTopConstraint.Constant = 20;
                //PageSubtitleTopConstraint.Constant = 10;
                //SelectionLabelTopConstraint.Constant = 15;
            }
            ErrorTextView.Hidden = true;
            SelectionLabel.Padding = new UIEdgeInsets(12, 25, 12, 25);
            OkButton.Enabled = false;
            SelectionLabel.AddGestureRecognizer(new UITapGestureRecognizer((UITapGestureRecognizer obj) => 
            {
                ShowSearch();
            }));
            HideSearch();
            SearchText.Padding = new UIEdgeInsets(0, 20, 0, 0);
            timer.Elapsed += Timer_Elapsed;
            timer.Stop();
            tableviewSource.DidScroll += Tableview_DidScroll;
            tableviewSource.DidSelect += Tableview_DidSelect;
            SearchResultTableView.Source = tableviewSource;
            SearchResultTableView.RowHeight = UITableView.AutomaticDimension;
            SearchResultTableView.EstimatedRowHeight = 100;
            SearchText.Placeholder = AppResources.DoctorSearchPlaceholder;

            LoadUserInfo().Forget();
        }

        private async Task LoadUserInfo()
        {
            ShowLoader();
            var response = await Presenter.GetCurrentUser();
            HideLoader();
            PageSubtitleText.Text = AppResources.DoctorSelectionSubtitleDisabled;
            if (response.IsSuccess)
            {
                PageTitleLabel.Text = $"{AppResources.Salutation} {response.Result.FirstName}";
                if (response.Result.AssignedDoctor != null)
                {
                    SelectDoctor(response.Result.AssignedDoctor);
                }
            }
            else
            {
                await ProcessAPIError(response);
            }
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
            this.InvokeOnMainThread(() =>
            {
                UIView.Animate(args.AnimationDuration, 0, args.GetAnimationOptions(), () =>
                {
                    TableViewBottomConstraint.Constant = -args.GetKeyboardHeight();
                    View.LayoutIfNeeded();
                }, null);
            });
        }

        private void KeyboardWillHide(object sender, UIKeyboardEventArgs args)
        {
            this.InvokeOnMainThread(() =>
            {
                UIView.Animate(args.AnimationDuration, 0, args.GetAnimationOptions(), () =>
                {
                    TableViewBottomConstraint.Constant = 0;
                    View.LayoutIfNeeded();
                }, null);
            });
        }

        partial void SearchText_Changed(BaseTextfield sender)
        {
            if (!String.IsNullOrEmpty(SearchText.Text))
            {
                timer.Stop();
                timer.Start();
            }
        }

        partial void Close_Tapped(UIButton sender)
        {
            HideSearch();
        }

        partial void Ok_Tapped(PrimaryActionButton sender, UIEvent @event)
        {
            Presenter.SelectDoctor(selectedDoctor).Forget();
        }

        partial void Cancel_Tapped(SignUpSecondaryActionButton sender)
        {
            Presenter.Cancel();
        }

        partial void SearchText_BeginEditing(BaseTextfield sender)
        {
            //SearchText.Background = UIImage.FromBundle("Textbox-active-background");
        }

        partial void SearchText_EndEditing(BaseTextfield sender)
        {
            //SearchText.Background = UIImage.FromBundle("Textbox-inactive-background");
        }

        private void ShowSearch()
        {
            SelectionLabel.IsEnabled = false;
            SearchContainer.Hidden = false;
            View.BringSubviewToFront(SearchContainer);
            SearchText.BecomeFirstResponder();
        }

        protected void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();

            this.InvokeOnMainThread(async () =>
            {
                tableviewSource.Clear();
                SearchResultTableView.ReloadData();
                if (!String.IsNullOrEmpty(SearchText.Text))
                {
                    var response = await Presenter.SearchDoctor(SearchText.Text);
                    tableviewSource.UpdateList(response);
                    SearchResultTableView.ReloadData();
                }
            });
        }

        private void Tableview_DidScroll(object sender, UIScrollView scrollView)
        {
            var bottom = scrollView.ContentSize.Height - scrollView.Frame.Size.Height;
            var scrollPosition = scrollView.ContentOffset.Y;

            if (scrollPosition > bottom - cellBuffer)
            {
                LoadMoreData().Forget();
            }
        }

        private void Tableview_DidSelect(object sender, Doctor doctor)
        {
            HideSearch();

            SelectDoctor(doctor);
        }

        private void SelectDoctor(Doctor doctor)
        {
            selectedDoctor = doctor;
            SelectionLabel.Text = $"{selectedDoctor.FullName}\n{selectedDoctor.OfficeName.ToNameCase()}";
            SelectionLabel.IsEnabled = true;
            OkButton.Enabled = true;

            //DoctorInfoText.Hidden = false;
            //DoctorInfoText.Text = AppResources.DoctorSelectionHelpText;
            PageSubtitleText.Text = AppResources.DoctorSelectionSubtitleEnabled;
            ErrorTextView.Text = doctor.Remarks;
            ErrorTextView.Hidden = false;
            if (doctor.Enabled)
            {
                ErrorTextView.TextColor = UIColor.Gray;
            }
            else
            {
                ErrorTextView.TextColor = UIColor.Red;
            }
        }

        private async Task LoadMoreData()
        {
            if (Presenter.HasMoreData && !isloading)
            {
                isloading = true;
                var response = await Presenter.LoadMore();
                tableviewSource.UpdateList(response);
                this.InvokeOnMainThread(() => SearchResultTableView.ReloadData());
                isloading = false;
            }
        }

        private void HideSearch()
        {
            SelectionLabel.IsEnabled = true;
            SearchContainer.Hidden = true;
            SearchText.Text = String.Empty;
            View.SendSubviewToBack(SearchContainer);
            SearchText.ResignFirstResponder();
            tableviewSource.Clear();
            SearchResultTableView.ReloadData();
        }
    }

    public class SearchDoctorTableviewSource : BaseTableViewSource<Doctor>
    {
        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return UITableView.AutomaticDimension;
        }

        public override nfloat GetHeightForFooter(UITableView tableView, nint section)
        {
            return 0;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell("SearchTableViewCell") as SearchTableViewCell;

            var data = DataList[indexPath.Row];
            cell.Configure($"{data.FullName}, {data.OfficeName.ToNameCase()}");

            return cell;
        }
    }
}

