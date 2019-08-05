
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Helseboka.Core.Legedialog.Interface;
using Helseboka.Droid.Common.Views;
using Helseboka.Core.Common.Extension;
using Android.Support.V7.Widget;
using Helseboka.Droid.Common.Adapters;
using Helseboka.Core.Legedialog.Model;
using Helseboka.Droid.Common.Listners;
using System.Threading.Tasks;
using Android.Support.V4.Widget;
using Helseboka.Droid.Common.Utils;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Resources.StringResources;
using Android.Graphics;

namespace Helseboka.Droid.Chat.Views
{
    public class ChatHomeFragment : BaseFragment, IUniversalAdapter
    {
        View helpFAQview;
        LinearLayout helpFAQContainer;
        ImageView helpButton;
        Button newQuestion;
        ProgressBar progressBar;
        RecyclerView chatListView;
        Boolean isLoading;
        List<MessageThread> messageThreads;
        SwipeRefreshLayout chatDataView;
        TextView chatHomeHelpTitle;

        public ILegedialogListPresenter Presenter
        {
            get => presenter as ILegedialogListPresenter;
        }

        public ChatHomeFragment(ILegedialogListPresenter presenter)
        {
            this.presenter = presenter;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_chat_home, null);

            helpFAQview = view.FindViewById<ScrollView>(Resource.Id.helpFAQview);
            helpFAQContainer = view.FindViewById<LinearLayout>(Resource.Id.helpFAQviewContainer);
            helpButton = view.FindViewById<ImageView>(Resource.Id.helpButton);
            newQuestion = view.FindViewById<Button>(Resource.Id.newChat);
            progressBar = view.FindViewById<ProgressBar>(Resource.Id.loading_progressbar);
            chatListView = view.FindViewById<RecyclerView>(Resource.Id.chatListView);
            chatDataView = view.FindViewById<SwipeRefreshLayout>(Resource.Id.chatDataView);
            chatHomeHelpTitle = view.FindViewById<TextView>(Resource.Id.chatHomeHelpTitle);

            helpButton.Click += HelpButton_Click;
            newQuestion.Click += NewQuestion_Click;
            chatDataView.Refresh += ChatDataView_Refresh;

            DesignHelpView(helpFAQContainer, Core.Common.EnumDefinitions.HelpFAQType.ChatHome).Forget();

            var layoutManager = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
            chatListView.SetLayoutManager(layoutManager);
            chatListView.SetAdapter(new GenericRecyclerAdapter(this));
            var scrollListner = new RecyclerViewScrollListener(layoutManager);
            scrollListner.LoadMore += ScrollListner_LoadMore;
            chatListView.AddOnScrollListener(scrollListner);

            chatHomeHelpTitle.Text = AppResources.ChatHomeHelpTitle;

            return view;
        }

        public override void OnResume()
        {
            base.OnResume();
            ShowTableLoader();
            RefreshData().Forget();
        }

        void ChatDataView_Refresh(object sender, EventArgs e)
        {
            RefreshData().Forget();
        }

        void HelpButton_Click(object sender, EventArgs e)
        {
            ShowHelpView(Core.Common.EnumDefinitions.HelpFAQType.ChatHome).Forget();
        }


        private void NewQuestion_Click(object sender, EventArgs e)
        {
            CheckDoctorAndProceed(() =>
            {
                NavigateToNewThreadCreation().Forget();
            });
        }

        private async Task NavigateToNewThreadCreation()
        {
            ShowLoader();
            var response = await Presenter.DidTapOnNewDialog();
            HideLoader();
        }

        private void ShowNoData()
        {
            helpButton.Visibility = ViewStates.Invisible;
            progressBar.Visibility = ViewStates.Gone;
            chatDataView.Visibility = ViewStates.Gone;
            helpFAQview.Visibility = ViewStates.Visible;
        }

        private void ShowDataTable()
        {
            helpButton.Visibility = ViewStates.Visible;
            progressBar.Visibility = ViewStates.Gone;
            chatDataView.Visibility = ViewStates.Visible;
            helpFAQview.Visibility = ViewStates.Gone;
        }

        private void ShowTableLoader()
        {
            helpButton.Visibility = ViewStates.Visible;
            progressBar.Visibility = ViewStates.Visible;
            chatDataView.Visibility = ViewStates.Gone;
            helpFAQview.Visibility = ViewStates.Gone;
        }

        private async Task RefreshData()
        {            
            isLoading = true;
            messageThreads = await Presenter.GetThreads();
            chatDataView.Refreshing = false;
            isLoading = false;
            if (messageThreads != null && messageThreads.Count > 0)
            {
                chatListView.GetAdapter().NotifyDataSetChanged();
                ShowDataTable();
            }
            else
            {
                ShowNoData();
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
                    int currentItemCount = messageThreads.Count;
                    messageThreads.AddRange(response);
                    chatListView.GetAdapter().NotifyItemRangeInserted(currentItemCount - 1, response.Count);
                }
            }
        }

        public int GetItemCount()
        {
            return messageThreads != null ? messageThreads.Count : 0;
        }

        public UniversalViewHolder CreateItemView(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.FromContext(Activity);
            var view = inflater.Inflate(Resource.Layout.template_home_chat, null);

            var viewMap = new Dictionary<int, View>();
            viewMap.Add(Resource.Id.chatTitle, view.FindViewById(Resource.Id.chatTitle));
            viewMap.Add(Resource.Id.chatSubTitle, view.FindViewById(Resource.Id.chatSubTitle));
            viewMap.Add(Resource.Id.chatDate, view.FindViewById(Resource.Id.chatDate));
            viewMap.Add(Resource.Id.statusTextView, view.FindViewById(Resource.Id.statusTextView));

            return new UniversalViewHolder(view, viewMap);
        }

        public void BindViewHolder(UniversalViewHolder holder, int position)
        {
            var thread = messageThreads[position];

            String message;
            String datestamp;
            switch (thread.TypeOfPartner)
            {
                case PartnerType.Doctor: message = $"{Resources.GetString(Resource.String.chat_dialog_title_doctor)} {thread.PartnerName.ToNameCase()}"; break;
                case PartnerType.MedicalCenter: message = $"{Resources.GetString(Resource.String.chat_dialog_title_office)} ({thread.PartnerName.ToNameCase()})"; break;
                default: message = thread.PartnerName.ToNameCase(); break;
            }

            datestamp = thread.LastUpdated.GetDayString(Activity);

            holder.GetView<TextView>(Resource.Id.chatTitle).Text = thread.Title;
            holder.GetView<TextView>(Resource.Id.chatSubTitle).Text = message;
            holder.GetView<TextView>(Resource.Id.chatDate).Text = datestamp;

            var statusTextView = holder.GetView<TextView>(Resource.Id.statusTextView);
            statusTextView.Text = thread.LastMessageStatus.GetChatStatusText();
            if(thread.LastMessageStatus == Core.Common.EnumDefinitions.ChatStatus.Error)
            {
                statusTextView.SetTextColor(Color.Red);
            }
            else
            {
                statusTextView.SetTextColor(Color.ParseColor("#bfbebe"));
            }
        }

        public void OnItemClick(int position)
        {
            Presenter.DidSelectThread(messageThreads[position]);
        }

        void ScrollListner_LoadMore(object sender, LoadMoreEventArgs e)
        {
            LoadMoreData().Forget();
        }
    }
}
