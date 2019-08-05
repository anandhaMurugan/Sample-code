using System;
using System.Text;
using Foundation;
using Helseboka.Core.Common.Interfaces;
using Security;

namespace Helseboka.iOS.Common.CommonImpl
{
	public class SecureStorageHandler : ISecureDataStorage
    {
        private static readonly String Keychain_Service = "HelseApps";

        public SecureStorageHandler()
        {
        }

		public void DeleteAll()
		{
			DeleteData(SecureDataType.APIKey);
			DeleteData(SecureDataType.LoginMode);
			DeleteData(SecureDataType.PIN);
		}

		public void DeleteData(SecureDataType dataType)
		{
            DeleteFromKeychain(dataType.ToString());
            //DeleteFromUserDefault(dataType.ToString());
		}

		public string GetData(SecureDataType dataType)
		{
            return GetData(dataType.ToString());
            //return GetFromUserDefault(dataType.ToString());
		}

		public void StoreData(string data, SecureDataType dataType)
		{
            SaveDataTokeychain(data, dataType.ToString());
            //SaveDataToUserDefault(data, dataType.ToString());
		}

        private void SaveDataToUserDefault(String data, String key)
        {
            var userDefault = new NSUserDefaults();
            userDefault.SetString(data, key);
            userDefault.Synchronize();
        }

        private String GetFromUserDefault(String key)
        {
            var userDefault = new NSUserDefaults();
            return userDefault.StringForKey(key);
        }

        private void DeleteFromUserDefault(String key)
        {
            var userDefault = new NSUserDefaults();
            userDefault.RemoveObject(key);
            userDefault.Synchronize();
        }

		private void SaveDataTokeychain(String data, String key)
		{
            var existingRecord = GetRecord(key);

            SecRecord match = SecKeyChain.QueryAsRecord(existingRecord, out var result);

            var newRecord = GetRecord(key);
            newRecord.ValueData = NSData.FromString(data);

			if (result == SecStatusCode.Success)
			{
                var res2 = SecKeyChain.Remove(existingRecord);
			}

            var res = SecKeyChain.Add(newRecord);
		}

		private String GetData(String key)
		{
            var existingRecord = GetRecord(key);

            SecRecord match = SecKeyChain.QueryAsRecord(existingRecord, out var result);
			if (result == SecStatusCode.Success)
			{
				var response = Encoding.UTF8.GetString(match.ValueData.ToArray());
                return response;
			}
			else 
			{
				return null;
			}
		}

		private void DeleteFromKeychain(String key)
		{
            var existingRecord = GetRecord(key);

			SecRecord match = SecKeyChain.QueryAsRecord(existingRecord, out var result);

			if (result == SecStatusCode.Success)
			{
                var removeResult = SecKeyChain.Remove(existingRecord);
			}
		}

        private SecRecord GetRecord(String key)
        {
            return new SecRecord(SecKind.GenericPassword)
            {
                Service = Keychain_Service,
                Account = key,
                Label = key
            };
        }

		public void SetAppFirstLaunch()
		{
            NSUserDefaults.StandardUserDefaults.SetBool(true, "Helseboka_Launched");
            NSUserDefaults.StandardUserDefaults.Synchronize();
		}

		public bool GetAppFirstLaunch()
		{
            return NSUserDefaults.StandardUserDefaults.BoolForKey("Helseboka_Launched");
		}
	}
}
