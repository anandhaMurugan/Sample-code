using System;
using System.Linq;
using System.Threading.Tasks;
using Helseboka.Core.MedicineModule.Interface;
using Helseboka.iOS.Common.Constant;
using Helseboka.iOS.Common.View;
using UIKit;
using Helseboka.iOS.Common.Extension;
using Helseboka.Core.Common.Extension;
using Foundation;
using Helseboka.Core.Common.Constant;
using System.Collections.Generic;
using Helseboka.Core.MedicineModule.Model;
using Helseboka.iOS.Common.TableViewDelegates;
using Helseboka.iOS.Medisiner.View.TableViewCell;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Common.EnumDefinitions;

namespace Helseboka.iOS.Medisiner.View
{
    public partial class MedisinListView : BaseView, IUITableViewDataSource, IUITableViewDelegate, IUITextFieldDelegate
    {
        private NSObject keyBoardWillShow;
        private NSObject keyBoardWillHide;
        private System.Timers.Timer timer = new System.Timers.Timer(AppConstant.SearchDelay);
        private SelectableEntityCollection<MedicineReminder> medicineReminders;
        private bool isSelecting = false;
        private HashSet<MedicineInfo> selectedMedicine = new HashSet<MedicineInfo>();

        public IMedicineHomePresenter Presenter
        {
            get => presenter as IMedicineHomePresenter;
            set => presenter = value;
        }

        public MedisinListView() { }

		public MedisinListView (IntPtr ptr) : base(ptr) {}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SearchView.Layer.BorderColor = Colors.UnselectedLabelBorderColor.CGColor;
            SearchView.Layer.BackgroundColor = Colors.SearchResultBackground.CGColor;
            SearchView.Layer.BorderWidth = 1;
            RenewPrescriptionButton.Enabled = false;

            timer.Elapsed += Timer_Elapsed;
            timer.Stop();

            MedicineListTableView.DataSource = this;
            MedicineListTableView.Delegate = this;
            MedicineListTableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

            SearchTextField.Delegate = this;

            MedicineListTableView.AddPullToRefresh(() => LoadMedicineList());

            var view = new HelpViewController(HelpFAQType.MedicineHome, UIImage.FromBundle("Medicine-help-image"), "Medicine.Home.HelpText".Translate());
            EmbedView(NoDataContainerView, this, view);

            //View.AddGestureRecognizer(new UITapGestureRecognizer(() => SearchTextField.ResignFirstResponder()));

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            SearchTextField.Text = String.Empty;
            ShowMedicineListView();
            MedicineListTableView.ShowLoader();
            LoadMedicineList().Forget();

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
                    SearchTableViewBottomConstraint.Constant = -args.GetKeyboardHeight();
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
                    SearchTableViewBottomConstraint.Constant = 0;
                    View.LayoutIfNeeded();
                }, null);
            });
        }

        [Export("textFieldShouldReturn:")]
        public bool ShouldReturn(UITextField textField)
        {
            textField.ResignFirstResponder();
            if (medicineReminders!= null && medicineReminders.Count > 0)
            {
                ShowMedicineListView();
            }
            else
            {
                ShowNoDataView();
            }
            textField.Text = String.Empty;
            return true;
        }

        partial void Help_Tapped(UIButton sender)
        {
            View.EndEditing(true);
            SearchTextField.Text = String.Empty;
            ShowMedicineListView();
            new ModalHelpViewController(HelpFAQType.MedicineHome).Show();
        }

        partial void SearchText_Changed(UITextField sender, UIEvent @event)
        {
            if (!String.IsNullOrEmpty(SearchTextField.Text))
            {
                timer.Stop();
                timer.Start();
            }
            else
            {
                timer.Stop();

                if (medicineReminders != null && medicineReminders.Count > 0)
                {
                    ShowMedicineListView();
                }
                else
                {
                    ShowNoDataView();
                }
            }
        }

        protected void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();

            this.InvokeOnMainThread(async () =>
            {
                ShowStatusBarActivityIndicator();
                var response = await Presenter.SearchMedicine(SearchTextField.Text);
                HideStatusBarActivityIndicator();
                if (response.IsSuccess)
                {
                    var source = new SearchMedicineDataSource(response.Result);
                    source.DidSelect += SearchTableView_DidSelect;
                    SearchTableView.Source = source;
                    ShowSearchResultView();
                    SearchTableView.ReloadData();
                }
            });
        }

        private void SearchTableView_DidSelect(object sender, MedicineInfo e)
        {
            SearchTextField.ResignFirstResponder();
            Presenter.SelectMedicineFromSearchResult(e);
        }

        private void Medicine_SelectionChanged(object sender, MedicineSelectionEventArgs e)
        {
            if (e.IsSelected)
            {
                selectedMedicine.Add(e.Medicine);
            }
            else
            {
                selectedMedicine.Remove(e.Medicine);
            }

            RenewPrescriptionButton.SetTitle(selectedMedicine.Count > 0 ? "Medicine.Home.Button.RenewPrescription".Translate() : "General.View.Cancel".Translate(), UIControlState.Normal);
        }

        private void Medicine_SendTapped(object sender, EventArgs e)
        {
            RenewPrescription().Forget();
        }

        private async Task RenewPrescription()
        {
            var response = await Presenter.RenewPrescription(selectedMedicine.ToList());
            if (response.IsSuccess)
            {
                selectedMedicine.Clear();
            }
        }


        partial void Renew_Tapped(PrimaryActionButton sender)
        {
            if (ApplicationCore.Instance.CurrentUser != null && ApplicationCore.Instance.CurrentUser.AssignedDoctor != null && !ApplicationCore.Instance.CurrentUser.AssignedDoctor.Enabled)
            {
                ShowInfoDialog(ApplicationCore.Instance.CurrentUser.AssignedDoctor.Remarks);
            }
            else
            {
                if (isSelecting)
                {
                    if (selectedMedicine.Count > 0)
                    {
                        var dialog = new RenewPrescriptionDialogView(selectedMedicine.ToList());
                        dialog.SendTapped += Medicine_SendTapped;
                        dialog.Show();
                        //PresentViewController(dialog, true, null);
                    }

                    RenewPrescriptionButton.SetTitle("Medicine.Home.Button.RenewPrescription".Translate(), UIControlState.Normal);
                }
                else
                {
                    selectedMedicine = new HashSet<MedicineInfo>();
                    RenewPrescriptionButton.SetTitle("General.View.Cancel".Translate(), UIControlState.Normal);
                    medicineReminders.ForEach((obj) => obj.IsSelected = false);
                }

                isSelecting = !isSelecting;
                MedicineListTableView.ReloadData();
            }
        }

        #region Medicine list view delegates
        public nint RowsInSection(UITableView tableView, nint section)
        {
            return medicineReminders != null ? medicineReminders.Count : 0;
        }

        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(MedicineReminderCell.Key) as MedicineReminderCell;

            cell.Configure(medicineReminders[indexPath.Row], isSelecting);
            cell.SelectionChanged -= Medicine_SelectionChanged;
            cell.SelectionChanged += Medicine_SelectionChanged;

            return cell;
        }

        [Export("tableView:didSelectRowAtIndexPath:")]
        public void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (isSelecting)
            {
                var cell = MedicineListTableView.CellAt(indexPath) as MedicineReminderCell;
                if (cell != null)
                {
                    cell.ToggleSelection();
                }
            }
            else
            {
                var reminder = medicineReminders[indexPath.Row];
                Presenter.NavigateToMedicineOverview(reminder.Entity);
            }
        }
        #endregion

        private async Task LoadMedicineList()
        {
            MedicineListTableView.IsLoading = true;
            var response = await Presenter.GetAllMedicineAndReminders();
            MedicineListTableView.IsLoading = false;
            if (response.IsSuccess)
            {
                medicineReminders = new SelectableEntityCollection<MedicineReminder>(response.Result, false);
                MedicineListTableView.HideLoader();

                if (medicineReminders != null && medicineReminders.Count > 0)
                {
                    MedicineListTableView.ReloadData();
                    isSelecting = false;
                    RenewPrescriptionButton.Enabled = true;
                    ShowMedicineListView();
                }
                else
                {
                    ShowNoDataView();
                }
            }
            else
            {
                var result = await ProcessAPIError(response);
                if(!result)
                {
                    ShowNoDataView();
                }
            }
        }

        private void ShowSearchResultView()
        {
            HelpButton.Hidden = false;
            SearchTableView.Hidden = false;
            NoDataContainerView.Hidden = true;
            MedicineListTableView.Hidden = true;
        }

        private void ShowMedicineListView()
        {
            HelpButton.Hidden = false;
            SearchTableView.Hidden = true;
            NoDataContainerView.Hidden = true;
            MedicineListTableView.Hidden = false;
        }

        private void ShowNoDataView()
        {
            HelpButton.Hidden = true;
            SearchTableView.Hidden = true;
            NoDataContainerView.Hidden = false;
            MedicineListTableView.Hidden = true;
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            SearchView.Layer.CornerRadius = SearchView.Frame.Height / 2;
        }
    }

    public class SearchMedicineDataSource : BaseTableViewSource<MedicineInfo>
    {
        public SearchMedicineDataSource(List<MedicineInfo> medicines)
        {
            DataList = medicines;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath.Row < DataList.Count)
            {
                var cell = tableView.DequeueReusableCell(MedicineSearchTableViewCell.Key) as MedicineSearchTableViewCell;

                var data = DataList[indexPath.Row];
                var description = $"{data.NameFormStrength}, {data.Form}";
                cell.Configure(description);
                return cell;
            }
            else
            {
                return new UITableViewCell();
            }
        }
    }
}

