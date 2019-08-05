using System;

using Foundation;
using Helseboka.iOS.Common.TableViewCell;
using MyTest.TableViewCellModel;
using UIKit;

namespace Helseboka.iOS.Profile.View.TableViewCell
{
    public partial class UserProfileTableViewCell : BaseTableViewCell
    {
        public static readonly NSString Key = new NSString("UserProfileTableViewCell");

        protected UserProfileTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

		public void UpdateCell(UserTableCellModel userCellData)
		{

			fieldNameLable.Text = userCellData.fieldName;
			fieldValueLable.Text = userCellData.fieldValue;

			// For the visibility of the right arrow.
			if (userCellData.isShowArrow)
			{
				arrowImage.Hidden = false;

			}
			else
			{
				arrowImage.Hidden = true;
			}
			// Handling the Bold Text.
			if (userCellData.isTextBold)
			{
				fieldValueLable.Font = UIFont.BoldSystemFontOfSize(18);
			}

			// Handling the text colorchange
            if (userCellData.LabelTextColor != null)
			{
                fieldNameLable.TextColor = userCellData.LabelTextColor;
			}
			if(userCellData.isValueColorChange)
			{
				fieldValueLable.TextColor = UIColor.FromRGB(82, 0, 160);
			}
        } 
    }


}
