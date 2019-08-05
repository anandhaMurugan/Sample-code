using System; using UIKit; using System.Threading.Tasks; using System.Collections.Generic; using System.Linq; using Helseboka.Core.Common.Extension; using Helseboka.Core.Common.Constant; using Helseboka.Core.Common.EnumDefinitions; using Helseboka.iOS.Common.Extension; using Helseboka.iOS.Common.Constant; using Helseboka.iOS.Common.View; using Helseboka.Core.Common.Interfaces; using Foundation; using CoreGraphics;
using Helseboka.Core.AppointmentModule.Interface;

namespace Helseboka.iOS.Legetimer.View
{
    public partial class AppointmentConfirmationView : BaseView
    {
        public static readonly String Identifier = "AppointmentConfirmationView";
        public IAppointmentPresenter Presenter
        {
            get => presenter as IAppointmentPresenter;
            set => presenter = value;
        }

        public AppointmentConfirmationView() : base() { }

        public AppointmentConfirmationView(IntPtr handler) : base(handler) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();             CheckDoctorDetails().Forget();
        }          private async Task CheckDoctorDetails()         {             var response = await Presenter.GetDoctor();             if(response.IsSuccess && response.Result != null && !response.Result.Enabled)             {                 PageTitle.Text = "Appointment.Confirmation.NotEnabled.Title".Translate();                 DescriptionText.Text = response.Result.Remarks;                 RememberText.Hidden = true;             }         }          partial void Ok_Tapped(PrimaryActionButton sender)
        {
            Presenter.GoBackToHome();
        }          partial void Help_Tapped(UIButton sender)
        {
            new ModalHelpViewController(Core.Common.EnumDefinitions.HelpFAQType.AppointmentConfirmation).Show();
        }
    }
}

