using System;
using Android.App;
using Helseboka.Droid.Common.EnumDefinitions;
using Helseboka.Droid.Common.Views;

namespace Helseboka.Droid.Common.Interfaces
{
    public interface IActivity
    {
        event EventHandler KeyboardHide;
        event EventHandler<int> KeyboardVisible;

        void NavigateTo(BaseFragment fragment, TransitionEffect effect = TransitionEffect.None);

        void HideToolbar();

        void ShowToolbar();

        void AttachKeyboardListner();

        void RemoveKeyboardListner();
    }
}
