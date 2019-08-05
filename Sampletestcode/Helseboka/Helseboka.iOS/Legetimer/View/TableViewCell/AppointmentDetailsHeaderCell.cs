using System;
using Helseboka.Core.AppointmentModule.Model;
using Foundation;
using UIKit;
using Helseboka.iOS.Common.TableViewCell;
using Helseboka.Core.Resources.StringResources;
using Helseboka.iOS.Common.View;

namespace Helseboka.iOS.Legetimer.View.TableViewCell
{
    public partial class AppointmentDetailsHeaderCell : BaseTableViewCell
    {
        public static readonly NSString Key = new NSString("AppointmentDetailsHeaderCell");

        protected AppointmentDetailsHeaderCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            FocusedMessageFromDoctor.TextInsets = new UIEdgeInsets(10, 20, 10, 20);
            FocusedMessageFromDoctor.Delegate= new LinkDelegate(Link_Tapped);
        }

        public void UpdateCell(AppointmentDetails data)
		{
            if(!String.IsNullOrEmpty(data.DoctorFocusedReply))
            {
                FocusedMessageFromDoctor.Text = data.DoctorFocusedReply;
            }
            else
            {
                FocusedMessageFromDoctor.Text = AppResources.AppointmentNoMessage;
            }
		}

        public void Link_Tapped(NSUrl obj)
        {
            UIApplication.SharedApplication.OpenUrl(obj);
        }
    }
}
