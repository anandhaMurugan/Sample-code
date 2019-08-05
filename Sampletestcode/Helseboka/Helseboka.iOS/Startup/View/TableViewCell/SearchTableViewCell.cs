using System;

using Foundation;
using Helseboka.iOS.Common.Constant;
using UIKit;

namespace Helseboka.iOS.Startup.View.TableViewCell
{
    public partial class SearchTableViewCell : UITableViewCell
    {
        protected SearchTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public SearchTableViewCell()
        {
            CommonInitialization();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            CommonInitialization();
        }

        private void CommonInitialization()
        {
            var selectionBackView = new UIView();
            selectionBackView.BackgroundColor = Colors.SearchResultBackground;
            this.SelectedBackgroundView = selectionBackView;
        }

        public void Configure(String text)
        {
            DescriptionTextLabel.Text = text;
        }
    }
}
