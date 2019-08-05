using System; using UIKit; using System.Threading.Tasks; using System.Collections.Generic; using System.Linq; using Helseboka.Core.Common.Extension; using Helseboka.Core.Common.Constant; using Helseboka.Core.Common.EnumDefinitions; using Helseboka.iOS.Common.Extension; using Helseboka.iOS.Common.Constant; using Helseboka.iOS.Common.View; using Helseboka.Core.Common.Interfaces; using Foundation; using CoreGraphics;
using Helseboka.Core.MedicineModule.Model;
using System.Text;

namespace Helseboka.iOS.Medisiner.View
{
    public partial class RenewPrescriptionDialogView : BaseModalViewController
    {         public event EventHandler SendTapped;          private List<MedicineInfo> medicines; 
        public RenewPrescriptionDialogView(List<MedicineInfo> medicineList) : base("RenewPrescriptionDialogView")
        {             medicines = medicineList;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
             var stringBuilder = new StringBuilder();             foreach (var medicine in medicines)
            {
                stringBuilder.Append(medicine.NameFormStrength);                 stringBuilder.Append(Environment.NewLine);
            }              MedicineNameLabel.Text = stringBuilder.ToString();
        }          public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();             ScrollViewHeightConstraint.Constant = Math.Min((int)(UIScreen.MainScreen.Bounds.Height * 0.5), Math.Max(200, (int)ScrollView.ContentSize.Height));
        }          partial void Close_Tapped(UIButton sender)
        {             Close();
        }          partial void Send_Tapped(MediumActionButton sender)
        {             Close();             SendTapped?.Invoke(this, EventArgs.Empty);
        }

    }
}

