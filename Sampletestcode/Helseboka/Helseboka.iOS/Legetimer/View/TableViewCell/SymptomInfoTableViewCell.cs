using System;
using Foundation;
using Helseboka.iOS.Common.TableViewCell;
using Helseboka.iOS.Common.Extension;
using UIKit;

namespace Helseboka.iOS.Legetimer.View.TableViewCell
{
    public partial class SymptomInfoTableViewCell : BaseTableViewCell, IUITextFieldDelegate
    {
        public static readonly NSString Key = new NSString("SymptomInfoTableViewCell");

        public event EventHandler EditingDidBegin
        {
            add { descriptionBox.EditingDidBegin += value; }
            remove { descriptionBox.EditingDidBegin -= value; }
        }

        public event EventHandler EditingChanged
        {
            add { descriptionBox.EditingChanged += value; }
            remove { descriptionBox.EditingChanged -= value; }
        }

        public event EventHandler EditingDidEnd
        {
            add { descriptionBox.EditingDidEnd += value; }
            remove { descriptionBox.EditingDidEnd -= value; }
        }

        protected SymptomInfoTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

		public override void AwakeFromNib()
		{
			base.AwakeFromNib();
			descriptionBox.Padding = new UIEdgeInsets(12, 30, 12, 30);
            descriptionBox.Delegate = this;
		}

        [Export("textFieldShouldReturn:")]
        public bool ShouldReturn(UITextField textField)
        {
            textField.ResignFirstResponder();
            return true;
        }

		public void UpdateCell(int row,string symptomDescription)
        {
            symtomCounterNameLabel.Text = $"{"Appointment.Details.Symptom.Title".Translate()} {row + 1}";
			descriptionBox.Text = symptomDescription;
            descriptionBox.Tag = row;
        } 
    }
}
