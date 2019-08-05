using System;
using System.Linq;
using System.Collections.Generic;
using Foundation;
using Helseboka.Core.AppointmentModule.Interface;
using Helseboka.iOS.Common.View;
using Helseboka.iOS.Legetimer.View.TableViewCell;
using UIKit;
using Helseboka.iOS.Common.Extension;
using Helseboka.Core.Common.Extension;
using System.Drawing;
using System.Threading.Tasks;
using CoreAnimation;

namespace Helseboka.iOS.Legetimer.View
{
	public partial class AddSymptomsView : BaseView,IUITableViewDelegate,IUITableViewDataSource
	{
        private NSObject keyBoardWillShow;
        private NSObject keyBoardWillHide;
        private UITextField activeField;

        public static readonly String Identifier = "AddSymptomsView";
        public IAppointmentPresenter Presenter
        {
            get => presenter as IAppointmentPresenter;
            set => presenter = value;
        }

		List<string> symptomList = new List<string>(){string.Empty, string.Empty};

		public AddSymptomsView() : base() { }

		public AddSymptomsView(IntPtr handler) : base(handler) { }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

            DismissKeyboardOnBackgroundTap();
			SymptomListTableView.Delegate = this;
			SymptomListTableView.DataSource = this;
            SymptomListTableView.ReloadData();
            NextButton.Enabled = false;
		}

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            keyBoardWillShow = UIKeyboard.Notifications.ObserveWillShow(KeyboardWillShow);
            keyBoardWillHide = UIKeyboard.Notifications.ObserveWillHide(KeyboardWillHide);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            keyBoardWillShow.Dispose();
            keyBoardWillHide.Dispose();
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

                //var contentInset = new UIEdgeInsets(0, 0, args.GetKeyboardHeight(), 0);
                //SymptomListTableView.ContentInset = contentInset;
                //SymptomListTableView.ScrollIndicatorInsets = contentInset;

                //SymptomListTableView.ScrollToRow(NSIndexPath.FromRowSection(activeField.Tag, 0), UITableViewScrollPosition.Bottom, true);

                UIView.Animate(args.AnimationDuration, 0, args.GetAnimationOptions(), () =>
                {
                    NextButtonBottomConstraint.Constant = args.GetKeyboardHeight() - bottompadding + 30;
                    View.LayoutIfNeeded();
                }, null);
            });
        }

        private void KeyboardWillHide(object sender, UIKeyboardEventArgs args)
        {
            this.InvokeOnMainThread(() =>
            {
                //SymptomListTableView.ContentInset = UIEdgeInsets.Zero;
                //SymptomListTableView.ScrollIndicatorInsets = UIEdgeInsets.Zero;

                UIView.Animate(args.AnimationDuration, 0, args.GetAnimationOptions(), () =>
                {
                    NextButtonBottomConstraint.Constant = 30;
                    View.LayoutIfNeeded();
                }, null);
            });
        }

        partial void Back_Pressed(UIButton sender)
        {
            Presenter.GoBackToDateSelection();
        }

        partial void Help_Tapped(UIButton sender)
        {
            new ModalHelpViewController(Core.Common.EnumDefinitions.HelpFAQType.AppointmentSymptom).Show();
        }

        partial void NextButton_Tapped(PrimaryActionButton sender)
        {
            View.EndEditing(true);
            Presenter.AddAppointment(symptomList.Where(x=>!String.IsNullOrEmpty(x)).ToList());
        }   

        partial void AddMore_Tapped(UIButton sender)
        {
            symptomList.Add(String.Empty);

            var newIndex = NSIndexPath.FromRowSection(symptomList.Count - 1, 0);

            CATransaction.Begin();
            CATransaction.CompletionBlock = () => SymptomListTableView.ScrollToBottom();
            SymptomListTableView.BeginUpdates();
            SymptomListTableView.InsertRows(new NSIndexPath[] { newIndex }, UITableViewRowAnimation.Automatic);
            SymptomListTableView.EndUpdates();
            CATransaction.Commit();

        }

        [Export("tableView:heightForRowAtIndexPath:")]
		public nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			return 100;
		}

		public nint RowsInSection(UITableView tableView, nint section)
		{
			return symptomList.Count;
		}

		public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
            var cell = tableView.DequeueReusableCell(SymptomInfoTableViewCell.Key, indexPath) as SymptomInfoTableViewCell;

            cell.EditingDidBegin -= Cell_EditingDidBegin;
            cell.EditingChanged -= Cell_EditingChanged;
            cell.EditingDidEnd -= Cell_EditingDidEnd;

            cell.EditingDidBegin += Cell_EditingDidBegin;
            cell.EditingChanged += Cell_EditingChanged;
            cell.EditingDidEnd += Cell_EditingDidEnd;

			var cellData = symptomList[indexPath.Row];
            cell.UpdateCell(indexPath.Row,cellData);

            return cell;
		}

        void Cell_EditingDidBegin(object sender, EventArgs e)
        {
            activeField = sender as UITextField;
        }

        void Cell_EditingChanged(object sender, EventArgs e)
        {
            var textField = sender as UITextField;

            if (symptomList.Any(x => !String.IsNullOrEmpty(x)))
            {
                NextButton.Enabled = true;
            }
            else
            {
                NextButton.Enabled = !String.IsNullOrEmpty(textField.Text);
            }
        }

        void Cell_EditingDidEnd(object sender, EventArgs e)
        {
            activeField = null;
            var textField = sender as UITextField;
            symptomList[(int)textField.Tag] = textField.Text.Trim();

            NextButton.Enabled = symptomList.Any(x => !String.IsNullOrEmpty(x));
        }
	}
}

