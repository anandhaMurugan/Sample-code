using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Helseboka.Core.Legedialog.Interface;
using Helseboka.Droid.Common.Views;
using Helseboka.Droid.Common.Utils;
using System.Threading.Tasks;
using Helseboka.Core.Common.Extension;
using Android.Text;
using Android.Text.Style;
using Android.Graphics;
using Helseboka.Core.Common.Model;

namespace Helseboka.Droid.Chat.Views
{
    public class NewChatFragment : BaseFragment
    {
        TextView messageToDoctor;
        TextView messageToDoctorHint;
        TextView messageToMedicalCenter;
        TextView messageToMedicalCenterHint;
        TextView toTextViewCollapsed;
        EditText messageSubjectField;
        EditText messageBody;
        Button sendMessageButton;
        ImageView back;
        RelativeLayout messageToContainerCollapsed;
        RelativeLayout messageToContainerExpanded;
        RelativeLayout messageToContainer;
        ImageView downArrow;
        View firstSeparator;
        ImageView separatorShadow;

        private bool? isDoctorSelected;

        public INewLegeDialogPresenter Presenter
        {
            get => presenter as INewLegeDialogPresenter;
        }

        public NewChatFragment(INewLegeDialogPresenter presenter)
        {
            this.presenter = presenter;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_new_chat, null);

            messageToContainerCollapsed = view.FindViewById<RelativeLayout>(Resource.Id.messageToContainerCollapsed);
            messageToContainerExpanded = view.FindViewById<RelativeLayout>(Resource.Id.messageToContainerExpanded);
            messageToContainer = view.FindViewById<RelativeLayout>(Resource.Id.messageToContainer);

            messageToDoctor = view.FindViewById<TextView>(Resource.Id.messageToDoctor);
            messageToDoctorHint = view.FindViewById<TextView>(Resource.Id.messageToDoctorHint);
            messageToMedicalCenter = view.FindViewById<TextView>(Resource.Id.messageToMedicalCenter);
            messageToMedicalCenterHint = view.FindViewById<TextView>(Resource.Id.messageToMedicalCenterHint);
            toTextViewCollapsed = view.FindViewById<TextView>(Resource.Id.toTextViewCollapsed);
            downArrow = view.FindViewById<ImageView>(Resource.Id.downArrow);
            messageSubjectField = view.FindViewById<EditText>(Resource.Id.messageSubjectField);
            messageBody = view.FindViewById<EditText>(Resource.Id.messageBody);
            sendMessageButton = view.FindViewById<Button>(Resource.Id.sendMessageButton);
            back = view.FindViewById<ImageView>(Resource.Id.back);
            firstSeparator = view.FindViewById<View>(Resource.Id.firstSeparator);
            separatorShadow = view.FindViewById<ImageView>(Resource.Id.separatorShadow);

            messageToDoctor.Selected = true;

            messageToContainerCollapsed.Click += MessageToContainerCollapsed_Click;
            messageToDoctor.Click += MessageToDoctor_Click;
            messageToMedicalCenter.Click += MessageToMedicalCenter_Click;
            messageSubjectField.TextChanged += MessageSubjectField_TextChanged;
            messageBody.TextChanged += MessageBody_TextChanged;
            messageSubjectField.FocusChange += EditText_FocusChange;
            messageBody.FocusChange += EditText_FocusChange;
            back.Click += Back_Click;
            sendMessageButton.Click += SendMessageButton_Click;

            LoadDoctorDetails().Forget();

            return view;
        }

        void MessageToDoctor_Click(object sender, EventArgs e)
        {
            isDoctorSelected = true;
            messageToDoctor.Selected = true;
            messageToMedicalCenter.Selected = false;
            HideSelection();
            CheckForm();
        }

        void MessageToMedicalCenter_Click(object sender, EventArgs e)
        {
            isDoctorSelected = false;
            messageToMedicalCenter.Selected = true;
            messageToDoctor.Selected = false;
            HideSelection();
            CheckForm();
        }

        void Back_Click(object sender, EventArgs e)
        {
            HideKeyboard(messageBody);
            HideKeyboard(messageSubjectField);
            Presenter.GoBack();
        }

        public override bool OnBackKeyPressed()
        {
            Presenter.GoBack();
            return true;
        }

        void EditText_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus && messageToContainerExpanded.Visibility == ViewStates.Visible)
            {
                HideSelection();
            }
        }


        void MessageSubjectField_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            CheckForm();
        }

        void MessageBody_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            CheckForm();
        }

        void SendMessageButton_Click(object sender, EventArgs e)
        {
            HideKeyboard();
            CheckDoctorAndProceed(() =>
            {
                Createthread().Forget();
            });
        }

        void MessageToContainerCollapsed_Click(object sender, EventArgs e)
        {
            HideKeyboard();

            messageToContainerCollapsed.Visibility = ViewStates.Invisible;
            messageToContainerExpanded.Visibility = ViewStates.Visible;

            int widthSpec = View.MeasureSpec.MakeMeasureSpec(messageToContainer.Width, MeasureSpecMode.Exactly);
            int heightSpec = View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
            messageToContainerExpanded.Measure(widthSpec, heightSpec);

            ValueAnimator mAnimator = GetSlideAnimator(0, messageToContainerExpanded.MeasuredHeight, messageToContainerExpanded);
            mAnimator.AnimationEnd += (x, y) =>
            {
                separatorShadow.Visibility = ViewStates.Visible;
                firstSeparator.Visibility = ViewStates.Gone;
                messageToContainerCollapsed.Visibility = ViewStates.Gone;
            };
            mAnimator.Start();

            downArrow.Animate().Rotation(180);
        }

        private async Task Createthread()
        {
            if (CheckForm())
            {
                ShowLoader();
                var response = await Presenter.CreateThread(messageSubjectField.Text, messageBody.Text, isDoctorSelected.Value);
                HideLoader();
                if (response.IsSuccess)
                {
                    Presenter.GoBack();
                }
            }
        }

        private bool CheckForm()
        {
            var isValid = isDoctorSelected.HasValue && !String.IsNullOrEmpty(messageSubjectField.Text) && !String.IsNullOrEmpty(messageBody.Text);
            sendMessageButton.Enabled = isValid;

            return isValid;
        }

        private void HideSelection()
        {
            if(isDoctorSelected.HasValue)
            {
                var selected = isDoctorSelected.Value ? messageToDoctor.Text : messageToMedicalCenter.Text;
                toTextViewCollapsed.Text = $"{Resources.GetString(Resource.String.chat_newchat_to)} {selected}";
                toTextViewCollapsed.SetTextAppearance(Resource.Style.chat_newchat_label_style);
            }

            ValueAnimator mAnimator = GetSlideAnimator(messageToContainerExpanded.Height, 0, messageToContainerExpanded);
            mAnimator.AnimationEnd += (x, y) =>
            {
                separatorShadow.Visibility = ViewStates.Gone;
                firstSeparator.Visibility = ViewStates.Visible;
                messageToContainerExpanded.Visibility = ViewStates.Gone;
                messageToContainerCollapsed.Visibility = ViewStates.Visible;
            };
            mAnimator.Start();
            downArrow.Animate().Rotation(0);
        }

        private async Task LoadDoctorDetails()
        {
            var response = await Presenter.GetDoctor();
            if (response.IsSuccess)
            {
                var doctor = response.Result;
                var doctorPrefix = Resources.GetString(Resource.String.chat_dialog_title_doctor);
                var medicalCenterPrefix = Resources.GetString(Resource.String.chat_dialog_title_office);
                var doctorText = $"{doctorPrefix} {doctor.FullName}";
                var doctorOfficeText = $"{medicalCenterPrefix} ({doctor.OfficeName.ToNameCase()})";

                var stringBuilder = new SpannableStringBuilder(doctorText);
                stringBuilder.SetSpan(new StyleSpan(TypefaceStyle.Bold), 0, doctorPrefix.Length, SpanTypes.InclusiveInclusive);

                messageToDoctor.TextFormatted = stringBuilder;

                stringBuilder = new SpannableStringBuilder(doctorOfficeText);
                stringBuilder.SetSpan(new StyleSpan(TypefaceStyle.Bold), 0, medicalCenterPrefix.Length, SpanTypes.InclusiveInclusive);

                messageToMedicalCenter.TextFormatted = stringBuilder;
            }
        }

    }
}