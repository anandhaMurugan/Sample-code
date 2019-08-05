using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.OS;
using Android.Views;
using Android.Widget;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.Profile.Interface;
using Helseboka.Core.Profile.Model;
using Helseboka.Droid.Common.Interfaces;
using Helseboka.Droid.Common.Views;

namespace Helseboka.Droid.ProfileModule.Views
{
    public class PersonalSettingsFragment : BaseFragment
    {
        private ImageView back;
        private Button saveButton;
        private TextView pageTitle;
        private TextView dobValue;
        private TextView genderValue;
        private EditText addressValue;
        private EditText telephoneValue;
        private TextView errorLabel;
        private TextView errorTextLabel;
        private ScrollView scrollView;

        private bool isKeyboardVisible = false;
        private User currentUser;

        private IProfilePresenter Presenter
        {
            get => presenter as IProfilePresenter;
        }

        public PersonalSettingsFragment(IProfilePresenter presenter)
        {
            this.presenter = presenter;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_profile_personal_settings, null);

            back = view.FindViewById<ImageView>(Resource.Id.back);
            saveButton = view.FindViewById<Button>(Resource.Id.saveButton);
            pageTitle = view.FindViewById<TextView>(Resource.Id.pageTitle);
            dobValue = view.FindViewById<TextView>(Resource.Id.dobValue);
            genderValue = view.FindViewById<TextView>(Resource.Id.genderValue);
            addressValue = view.FindViewById<EditText>(Resource.Id.addressValue);
            telephoneValue = view.FindViewById<EditText>(Resource.Id.telephoneValue);
            errorLabel = view.FindViewById<TextView>(Resource.Id.errorLabel);
            errorTextLabel = view.FindViewById<TextView>(Resource.Id.errorTextLabel);
            scrollView = view.FindViewById<ScrollView>(Resource.Id.scrollView);

            errorLabel.Visibility = ViewStates.Gone;
            telephoneValue.TextChanged += MobilePhoneNumber_TextChanged;

            back.Click += Back_Click;
            saveButton.Click += Save_Click;

            LoadData().Forget();

            if (Activity is IActivity mainActivity)
            {
                mainActivity.AttachKeyboardListner();
                mainActivity.KeyboardHide -= MainActivity_KeyboardHide;
                mainActivity.KeyboardVisible -= MainActivity_KeyboardVisible;
                mainActivity.KeyboardHide += MainActivity_KeyboardHide;
                mainActivity.KeyboardVisible += MainActivity_KeyboardVisible;
            }

            return view;
        }

        private void MobilePhoneNumber_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Text.ToString()))
            {
                errorLabel.Visibility = ViewStates.Invisible;  
            }
            else
            {
                Regex regex = new Regex(@"^[0-9+ ]+$");
                if (!regex.IsMatch(e.Text.ToString()))
                {
                    errorTextLabel.Visibility = ViewStates.Visible;
                    scrollView.FullScroll(FocusSearchDirection.Down);
                }
                else
                {
                    errorTextLabel.Visibility = ViewStates.Gone;
                    errorLabel.Visibility = ViewStates.Invisible;
                }
            }
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            if (Activity is IActivity mainActivity)
            {
                mainActivity.KeyboardHide -= MainActivity_KeyboardHide;
                mainActivity.KeyboardVisible -= MainActivity_KeyboardVisible;
                mainActivity.RemoveKeyboardListner();
            }
        }

        void MainActivity_KeyboardHide(object sender, EventArgs e)
        {
            if (Activity is IActivity mainActivity)
            {
                isKeyboardVisible = false;
                mainActivity.ShowToolbar();
            }
        }

        void MainActivity_KeyboardVisible(object sender, int e)
        {
            if (Activity is IActivity mainActivity)
            {
                isKeyboardVisible = true;
                mainActivity.HideToolbar();
            }
        }

        void Back_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(telephoneValue.Text))
            {
                errorLabel.Visibility = ViewStates.Visible;
                scrollView.FullScroll(FocusSearchDirection.Down);
            }
            else
            {
                if (errorTextLabel.Visibility != ViewStates.Visible && errorLabel.Visibility != ViewStates.Visible)
                {
                    OnBackKeyPressed();
                }
            }
        }

        void Save_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(telephoneValue.Text))
            {
                errorLabel.Visibility = ViewStates.Visible;
                scrollView.FullScroll(FocusSearchDirection.Down);
            }
            else
            {
                if (errorTextLabel.Visibility != ViewStates.Visible && errorLabel.Visibility != ViewStates.Visible)
                {
                    HideKeyboard();
                    Save().Forget();
                }
            }
        }

        public override bool OnBackKeyPressed()
        {
            if (isKeyboardVisible)
            {
                LoadData().Forget();
                HideKeyboard();
            }
            else
            {
                Presenter.GoBackToHome();
            }
            return true;
        }

        private async Task LoadData()
        {
            currentUser = await Presenter.GetCurrentUserProfile();
            if (currentUser != null)
            {
                pageTitle.Text = $"{currentUser.FirstName} {currentUser.LastName}";
                dobValue.Text = currentUser.DateOfBirth;
                genderValue.Text = currentUser.Gender;
                addressValue.Text = currentUser.Address;
                telephoneValue.Text = currentUser.Phone;
            }
        }

        private async Task Save()
        {
            var response = await Presenter.UpdateMobile(telephoneValue.Text, addressValue.Text);
            if (response.IsSuccess)
            {
                await LoadData();
            }
        }
    }
}