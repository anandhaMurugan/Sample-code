
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
using Helseboka.Droid.Common.Views;
using Helseboka.Droid.Common.Utils;
using Helseboka.Core.Common.Extension;
using Helseboka.Droid.Common.Adapters;
using Helseboka.Core.MedicineModule.Model;
using Android.Support.V7.Widget;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using Android.Support.V4.Content;

namespace Helseboka.Droid.MedicineModule.Views
{
    public class SelectMedicineFragment : IUniversalAdapter
    {
        private Activity activity;
        private Dialog dialog;
        private HashSet<MedicineInfo> selectedMedicines = new HashSet<MedicineInfo>();
        private List<MedicineInfo> MedicineList { get; set; }
        private MedicineInfo CurrentMedicine { get; set; }
        private Action<HashSet<MedicineInfo>> onMedicineSelected;

        private MaxHeightRecyclerView dataListView;
        private ImageView closeButton;
        private Button okButton;
        private TextView dialogTitle;

        public SelectMedicineFragment(Activity activity, List<MedicineInfo> selectableMedicines, MedicineInfo currentMedicine, Action<HashSet<MedicineInfo>> onMedicineSelected)
        {
            this.activity = activity;
            this.MedicineList = selectableMedicines;
            this.CurrentMedicine = currentMedicine;
            this.onMedicineSelected = onMedicineSelected;
        }

        public void Show()
        {
            var inflater = (LayoutInflater)activity.GetSystemService(Context.LayoutInflaterService);

            var view = inflater.Inflate(Resource.Layout.dialog_medicine_selection, null);

            dataListView = view.FindViewById<MaxHeightRecyclerView>(Resource.Id.dataListView);
            closeButton = view.FindViewById<ImageView>(Resource.Id.closeButton);
            okButton = view.FindViewById<Button>(Resource.Id.okButton);
            dialogTitle = view.FindViewById<TextView>(Resource.Id.dialogTitle);

            closeButton.Click += CloseButton_Click;
            okButton.Click += OkButton_Click;

            dataListView.SetLayoutManager(new LinearLayoutManager(activity, LinearLayoutManager.Vertical, false));
            dataListView.SetAdapter(new GenericRecyclerAdapter(this));

            dialog = new Dialog(activity, Android.Resource.Style.ThemeLightNoTitleBar);
            dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            dialog.SetContentView(view);
            dialog.SetCancelable(false);

            if (CurrentMedicine != null)
            {
                dialogTitle.Text = $"{activity.Resources.GetString(Resource.String.medicine_alarmview_select_medicine_title)} {CurrentMedicine.Name}:";
            }

            dialog.Show();

            dataListView.GetAdapter().NotifyDataSetChanged();
        }

        void OkButton_Click(object sender, EventArgs e)
        {
            if (selectedMedicines != null && selectedMedicines.Count > 0 && onMedicineSelected != null)
            {
                onMedicineSelected(selectedMedicines);
            }

            CloseDialog();
        }


        void CloseButton_Click(object sender, EventArgs e)
        {
            CloseDialog();
        }

        private void CloseDialog()
        {
            if (dialog != null)
            {
                dialog.Dismiss();
            }
        }

        public void BindViewHolder(UniversalViewHolder holder, int position)
        {
            if(MedicineList != null && position < MedicineList.Count)
            {
                var medicine = MedicineList[position];
                var selectionBox = holder.GetView<ImageView>(Resource.Id.selectionBox);
                var medicineName = holder.GetView<TextView>(Resource.Id.medicineName);
                var separator = holder.GetView<View>(Resource.Id.separator);

                separator.Visibility = position == MedicineList.Count - 1 ? ViewStates.Gone : ViewStates.Visible;
                selectionBox.Selected = selectedMedicines.Contains(medicine);

                var strengthText = $", {medicine.Strength}";
                var totalText = medicine.Name + strengthText;
                var formattedText = new SpannableStringBuilder(totalText);

                formattedText.SetSpan(new StyleSpan(TypefaceStyle.Bold), 0, medicine.Name.Length, SpanTypes.InclusiveInclusive);
                formattedText.SetSpan(new AbsoluteSizeSpan(18.ConvertToPixel(activity)), 0, medicine.Name.Length, SpanTypes.InclusiveInclusive);

                formattedText.SetSpan(new StyleSpan(TypefaceStyle.Normal), medicine.Name.Length, totalText.Length, SpanTypes.InclusiveInclusive);
                formattedText.SetSpan(new AbsoluteSizeSpan(14.ConvertToPixel(activity)), medicine.Name.Length, totalText.Length, SpanTypes.InclusiveInclusive);

                medicineName.TextFormatted = formattedText;
            }
        }

        public UniversalViewHolder CreateItemView(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.FromContext(activity);
            var view = inflater.Inflate(Resource.Layout.template_medicine_alarmview_selectmedicine, null);

            var viewMap = new Dictionary<int, View>();
            viewMap.Add(Resource.Id.selectionBox, view.FindViewById(Resource.Id.selectionBox));
            viewMap.Add(Resource.Id.medicineName, view.FindViewById(Resource.Id.medicineName));
            viewMap.Add(Resource.Id.separator, view.FindViewById(Resource.Id.separator));

            return new UniversalViewHolder(view, viewMap);
        }

        public int GetItemCount()
        {
            return MedicineList != null ? MedicineList.Count : 0;
        }

        public void OnItemClick(int position)
        {
            var viewHolder = dataListView.FindViewHolderForAdapterPosition(position) as UniversalViewHolder;
            var selectionBox = viewHolder.GetView<ImageView>(Resource.Id.selectionBox);

            selectionBox.Selected = !selectionBox.Selected;

            if(selectionBox.Selected)
            {
                selectedMedicines.Add(MedicineList[position]);
            }
            else
            {
                selectedMedicines.Remove(MedicineList[position]);
            }
        }
    }
}
