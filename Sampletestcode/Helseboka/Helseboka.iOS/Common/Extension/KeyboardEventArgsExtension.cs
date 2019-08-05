using System;
using UIKit;

namespace Helseboka.iOS.Common.Extension
{
    public static class KeyboardEventArgsExtension
    {
        public static UIViewAnimationOptions GetAnimationOptions(this UIKeyboardEventArgs args)
        {
            UIViewAnimationCurve curve = args.AnimationCurve;
            // UIViewAnimationCurve and UIViewAnimationOptions are shifted by 16 bits
            // http://stackoverflow.com/questions/18870447/how-to-use-the-default-ios7-uianimation-curve/18873820#18873820
            return (UIViewAnimationOptions)((int)curve << 16);
        }

        public static nfloat GetKeyboardHeight(this UIKeyboardEventArgs args)
        {
            return args.FrameEnd.Height;
        }
    }
}
