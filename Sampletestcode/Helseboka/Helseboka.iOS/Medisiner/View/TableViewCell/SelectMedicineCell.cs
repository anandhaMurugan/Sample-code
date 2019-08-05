using System; using UIKit; using System.Threading.Tasks; using System.Collections.Generic; using System.Linq; using Helseboka.Core.Common.Extension; using Helseboka.Core.Common.Constant; using Helseboka.Core.Common.EnumDefinitions; using Helseboka.iOS.Common.Extension; using Helseboka.iOS.Common.Constant; using Helseboka.iOS.Common.View; using Helseboka.Core.Common.Interfaces; using Foundation; using CoreGraphics;
using Helseboka.iOS.Common.TableViewCell;
using Helseboka.Core.MedicineModule.Model;
using Helseboka.Core.Common.Model;

namespace Helseboka.iOS.Medisiner.View.TableViewCell
{
    public partial class SelectMedicineCell : BaseTableViewCell
    {
        public static readonly NSString Key = new NSString("SelectMedicineCell");
         public new event EventHandler<MedicineInfo> Selected;         public event EventHandler<MedicineInfo> UnSelected;          private SelectableEntity<MedicineInfo> selectableMedicine; 
        protected SelectMedicineCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }          public void Configure(SelectableEntity<MedicineInfo> medicine)         {             this.selectableMedicine = medicine;             var atttibuted = new NSMutableAttributedString();             atttibuted.Append(new NSAttributedString(medicine.Entity.Name, Fonts.GetBoldFont(18), Colors.LoginHelpTextColor));             atttibuted.Append(new NSAttributedString($" {medicine.Entity.Strength}, {medicine.Entity.Form}", Fonts.GetNormalFont(14), Colors.LoginHelpTextColor));              MedicineName.AttributedText = atttibuted;              SelectCheckBox.SelectionChanged -= SelectCheckBox_SelectionChanged;             SelectCheckBox.Selected = medicine.IsSelected;             SelectCheckBox.SelectionChanged += SelectCheckBox_SelectionChanged;         }

        private void SelectCheckBox_SelectionChanged(object sender, bool e)
        {             selectableMedicine.IsSelected = e;             if (e)             {                 Selected?.Invoke(this, selectableMedicine.Entity);             }             else             {                 UnSelected?.Invoke(this, selectableMedicine.Entity);             }
        }      }
}
