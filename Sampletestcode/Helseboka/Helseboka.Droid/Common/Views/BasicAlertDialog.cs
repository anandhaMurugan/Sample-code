using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;

namespace Helseboka.Droid.Common.Views
{
    public class BasicAlertDialog
    {
        private Context context;
        private Dialog dialog;
        private String message;
        private String title;
        private String buttonText;
        private Action OnCompleted;

        private Button okButton;
        private TextView dialogMessage;

        public IntPtr Handle => throw new NotImplementedException();

        public BasicAlertDialog(Context context, String message, String title = "", String buttonText = "", Action onCompleted = null)
        {
            this.context = context;
            this.message = message;
            this.title = title;
            this.buttonText = buttonText;
            this.OnCompleted = onCompleted;
        }

        public void Show()
        {
            var inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            var view = inflater.Inflate(Resource.Layout.dialog_basic_alert, null);

            okButton = view.FindViewById<Button>(Resource.Id.okButton);
            dialogMessage = view.FindViewById<TextView>(Resource.Id.dialogMessage);

            dialogMessage.Text = message;

            if (!String.IsNullOrEmpty(title))
            {
                view.FindViewById<TextView>(Resource.Id.dialogTitle).Text = title;
            }

            if (!String.IsNullOrEmpty(buttonText))
            {
                okButton.Text = title;
            }

            okButton.Click += OkButton_Click;

            dialog = new Dialog(context, Android.Resource.Style.ThemeLightNoTitleBar);
            dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            dialog.SetContentView(view);
            dialog.SetCancelable(false);
            dialog.DismissEvent += Dialog_DismissEvent;

            dialog.Show();
        }

        void OkButton_Click(object sender, EventArgs e)
        {
            if (dialog != null)
            {
                dialog.Dismiss();
            }
        }

        void Dialog_DismissEvent(object sender, EventArgs e)
        {
            OnCompleted?.Invoke();
            this.OnCompleted = null;
        }
       
    }
}
