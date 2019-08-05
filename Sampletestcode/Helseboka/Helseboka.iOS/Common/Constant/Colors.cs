using System;
using UIKit;

namespace Helseboka.iOS.Common.Constant
{
	public static class Colors
    {
		/// <summary>
        /// Pin view outer circle border color
        /// </summary>
		public static readonly UIColor BorderColor = UIColor.FromRGB((byte)31, (byte)202, (byte)211);

        /// <summary>
        /// Pin view outer circle fill color. rgb 245 255 255
        /// </summary>
		public static readonly UIColor FillColor = UIColor.FromRGB((byte)245, (byte)255, (byte)255);

        /// <summary>
        /// Pin view inner circle fill color
        /// </summary>
		public static readonly UIColor CircularFillColor = UIColor.FromRGB((byte)23, (byte)199, (byte)208);

        /// <summary>
        /// Home page help view expander/collapser border color
        /// </summary>
		public static readonly UIColor ExpanderBorderColor = UIColor.FromRGB((byte)215, (byte)215, (byte)215);

		/// <summary>
        /// Home page help view expander/collapser fill color
        /// </summary>
        public static readonly UIColor ExpanderFillColor = UIColor.FromRGB((byte)248, (byte)255, (byte)255);

		/// <summary>
        /// Description text color e.g. no data text in home page
        /// </summary>
        public static readonly UIColor DescriptionLabelTextColor = UIColor.FromRGB((byte)69, (byte)69, (byte)69);

        /// <summary>
        /// Help text color in BankId page, (63, 63, 63)
        /// </summary>
		public static readonly UIColor LoginHelpTextColor = UIColor.FromRGB((byte)63, (byte)63, (byte)63);

		/// <summary>
        /// Secondary action button text color - TouchId registration page cancel button
        /// </summary>
        public static readonly UIColor CancelButtonTextColor = UIColor.FromRGB((byte)122, (byte)122, (byte)122);

		/// <summary>
        /// Theme color
        /// </summary>
        public static readonly UIColor ThemeTurkise = UIColor.FromRGB((byte)74, (byte)217, (byte)223);

        /// <summary>
        /// Background color for selected search result - e.g. doctor or Legecenter search - (245, 255, 255)
        /// </summary>
        public static readonly UIColor SearchResultBackground = UIColor.FromRGB((byte)245, (byte)255, (byte)255);

        /// <summary>
        /// Page title text color. (18, 18, 18)
        /// </summary>
        public static readonly UIColor TitleTextColor = UIColor.FromRGB((byte)18, (byte)18, (byte)18);

        /// <summary>
        /// Border color of label when unselected e.g. doctor or Legecenter selection - (25,206,214)
        /// </summary>
        public static readonly UIColor UnselectedLabelBorderColor = UIColor.FromRGB((byte)25, (byte)206, (byte)214);

        /// <summary>
        /// Border color of label when selected e.g. doctor or Legecenter selection
        /// </summary>
        public static readonly UIColor SelectedLabelBorderColor = UIColor.FromRGB((byte)3, (byte)181, (byte)189);

        /// <summary>
        /// Text color for send message in legedialog
        /// </summary>
        public static readonly UIColor SentMessageTextColor = UIColor.FromRGB((byte)120, (byte)120, (byte)120);

        /// <summary>
        /// Text color for received message in legedialog
        /// </summary>
        public static readonly UIColor ReceivedMessageTextColor = UIColor.FromRGB((byte)29, (byte)29, (byte)29);

        /// <summary>
        /// Border color of label for sent message in legedialog chat page
        /// </summary>
        public static readonly UIColor SentMessageBorderColor = UIColor.FromRGB((byte)236, (byte)236, (byte)236);

        /// <summary>
        /// Date label text color in chat page for legedialog
        /// </summary>
        public static readonly UIColor DateLabelTextColor = UIColor.FromRGB((byte)191, (byte)190, (byte)190);

        /// <summary>
        /// Border color of doctor or medical center name in new legedialog page
        /// </summary>
        public static readonly UIColor DoctorSelectionLabelBorderColor = UIColor.FromRGB((byte)228, (byte)253, (byte)252);

        /// <summary>
        /// The fill color of doctor or medical center label in new legedialog creation view
        /// </summary>
        public static readonly UIColor DoctorSelectionLebelFillColor = UIColor.FromRGB((byte)232, (byte)252, (byte)252);

        /// <summary>
        /// Placeholder Text Color for new lege dialog view. (77, 77, 77, 59%)
        /// </summary>
        public static readonly UIColor PlaceholderTextColor = UIColor.FromRGBA((byte)77, (byte)77, (byte)77, (byte)(0.59 * 255));

        /// <summary>
        /// Separator color for tableview e.g. Medicine alarm view. (151, 151, 151)
        /// </summary>
        public static readonly UIColor SeparatorColor = UIColor.FromRGB((byte)151, (byte)151, (byte)151);

        /// <summary>
        /// The color of the add more medicine label. (56, 56, 56)
        /// </summary>
        public static readonly UIColor AddMoreMedicineLabelColor = UIColor.FromRGB((byte)56, (byte)56, (byte)56);

        /// <summary>
        /// The color of the alarm label text. (187, 180, 180)
        /// </summary>
        public static readonly UIColor AlarmLabelTextColor = UIColor.FromRGB((byte)187, (byte)180, (byte)180);

        /// <summary>
        /// The color of the date label text. (128, 128, 128)
        /// </summary>
        public static readonly UIColor DateSelectionLabelTextColor = UIColor.FromRGB((byte)128, (byte)128, (byte)128);



		/// <summary>
        /// The color of the gray label text. (169,169,169)
        /// </summary>
        public static readonly UIColor GrayLabelTextColor = UIColor.FromRGB((byte)169,(byte)169, (byte)169);


		/// <summary>
        /// The color of the HyperLink Black label text. (88,88,88)
        /// </summary>
        public static readonly UIColor HyperLinkLabelTextColor = UIColor.FromRGB((byte)88, (byte)88, (byte)88);


		/// <summary>
        /// The color of the HyperLink Purple Hyperlink Button Color (36,0,161)
        /// </summary>
        public static readonly UIColor HyperLinkButtonTextColor = UIColor.FromRGB((byte)36, (byte)0, (byte)161);

        /// <summary>
        /// The color of the shadow. rgba 213 213 213 0.5
        /// </summary>
        public static readonly UIColor ShadowColor = UIColor.FromRGBA((byte)213, (byte)213, (byte)213, (byte)(255 * 0.5));

        /// <summary>
        /// The color of the dashboard alarm border. rgb 212 163 255
        /// </summary>
        public static readonly UIColor DashboardAlarmBorderColor = UIColor.FromRGB((byte)212, (byte)163, (byte)255);

        /// <summary>
        /// The color of the dashboard alarm place holder border. rgb 229 229 229
        /// </summary>
        public static readonly UIColor DashboardAlarmPlaceHolderBorderColor = UIColor.FromRGB((byte)229, (byte)229, (byte)229);

        /// <summary>
        /// The color of the dashboard appointment border. rgb 122 243 243
        /// </summary>
        public static readonly UIColor DashboardAppointmentBorderColor = UIColor.FromRGB((byte)122, (byte)243, (byte)243);

        /// <summary>
        /// The color of the dashboard appointment alert background. rgba 186 247 247 45
        /// </summary>
        public static readonly UIColor DashboardAppointmentAlertBackgroundColor = UIColor.FromRGBA((byte)186, (byte)247, (byte)247, (byte)(255 * 0.45));

        /// <summary>
        /// The color of the appointment other view border. rgb 221 221 221
        /// </summary>
        public static readonly UIColor AppointmentOtherViewBorderColor = UIColor.FromRGB((byte)221, (byte)221, (byte)221);

        /// <summary>
        /// The color of the appointment other view circle border. rgb 138 136 146
        /// </summary>
        public static readonly UIColor AppointmentOtherViewCircleBorderColor = UIColor.FromRGB((byte)138, (byte)136, (byte)146);

        /// <summary>
        /// The color of the pop up shadow. rgba 90 88 88 0.5
        /// </summary>
        public static readonly UIColor PopUpShadowColor = UIColor.FromRGBA((byte)90, (byte)88, (byte)88, (byte)(0.5 * 255));

        /// <summary>
        /// The color of the pop up selected button. rgb 231 255 255
        /// </summary>
        public static readonly UIColor PopUpSelectedButtonColor = UIColor.FromRGB((byte)231, (byte)255, (byte)255);

       /// <summary>
       /// The color of the disabled button text. (0, 0, 0, 28%)
       /// </summary>
        public static readonly UIColor DisabledButtonTextColor = UIColor.FromRGBA((byte)0, (byte)0, (byte)0, (byte)(0.28 * 255));

        /// <summary>
        /// The color of the alarm switch off. 193 193 193
        /// </summary>
        public static readonly UIColor AlarmSwitchOffColor = UIColor.FromRGB((byte)193, (byte)193, (byte)193);

        /// <summary>
        /// The color of the alarm switch on. 81 0 160
        /// </summary>
        public static readonly UIColor AlarmSwitchOnColor = UIColor.FromRGB((byte)81, (byte)0, (byte)160);

        /// <summary>
        /// The color of the my profile logout. 82 0 160
        /// </summary>
        public static readonly UIColor MyProfileLogoutColor = UIColor.FromRGB(82, 0, 160);

        public static readonly UIColor MyProfileDeleteProfileColor = UIColor.Red;

        /// <summary>
        /// The color of the onboarding button title. 107, 107, 107
        /// </summary>
        public static readonly UIColor OnboardingButtonTitleColor = UIColor.FromRGB(107, 107, 107);
    }
}
