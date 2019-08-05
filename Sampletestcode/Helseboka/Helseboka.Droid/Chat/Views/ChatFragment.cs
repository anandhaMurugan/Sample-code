
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Helseboka.Core.Legedialog.Interface;
using Helseboka.Core.Legedialog.Model;
using Helseboka.Core.Common.Extension;
using Helseboka.Droid.Common.Adapters;
using Helseboka.Droid.Common.Listners;
using Helseboka.Droid.Common.Views;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Model;
using Android.Graphics;
using Helseboka.Core.Resources.StringResources;

namespace Helseboka.Droid.Chat.Views
{
    public class ChatFragment : BaseFragment, IUniversalAdapter
    {
        ImageView back;
        TextView chatPartner;
        TextView chatTitle;
        RecyclerView chatListView;
        Button sendMessageButton;
        EditText chatMessageBox;

        bool isLoading = false;
        List<ChatMessage> chatMessages;


        public IChatPresenter Presenter
        {
            get => presenter as IChatPresenter;
        }

        public ChatFragment(IChatPresenter presenter)
        {
            this.presenter = presenter;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_chat, null);

            back = view.FindViewById<ImageView>(Resource.Id.back);
            chatPartner = view.FindViewById<TextView>(Resource.Id.chatPartner);
            chatTitle = view.FindViewById<TextView>(Resource.Id.chatTitle);
            chatListView = view.FindViewById<RecyclerView>(Resource.Id.chatDataList);
            sendMessageButton = view.FindViewById<Button>(Resource.Id.chatSendMessageButton);
            chatMessageBox = view.FindViewById<EditText>(Resource.Id.chatMessageBox);

            chatMessageBox.TextChanged += ChatMessageBox_TextChanged;
            sendMessageButton.Click += SendMessageButton_Click;
            back.Click += Back_Click;
            chatListView.LayoutChange += ChatListView_LayoutChange;

            var layoutManager = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
            chatListView.SetLayoutManager(layoutManager);
            chatListView.SetAdapter(new GenericRecyclerAdapter(this));
            var scrollListner = new RecyclerViewReverseScrollListener(layoutManager);
            scrollListner.LoadMore += ScrollListner_LoadMore;
            chatListView.AddOnScrollListener(scrollListner);

            String pageSalute = String.Empty;
            switch (Presenter.Thread.TypeOfPartner)
            {
                case PartnerType.Doctor: pageSalute = Resources.GetString(Resource.String.chat_dialog_title_doctor); break;
                case PartnerType.MedicalCenter: Resources.GetString(Resource.String.chat_dialog_title_office); break;
            }

            chatPartner.Text = $"{pageSalute} {Presenter.Thread.PartnerName.ToNameCase()}";
            chatTitle.Text = Presenter.Thread.Title;

            return view;
        }

        public override void OnResume()
        {
            base.OnResume();

            RefreshData().Forget();
        }

        void ChatListView_LayoutChange(object sender, View.LayoutChangeEventArgs e)
        {
            if (e.Bottom < e.OldBottom)
            {
                chatListView.PostDelayed(() =>
                {
                    chatListView.SmoothScrollToPosition(chatMessages.Count - 1);
                }, 100);
            }
        }


        void Back_Click(object sender, EventArgs e)
        {
            HideKeyboard(chatMessageBox);
            Presenter.GoBack();
        }

        public override bool OnBackKeyPressed()
        {
            HideKeyboard(chatMessageBox);
            Presenter.GoBack();
            return true;
        }


        void ChatMessageBox_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            sendMessageButton.Enabled = !String.IsNullOrEmpty(chatMessageBox.Text);
        }

        void SendMessageButton_Click(object sender, EventArgs e)
        {
            HideKeyboard();
            CheckDoctorAndProceed(() =>
            {
                SendMessage(chatMessageBox.Text).Forget();
            });
        }

        public int GetItemCount()
        {
            return chatMessages != null ? chatMessages.Count : 0;
        }

        public UniversalViewHolder CreateItemView(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.FromContext(Activity);
            var view = inflater.Inflate(Resource.Layout.template_chat_chatview, null);

            var viewMap = new Dictionary<int, View>();
            viewMap.Add(Resource.Id.chatMessage, view.FindViewById(Resource.Id.chatMessage));
            viewMap.Add(Resource.Id.sentDate, view.FindViewById(Resource.Id.sentDate));
            viewMap.Add(Resource.Id.receivedDate, view.FindViewById(Resource.Id.receivedDate));
            viewMap.Add(Resource.Id.chatMessageContainer, view.FindViewById(Resource.Id.chatMessageContainer));

            return new UniversalViewHolder(view, viewMap);
        }

        public void BindViewHolder(UniversalViewHolder holder, int position)
        {
            var data = chatMessages[position];

            var message = holder.GetView<TextView>(Resource.Id.chatMessage);
            var senddate = holder.GetView<TextView>(Resource.Id.sentDate);
            var receivedDate = holder.GetView<TextView>(Resource.Id.receivedDate);
            var messageContainer = holder.GetView<LinearLayout>(Resource.Id.chatMessageContainer);

            message.Text = data.Text;
            if (data.MessageDirection == Core.Common.EnumDefinitions.MessageDirection.Sent)
            {
                senddate.Text = GetTimestamp(data.Created);
                senddate.Visibility = ViewStates.Visible;
                receivedDate.Visibility = ViewStates.Visible;
                receivedDate.SetTextColor(Color.ParseColor("#bfbebe"));
                receivedDate.Click -= ErrorStatus_Click;
                message.SetTextAppearance(Resource.Style.chat_chatview_sent_message_style);
                messageContainer.SetBackgroundResource(Resource.Drawable.shape_chat_sent_border);

                receivedDate.Text = data.StatusOfChat.GetChatStatusText();

                if (data.StatusOfChat == ChatStatus.Error)
                {
                    receivedDate.SetTextColor(Color.Red);
                    receivedDate.Tag = position;

                    receivedDate.Click += ErrorStatus_Click;
                }
            }
            else
            {
                receivedDate.Text = GetTimestamp(data.Created);
                receivedDate.Visibility = ViewStates.Visible;
                senddate.Visibility = ViewStates.Gone;
                receivedDate.SetTextColor(Color.ParseColor("#bfbebe"));
                receivedDate.Click -= ErrorStatus_Click;
                message.SetTextAppearance(Resource.Style.chat_chatview_received_message_style);
                messageContainer.SetBackgroundResource(Resource.Drawable.shape_chat_received_border);
            }
        }

        void ErrorStatus_Click(object sender, EventArgs e)
        {
            if(sender is View view)
            {
                var position = (int)view.Tag;
                if(chatMessages != null && chatMessages.Count > position)
                {
                    var message = chatMessages[position];

                    var dialog = new YesNoDialog(Activity, AppResources.ChatResendConfirmationMessage, AppResources.ChatResendConfirmationTitle, () =>
                    {
                        SendMessage(message.Text).Forget();
                    });
                    dialog.Show();
                }
            }
        }


        public void OnItemClick(int position)
        {
            
        }

        void ScrollListner_LoadMore(object sender, LoadMoreEventArgs e)
        {
            LoadMoreData().Forget();
        }

        private String GetTimestamp(DateTime date)
        {
            if (date.GetDay() == Day.Today)
            {
                return $"{Resources.GetString(Resource.String.today)} {Resources.GetString(Resource.String.general_view_timeprefix)} {date.GetTimeString()}";
            }
            else if (date.GetDay() == Day.Yesterday)
            {
                return Resources.GetString(Resource.String.yesterday);
            }
            else
            {
                return date.ToString("dd.MM.yy");
            }
        }


        private async Task SendMessage(String message)
        {
            if (!String.IsNullOrEmpty(message))
            {
                HideKeyboard(chatMessageBox);
                ShowLoader();
                var response = await Presenter.SendMessage(message);

                if (response.IsSuccess)
                {
                    chatMessageBox.Text = String.Empty;
                    sendMessageButton.Enabled = false;
                    await RefreshData();
                    HideLoader();
                }
                else
                {
                    HideLoader();
                    ProcessAPIError(response);
                }
            }

        }

        private async Task RefreshData()
        {
            isLoading = true;
            chatMessages = await Presenter.RefreshMessage();
            isLoading = false;
            if (chatMessages != null)
            {
                chatListView.GetAdapter().NotifyDataSetChanged();
                chatListView.ScrollToPosition(chatMessages.Count - 1);
            }
        }

        private async Task LoadMoreData()
        {
            if (Presenter.HasMoreData && !isLoading)
            {
                isLoading = true;
                var response = await Presenter.LoadMore();
                isLoading = false;
                if (response != null)
                {
                    int currentItemCount = chatMessages.Count;
                    chatMessages.InsertRange(0, response);
                    chatListView.GetAdapter().NotifyItemRangeInserted(0, response.Count);
                }
            }
        }

    }
}
