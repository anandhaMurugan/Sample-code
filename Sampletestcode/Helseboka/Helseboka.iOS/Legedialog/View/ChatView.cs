using System;
using System.Threading.Tasks;
using Foundation;
using Helseboka.Core.Legedialog.Interface;
using Helseboka.Core.Legedialog.Model;
using Helseboka.iOS.Common.TableViewDelegates;
using Helseboka.iOS.Common.Extension;
using Helseboka.Core.Common.Extension;
using Helseboka.iOS.Common.View;
using UIKit;
using System.Collections.Generic;
using System.Linq;
using Helseboka.Core.Common.Model;
using Helseboka.iOS.Common.View.PopUpDialogs;
using Helseboka.Core.Resources.StringResources;
using SafariServices;

namespace Helseboka.iOS.Legedialog.View
{
    public interface IChatMessageDataSourceDelegate
    {
        void Link_Tapped(NSUrl obj);
    }

    public partial class ChatView : BaseView , IChatMessageDataSourceDelegate
    {
        private const int cellBuffer = 5;
        private ChatMessageDataSource tableviewSource = new ChatMessageDataSource();
        private NSObject keyBoardWillShow;
        private NSObject keyBoardWillHide;
        private bool isLoading = false;

        public IChatPresenter Presenter
        {
            get => presenter as IChatPresenter;
            set => presenter = value;
        }

        public ChatView() { }

        public ChatView(IntPtr handler) : base(handler) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SendButton.Enabled = false;
            tableviewSource.DidScroll += Tableview_DidScroll;
            tableviewSource.ResendMessage += TableviewSource_ResendMessage;
            tableviewSource.Delegate = this;
            DataTableView.Source = tableviewSource;
            DataTableView.AllowsSelection = false;
            DataTableView.RowHeight = UITableView.AutomaticDimension;
            DataTableView.EstimatedRowHeight = 100;
            String pageSalute = String.Empty;
            switch(Presenter.Thread.TypeOfPartner)
            {
                case PartnerType.Doctor: pageSalute = "Chat.Dialog.Title.Doctor".Translate(); break;
                case PartnerType.MedicalCenter: pageSalute = "Chat.Dialog.Title.Office".Translate(); break;
            }

            ChatPageTitleLabel.Text = $"{pageSalute} {Presenter.Thread.PartnerName.ToNameCase()} \n{Presenter.Thread.Title}";
            ChatPageTitleLabel.AddGestureRecognizer(new UITapGestureRecognizer(() => MessageComposeField.ResignFirstResponder()));

            RefreshData().Forget();
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

                UIView.Animate(args.AnimationDuration, 0, args.GetAnimationOptions(), () =>
                {
                    ComposeMessageViewBottomConstraint.Constant = -(args.GetKeyboardHeight() - bottompadding);
                    View.LayoutIfNeeded();
                }, () =>
                {
                    if (DataTableView != null && DataTableView.IndexPathsForVisibleRows != null && DataTableView.IndexPathsForVisibleRows.Length > 0)
                    {
                        var current = DataTableView.IndexPathsForVisibleRows.Last();
                        DataTableView.ScrollToRow(current, UITableViewScrollPosition.Bottom, true);
                    }
                });
            });
        }

        private void KeyboardWillHide(object sender, UIKeyboardEventArgs args)
        {
            this.InvokeOnMainThread(() =>
            {
                UIView.Animate(args.AnimationDuration, 0, args.GetAnimationOptions(), () =>
                {
                    ComposeMessageViewBottomConstraint.Constant = 0;
                    View.LayoutIfNeeded();
                }, null);
            });
        }

        private void Tableview_DidScroll(object sender, UIScrollView scrollView)
        {
            if (DataTableView != null && DataTableView.IndexPathsForVisibleRows != null && DataTableView.IndexPathsForVisibleRows.Length > 0)
            {
                var firstVisibleRow = DataTableView.IndexPathsForVisibleRows.First().Row;

                if (firstVisibleRow < cellBuffer)
                {
                    LoadMoreData().Forget();
                }
            }
        }


        partial void Attachment_Tapped(UIButton sender)
        {
            
        }

        partial void MessageSend_Tapped(SmallActionButton sender)
        {
            MessageComposeField.ResignFirstResponder();
            CheckDoctorAndProceed(() =>
            {
                SendMessage(MessageComposeField.Text).Forget();
            });
        }

        partial void Back_Tapped(UIButton sender)
        {
            Presenter.GoBack();
        }

        partial void MessageEditing_Changed(UITextField sender, UIEvent @event)
        {
            SendButton.Enabled = !String.IsNullOrEmpty(sender.Text);
        }

        private async Task SendMessage(String message)
        {
            if (!String.IsNullOrEmpty(message))
            {
                MessageComposeField.ResignFirstResponder();
                ShowLoader();
                var response = await Presenter.SendMessage(message);

                if (response.IsSuccess)
                {
                    MessageComposeField.Text = String.Empty;
                    SendButton.Enabled = false;
                    await RefreshData();
                    HideLoader();
                }
                else
                {
                    HideLoader();
                    await ProcessAPIError(response);
                }
            }

        }

        private async Task RefreshData()
        {
            isLoading = true;
            tableviewSource.Clear();
            var response = await Presenter.RefreshMessage();
            if (response != null && response.Count > 0)
            {
                tableviewSource.UpdateList(response);
                this.InvokeOnMainThread(() => {
                    DataTableView.ReloadData();
                    DataTableView.ScrollToRow(NSIndexPath.FromRowSection(response.Count - 1, 0), UITableViewScrollPosition.Bottom, false);
                    isLoading = false;
                });
            }
        }

        private async Task LoadMoreData()
        {
            if (Presenter.HasMoreData && !isLoading)
            {
                isLoading = true;
                var response = await Presenter.LoadMore();
                if (response != null && response.Count > 0)
                {
                    tableviewSource.UpdateList(response, true);
                    this.InvokeOnMainThread(() =>
                    {
                        var firstVisibleRow = DataTableView.IndexPathsForVisibleRows.First().Row;
                        DataTableView.ReloadData();
                        DataTableView.ScrollToRow(NSIndexPath.FromRowSection(firstVisibleRow + response.Count, 0), UITableViewScrollPosition.Top, false);
                        isLoading = false;
                    });
                }
            }
        }

        void TableviewSource_ResendMessage(object sender, ChatMessage e)
        {
            var dialog = new YesNoDialogView(AppResources.ChatResendConfirmationTitle, AppResources.ChatResendConfirmationMessage);
            dialog.LeftButtonTapped += (s, ed) => SendMessage(e.Text).Forget();
            dialog.Show();
        }

        public void Link_Tapped(NSUrl obj)
        {
            UIApplication.SharedApplication.OpenUrl(obj);
        }

    }

    public class ChatMessageDataSource : BaseTableViewSource<ChatMessage>
    {
        public event EventHandler<ChatMessage> ResendMessage;
        public IChatMessageDataSourceDelegate Delegate;

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (DataList != null && DataList.Count > indexPath.Row)
            {
                var cell = tableView.DequeueReusableCell("ChatDialogCell") as ChatDialogCell;

                cell.Configure(DataList[indexPath.Row], Delegate.Link_Tapped);
                cell.ResentChat -= Cell_ResentChat;
                cell.ResentChat += Cell_ResentChat;

                return cell;
            }
            else
            {
                return new UITableViewCell();
            }
        }

        void Cell_ResentChat(object sender, ChatMessage e)
        {
            ResendMessage?.Invoke(sender, e);
        }
    }
}

