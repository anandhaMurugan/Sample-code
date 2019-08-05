using System;
using Foundation;
using UIKit;

namespace Helseboka.iOS.Common.View.DateTimePicker
{
    public partial class DatePickerDialog : BaseModalViewController
    {
        public UIDatePickerMode Mode;

        public DateTime? DefaultDate;

        public DateTime? MaxiumDate;

        public DateTime? MinimumDate;

        public event EventHandler<DateTime> DateSelected;

        public String DialogTitle;

        public DatePickerDialog()
        {
            ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;
            ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ContainerView.Layer.CornerRadius = 5F;

            DatePicker.Mode = Mode;

            if (DefaultDate.HasValue)
                DatePicker.Date = (NSDate)DefaultDate;
            else
                DatePicker.Date = (NSDate)DateTime.Now;

            if (MaxiumDate.HasValue)
                DatePicker.MaximumDate = (NSDate)MaxiumDate;

            if (MinimumDate.HasValue)
                DatePicker.MinimumDate = (NSDate)MinimumDate;

            if (!string.IsNullOrEmpty(DialogTitle))
                TitleLabel.Text = DialogTitle;
        }

        partial void Cancel(NSObject sender)
        {
            Close();
        }

        partial void Done(NSObject sender)
        {
            var selectedDate = DateTime.SpecifyKind((DateTime)DatePicker.Date, DateTimeKind.Utc).ToLocalTime();
            DateSelected?.Invoke(this, selectedDate);
            Close();
        }
    }
}

