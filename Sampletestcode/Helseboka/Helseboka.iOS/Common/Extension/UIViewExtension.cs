using System;
using System.Collections.Generic;
using CoreGraphics;
using Helseboka.iOS.Common.Constant;
using Helseboka.iOS.Common.PlatformEnums;
using UIKit;

namespace Helseboka.iOS.Common.Extension
{
    public static class UITableViewExtension
    {
        public static void ScrollToBottom(this UITableView tableView)
        {
            if (tableView.ContentSize.Height > tableView.Frame.Height)
            {
                var offset = new CGPoint(0, tableView.ContentSize.Height - tableView.Frame.Height);
                tableView.SetContentOffset(offset, true);
            }
        }
    }

    public static class UIViewExtensionConstraints
    {
        public static void AddBorder(this UIView view, UIColor borderColor, float cornerRadius, float borderWidth = 1)
        {
            view.Layer.CornerRadius = cornerRadius;
            view.Layer.BorderColor = borderColor.CGColor;
            view.Layer.BorderWidth = borderWidth;
        }

        public static void AddShadow(this UIView view, UIColor shadowColor, float blur, float height, float width = 0)
        {
            view.Layer.ShadowColor = shadowColor.CGColor;
            view.Layer.ShadowOffset = new CGSize(width, height);
            view.Layer.ShadowOpacity = 0.5f;
            view.Layer.ShadowRadius = blur / 2;
        }

        public static void AddShadow(this UIView view)
        {
            view.AddShadow(Colors.ShadowColor, 10, 2);
        }

        public static void RemoveAllSubViews(this UIView view)
        {
            foreach (var item in view.Subviews)
            {
                item.RemoveFromSuperview();
            }
        }

        /// <summary>
        /// Find the first responder in the <paramref name="view"/>'s subview hierarchy
        /// </summary>
        /// <param name="view">
        /// A <see cref="UIView"/>
        /// </param>
        /// <returns>
        /// A <see cref="UIView"/> that is the first responder or null if there is no first responder
        /// </returns>
        public static UIView FindFirstResponder(this UIView view)
        {
            if (view.IsFirstResponder)
            {
                return view;
            }
            foreach (UIView subView in view.Subviews)
            {
                var firstResponder = subView.FindFirstResponder();
                if (firstResponder != null)
                    return firstResponder;
            }
            return null;
        }

        /// <summary>
        /// Find the first Superview of the specified type (or descendant of)
        /// </summary>
        /// <param name="view">
        /// A <see cref="UIView"/>
        /// </param>
        /// <param name="stopAt">
        /// A <see cref="UIView"/> that indicates where to stop looking up the superview hierarchy
        /// </param>
        /// <param name="type">
        /// A <see cref="Type"/> to look for, this should be a UIView or descendant type
        /// </param>
        /// <returns>
        /// A <see cref="UIView"/> if it is found, otherwise null
        /// </returns>
        public static UIView FindSuperviewOfType(this UIView view, UIView stopAt, Type type)
        {
            if (view.Superview != null)
            {
                if (type.IsAssignableFrom(view.Superview.GetType()))
                {
                    return view.Superview;
                }

                if (view.Superview != stopAt)
                    return view.Superview.FindSuperviewOfType(stopAt, type);
            }

            return null;
        }

    }

	public static class UIViewTinyConstraints
	{
		public static NSLayoutConstraint LeadTo(this UIView firstView, UIView secondView, int constant = 0, bool isSafeLayoutGuide = false)
        {
            if (!UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                isSafeLayoutGuide = false;
            }
            firstView.TranslatesAutoresizingMaskIntoConstraints = false;
            secondView.TranslatesAutoresizingMaskIntoConstraints = false;
            var constraint = firstView.LeadingAnchor.ConstraintEqualTo(isSafeLayoutGuide ? secondView.SafeAreaLayoutGuide.LeadingAnchor : secondView.LeadingAnchor, constant);
            constraint.Active = true;
            return constraint;
        }

        public static NSLayoutConstraint TrailTo(this UIView firstView, UIView secondView, int constant = 0, bool isSafeLayoutGuide = false)
        {
            if (!UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                isSafeLayoutGuide = false;
            }
            firstView.TranslatesAutoresizingMaskIntoConstraints = false;
            secondView.TranslatesAutoresizingMaskIntoConstraints = false;
            var constraint = firstView.TrailingAnchor.ConstraintEqualTo(isSafeLayoutGuide ? secondView.SafeAreaLayoutGuide.TrailingAnchor : secondView.TrailingAnchor, constant);
            constraint.Active = true;
            return constraint;
        }

        public static NSLayoutConstraint TopTo(this UIView firstView, UIView secondView, int constant = 0, bool isSafeLayoutGuide = false)
        {
            if (!UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                isSafeLayoutGuide = false;
            }
            firstView.TranslatesAutoresizingMaskIntoConstraints = false;
            secondView.TranslatesAutoresizingMaskIntoConstraints = false;
            var constraint = firstView.TopAnchor.ConstraintEqualTo(isSafeLayoutGuide ? secondView.SafeAreaLayoutGuide.TopAnchor : secondView.TopAnchor, constant);
            constraint.Active = true;
            return constraint;
        }

        public static NSLayoutConstraint BottomTo(this UIView firstView, UIView secondView, int constant = 0, bool isSafeLayoutGuide = false)
        {
            if (!UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                isSafeLayoutGuide = false;
            }
            firstView.TranslatesAutoresizingMaskIntoConstraints = false;
            secondView.TranslatesAutoresizingMaskIntoConstraints = false;
            var constraint = firstView.BottomAnchor.ConstraintEqualTo(isSafeLayoutGuide ? secondView.SafeAreaLayoutGuide.BottomAnchor : secondView.BottomAnchor, constant);
            constraint.Active = true;
            return constraint;
        }

        public static NSLayoutConstraint TopToBottom(this UIView firstView, UIView secondView, int constant = 0, bool isSafeLayoutGuide = false)
        {
            if (!UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                isSafeLayoutGuide = false;
            }
            firstView.TranslatesAutoresizingMaskIntoConstraints = false;
            secondView.TranslatesAutoresizingMaskIntoConstraints = false;
            var constraint = firstView.TopAnchor.ConstraintEqualTo(isSafeLayoutGuide ? secondView.SafeAreaLayoutGuide.BottomAnchor : secondView.BottomAnchor, constant);
            constraint.Active = true;
            return constraint;
        }

        public static NSLayoutConstraint BottomToTop(this UIView firstView, UIView secondView, int constant = 0, bool isSafeLayoutGuide = false)
        {
            if (!UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                isSafeLayoutGuide = false;
            }
            firstView.TranslatesAutoresizingMaskIntoConstraints = false;
            secondView.TranslatesAutoresizingMaskIntoConstraints = false;
            var constraint = firstView.BottomAnchor.ConstraintEqualTo(isSafeLayoutGuide ? secondView.SafeAreaLayoutGuide.TopAnchor : secondView.TopAnchor, constant);
            constraint.Active = true;
            return constraint;
        }

        public static NSLayoutConstraint WidthToView(this UIView firstView, UIView secondView, int multiplier = 1, int constant = 0, bool isSafeLayoutGuide = false)
        {
            if (!UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                isSafeLayoutGuide = false;
            }
            firstView.TranslatesAutoresizingMaskIntoConstraints = false;
            secondView.TranslatesAutoresizingMaskIntoConstraints = false;
            var constraint = firstView.WidthAnchor.ConstraintEqualTo(isSafeLayoutGuide ? secondView.SafeAreaLayoutGuide.WidthAnchor : secondView.WidthAnchor, multiplier, constant);
            constraint.Active = true;
            return constraint;
        }

        public static NSLayoutConstraint WidthEquals(this UIView firstView, int constant)
        {
            firstView.TranslatesAutoresizingMaskIntoConstraints = false;
            var constraint = firstView.WidthAnchor.ConstraintEqualTo(constant);
            constraint.Active = true;
            return constraint;
        }

        public static NSLayoutConstraint HeightToView(this UIView firstView, UIView secondView, int multiplier = 1, int constant = 0, bool isSafeLayoutGuide = false)
        {
            if (!UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                isSafeLayoutGuide = false;
            }
            firstView.TranslatesAutoresizingMaskIntoConstraints = false;
            secondView.TranslatesAutoresizingMaskIntoConstraints = false;
            var constraint = firstView.HeightAnchor.ConstraintEqualTo(isSafeLayoutGuide ? secondView.SafeAreaLayoutGuide.HeightAnchor : secondView.HeightAnchor, multiplier, constant);
            constraint.Active = true;
            return constraint;
        }

        public static NSLayoutConstraint HeightEquals(this UIView firstView, int constant)
        {
            firstView.TranslatesAutoresizingMaskIntoConstraints = false;
            var constraint = firstView.HeightAnchor.ConstraintEqualTo(constant);
            constraint.Active = true;
            return constraint;
        }

        public static NSLayoutConstraint LeadToSuperView(this UIView view, int constant = 0, bool isSafeLayoutGuide = false)
        {
            return view.LeadTo(view.Superview, constant, isSafeLayoutGuide);
        }

        public static NSLayoutConstraint TrailToSuperView(this UIView view, int constant = 0, bool isSafeLayoutGuide = false)
        {
            return view.TrailTo(view.Superview, constant, isSafeLayoutGuide);
        }

        public static NSLayoutConstraint TopToSuperView(this UIView view, int constant = 0, bool isSafeLayoutGuide = false)
        {
            return view.TopTo(view.Superview, constant, isSafeLayoutGuide);
        }

        public static NSLayoutConstraint BottomToSuperView(this UIView view, int constant = 0, bool isSafeLayoutGuide = false)
        {
            return view.BottomTo(view.Superview, constant, isSafeLayoutGuide);
        }

        public static void AllEdgesToView(this UIView firstView, UIView secondView, int constant = 0, bool isSafeLayoutGuide = false)
        {
            firstView.LeadTo(secondView, constant, isSafeLayoutGuide);
            firstView.TrailTo(secondView, -constant, isSafeLayoutGuide);
            firstView.TopTo(secondView, constant, isSafeLayoutGuide);
            firstView.BottomTo(secondView, -constant, isSafeLayoutGuide);
        }

        public static void AllEdgesToSuperView(this UIView view, int constant = 0, bool isSafeLayoutGuide = false)
        {
            view.AllEdgesToView(view.Superview, constant, isSafeLayoutGuide);
        }

        public static void EdgesToView(this UIView firstView, UIView secondView, int constant = 0, bool isSafeLayoutGuide = false)
        {
            firstView.LeadTo(secondView, constant, isSafeLayoutGuide);
            firstView.TrailTo(secondView, -constant, isSafeLayoutGuide);
        }

        public static void EdgesToSuperview(this UIView view, int constant = 0, bool isSafeLayoutGuide = false)
        {
            view.EdgesToView(view.Superview, constant, isSafeLayoutGuide);
        }
	}
}
