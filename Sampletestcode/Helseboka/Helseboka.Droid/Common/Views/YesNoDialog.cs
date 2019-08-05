using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;

namespace Helseboka.Droid.Common.Views
{
    public class YesNoDialog
    {
        private Context context;
        private Dialog dialog;
        private String message;
        private String title;
        private Action onYesTapped;
        private Action onDialogClose;
        private String yesButtonText;
        private String noButtonText;

        private Button yesButton;
        private Button noButton;
        private TextView dialogTitle;
        private TextView dialogMessage;
        private ImageView closeButton;

        public Java.Lang.ICharSequence FormattedMessage { get; set; }


        public YesNoDialog(Context context, String message, String title, Action onYesTapped, String yesButtonText = "", String noButtonText = "", Action onDialogClose = null)
        {
            this.context = context;
            this.message = message;
            this.title = title;
            this.onYesTapped = onYesTapped;
            this.yesButtonText = yesButtonText;
            this.noButtonText = noButtonText;
            this.onDialogClose = onDialogClose;
        }

        public void Show()
        {
            var inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            var view = inflater.Inflate(Resource.Layout.dialog_yes_no, null);

            yesButton = view.FindViewById<Button>(Resource.Id.yesButton);
            noButton = view.FindViewById<Button>(Resource.Id.noButton);
            dialogMessage = view.FindViewById<TextView>(Resource.Id.dialogMessage);
            dialogTitle = view.FindViewById<TextView>(Resource.Id.dialogTitle);
            closeButton = view.FindViewById<ImageView>(Resource.Id.closeButton);

            dialogMessage.Text = message;
            dialogTitle.Text = title;

            if(!String.IsNullOrEmpty(yesButtonText))
            {
                yesButton.Text = yesButtonText;
            }

            if (!String.IsNullOrEmpty(noButtonText))
            {
                noButton.Text = noButtonText;
            }

            if (FormattedMessage != null)
            {
                dialogMessage.TextFormatted = FormattedMessage;
            }

            yesButton.Click += YesButton_Click;
            noButton.Click += NoButton_Click;
            closeButton.Click += CloseButton_Click;

            dialog = new Dialog(context, Android.Resource.Style.ThemeLightNoTitleBar);
            dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            dialog.SetContentView(view);
            dialog.SetCancelable(false);

            dialog.Show();
        }

        void YesButton_Click(object sender, EventArgs e)
        {
            onYesTapped?.Invoke();
            Close();
        }

        void NoButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Close()
        {
            if(dialog != null)
            {
                dialog.Dismiss();
                onDialogClose?.Invoke();
            }
        }

    }
}
