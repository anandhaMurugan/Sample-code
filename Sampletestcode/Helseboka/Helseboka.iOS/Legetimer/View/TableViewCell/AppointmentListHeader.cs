using System;

using Foundation;
using Helseboka.iOS.Common.TableViewCell;
using UIKit;

namespace Helseboka.iOS.Legetimer.View.TableViewCell
{
    public partial class AppointmentListHeader : BaseTableViewCell
    {
        public static readonly NSString Key = new NSString("AppointmentListHeader");

        protected AppointmentListHeader(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public void Configure(String headerText, bool isShowLine)
        {
            SideLine.Hidden = !isShowLine;
            SectionHeaderTitleLabel.Text = headerText;
        }
    }
}
