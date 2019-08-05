using System;
using Helseboka.Core.Startup.Interface;
using Helseboka.iOS.Common.View;
using Helseboka.iOS.Common.Extension;
using UIKit;
using Helseboka.iOS.Common.Utilities;
using Helseboka.iOS.Common.PlatformEnums;

namespace Helseboka.iOS.Startup.View
{
    public partial class PINConfirmation : BaseView
    {
        public IPINConfirmation Presenter
        {
            get => presenter as IPINConfirmation;
            set => presenter = value;
        }

		private bool isConfirmPIN = false;
		private String pin;

		public PINConfirmation() { }

		public PINConfirmation(IntPtr handler) : base(handler) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            if (Device.DeviceType == DeviceType.iPhones_5_5s_5c_SE)
            {
                PINViewTopConstraint.Constant = 30;
            }
        }

		public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            PINView.Completed += PINView_Completed;
            PINView.Focus();
        }

		public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            PINView.Completed -= PINView_Completed;
        }

		private void PINView_Completed(object sender, string e)
        {
			if(isConfirmPIN)
			{
				var confirmPIN = PINView.Password;
				if (pin.Equals(confirmPIN))
				{
                    PINView.UnFocus();
                    PINView.UserInteractionEnabled = false;
                    Presenter.PINSelectionCompleted(pin);
                    PINView.UserInteractionEnabled = true;
				}
				else
				{
					pin = null;
                    PageHeaderLabel.Text = "Login.PIN.New.Header".Translate();
                    PINView.Clear();
                    PINView.Focus();
					isConfirmPIN = false;
				}
			}
			else
			{
				pin = PINView.Password;
                PageHeaderLabel.Text = "Login.PIN.Confirm".Translate();
				PINView.Clear();
				PINView.Initialize();
				isConfirmPIN = true;
			}
        }
    }
}

