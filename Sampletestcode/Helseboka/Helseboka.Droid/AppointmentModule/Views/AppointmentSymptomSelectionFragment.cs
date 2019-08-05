
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
using Helseboka.Core.Common.Extension;
using Helseboka.Droid.Common.Utils;
using Helseboka.Core.Common.EnumDefinitions;
using Android.Support.V7.Widget;
using Helseboka.Droid.Common.Adapters;
using Helseboka.Core.AppointmentModule.Interface;
using Helseboka.Droid.Common.Interfaces;
using Android.Text;
using Android.Graphics;
using Android.Text.Style;
using Android.Support.V4.Content;

namespace Helseboka.Droid.AppointmentModule.Views
{
    public class AppointmentSymptomSelectionFragment : BaseFragment, IMultipleItemUniversalAdapter
    {
        private ImageView backButton;
        private Button nextButton;
        private ImageView helpButton;
        private RecyclerView dataListView;
        private List<String> symptomList = new List<String>() { String.Empty, String.Empty };

        private IAppointmentPresenter Presenter
        {
            get => presenter as IAppointmentPresenter;
        }

        public AppointmentSymptomSelectionFragment(IAppointmentPresenter presenter)
        {
            this.presenter = presenter;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_appointment_symptom_selection, null);

            backButton = view.FindViewById<ImageView>(Resource.Id.back);
            nextButton = view.FindViewById<Button>(Resource.Id.nextButton);
            helpButton = view.FindViewById<ImageView>(Resource.Id.helpButton);
            dataListView = view.FindViewById<RecyclerView>(Resource.Id.dataListView);

            dataListView.SetLayoutManager(new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false));
            dataListView.SetAdapter(new GenericRecyclerAdapter(this));

            backButton.Click += BackButton_Click;
            helpButton.Click += HelpButton_Click;
            nextButton.Click += NextButton_Click;

            dataListView.GetAdapter().NotifyDataSetChanged();

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
                mainActivity.ShowToolbar();
            }
        }

        void MainActivity_KeyboardVisible(object sender, int e)
        {
            if (Activity is IActivity mainActivity)
            {
                mainActivity.HideToolbar();
            }
        }

        void BackButton_Click(object sender, EventArgs e)
        {
            HideKeyboard();
            Presenter.GoBackToDateSelection();
        }

        public override bool OnBackKeyPressed()
        {
            Presenter.GoBackToDateSelection();
            return true;
        }

        void NextButton_Click(object sender, EventArgs e)
        {
            Presenter.AddAppointment(symptomList.Where(x => !String.IsNullOrEmpty(x)).ToList());
        }


        void HelpButton_Click(object sender, EventArgs e)
        {
            var help = new ModalHelpView(Activity, HelpFAQType.AppointmentSymptom);
            help.Show().Forget();
        }

        public int GetItemCount()
        {
            return symptomList.Count + 1;
        }

        public int GetItemViewType(int position)
        {
            return position < symptomList.Count ? 0 : 1;
        }

        public UniversalViewHolder CreateItemView(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.FromContext(Activity);
            var view = inflater.Inflate(viewType == 0 ? Resource.Layout.template_appointment_symptom_selection : Resource.Layout.template_appointment_symptom_selection_add_more, null);
            view.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

            var viewMap = new Dictionary<int, View>();

            if (viewType == 0)
            {
                viewMap.Add(Resource.Id.titleText, view.FindViewById(Resource.Id.titleText));
                viewMap.Add(Resource.Id.messageBody, view.FindViewById(Resource.Id.messageBody));
                viewMap.Add(Resource.Id.messageContainer, view.FindViewById(Resource.Id.messageContainer));
            }

            return new UniversalViewHolder(view, viewMap);
        }

        public void BindViewHolder(UniversalViewHolder holder, int position)
        {
            if (holder.ItemViewType == 0 && position < symptomList.Count)
            {
                holder.GetView<TextView>(Resource.Id.titleText).Text = $"{Resources.GetString(Resource.String.appointment_details_symptom_title)} {position + 1}";
                var editText = holder.GetView<EditText>(Resource.Id.messageBody);
                editText.Text = symptomList[position];
                editText.Tag = position;

                editText.TextChanged -= EditText_TextChanged;
                editText.FocusChange -= EditText_FocusChange;

                editText.TextChanged += EditText_TextChanged;
                editText.FocusChange += EditText_FocusChange;

                var placeholder = Resources.GetString(position == 0 ? Resource.String.appointment_symptom_selection_hint_1 : Resource.String.appointment_symptom_selection_hint_2);
                SpannableStringBuilder formattedHint = new SpannableStringBuilder(placeholder);
                var placeholderFont = Typeface.CreateFromAsset(Activity.Assets, "fonts/avenir_next_lt_pro_regular.otf");
                formattedHint.SetSpan(new StyleSpan(TypefaceStyle.Normal), 0, placeholder.Length, SpanTypes.InclusiveInclusive);
                formattedHint.SetSpan(new CustomTypefaceSpan("", placeholderFont), 0, placeholder.Length, SpanTypes.InclusiveInclusive);
                formattedHint.SetSpan(new AbsoluteSizeSpan(15.ConvertToPixel(Activity)), 0, placeholder.Length, SpanTypes.InclusiveInclusive);
                formattedHint.SetSpan(new ForegroundColorSpan(Color.ParseColor("#828282")), 0, placeholder.Length, 0);
                editText.HintFormatted = formattedHint;
            }
            else if(holder.ItemViewType == 1)
            {
                holder.ItemView.Click -= AddMore_Click;
                holder.ItemView.Click += AddMore_Click;
            }
        }

        void AddMore_Click(object sender, EventArgs e)
        {
            symptomList.Add(String.Empty);
            dataListView.GetAdapter().NotifyItemInserted(symptomList.Count - 1);
        }


        void EditText_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (sender is EditText view)
            {
                var position = (int)view.Tag;
                if (position < symptomList.Count)
                {
                    symptomList[position] = view.Text;
                }
                nextButton.Enabled = symptomList.Any(x => !String.IsNullOrEmpty(x));
            }
        }

        void EditText_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (sender is EditText view && view.Parent is View parentView)
            {
                parentView.SetBackgroundResource(e.HasFocus ? Resource.Drawable.shape_selectable_active_background : Resource.Drawable.shape_selectable_nodata_background);
            }
        }


        public void OnItemClick(int position)
        {
        }
    }
}
