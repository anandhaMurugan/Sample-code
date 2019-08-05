using System;
using Foundation;
using Helseboka.Core.Legedialog.Interface;
using Helseboka.iOS.Common.View;
using UIKit;
using Helseboka.iOS.Common.Extension;
using Helseboka.iOS.Common.Constant;
using System.Threading.Tasks;
using Helseboka.Core.Common.Extension;
using CoreGraphics;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Resources.StringResources;

namespace Helseboka.iOS.Legedialog.View
{
    public partial class NewDialogView : BaseView
    {
        private NSObject keyBoardWillShow;
        private NSObject keyBoardWillHide;
        private bool? isDoctorSelected;
        private String bodyPlaceholderText;

        public INewLegeDialogPresenter Presenter
        {
            get => presenter as INewLegeDialogPresenter;
            set => presenter = value;
        }

        public NewDialogView() { }

        public NewDialogView(IntPtr handler) : base(handler) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            bodyPlaceholderText = "Chat.NewDialog.Placeholder".Translate() + "…";
            BodyText.Text = bodyPlaceholderText;
            BodyText.TextColor = Colors.PlaceholderTextColor;
            SendButton.Enabled = false;
            ShowSenderView();

            ViewDidLoadTask().Forget();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            keyBoardWillShow = UIKeyboard.Notifications.ObserveWillShow(KeyboardWillShow);
            keyBoardWillHide = UIKeyboard.Notifications.ObserveWillHide(KeyboardWillHide);
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            DoctorLabel.Layer.CornerRadius = DoctorLabel.Frame.Height / 2;
            MedicalCenterLabel.Layer.CornerRadius = MedicalCenterLabel.Frame.Height / 2;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            keyBoardWillShow.Dispose();
            keyBoardWillHide.Dispose();
        }

        private void SenderView_Tapped()
        {
            ShowSenderSelectionView();

        }

        private void ShowSenderSelectionView()
        {
            //View.EndEditing(true);
            //SenderView.Hidden = true;
            //SenderSelectionView.Hidden = false;
            //SenderSeparatorTopToSenderViewBottomConstraint.Active = false;
            //SenderSeparatorTopToSenderViewSelectionBottomConstraint.Active = true;

            if (!isDoctorSelected.HasValue || isDoctorSelected.Value)
            {
                DoctorLabel.Layer.BackgroundColor = Colors.DoctorSelectionLebelFillColor.CGColor;
                MedicalCenterLabel.Layer.BackgroundColor = UIColor.White.CGColor;
            }
            else
            {
                DoctorLabel.Layer.BackgroundColor = UIColor.White.CGColor;
                MedicalCenterLabel.Layer.BackgroundColor = Colors.DoctorSelectionLebelFillColor.CGColor;
            }

            SenderView.UserInteractionEnabled = false;
            DoctorLabel.UserInteractionEnabled = true;
            MedicalCenterLabel.UserInteractionEnabled = true;

            CheckForm();

            SenderSeparator.Hidden = true;
            UIView.AnimationsEnabled = false;
            View.LayoutIfNeeded();
            UIView.AnimationsEnabled = true;

            UIView.Animate(0.5, 0, UIViewAnimationOptions.CurveEaseInOut, () =>
            {
                View.EndEditing(true);
                SenderView.Hidden = true;
                SenderSelectionView.Hidden = false;
                SenderSeparatorTopToSenderViewBottomConstraint.Active = false;
                SenderSeparatorTopToSenderViewSelectionBottomConstraint.Active = true;

                View.LayoutIfNeeded();
            }, null);

            UIView.Animate(0.5, 0, UIViewAnimationOptions.CurveEaseInOut, () =>
            {
                SelectionDropDownImage.Transform = CGAffineTransform.MakeRotation((float)Math.PI);
            }, null);
        }

        private void ShowSenderView()
        {
            SenderView.UserInteractionEnabled = true;
            DoctorLabel.UserInteractionEnabled = false;
            MedicalCenterLabel.UserInteractionEnabled = false;

            var toLabelText = AppResources.NewChatSenderPrefix;

            if (isDoctorSelected.HasValue)
            {
                if (isDoctorSelected.Value)
                {
                    senderLabel.Text = toLabelText + DoctorLabel.Text;
                }
                else
                {
                    senderLabel.Text = toLabelText + MedicalCenterLabel.Text;
                }
                senderLabel.TextColor = Colors.TitleTextColor;
            }
            else
            {
                senderLabel.Text = $"{toLabelText} {AppResources.NewChatSenderPlaceholder}";
                senderLabel.TextColor = Colors.PlaceholderTextColor;
            }

            CheckForm();

            SenderSeparator.Hidden = false;
            UIView.AnimationsEnabled = false;
            View.LayoutIfNeeded();
            UIView.AnimationsEnabled = true;

            UIView.Animate(0.5, 0, UIViewAnimationOptions.CurveEaseInOut, () =>
            {
                SenderSeparatorTopToSenderViewBottomConstraint.Active = true;
                SenderSeparatorTopToSenderViewSelectionBottomConstraint.Active = false;

                View.LayoutIfNeeded();
            }, () =>
            {
                SenderView.Hidden = false;
                SenderSelectionView.Hidden = true;
            });

            UIView.Animate(0.5, 0, UIViewAnimationOptions.CurveEaseInOut, () =>
            {
                SelectionDropDownImage.Transform = CGAffineTransform.MakeRotation(0);
            }, null);
        }

        private bool CheckForm()
        {
            var isValid = isDoctorSelected.HasValue && !String.IsNullOrEmpty(SubjectTextfield.Text) && !String.IsNullOrEmpty(BodyText.Text) && BodyText.Text != bodyPlaceholderText;
            SendButton.Enabled = isValid;
            return isValid;
        }

        private void Doctor_Tapped()
        {
            isDoctorSelected = true;
            ShowSenderView();
        }

        private void MedicalCenter_Tapped()
        {
            isDoctorSelected = false;
            ShowSenderView();
        }


        private void KeyboardWillShow(object sender, UIKeyboardEventArgs args)
        {
            this.InvokeOnMainThread(() =>
            {                
                float bottompadding = 0;
                if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                {
                    bottompadding = (float)View.SafeAreaInsets.Bottom;
                }

                UIView.Animate(args.AnimationDuration, 0, args.GetAnimationOptions(), () =>
                {
                    ShowSenderView();
                    BodyViewBottomConstraint.Constant = -(args.GetKeyboardHeight() - bottompadding);
                    View.LayoutIfNeeded();
                }, null);
            });
        }

        private void KeyboardWillHide(object sender, UIKeyboardEventArgs args)
        {
            this.InvokeOnMainThread(() =>
            {
                UIView.Animate(args.AnimationDuration, 0, args.GetAnimationOptions(), () =>
                {
                    BodyViewBottomConstraint.Constant = 0;
                    View.LayoutIfNeeded();
                }, null);
            });
        }

        partial void Back_Tapped(UIButton sender)
        {
            Presenter.GoBack();
        }

        partial void Attachment_Tapped(UIButton sender)
        {
            
        }

        partial void Send_Tapped(UIButton sender)
        {
            CheckDoctorAndProceed(() =>
            {
                Createthread().Forget();
            });
        }

        private async Task Createthread()
        {
            if (CheckForm())
            {
                View.EndEditing(true);
                ShowLoader();
                var response = await Presenter.CreateThread(SubjectTextfield.Text, BodyText.Text, isDoctorSelected.Value);
                HideLoader();
                if (response.IsSuccess)
                {
                    Presenter.GoBack();
                }
            }
        }

        private void BodyText_Started(object sender, EventArgs e)
        {
            if (BodyText.Text.Equals(bodyPlaceholderText))
            {
                BodyText.Text = String.Empty;
            }

            BodyText.TextColor = Colors.TitleTextColor;
        }


        private void BodyText_Changed(object sender, EventArgs e)
        {
            CheckForm();
        }

        private void BodyText_Ended(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(BodyText.Text))
            {
                BodyText.Text = bodyPlaceholderText;
                BodyText.TextColor = Colors.PlaceholderTextColor;
            }
            else
            {
                BodyText.TextColor = Colors.TitleTextColor;
            }
        }

        partial void Subject_DidChanged(UITextField sender)
        {
            CheckForm();
        }

        private async Task ViewDidLoadTask()
        {
            ShowLoader();
            var response = await Presenter.GetDoctor();
            HideLoader();
            if (response.IsSuccess)
            {
                var doctor = response.Result;
                var doctorPrefix = "Chat.Dialog.Title.Doctor".Translate();
                var medicalCenterPrefix = "Chat.Dialog.Title.Office".Translate();

                var doctorAttributedText = new NSMutableAttributedString();
                doctorAttributedText.Append(new NSAttributedString(doctorPrefix, Fonts.GetBoldFont(15), Colors.TitleTextColor));
                doctorAttributedText.Append(new NSAttributedString($" {doctor.FullName}", Fonts.GetMediumFont(15), Colors.TitleTextColor));
                DoctorLabel.AttributedText = doctorAttributedText;

                var storeAttributetText = new NSMutableAttributedString();
                storeAttributetText.Append(new NSAttributedString(medicalCenterPrefix, Fonts.GetBoldFont(15), Colors.TitleTextColor));
                storeAttributetText.Append(new NSAttributedString($" ({doctor.OfficeName.ToNameCase()})", Fonts.GetMediumFont(15), Colors.TitleTextColor));
                MedicalCenterLabel.AttributedText = storeAttributetText;


                DoctorLabel.Padding = new UIEdgeInsets(10, 10, 10, 10);
                MedicalCenterLabel.Padding = new UIEdgeInsets(10, 10, 10, 10);

                DoctorLabel.Layer.BackgroundColor = Colors.DoctorSelectionLebelFillColor.CGColor;
                DoctorLabel.Layer.BorderColor = Colors.DoctorSelectionLabelBorderColor.CGColor;
                DoctorLabel.Layer.BorderWidth = 2;

                MedicalCenterLabel.Layer.BackgroundColor = UIColor.White.CGColor;
                MedicalCenterLabel.Layer.BorderColor = Colors.DoctorSelectionLabelBorderColor.CGColor;
                MedicalCenterLabel.Layer.BorderWidth = 2;

                SenderView.AddGestureRecognizer(new UITapGestureRecognizer(SenderView_Tapped));
                DoctorLabel.AddGestureRecognizer(new UITapGestureRecognizer(Doctor_Tapped));
                MedicalCenterLabel.AddGestureRecognizer(new UITapGestureRecognizer(MedicalCenter_Tapped));
                BodyText.Changed += BodyText_Changed;
                BodyText.Started += BodyText_Started;
                BodyText.Ended += BodyText_Ended;

                View.UpdateConstraints();
            }
        }

    }
}

