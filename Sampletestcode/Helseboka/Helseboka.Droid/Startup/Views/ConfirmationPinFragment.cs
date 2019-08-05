
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
using Helseboka.Core.Startup.Interface;
using Helseboka.Droid.Common.Views;

namespace Helseboka.Droid.Startup.Views
{
    public class ConfirmationPinFragment : BaseFragment
    {
        EditText hiddenTextField;
        LinearLayout pinView;
        TextView pageTitle;
        bool isConfirmPIN = false;
        String pin;

        private IPINConfirmation Presenter
        {
            get => presenter as IPINConfirmation;
        }

        public ConfirmationPinFragment(IPINConfirmation presenter)
        {
            this.presenter = presenter;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_pin_confirmation, null);

            hiddenTextField = view.FindViewById<EditText>(Resource.Id.hiddenPasswordField);
            pinView = view.FindViewById<LinearLayout>(Resource.Id.pinview);
            pageTitle = view.FindViewById<TextView>(Resource.Id.pageTitle);

            pinView.Click += PinView_Click;
            hiddenTextField.TextChanged += HiddenTextField_TextChanged;

            ShowKeyboard(hiddenTextField);

            Activity.Window.SetSoftInputMode(SoftInput.AdjustNothing);

            return view;
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();

            Activity.Window.SetSoftInputMode(SoftInput.AdjustResize);
        }

        void PinView_Click(object sender, EventArgs e)
        {
            ShowKeyboard(hiddenTextField);
        }

        void HiddenTextField_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            for (int index = 0; index < 4; index++)
            {
                var view = pinView.FindViewWithTag(index.ToString());
                if (index < hiddenTextField.Text.Length)
                {
                    view.SetBackgroundResource(Resource.Drawable.shape_pin_fill);
                }
                else
                {
                    view.SetBackgroundResource(Resource.Drawable.shape_pin_empty);
                }
            }

            if (hiddenTextField.Text.Length == 4)
            {
                PIN_Completed();
            }
        }

        void PIN_Completed()
        {
            if (isConfirmPIN)
            {
                var confirmPIN = hiddenTextField.Text;
                if (pin.Equals(confirmPIN))
                {
                    HideKeyboard(hiddenTextField);
                    pinView.Enabled = false;
                    Presenter.PINSelectionCompleted(pin);
                }
                else
                {
                    pin = null;
                    pageTitle.Text = Resources.GetString(Resource.String.login_pin_new_header);
                    hiddenTextField.Text = String.Empty;
                    ShowKeyboard(hiddenTextField);
                    isConfirmPIN = false;
                }
            }
            else
            {
                pin = hiddenTextField.Text;
                pageTitle.Text = Resources.GetString(Resource.String.login_pin_confirm);
                hiddenTextField.Text = String.Empty;
                ShowKeyboard(hiddenTextField);
                isConfirmPIN = true;
            }
        }

    }
}
