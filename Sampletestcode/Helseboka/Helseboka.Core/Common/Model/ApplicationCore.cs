using System;
using System.Threading.Tasks;
using Helseboka.Core.AppointmentModule.DataAccess;
using Helseboka.Core.AppointmentModule.Interface;
using Helseboka.Core.Auth.DataAccess;
using Helseboka.Core.Auth.Interface;
using Helseboka.Core.Common.CommonImpl;
using Helseboka.Core.Common.DataAccess;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.Common.Interface;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.HelpAndFAQ.DataAccess;
using Helseboka.Core.HelpAndFAQ.Interface;
using Helseboka.Core.Legedialog.DataAccess;
using Helseboka.Core.Legedialog.Interface;
using Helseboka.Core.MedicineModule.DataAccess;
using Helseboka.Core.MedicineModule.Interface;
using Helseboka.Core.Profile.DataAccess;
using Helseboka.Core.Profile.Interface;
using Helseboka.Core.Profile.Model;
using Helseboka.Core.Terms.DataAccess;
using Helseboka.Core.Terms.Interface;

namespace Helseboka.Core.Common.Model
{
    public class ApplicationCore
    {
        public static IContainer Container { get; private set; }

        static ApplicationCore()
        {
            Container = new UnityContainerImpl();
            Container.RegisterType<ILogger, ConsoleLogger>();
            Container.RegisterType<IConfig, DevConfig>();
            Container.RegisterType<IBankIdConfig, ProdBankIdConfig>();
            Container.RegisterType<IHttpClient, RESTClient>();
            Container.RegisterType<ISerializer, JSONSerializer>();
            Container.RegisterType<ICrypto, BCCryptoHandler>();

            Container.RegisterType<IMessagingAPI, MessagingDataHandler>();
            Container.RegisterType<IAuthAPI, AuthDataHandler>();
            Container.RegisterType<ITermsAPI, TermsDataHandler>();
            Container.RegisterType<IUserAPI, ProfileDataHandler>();
            Container.RegisterType<IARService, ARDataHandler>();
            Container.RegisterType<IMedicineAPI, MedicineDataHandler>();
            Container.RegisterType<IAppointmentAPI, AppointmentDataHandler>();
            Container.RegisterType<IHelpFAQDataAccess, HelpDataHandler>();
        }

        #region Singleton Implementation
        //Lazy singleton initialization
        // http://csharpindepth.com/Articles/General/Singleton.aspx
        // Double-check-Locking will not be required - https://msdn.microsoft.com/en-us/library/ff650316.aspx
        private ApplicationCore()
        {
        }

        public static ApplicationCore Instance { get { return Nested.instance; } }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

			internal static readonly ApplicationCore instance = new ApplicationCore();
        }
		#endregion

		private ISecureDataStorage SecureStorage
        {
            get => Container.Resolve<ISecureDataStorage>();
        }

        public User CurrentUser { get; private set; }

		public void Initialize()
		{

		}

        public async Task<Response<User>> GetUserDetails()
        {
            var dataHandler = Container.Resolve<IUserAPI>();
            var response = await dataHandler.GetUserInfo();
            if (response.IsSuccess)
            {
                CurrentUser = response.Result;
                Container.Resolve<INotificationService>().CompletePendingTask().Forget();
                await CurrentUser.GetDoctor();
            }

            return response;
        }

        public async Task<Response<PaginationResponse<MedicalCenter>>> SearchDoctorOrOffice(String searchText, int pageNum, int pageSize)
        {
            var dataHandler = Container.Resolve<IARService>();
            return await dataHandler.SearchDoctorOrOffice(searchText, pageNum, pageSize);
        }
    }
}
