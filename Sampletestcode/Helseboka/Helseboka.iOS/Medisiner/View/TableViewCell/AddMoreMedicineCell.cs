using System;

using Foundation;
using Helseboka.Core.MedicineModule.Model;
using Helseboka.iOS.Common.Constant;
using UIKit;

namespace Helseboka.iOS.Medisiner.View.TableViewCell
{
    public partial class AddMoreMedicineCell : UITableViewCell
    {
        private MedicineInfo medicineInfo;
        public event EventHandler<MedicineInfo> RemoveTapped;

        public static readonly NSString Key = new NSString("AddMoreMedicineCell");

        protected AddMoreMedicineCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public void Configure(MedicineInfo medicine)
        {
            this.medicineInfo = medicine;
            var medicineAttributedText = new NSMutableAttributedString();
            medicineAttributedText.Append(new NSAttributedString(medicine.Name, Fonts.GetBoldFont(18), Colors.LoginHelpTextColor));
            medicineAttributedText.Append(new NSAttributedString($", {medicine.Strength}", Fonts.GetNormalFont(14)));
            MedicineLabel.AttributedText = medicineAttributedText;
        }

        partial void MedicineClose_Tapped(UIButton sender)
        {
            RemoveTapped?.Invoke(this, medicineInfo);
        }
    }
}
