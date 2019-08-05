using System;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Legedialog.Model;

namespace Helseboka.Core.Legedialog.Interface
{
	public interface IChatRouter : IBaseRouter
    {
        void NavigateToChat(MessageThread thread);

        void GoBackToHome();

        void NavigateToNewDialogView();
    }
}
