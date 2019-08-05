using System;

using Foundation;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Extension;
using Helseboka.iOS.Common.Constant;
using Helseboka.iOS.Common.TableViewCell;
using UIKit;

namespace Helseboka.iOS.Legedialog.View
{
	public partial class DialogCell : BaseTableViewCell
    {
        protected DialogCell(IntPtr handle) : base(handle) { }

        public void Configure(String title, String description, String time, ChatStatus status)
		{
			HeadlineLabel.Text = title;
			SubtitleLabel.Text = description;
			TimeLabel.Text = time;

            StatusLabel.Text = status.GetChatStatusText();
            if(status == ChatStatus.Error)
            {
                StatusLabel.TextColor = UIColor.Red;
            }
            else
            {
                StatusLabel.TextColor = Colors.DateLabelTextColor;
            }
        }
    }
}
