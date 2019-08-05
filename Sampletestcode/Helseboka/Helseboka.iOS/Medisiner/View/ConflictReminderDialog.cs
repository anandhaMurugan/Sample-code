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
using Foundation;
using CoreGraphics;
using System.Diagnostics;
using Helseboka.Core.Common.Model;
using Helseboka.iOS.Medisiner.View.TableViewCell;
using Helseboka.Core.MedicineModule.Model;

namespace Helseboka.iOS.Medisiner.View
{
    public partial class ConflictReminderDialog : BaseModalViewController, IUITableViewDelegate, IUITableViewDataSource
    {
        private HashSet<MedicineInfo> selectedMedicines = new HashSet<MedicineInfo>();

        public static readonly String Identifier = "ConflictReminderDialog";

        public event EventHandler OkTapped;
        public event EventHandler CancelTapped;

        public List<MedicineReminder> MedicineReminderList { get; set; }

        public ConflictReminderDialog() : base() { }

        public ConflictReminderDialog(IntPtr handler) : base(handler) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            MedicineListTableView.Delegate = this;
            MedicineListTableView.DataSource = this;

            CancelButton.SetBackgroundImage(UIImage.FromBundle("Medium-action-button-disabled-background"), UIControlState.Normal);
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            TableViewHeightConstraint.Constant = Math.Min((int)(UIScreen.MainScreen.Bounds.Height * 0.5), Math.Max(150, (int)MedicineListTableView.ContentSize.Height));
        }

        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(ReminderConflictAlertCell.Key) as ReminderConflictAlertCell;

            cell.Configure(MedicineReminderList[indexPath.Row]);

            return cell;
        }

        public nint RowsInSection(UITableView tableView, nint section)
        {
            return MedicineReminderList != null ? MedicineReminderList.Count : 0;
        }

        partial void Ok_Tapped(MediumActionButton sender)
        {
            OkTapped?.Invoke(this, EventArgs.Empty);
            Close();
        }

        partial void Cancel_Tapped(MediumActionButton sender)
        {
            CancelTapped?.Invoke(this, EventArgs.Empty);
            Close();
        }

        partial void Close_Tapped(UIButton sender)
        {
            Close();
        }
    }
}

