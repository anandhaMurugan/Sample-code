using System;

using Foundation;
using Helseboka.iOS.Common.Constant;
using Helseboka.iOS.Common.TableViewCell;
using UIKit;

namespace Helseboka.iOS.Medisiner.View
{
    public partial class MedicineSearchTableViewCell : BaseTableViewCell
    {
        public static readonly NSString Key = new NSString("MedicineSearchTableViewCell");

        protected MedicineSearchTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public MedicineSearchTableViewCell()
        {
            CommonInitialization();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            CommonInitialization();
        }

        public void Configure(String text)
        {
            SearchLabel.Text = text;
        }

        private void CommonInitialization()
        {
            var selectionBackView = new UIView();
            selectionBackView.BackgroundColor = Colors.SearchResultBackground;
            this.SelectedBackgroundView = selectionBackView;
        }
    }
}
