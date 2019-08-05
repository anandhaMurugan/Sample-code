using System;
using UIKit;

namespace MyTest.TableViewCellModel
{
	public class UserTableCellModel
    {

        public string fieldName { get; set; }

        public string fieldValue { get; set; }

        public bool isShowArrow { get; set; }

        public bool isTextBold { get; set; }
    
        public UIColor LabelTextColor { get; set; }

        public bool isValueColorChange { get; set; }
       

        public UserTableCellModel(string name, string value, bool ShowArrow, bool textBold, UIColor labelColor = null, bool valueColorChange =false)
        {
            fieldName = name;
            fieldValue = value;
            isTextBold = textBold;
            isShowArrow = ShowArrow;
            LabelTextColor = labelColor;
            isValueColorChange = valueColorChange;

        }
    } 
}
