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
using Helseboka.Core.Common.Model;
using CoreAnimation;
using Helseboka.iOS.Common.View.PopUpDialogs;

namespace Helseboka.iOS.Medisiner.View
{
    public partial class MedicineAlarmView : BaseView, IUITableViewDataSource, IUITableViewDelegate
    {
        public IMedicineAlarmPresenter Presenter
        {
            get => presenter as IMedicineAlarmPresenter;
            set => presenter = value;
        }

        private HashSet<MedicineInfo> medicineList = new HashSet<MedicineInfo>();
        private List<DateTime> frequencies = new List<DateTime>();
        private List<DayOfWeek> SelectedDays = new List<DayOfWeek>();
        private DateTime startDate = DateTime.Now;
        private DateTime? endDate;
        private Boolean AlarmToggleStatus = true;

        public MedicineReminder Medicine { get; set; }

        public MedicineAlarmView() : base()
        {
        }

        public MedicineAlarmView(IntPtr handler) : base(handler) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            MedicineName.Text = Medicine.Medicine.Name;
            MedicineStrength.Text = Medicine.Medicine.Strength;

            AlarmTableView.DataSource = this;
            AlarmTableView.Delegate = this;
            AlarmTableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            AlarmTableView.AllowsSelection = false;

            AlarmTableView.RowHeight = UITableView.AutomaticDimension;
            AlarmTableView.EstimatedRowHeight = 140;

            LoadExsistingReminderDetails();

            Validate();
        }

        public void LoadExsistingReminderDetails()
        {
            if (Medicine != null && Medicine.Reminder != null)
            {
                var reminder = Medicine.Reminder;

                if (reminder.Days != null && reminder.Days.Count > 0)
                {
                    SelectedDays = reminder.Days;
                }

                if (reminder.FrequencyPerDay != null && reminder.FrequencyPerDay.Count > 0)
                {
                    foreach (var item in reminder.FrequencyPerDay)
                    {
                        var frequencyTime = item.ConvertTimeToDateTime();
                        if(frequencyTime.HasValue)
                        {
                            frequencies.Add(frequencyTime.Value);
                        }
                    }
                }

                startDate = reminder.StartDate;
                endDate = reminder.EndDate;
            }


            AlarmTableView.ReloadData();
        }

        #region TableView Delegates
        public nint RowsInSection(UITableView tableView, nint section)
        {
            return medicineList.Count + frequencies.Count + 3;
        }

        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath.Row == 0)
            {
                var cell = tableView.DequeueReusableCell(ToggleAlarmCell.Key) as ToggleAlarmCell;

                cell.ToggleChanged -= AlarmToggle_Changed;
                cell.ToggleChanged += AlarmToggle_Changed;

                return cell;
            }
            else if (indexPath.Row <= medicineList.Count)
            {
                var cell = tableView.DequeueReusableCell(AddMoreMedicineCell.Key) as AddMoreMedicineCell;
                cell.Configure(medicineList.ElementAt(indexPath.Row - 1));
                cell.RemoveTapped -= Medicine_RemoveTapped;
                cell.RemoveTapped += Medicine_RemoveTapped;
                return cell;
            }
            else if (indexPath.Row == medicineList.Count + 1)
            {
                var cell = tableView.DequeueReusableCell(FullWeekTableViewCell.Key) as FullWeekTableViewCell;

                cell.AddMoreMedicineTapped -= Medicine_MoreTapped;
                cell.AddMoreMedicineTapped += Medicine_MoreTapped;

                cell.DaySelected -= Day_Selected;
                cell.DaySelected += Day_Selected;

                cell.DayUnSelected -= Day_UnSelected;
                cell.DayUnSelected += Day_UnSelected;

                if (SelectedDays != null && SelectedDays.Count > 0)
                {
                    cell.ConfigureSelected(SelectedDays);
                }

                return cell;
            }
            else if (indexPath.Row == frequencies.Count + medicineList.Count + 2)
            {
                var cell = tableView.DequeueReusableCell(DateSelectionCell.Key) as DateSelectionCell;

                cell.AddMoreFrequencyTapped -= Frequency_MoreTapped;
                cell.AddMoreFrequencyTapped += Frequency_MoreTapped;

                return cell;
            }
            else
            {
                var index = indexPath.Row - medicineList.Count - 2;
                var cell = tableView.DequeueReusableCell(AddAlarmCell.Key) as AddAlarmCell;
                cell.Tag = index;
                cell.Configure(frequencies[index].GetTimeString());
                cell.DeleteTapped -= Frequency_DeleteTapped;
                cell.DeleteTapped += Frequency_DeleteTapped;

                return cell;
            }
        }

        [Export("tableView:heightForRowAtIndexPath:")]
        public nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return UITableView.AutomaticDimension;
        }


        #endregion

        private static UIView GetSeparatorView(UITableView tableView)
        {
            var separator = new UIView();
            separator.BackgroundColor = Colors.SeparatorColor;

            return separator;
        }

        private void AlarmToggle_Changed(object sender, bool e)
        {
            AlarmToggleStatus = e;
            Validate();
        }

        private void Medicine_RemoveTapped(object sender, MedicineInfo e)
        {
            var index = medicineList.ToList().IndexOf(e);
            medicineList.Remove(e);
            AlarmTableView.DeleteRows(new NSIndexPath[] { NSIndexPath.FromRowSection(index + 1, 0) }, UITableViewRowAnimation.Automatic);
        }


        private void Medicine_MoreTapped(object sender, EventArgs e)
        {
            var medicineSelectionView = UIStoryboard.FromName("Medicine", null).InstantiateViewController(SelectMedicineView.Identifier) as SelectMedicineView;

            var existingMedicineList = medicineList.ToList();
            existingMedicineList.Add(Medicine.Medicine);

            var selectableMedicineList = Presenter.GetNewMedicines(existingMedicineList);

            if (selectableMedicineList != null && selectableMedicineList.Count > 0)
            {
                medicineSelectionView.CurrentMedicine = Medicine.Medicine;
                medicineSelectionView.MedicineList = new SelectableEntityCollection<MedicineInfo>(selectableMedicineList, false);
                medicineSelectionView.MedicineSelected += MedicineSelectionView_MedicineSelected;
                medicineSelectionView.Show();
            }
            else
            {
                ShowInfoDialog("Medicine.Reminder.Alert.NoMedicine".Translate());
            }

        }

        private void MedicineSelectionView_MedicineSelected(object sender, HashSet<MedicineInfo> e)
        {
            e.Remove(Medicine.Medicine);
            medicineList.UnionWith(e);

            AlarmTableView.ReloadData();
        }

        private void Frequency_MoreTapped(object sender, EventArgs e)
        {
            var dialog = new DatePickerDialog();
            dialog.DialogTitle = "Medicine.Reminder.SelectFrequency.Title".Translate();
            dialog.Mode = UIDatePickerMode.Time;
            dialog.DateSelected += Dialog_DateSelected;
            dialog.Show();
        }

        private void Dialog_DateSelected(object sender, DateTime e)
        {
            frequencies.Add(e);

            var newIndex = NSIndexPath.FromRowSection(frequencies.Count + medicineList.Count + 1, 0);

            CATransaction.Begin();
            CATransaction.CompletionBlock = () => AlarmTableView.ScrollToBottom();
            AlarmTableView.BeginUpdates();
            AlarmTableView.InsertRows(new NSIndexPath[] { newIndex }, UITableViewRowAnimation.Automatic);
            AlarmTableView.EndUpdates();
            CATransaction.Commit();

            Validate();
        }

        private void Frequency_DeleteTapped(object sender, String e)
        {
            var itemToRemove = frequencies.Where((x) => x.GetTimeString() == e).ToList();

            if (itemToRemove != null && itemToRemove.Count > 0)
            {
                var index = frequencies.IndexOf(itemToRemove[0]);
                frequencies.Remove(itemToRemove[0]);
                AlarmTableView.DeleteRows(new NSIndexPath[] { NSIndexPath.FromRowSection(index + medicineList.Count + 2, 0) }, UITableViewRowAnimation.Automatic);
            }
        }

        private void Day_Selected(object sender, DayOfWeek e)
        {
            if (!SelectedDays.Exists((obj) => obj == e))
            {
                SelectedDays.Add(e);
            }

            Validate();
        }

        private void Day_UnSelected(object sender, DayOfWeek e)
        {
            if (SelectedDays.Exists((obj) => obj == e))
            {
                SelectedDays.Remove(e);
            }

            Validate();
        }

        private void ConflictReminderDialog_OkTapped(object sender, EventArgs e)
        {
            Save().Forget();
        }

        void ConflictReminderDialog_CancelTapped(object sender, EventArgs e)
        {
            medicineList.Clear();
            Save().Forget();
            AlarmTableView.ReloadData();
        }


        partial void Save_Tapped(MediumActionButton sender)
        {
            if (AlarmToggleStatus)
            {
                var conflictingList = Presenter.GetConflictingReminders(medicineList.ToList());

                if (conflictingList != null && conflictingList.Count > 0)
                {
                    var conflictReminderDialog = UIStoryboard.FromName("Medicine", null).InstantiateViewController(ConflictReminderDialog.Identifier) as ConflictReminderDialog;
                    conflictReminderDialog.MedicineReminderList = conflictingList;
                    conflictReminderDialog.OkTapped += ConflictReminderDialog_OkTapped;
                    conflictReminderDialog.CancelTapped += ConflictReminderDialog_CancelTapped;
                    conflictReminderDialog.Show();
                }
                else
                {
                    Save().Forget();
                }
            }
            else
            {
                DeleteReminder().Forget();
            }
        }

        partial void Back_Tapped(UIButton sender)
        {
            Presenter.GoBack();
        }

        private async Task DeleteReminder()
        {
            ShowLoader();
            var response = await Presenter.RemoveReminder(Medicine);
            HideLoader();
            if (response.IsSuccess)
            {
                Presenter.GoBack();
            }
            else
            {
                await ProcessAPIError(response);
            }
        }

        private async Task Save()
        {
            ShowLoader();
            var response = await Presenter.AddReminder(Medicine, SelectedDays, frequencies, startDate, endDate, medicineList.ToList());
            HideLoader();
            if (response.IsSuccess)
            {
                Presenter.GoBackToHome();
            }
            else
            {
                await ProcessAPIError(response);
            }
        }

        private void Validate()
        {
            bool isValid = (frequencies != null && frequencies.Count > 0 && SelectedDays != null && SelectedDays.Count > 0) || (Medicine.HasReminder && !AlarmToggleStatus);
            SaveButton.Enabled = isValid;
        }
    }
}

