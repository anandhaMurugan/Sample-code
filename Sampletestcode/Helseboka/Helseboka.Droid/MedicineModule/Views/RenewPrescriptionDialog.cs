
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Helseboka.Core.MedicineModule.Model;
using Helseboka.Droid.Common.Adapters;
using Helseboka.Droid.Common.Views;

namespace Helseboka.Droid.MedicineModule.Views
{
    public class RenewPrescriptionDialog : IUniversalAdapter
    {
        private Context context;
        private List<MedicineInfo> medicineList;
        private Dialog dialog;
        private MaxHeightRecyclerView dataListView;
        private ImageView closeButton;
        private Button sendButton;

        public event EventHandler SendTapped;

        public RenewPrescriptionDialog(Context context, List<MedicineInfo> medicineList)
        {
            this.context = context;
            this.medicineList = medicineList;
        }

        public void Show()
        {
            var inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);

            var view = inflater.Inflate(Resource.Layout.dialog_renew_prescription, null);

            dataListView = view.FindViewById<MaxHeightRecyclerView>(Resource.Id.dataListView);
            closeButton = view.FindViewById<ImageView>(Resource.Id.closeButton);
            sendButton = view.FindViewById<Button>(Resource.Id.sendButton);

            sendButton.Click += SendButton_Click;
            closeButton.Click += CloseButton_Click;

            var layoutManager = new LinearLayoutManager(context, LinearLayoutManager.Vertical, false);
            dataListView.SetLayoutManager(layoutManager);
            dataListView.SetAdapter(new GenericRecyclerAdapter(this));

            dialog = new Dialog(context, Android.Resource.Style.ThemeLightNoTitleBar);
            dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            dialog.SetContentView(view);
            dialog.SetCancelable(false);

            dialog.Show();

            dataListView.GetAdapter().NotifyDataSetChanged();
        }

        void SendButton_Click(object sender, EventArgs e)
        {
            if (dialog != null)
            {
                dialog.Dismiss();
            }
            SendTapped?.Invoke(this, EventArgs.Empty);
        }


        void CloseButton_Click(object sender, EventArgs e)
        {
            if (dialog != null)
            {
                dialog.Dismiss();
            }
        }

        public void BindViewHolder(UniversalViewHolder holder, int position)
        {
            if (medicineList != null && position < medicineList.Count)
            {
                holder.GetView<TextView>(Resource.Id.titleText).Text = medicineList[position].NameFormStrength;
            }
        }

        public UniversalViewHolder CreateItemView(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.FromContext(context);
            var view = inflater.Inflate(Resource.Layout.template_renew_prescription_medicine, null);

            var viewMap = new Dictionary<int, View>();
            viewMap.Add(Resource.Id.titleText, view.FindViewById(Resource.Id.titleText));

            return new UniversalViewHolder(view, viewMap);
        }

        public int GetItemCount()
        {
            return medicineList != null ? medicineList.Count : 0;
        }

        public void OnItemClick(int position)
        {
        }
    }
}
