using System;
using Android.Graphics;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.MobilephoneNumber.Interface;
using Helseboka.Core.Profile.Model;
using Helseboka.Droid.Common.Interfaces;
using Helseboka.Droid.Common.Views;
using System.Text.RegularExpressions;

namespace Helseboka.Droid.MobilephoneNumber.Views
{
    public class MobilePhoneNumberFragment : BaseFragment
    {
        private Context context;
        private Dialog dialog;
        private Action onDialogClose;
        private Button okButton;
        private EditText mobilePhoneNumber;
        private TextView errorMessage;
        private TextView errorMessageText;

        private User currentUser;

        public IMobilePhoneNumberPresenter Presenter
        {
            get => presenter as IMobilePhoneNumberPresenter;
        }

        public MobilePhoneNumberFragment(IMobilePhoneNumberPresenter presenter, Context context)
        {
            this.presenter = presenter;
            this.context = context;
        }

        public void Show()
        {
            var inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            var view = inflater.Inflate(Resource.Layout.fragment_mobilephone_number, null);

            okButton = view.FindViewById<Button>(Resource.Id.Okbutton);
            mobilePhoneNumber = view.FindViewById<EditText>(Resource.Id.mobilephoneNumber);
            errorMessage = view.FindViewById<TextView>(Resource.Id.errorlabel);
            errorMessageText = view.FindViewById<TextView>(Resource.Id.errorlabeltext);

            errorMessage.Visibility = ViewStates.Gone;
            mobilePhoneNumber.TextChanged += MobilephoneNumber_TextChanged;
            okButton.Click += OkButton_Clicked;

            dialog = new Dialog(context, Android.Resource.Style.ThemeLightNoTitleBar);
            dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            dialog.SetContentView(view);
            dialog.SetCancelable(false);

            LoadData().Forget();
            dialog.Show();           
        }

        private void MobilephoneNumber_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
           if (string.IsNullOrEmpty(e.Text.ToString()))
            {
                errorMessage.Visibility = ViewStates.Invisible;
            }
            else
            {
                Regex regex = new Regex(@"^[0-9+ ]+$");
                if (!regex.IsMatch(e.Text.ToString()))
                {
                    errorMessageText.Visibility = ViewStates.Visible;
                }
                else
                {
                    errorMessageText.Visibility = ViewStates.Gone;
                    errorMessage.Visibility = ViewStates.Invisible;
                }
            }
        }

        void OkButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(mobilePhoneNumber.Text))
            {
                errorMessage.Visibility = ViewStates.Visible;
            }
            else
            { 
                if (errorMessageText.Visibility != ViewStates.Visible && errorMessage.Visibility != ViewStates.Visible)
                {
                    HideKeyboard();
                    Save().Forget();
                }       
            }
        }

        private async Task LoadData()
        {
            currentUser = await Presenter.GetCurrentUserProfile();
            if (currentUser != null)
            {
                mobilePhoneNumber.Text = currentUser.Phone;
            }
        }

        private async Task Save()
        {
            var response = await Presenter.UpdateMobilePhoneNumber(mobilePhoneNumber.Text);
            if (response.IsSuccess)
            {
                await LoadData();
                Close();
                Presenter.UpdateSuccess();
            }
        }

        private void Close()
        {
            if (dialog != null)
            {
                dialog.Dismiss();
                onDialogClose?.Invoke();
            }
        }
    }
}