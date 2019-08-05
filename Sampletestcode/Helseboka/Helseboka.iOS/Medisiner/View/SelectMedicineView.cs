using System; using UIKit; using System.Threading.Tasks; using System.Collections.Generic; using System.Linq; using Helseboka.Core.Common.Extension; using Helseboka.Core.Common.Constant; using Helseboka.Core.Common.EnumDefinitions; using Helseboka.iOS.Common.Extension; using Helseboka.iOS.Common.Constant; using Helseboka.iOS.Common.View; using Helseboka.Core.Common.Interfaces; using Foundation; using CoreGraphics; using System.Diagnostics;
using Helseboka.Core.Common.Model;
using Helseboka.iOS.Medisiner.View.TableViewCell;
using Helseboka.Core.MedicineModule.Model;

namespace Helseboka.iOS.Medisiner.View
{
    public partial class SelectMedicineView : BaseModalViewController, IUITableViewDelegate, IUITableViewDataSource
    {         private HashSet<MedicineInfo> selectedMedicines = new HashSet<MedicineInfo>();          public static readonly String Identifier = "SelectMedicineView";          public event EventHandler<HashSet<MedicineInfo>> MedicineSelected;          public SelectableEntityCollection<MedicineInfo> MedicineList { get; set; }          public MedicineInfo CurrentMedicine { get; set; }          public SelectMedicineView() : base() { }          public SelectMedicineView(IntPtr handler) : base(handler) { }
         public override void ViewDidLoad()
        {
            base.ViewDidLoad();
             MedicineListTableView.Delegate = this;             MedicineListTableView.DataSource = this;              if(CurrentMedicine != null)             {                 DialogTitleLabel.Text += $" {CurrentMedicine.Name}:";             }              Validate();
        }          public override void ViewDidLayoutSubviews()         {             base.ViewDidLayoutSubviews();             TableViewHeightConstraint.Constant = Math.Min((int)(UIScreen.MainScreen.Bounds.Height * 0.5), Math.Max(150, (int)MedicineListTableView.ContentSize.Height));         }

        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)         {             var cell = tableView.DequeueReusableCell(SelectMedicineCell.Key) as SelectMedicineCell;              cell.Configure(MedicineList[indexPath.Row]);              cell.Selected -= Medicine_Selected;             cell.UnSelected -= Medicine_UnSelected;              cell.Selected += Medicine_Selected;             cell.UnSelected += Medicine_UnSelected;              return cell;         }          public nint RowsInSection(UITableView tableView, nint section)         {             return MedicineList != null ? MedicineList.Count : 0;         }

        private void Medicine_Selected(object sender, MedicineInfo e)
        {             selectedMedicines.Add(e);              Validate();
        }

        private void Medicine_UnSelected(object sender, MedicineInfo e)
        {             selectedMedicines.Remove(e);              Validate();
        }          partial void Ok_Tapped(MediumActionButton sender)
        {
            if (selectedMedicines.Count > 0)             {                 MedicineSelected?.Invoke(this, selectedMedicines);             }              Close();
        }          partial void Close_Tapped(UIButton sender)
        {
            Close();
        }          private void Validate()         {             OkButton.Enabled = selectedMedicines.Count > 0;         }
    }
}

