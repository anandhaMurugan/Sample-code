using System;
using System.Text.RegularExpressions;
using Foundation;
using Helseboka.iOS.Common.Constant;
using Helseboka.iOS.Common.Extension;
using Helseboka.iOS.Common.TableViewCell;
using MyTest.TableViewCellModel;
using UIKit;

namespace Helseboka.iOS.Profile.View.TableViewCell
{
    public partial class UserPersonalInfoTableCell : BaseTableViewCell
    {
        public IUserDataSourceDelegate Delegate;
        public static readonly NSString Key = new NSString("UserPersonalInfoTableCell");
        public String Value
        {
            get => InfoValueLabel.Text;
        }

        protected UserPersonalInfoTableCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

		public void UpdateCell(UserPersonalInfoCellModel personalInfo)
		{
			InfoNameLabel.Text = personalInfo.FieldName;
			InfoValueLabel.Text = personalInfo.FieldValue;
            InfoValueLabel.Enabled = personalInfo.IsEditable;
            InfoValueLabel.KeyboardType = personalInfo.KeyboardType;

            if ("Profile.PersonalSettings.Telefone".Translate() == personalInfo.FieldName)
            {
                if (!string.IsNullOrEmpty(InfoValueLabel.Text))
                {
                    Delegate.ErrorTextLogics(false, false);
                }
                    InfoValueLabel.EditingChanged += (object sender, EventArgs e) =>
                {
                    bool emptyCheck = false;
                    bool regexCheck = false;
                    Regex regex = new Regex(@"^[0-9+ ]+$");
                    if (string.IsNullOrEmpty(InfoValueLabel.Text))
                    {
                        emptyCheck = true;
                    }
                   
                    if (!regex.IsMatch(InfoValueLabel.Text))
                    {        
                        if (!string.IsNullOrEmpty(InfoValueLabel.Text))
                        {
                            regexCheck = true;
                        }
                    }
                    Delegate.ErrorTextLogics(emptyCheck, regexCheck);
                };
            }

            if (personalInfo.IsEditable)
            {
                InfoValueLabel.TextColor = Colors.TitleTextColor;
            }
            else
            {
                InfoValueLabel.TextColor = UIColor.LightGray;
            }

            if (personalInfo.LabelTextColor != null)
            {
                InfoNameLabel.TextColor = personalInfo.LabelTextColor;
            }
        } 
    }

    public class UserPersonalInfoCellModel
    {
        public Boolean IsEditable { get; private set; }

        public UIKeyboardType KeyboardType { get; private set; }

        public string FieldName { get; private set; }

        public string FieldValue { get; private set; }

        public UIColor LabelTextColor { get; set; }


        public UserPersonalInfoCellModel(string name, string value, bool isEditable = false, UIKeyboardType keyboardType = UIKeyboardType.Default, UIColor labelColor = null)
        {
            FieldName = name;
            FieldValue = value;
            this.IsEditable = isEditable;
            this.KeyboardType = keyboardType;
            LabelTextColor = labelColor;
        }
    }
}