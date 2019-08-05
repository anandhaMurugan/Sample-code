
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.Profile.Interface;
using Helseboka.Core.Profile.Model;
using Helseboka.Droid.Common.Utils;
using Helseboka.Droid.Common.Views;


namespace Helseboka.Droid.ProfileModule.Views
{
    public class DoctorDetailsFragment : BaseFragment
    {
        private ImageView back;
        private TextView medicalOfficeValue;
        private TextView doctorValue;
        private TextView changeDoctorDetails;
        private TextView address;
        private TextView telephoneValue;

        private User currentUser;
        private IProfilePresenter Presenter
        {
            get => presenter as IProfilePresenter;
        }

        public DoctorDetailsFragment(IProfilePresenter presenter)
        {
            this.presenter = presenter;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_profile_doctor_details, null);

            back = view.FindViewById<ImageView>(Resource.Id.back);
            medicalOfficeValue = view.FindViewById<TextView>(Resource.Id.medicalOfficeValue);
            doctorValue = view.FindViewById<TextView>(Resource.Id.doctorValue);
            changeDoctorDetails = view.FindViewById<TextView>(Resource.Id.changeDoctorDetails);
            address = view.FindViewById<TextView>(Resource.Id.address);
            telephoneValue = view.FindViewById<TextView>(Resource.Id.telephoneValue);

            back.Click += Back_Click;
            changeDoctorDetails.Click += ChangeDoctorDetails_Click;

            LoadTableData().Forget();

            return view;
        }

        void Back_Click(object sender, EventArgs e)
        {
            Presenter.GoBackToHome();
        }

        public override bool OnBackKeyPressed()
        {
            Presenter.GoBackToHome();
            return true;
        }

        void ChangeDoctorDetails_Click(object sender, EventArgs e)
        {
            Presenter.ShowDoctorSelectionView();
        }


        private async Task LoadTableData()
        {
            currentUser = await Presenter.GetCurrentUserProfile();
            if (currentUser != null)
            {
                medicalOfficeValue.Text = currentUser.AssignedDoctor.OfficeName.ToNameCase();
                doctorValue.Text = $"{currentUser.AssignedDoctor.FullName}";
                address.Text = $"{currentUser.AssignedDoctor.OfficeStreet}\n{currentUser.AssignedDoctor.OfficeZip} {currentUser.AssignedDoctor.OfficeCity}";
                telephoneValue.Text = String.Format("{0:## ### ###}", currentUser.AssignedDoctor.PhoneNumber);
            }
        }
    }
}
