using System;
using Helseboka.Core.Common.Extension;
using Foundation;
using UIKit;
using Helseboka.Core.Legedialog.Model;
using Helseboka.iOS.Common.Constant;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.iOS.Common.TableViewCell;
using Helseboka.iOS.Common.Extension;
using Helseboka.Core.Resources.StringResources;
using SafariServices;
using Helseboka.iOS.Common.View;

namespace Helseboka.iOS.Legedialog.View
{
    public partial class ChatDialogCell : BaseTableViewCell
    {
        private ChatMessage message;
        private UITapGestureRecognizer resendMessageTapGesture;

        protected ChatDialogCell(IntPtr handle) : base(handle) { }

        public event EventHandler<ChatMessage> ResentChat;

        public void Configure(ChatMessage message, Action<NSUrl> onLinkTap)
        {
            this.message = message;
            ChatMessage.Text = message.Text;
            ChatMessage.Delegate = new LinkDelegate(onLinkTap);
            ChatMessage.Font = Fonts.GetMediumFont(15);
            ChatMessage.TextColor = Colors.SentMessageTextColor;
            MessageBackgroundView.Layer.CornerRadius = 10;
            MessageBackgroundView.Layer.BorderWidth = 1;
            if (message.MessageDirection == Core.Common.EnumDefinitions.MessageDirection.Sent)
            {
                MessageBackgroundView.Layer.BorderColor = Colors.SentMessageBorderColor.CGColor;
                SentMessageDate.Text = GetTimestamp(message.Created);
                SentMessageDate.Hidden = false;
                ReceivedMessageDate.Hidden = false;
                ReceivedMessageDate.TextColor = Colors.DateLabelTextColor;
                ReceivedMessageDate.UserInteractionEnabled = false;

                ReceivedMessageDate.Text = message.StatusOfChat.GetChatStatusText();

                if(message.StatusOfChat == ChatStatus.Error)
                {
                    ReceivedMessageDate.TextColor = UIColor.Red;

                    if(resendMessageTapGesture != null)
                    {
                        ReceivedMessageDate.RemoveGestureRecognizer(resendMessageTapGesture);
                        resendMessageTapGesture = null;
                    }

                    resendMessageTapGesture = new UITapGestureRecognizer(ErrorStatus_tapped);
                    ReceivedMessageDate.AddGestureRecognizer(resendMessageTapGesture);
                    ReceivedMessageDate.UserInteractionEnabled = true;
                }
            }
            else
            {
                MessageBackgroundView.Layer.BorderColor = Colors.UnselectedLabelBorderColor.CGColor;
                ReceivedMessageDate.Text = GetTimestamp(message.Created);
                SentMessageDate.Hidden = true;
                ReceivedMessageDate.Hidden = false;
                ReceivedMessageDate.TextColor = Colors.DateLabelTextColor;
                ReceivedMessageDate.UserInteractionEnabled = false;
            }
        }

        void ErrorStatus_tapped()
        {
            ResentChat?.Invoke(this, message);
        }


        private String GetTimestamp(DateTime date)
        {
            if (date.GetDay() == Day.Today)
            {
                return $"{"Today".Translate()} {"General.View.TimePrefix".Translate()} {date.GetTimeString()}";
            }
            else if (date.GetDay() == Day.Yesterday)
            {
                return "Yesterday".Translate();
            }
            else
            {
                return date.ToString("dd.MM.yy");
            }
        }

    }
}
