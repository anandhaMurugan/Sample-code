using System;
using Helseboka.iOS.Common.Extension;
using Foundation;
using Helseboka.iOS.Common.TableViewCell;
using UIKit;

namespace Helseboka.iOS.Legetimer.View.TableViewCell
{
    public partial class SymptomTableViewCell : BaseTableViewCell
    {
        public static readonly NSString Key = new NSString("SymptomTableViewCell");
        
		protected SymptomTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public void UpdateCell(int index, String description)
        {
            symptomNameLabel.Text = $"{"Appointment.Details.Symptom.Title".Translate()} {index + 1}";
            symptomDescriptionLabel.Text = description;         
        } 
    }
}



