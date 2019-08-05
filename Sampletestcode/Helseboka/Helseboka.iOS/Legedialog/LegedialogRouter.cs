using System;
using Helseboka.Core.Common.CommonImpl;
using Helseboka.Core.Legedialog.DataAccess;
using Helseboka.Core.Legedialog.Interface;
using Helseboka.Core.Legedialog.Model;
using Helseboka.Core.Legedialog.Presenter;
using Helseboka.iOS.Legedialog.View;
using Helseboka.iOS.Common.Extension;
using UIKit;

namespace Helseboka.iOS.Legedialog
{
	public class LegedialogRouter : UINavigationController, IChatRouter
    {
		private UIStoryboard LegeDialog { get; set; }

        public LegedialogRouter()
        {
            LegeDialog = UIStoryboard.FromName("Legedialog", null);
			var legedialogListView = LegeDialog.InstantiateViewController("LegeDialogListView") as LegeDialogListView;
            legedialogListView.TabBarItem = new UITabBarItem("Home.TabBar.Chat.Title".Translate(), UIImage.FromBundle("Legedialog"), UIImage.FromBundle("Legedialog-active"));

			legedialogListView.Presenter = new LegedialogListPresenter(this);

			this.ViewControllers = new UIViewController[] { legedialogListView };
			NavigationBar.Hidden = true;
        }

        public void NavigateToChat(MessageThread thread)
        {
            var chatView = LegeDialog.InstantiateViewController("ChatView") as ChatView;
            if (chatView != null)
            {
                chatView.Presenter = new ChatPresenter(this, thread);
                TabBarController.TabBar.Hidden = true;
                PushViewController(chatView, true);
            }
        }

        public void NavigateToNewDialogView()
        {
            var newDialogView = LegeDialog.InstantiateViewController("NewDialogView") as NewDialogView;
            if (newDialogView != null)
            {
                newDialogView.Presenter = new NewLegeDialogPresenter(this);
                TabBarController.TabBar.Hidden = true;
                PushViewController(newDialogView, true);
            }
        }

        public void GoBackToHome()
        {
            TabBarController.TabBar.Hidden = false;
            PopToRootViewController(true);
        }

		public void Start()
		{
			
		}

		public void Finish()
		{
			
		}
	}
}
