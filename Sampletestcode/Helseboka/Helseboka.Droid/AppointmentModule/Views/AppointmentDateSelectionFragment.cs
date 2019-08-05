
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
using Helseboka.Core.AppointmentModule.Interface;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Model;
using System.Threading.Tasks;
using Android.Text;
using Android.Text.Style;
using Android.Graphics;

namespace Helseboka.Droid.AppointmentModule.Views
{
    public class AppointmentDateSelectionFragment : BaseFragment
    {
        private ImageView backButton;
        private LinearLayout weekViewContainer;
        private Button nextButton;
        private ImageView helpButton;
        private ImageView morningSelection;
        private ImageView middaySelection;
        private ImageView afternoonSelection;
        private TextView timeSelectionHelpText;

        private HashSet<DayOfWeek> selectedDays = new HashSet<DayOfWeek>();
        private HashSet<TimeOfDay> availableTimes = new HashSet<TimeOfDay>();

        private IAppointmentPresenter Presenter
        {
            get => presenter as IAppointmentPresenter;
        }

        public AppointmentDateSelectionFragment(IAppointmentPresenter presenter)
        {
            this.presenter = presenter;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_new_appointment_date_selection, null);

            backButton = view.FindViewById<ImageView>(Resource.Id.back);
            weekViewContainer = view.FindViewById<LinearLayout>(Resource.Id.weekViewContainer);
            nextButton = view.FindViewById<Button>(Resource.Id.nextButton);
            helpButton = view.FindViewById<ImageView>(Resource.Id.helpButton);
            morningSelection = view.FindViewById<ImageView>(Resource.Id.morningSelection);
            middaySelection = view.FindViewById<ImageView>(Resource.Id.middaySelection);
            afternoonSelection = view.FindViewById<ImageView>(Resource.Id.afternoonSelection);
            timeSelectionHelpText = view.FindViewById<TextView>(Resource.Id.timeSelectionHelpText);

            backButton.Click += BackButton_Click;
            helpButton.Click += HelpButton_Click;
            nextButton.Click += NextButton_Click;

            morningSelection.Click += MorningSelection_Click;
            middaySelection.Click += MiddaySelection_Click;
            afternoonSelection.Click += AfternoonSelection_Click;

            LoadDoctorTelephone().Forget();

            LoadWeekView();

            return view;
        }

        private async Task LoadDoctorTelephone()
        {
            var helpText = Resources.GetString(Resource.String.appointment_dateselection_help);
            var response = await Presenter.GetDoctor();
            if(response.IsSuccess && response.Result != null && !String.IsNullOrEmpty(response.Result.EmergencyNumber))
            {
                var totalText = $"{helpText} {response.Result.EmergencyNumber}.";
                var formattedText = new SpannableStringBuilder(totalText);
                formattedText.SetSpan(new ForegroundColorSpan(Color.Black), helpText.Length + 1, totalText.Length - 1, SpanTypes.InclusiveInclusive);
                formattedText.SetSpan(new StyleSpan(TypefaceStyle.Bold), helpText.Length + 1, totalText.Length - 1, SpanTypes.InclusiveInclusive);
                formattedText.SetSpan(new UnderlineSpan(), helpText.Length + 1, totalText.Length - 1, SpanTypes.InclusiveInclusive);

                timeSelectionHelpText.TextFormatted = formattedText;
            }
            else
            {
                timeSelectionHelpText.Text = $"{helpText}.";
            }
        }

        void BackButton_Click(object sender, EventArgs e)
        {
            Presenter.GoBackToHome();
        }

        public override bool OnBackKeyPressed()
        {
            Presenter.GoBackToHome();
            return true;
        }

        void NextButton_Click(object sender, EventArgs e)
        {
            Presenter.DidSelectAppointmentDateTime(selectedDays.ToList(), availableTimes.ToList());
        }


        void HelpButton_Click(object sender, EventArgs e)
        {
            var help = new ModalHelpView(Activity, HelpFAQType.AppointmentDateSelection);
            help.Show().Forget();
        }

        void MorningSelection_Click(object sender, EventArgs e)
        {
            morningSelection.Selected = !morningSelection.Selected;
            AddRemoveAvailableTime(TimeOfDay.Early, morningSelection.Selected);
        }

        void MiddaySelection_Click(object sender, EventArgs e)
        {
            middaySelection.Selected = !middaySelection.Selected;
            AddRemoveAvailableTime(TimeOfDay.Midday, middaySelection.Selected);
        }

        void AfternoonSelection_Click(object sender, EventArgs e)
        {
            afternoonSelection.Selected = !afternoonSelection.Selected;
            AddRemoveAvailableTime(TimeOfDay.Late, afternoonSelection.Selected);
        }


        void WeekView_Click(object sender, EventArgs e)
        {
            if (sender is View view)
            {
                var selectionBox = view.FindViewById<ImageView>(Resource.Id.selectionBox);
                selectionBox.Selected = !selectionBox.Selected;

                var day = (DayOfWeek)((int)view.Tag);
                var isExist = selectedDays.Contains(day);
                if (selectionBox.Selected)
                {
                    if (!isExist)
                    {
                        selectedDays.Add(day);
                    }
                }
                else
                {
                    if (isExist)
                    {
                        selectedDays.Remove(day);
                    }
                }
            }
        }

        private void AddRemoveAvailableTime(TimeOfDay time, bool isSelected)
        {
            if (isSelected)
            {
                availableTimes.Add(time);
            }
            else
            {
                availableTimes.Remove(time);
            }
        }

        private void LoadWeekView()
        {
            for (int index = 1; index <= 5; index++)
            {
                DayOfWeek day = (DayOfWeek)index;
                String localizedDay = $"appointment_new_selecttime_{day.ToString().ToLower()}".Translate(Activity);

                var view = Activity.LayoutInflater.Inflate(Resource.Layout.template_weekview, null);
                var selectionBox = view.FindViewById<ImageView>(Resource.Id.selectionBox);
                var parentLayoutParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                parentLayoutParams.Gravity = GravityFlags.CenterVertical;
                view.LayoutParameters = parentLayoutParams;

                view.Tag = (int)day;
                view.FindViewById<TextView>(Resource.Id.dayLabel).Text = localizedDay;

                view.Click += WeekView_Click;

                weekViewContainer.AddView(view);
                if (index != 6)
                {
                    var space = new Space(Activity);
                    space.LayoutParameters = new LinearLayout.LayoutParams(0, 1, 1);
                    weekViewContainer.AddView(space);
                }
            }
        }
    }
}
