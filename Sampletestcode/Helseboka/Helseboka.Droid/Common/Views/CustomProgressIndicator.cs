using System;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Views;
using Android.Widget;

namespace Helseboka.Droid.Common.Views
{
    public class CustomProgressIndicator
    {
        private Dialog dialog;
        private Context context;

        public CustomProgressIndicator(Context context)
        {
            this.context = context;
        }


        public void Show()
        {
            try
            {
                LayoutInflater inflator = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
                View view = inflator.Inflate(Resource.Layout.dialog_loading, null);

                dialog = new Dialog(context, Resource.Style.dialog_loading_style);
                dialog.SetContentView(view);
                dialog.SetCancelable(false);
                dialog.Show();
            }
            catch (Exception ex)
            {
                
            }
        }

        public void Dismiss()
        {
            if (dialog != null)
            {
                dialog.Dismiss();
            }
        }
    }
}
