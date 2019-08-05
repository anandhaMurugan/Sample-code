using System;
using Helseboka.Core.Common.CommonImpl;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;

namespace Helseboka.Core.Common.Constant
{
	public static class APIEndPoints
	{
		private static IConfig Config
		{
            get => ApplicationCore.Container.Resolve<IConfig>();
        }

        private static IBankIdConfig BankIDConfig
        {
            get => ApplicationCore.Container.Resolve<IBankIdConfig>();
        }

        private const String authStart = "/authstart2?BidType={0}&okUrl={1}&cancelUrl={2}&errorUrl={3}&apiVersion={4}";
        public static String AuthStart
		{
			get => Config.BaseURL + BankIDConfig.Bankid + authStart;
		}

        private const String authenticate = "/authenticate2?sid={0}&token={1}";
        public static String Authenticate
        {
			get => Config.BaseURL + BankIDConfig.Authentication + authenticate;
        }
        private const String userInfo = "user";
        public static String UserInfo
        {
            get => Config.BaseURL + userInfo;
        }

        private const String getUserTerms = "user/terms";
        public static String GetUserTerms
        {
            get => Config.BaseURL + getUserTerms;
        }

        private const String updateNotificationToken = "notification/tokens";
        public static String UpdateNotificationToken
        {
            get => Config.BaseURL + updateNotificationToken;
        }

        private const String searchDoctorAndOffice = "ar/search?name={0}&page={1}&count={2}";
        public static String SearchDoctorAndOffice
        {
            get => Config.BaseURL + searchDoctorAndOffice;
        }

        private const String getDoctorDetails = "ar/doctor/{0}";
        public static String GetDoctorDetails
        {
            get => Config.BaseURL + getDoctorDetails;
        }

        private const String getAllThreads = "thread/?page={0}&count={1}";
        public static String GetAllThreads
        {
            get => Config.BaseURL + getAllThreads;
        }

        private const String getMessagesForAThread = "thread/{0}/message?page={1}&count={2}";
        public static String GetMessagesForAThread
        {
            get => Config.BaseURL + getMessagesForAThread;
        }

        private const String sendMessageToAThread = "thread/{0}/message";
        public static String SendMessageToAThread
        {
            get => Config.BaseURL + sendMessageToAThread;
        }

        private const String createNewThread = "message";
        public static String CreateNewThread
        {
            get => Config.BaseURL + createNewThread;
        }

        private const String searchMedicine = "medicine/search?name={0}";
        public static String SearchMedicine
        {
            get => Config.BaseURL + searchMedicine;
        }

        private const String getAllMedicineAndReminders = "user/medicine";
        public static String GetAllMedicineAndReminders
        {
            get => Config.BaseURL + getAllMedicineAndReminders;
        }

        private const String addMedicine = "user/medicine";
        public static String AddMedicine
        {
            get => Config.BaseURL + addMedicine;
        }

        // Development Dummy - 
        private const String renewPrescription = "prescriptions";
        public static String RenewPrescription
        {
            get => Config.BaseURL + renewPrescription;
        }

        private const String deleteMedicine = "user/medicine/{0}";
        public static String DeleteMedicine
        {
            get => Config.BaseURL + deleteMedicine;
        }

        private const String deleteReminder = "user/medicine/{0}/reminder";
        public static String DeleteReminder
        {
            get => Config.BaseURL + deleteReminder;
        }

        private const String getAlarms = "user/medicine/alarm?date={0}";
        public static String GetAlarms
        {
            get => Config.BaseURL + getAlarms;
        }

        private const String markAlarmAsComplete = "user/medicine/alarm/{0}/complete";
        public static String MarkAlarmAsComplete
        {
            get => Config.BaseURL + markAlarmAsComplete;
        }

        // Development Dummy -
        private const String addAppointment = "appointments";
        public static String AddAppointment
        {
            get => Config.BaseURL + addAppointment;
        }

        private const String getAllAppointments = "appointments/?page={0}&count={1}";
        public static String GetAllAppointments
        {
            get => Config.BaseURL + getAllAppointments;
        }

        private const String cancelAppointment = "appointments/{0}/cancel";
        public static String CancelAppointment
        {
            get => Config.BaseURL + cancelAppointment;
        }

        private const String nextAppointment = "appointments/next";
        public static String NextAppointment
        {
            get => Config.BaseURL + nextAppointment;
        }

        private const String deleteProfile = "gdpr";
        public static String DeleteProfile
        {
            get => Config.BaseURL + deleteProfile;
        }

        private const String getUpdateInfo = "static-data/app-versions";
        public static String GetUpdateInfo
        {
            get => Config.BaseURL + getUpdateInfo;
        }

        private const String getUpdateIosUrl = "https://itunes.apple.com/no/app/helseboka/id1167998336?l=nb&mt=8";
        public static String GetUpdateIosUrl
        {
            get => getUpdateIosUrl;
        }

        private const String getUpdateAndroidUrl = "https://play.google.com/store/apps/details?id=com.helseapps.helseboka&hl=en_US";
        public static String GetUpdateAndroidUrl
        {
            get => getUpdateAndroidUrl;
        }
    }
}

