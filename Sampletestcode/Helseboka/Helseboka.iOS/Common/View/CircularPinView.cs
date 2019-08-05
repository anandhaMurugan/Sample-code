using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using UIKit;
using Helseboka.Core.Common.Extension;

namespace Helseboka.iOS.Common.View
{
	[Register("CircularPinView"), DesignTimeVisible(true)]
	public class CircularPinView : UIView, IUITextFieldDelegate
    {
		private UITapGestureRecognizer TapGestureRecognizer { get; set; }
		private List<Circle> InnerCircleList { get; set; } = new List<Circle>();
		private UITextField PasswordField { get; set; }
		private bool newEditingStarted = false;

		public event EventHandler<String> Completed;
        public event EventHandler<String> EditingChanged;

		public String Password { get; private set; }

		private int maxLength = 4;
		[Export("MaxLength"), Browsable(true)]
        public int MaxLength 
		{
			get => maxLength;
			set
			{
				maxLength = value;
				Initialize();
			} 
		}
        
		[Export("InnerCircleRatio"), Browsable(true)]
        public float InnerCircleRatio { get; set; } = 0.6f;

		[Export("PaddingBetweenCircleFactor"), Browsable(true)]
        public float PaddingBetweenCircleFactor { get; set; } = 0.2f;

		public CircularPinView(IntPtr handle) : base(handle) { }

		public CircularPinView()
        {
            // Called when created from code.
            Initialize();
        }

        public override void AwakeFromNib()
        {
            // Called when loaded from xib or storyboard.
            Initialize();
        }
        
		public void Initialize()
        {
			Reset();
			var availableWidth = Frame.Width;

			// availableWidth = MaxLength * CircleWidth + (MaxLength - 1) * Padding
			// Padding = PaddingBetweenCircleFactor * CircleWidth
            // Solve for CircleWidth
			var circleWidth = availableWidth / (MaxLength + ((MaxLength - 1) * PaddingBetweenCircleFactor));
			var padding = circleWidth * PaddingBetweenCircleFactor;
			var circleYindex = (Frame.Height / 2) - (circleWidth / 2);
			double xIndex = 0;
            
			for (int index = 0; index < MaxLength; index++)
			{
				var circle = new Circle();
				circle.Frame = new CGRect(xIndex, circleYindex, circleWidth, circleWidth);
				circle.InnerCircleRatio = InnerCircleRatio;

				InnerCircleList.Add(circle);
				AddSubview(circle);

				xIndex += circleWidth + padding;
			}

			PasswordField = new UITextField(new CGRect(0, 0, Frame.Width, 50));
			PasswordField.SecureTextEntry = true;
			PasswordField.KeyboardType = UIKeyboardType.NumberPad;
			PasswordField.Delegate = this;
            PasswordField.EditingChanged += PasswordField_EditingChanged;
			PasswordField.Hidden = true;
            
            
			AddSubview(PasswordField);

			TapGestureRecognizer = new UITapGestureRecognizer(() => Focus());
			AddGestureRecognizer(TapGestureRecognizer);

			PasswordField.BecomeFirstResponder();
        }

		private void Reset()
		{
			Password = String.Empty;
			foreach (UIView view in Subviews)
            {
                view.RemoveFromSuperview();
            }
			InnerCircleList.Clear();
			if(PasswordField != null)
			{
                PasswordField.EditingChanged -= PasswordField_EditingChanged;
				PasswordField.Delegate = null;
				PasswordField = null;
			}
			if (TapGestureRecognizer !=  null)
			{
				RemoveGestureRecognizer(TapGestureRecognizer);
			}
		}

		public void Focus()
		{
			PasswordField.BecomeFirstResponder();
		}

		public void UnFocus()
        {
			PasswordField.ResignFirstResponder();
        }

		public void Clear()
		{
			PasswordField.Text = String.Empty;
			Password = String.Empty;
			newEditingStarted = true;
			foreach(Circle circle in InnerCircleList)
			{
				circle.HideInnerCircle();
			}
		}

		[Export("textFieldShouldBeginEditing:")]
		public bool ShouldBeginEditing(UITextField textField)
		{
			newEditingStarted = true;

			return true;
		}
        
		[Export("textField:shouldChangeCharactersInRange:replacementString:")]
		public bool ShouldChangeCharacters(UITextField textField, NSRange range, string replacementString)
		{
			using (NSString original = new NSString(textField.Text), replace = new NSString(replacementString))
            {
				var newText = original.Replace(range, replace).ToString();
				if ((newText.Length <= MaxLength && newText.IsDigit()) || String.IsNullOrEmpty(newText))
				{
					if (newEditingStarted)
					{
						// As per default behavior of UITextfield on iOS 6 onwards, if UITextfield has secure attribute turned on
                        // Then if user leaves keyboard, then after user comeback it will be a new entry.
						Password = replacementString;
						newEditingStarted = false;
					}
					else
					{
						Password = newText;
					}

					UpdateCirclePinView();

					return true;
				}
				else 
				{
					return false;
				}
            }
		}

        private void PasswordField_EditingChanged(object sender, EventArgs e)
        {
            if (Password.Length == MaxLength && Completed != null)
            {
                RaiseCompletedAfterDelay(Password).Forget();
            }
            else
            {
                EditingChanged?.Invoke(this, PasswordField.Text);
            }
        }

		private async Task RaiseCompletedAfterDelay(String password)
		{
			await Task.Delay(200);
			Completed?.Invoke(Self, password);
		}

		private void UpdateCirclePinView()
		{
			for (int index = 0; index < InnerCircleList.Count; index++)
			{
				if (index < Password.Length)
				{
					InnerCircleList[index].ShowInnerCircle();
				}
				else
				{
					InnerCircleList[index].HideInnerCircle();
				}
			}
		}
	}
}
