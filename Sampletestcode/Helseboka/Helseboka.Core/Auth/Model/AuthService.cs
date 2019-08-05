using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Helseboka.Core.Auth.Interface;
using Helseboka.Core.Common.CommonImpl;
using Helseboka.Core.Common.Constant;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;

namespace Helseboka.Core.Auth.Model
{
    public enum BankIDType
    {
        BID,
        BIM
    }

    public class AuthService
    {
        #region Singleton Implementation
        //Lazy singleton initialization
        // http://csharpindepth.com/Articles/General/Singleton.aspx
        // Double-check-Locking will not be required - https://msdn.microsoft.com/en-us/library/ff650316.aspx
        private AuthService()
        {
        }

        public static AuthService Instance { get { return Nested.instance; } }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly AuthService instance = new AuthService();
        }
        #endregion

        protected IConfig Config
        {
            get => ApplicationCore.Container.Resolve<IConfig>();
        }

        private IAuthAPI DataHandler
        {
            get => ApplicationCore.Container.Resolve<IAuthAPI>();
        }

        private ISecureDataStorage SecureStorage
        {
            get => ApplicationCore.Container.Resolve<ISecureDataStorage>();
        }

        private IDeviceHandler DeviceHandler
        {
            get => ApplicationCore.Container.Resolve<IDeviceHandler>();
        }

        private ICrypto CryptoHandler
        {
            get => ApplicationCore.Container.Resolve<ICrypto>();
        }

        private bool isAPIKeyRetrieviedFromKeyChain = false;
        private bool isPINHashRetrieviedFromKeyChain = false;
        private String pinHash;

        public LoginMode? ModeOfLogin { get; private set; }
        public String SessionID { get; private set; }
        public String AuthUrl { get; private set; }
        public String Token { get; private set; }
        public String APIKey { get; private set; }

        public bool IsFirstTimeUser()
        {
            var isFirstLaunchValuePresent = SecureStorage.GetAppFirstLaunch();
            // Delete all data from keychain when app uninstall and install again
            if (!isFirstLaunchValuePresent)
            {
                SecureStorage.DeleteAll();
                SecureStorage.SetAppFirstLaunch();
                return true;
            }
            else
            {
                return false;
            }
        }

        private String RetrieveAPIKey()
        {
            if (!isAPIKeyRetrieviedFromKeyChain)
            {
                APIKey = SecureStorage.GetData(SecureDataType.APIKey);
                isAPIKeyRetrieviedFromKeyChain = true;
            }

            return APIKey;
        }

        private LoginMode RetrieveLoginMode()
        {
            if (!ModeOfLogin.HasValue)
            {
                var loginModeValue = SecureStorage.GetData(SecureDataType.LoginMode);
                if (!String.IsNullOrEmpty(loginModeValue) && Enum.TryParse<LoginMode>(loginModeValue, out var result))
                {
                    ModeOfLogin = result;
                }
                else
                {
                    ModeOfLogin = LoginMode.BankID;
                }
            }

            return ModeOfLogin.Value;
        }

        private String RetrievePINHash()
        {
            if (!isPINHashRetrieviedFromKeyChain)
            {
                pinHash = SecureStorage.GetData(SecureDataType.PIN);
                isPINHashRetrieviedFromKeyChain = true;
            }

            return pinHash;
        }

        public bool CanLoginWithBio()
        {
            if (!IsFirstTimeUser())
            {
                RetrieveLoginMode();

                return ModeOfLogin.HasValue && ModeOfLogin.Value == LoginMode.Biometric;
            }
            else
            {
                ModeOfLogin = LoginMode.BankID;
                return false;
            }
        }

        public bool CanLoginWithPIN()
        {
            if (!IsFirstTimeUser())
            {
                RetrieveLoginMode();

                if (ModeOfLogin.HasValue && ModeOfLogin.Value == LoginMode.PIN)
                {
                    RetrievePINHash();

                    return !String.IsNullOrEmpty(pinHash);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                ModeOfLogin = LoginMode.BankID;
                return false;
            }
        }

        public void RegisterBio()
        {
            SecureStorage.StoreData(APIKey, SecureDataType.APIKey);
            SecureStorage.StoreData(LoginMode.Biometric.ToString(), SecureDataType.LoginMode);
        }

        public void RegisterPIN(string pin)
        {
            var hash = CryptoHandler.Hash(pin);
            SecureStorage.StoreData(hash, SecureDataType.PIN);
            SecureStorage.StoreData(APIKey, SecureDataType.APIKey);
            SecureStorage.StoreData(LoginMode.PIN.ToString(), SecureDataType.LoginMode);

            ModeOfLogin = LoginMode.PIN;
            pinHash = hash;
        }

        public async Task<Response> LoginWithPIN(String pin)
        {
            RetrievePINHash();
            var isSuccess = CryptoHandler.Verify(pin, pinHash);
            if (isSuccess)
            {
                RetrieveAPIKey();
                return await ApplicationCore.Instance.GetUserDetails();
            }
            else
            {
                return Response.GetAPIErrorResponse(APIError.WrongPIN);
            }
        }

        public async Task<Response> LoginWithBIO()
        {
            RetrieveAPIKey();
            return await ApplicationCore.Instance.GetUserDetails();
        }

        public async Task<Response> StartAuth(BankIDType bIDType)
        {
            var response = await DataHandler.AuthStart(bIDType.ToString());

            if (response.IsSuccess)
            {
                SessionID = response.Result.SID;
                AuthUrl = response.Result.URI;
                Token = response.Result.Token;
            }

            return response;
        }

        public async Task<Response> CompleteAuth()
        {
            var response = await DataHandler.Authenticate(SessionID, Token);

            if (response.IsSuccess)
            {
                if (response.Result != null && !String.IsNullOrEmpty(response.Result.APIKey))
                {
                    APIKey = response.Result.APIKey;

                    SecureStorage.StoreData(APIKey, SecureDataType.APIKey);

                    return await ApplicationCore.Instance.GetUserDetails();
                }
                else
                {
                    return Response.GetGenericAPIErrorResponse();
                }
            }

            return response;
        }

        public Dictionary<String, String> GetSecurityHeaders()
        {
            var headers = new Dictionary<String, String>();
            headers.Add(APIConstant.AuthorizationHeaderKey, GetBasicAuthHeaderValue());
            headers.Add(APIConstant.SessionIDHeaderKey, SessionID);

            return headers;
        }

        public String GetBasicAuthHeaderValue()
        {
            var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Config.UserName}:{Config.Password}"));
            return String.Format(APIConstant.AuthorizationHeaderValueFormat, authValue);
        }

        public void Logout()
        {
            pinHash = String.Empty;
            ModeOfLogin = null;
            isAPIKeyRetrieviedFromKeyChain = false;
            isPINHashRetrieviedFromKeyChain = false;
            SessionID = String.Empty;
            AuthUrl = String.Empty;
            Token = String.Empty;
            APIKey = String.Empty;
        }

        public void DeleteAllUserAuthData()
        {
            Logout();
            SecureStorage.DeleteAll();
        }
    }
}
