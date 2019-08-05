
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Helseboka.Core.AppointmentModule.Interface;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Droid.Common.Views;
using Helseboka.Core.Common.Extension;
using System.Threading.Tasks;

namespace Helseboka.Droid.AppointmentModule.Views
{
    public class AppointmentConfirmationFragment : BaseFragment
    {
        private Button okButton;
        private ImageView helpButton;
        private TextView pageTitle;
        private TextView desctiptionText;

        private IAppointmentPresenter Presenter
        {
            get => presenter as IAppointmentPresenter;
        }

        public AppointmentConfirmationFragment(IAppointmentPresenter presenter)
        {
            this.presenter = presenter;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_appointment_confirmation, null);

            okButton = view.FindViewById<Button>(Resource.Id.okButton);
            helpButton = view.FindViewById<ImageView>(Resource.Id.helpButton);
            pageTitle = view.FindViewById<TextView>(Resource.Id.pageTitle);
            desctiptionText = view.FindViewById<TextView>(Resource.Id.desctiptionText);

            helpButton.Click += HelpButton_Click;
            okButton.Click += OkButton_Click;

            CheckDoctorDetails().Forget();

            return view;
        }

        void HelpButton_Click(object sender, EventArgs e)
        {
            var help = new ModalHelpView(Activity, HelpFAQType.AppointmentConfirmation);
            help.Show().Forget();
        }

        public override bool OnBackKeyPressed()
        {
            Presenter.GoBackToHome();
            return true;
        }

        void OkButton_Click(object sender, EventArgs e)
        {
            Presenter.GoBackToHome();
        }

        private async Task CheckDoctorDetails()
        {
            var response = await Presenter.GetDoctor();
            if (response.IsSuccess && response.Result != null && !response.Result.Enabled)
            {
                pageTitle.Text = Resources.GetString(Resource.String.appointment_confirmation_notenabled_title);
                desctiptionText.Text = response.Result.Remarks;
            }
        }
    }
}
