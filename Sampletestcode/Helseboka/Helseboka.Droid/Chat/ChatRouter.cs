using System;
using Android.App;
using Helseboka.Core.Legedialog.Interface;
using Helseboka.Core.Legedialog.Model;
using Helseboka.Core.Legedialog.Presenter;
using Helseboka.Droid.Chat.Views;
using Helseboka.Droid.Common.EnumDefinitions;
using Helseboka.Droid.Common.Interfaces;
using Helseboka.Droid.Common.Views;

namespace Helseboka.Droid.Chat
{
    public class ChatRouter : IChatRouter
    {
        private IActivity Activity { get; set; }
        public ChatRouter(IActivity activity)
        {
            Activity = activity;
        }

        public void NavigateToChat(MessageThread thread)
        {
            Activity.HideToolbar();
            var fragment = new ChatFragment(new ChatPresenter(this, thread));
            Activity.NavigateTo(fragment, TransitionEffect.Push);
        }

        public void GoBackToHome()
        {
            Activity.ShowToolbar();
            var fragment = new ChatHomeFragment(new LegedialogListPresenter(this));
            Activity.NavigateTo(fragment, TransitionEffect.Pop);
        }

        public void NavigateToNewDialogView()
        {
            Activity.HideToolbar();
            var fragment = new NewChatFragment(new NewLegeDialogPresenter(this));
            Activity.NavigateTo(fragment, TransitionEffect.Push);
        }

        public BaseFragment GetCurrentFragment()
        {
            return new ChatHomeFragment(new LegedialogListPresenter(this));
            
        }
    }
}
