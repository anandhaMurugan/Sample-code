using System;
using Android.Content;
using Android.Preferences;
using Helseboka.Core.Common.Interfaces;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;

namespace Helseboka.Droid.Common.CommonImpl
{
    public class SecureStorageHandler : ISecureDataStorage
    {
        private Context context;
        public SecureStorageHandler(Context context)
        {
            this.context = context;
        }

        public void DeleteAll()
        {
            try
            {
                SecureStorage.RemoveAll();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public void DeleteData(SecureDataType dataType)
        {
            try
            {
                SecureStorage.Remove(dataType.ToString());
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            //var prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            //var editor = prefs.Edit();
            //editor.Remove(dataType.ToString());
            //editor.Apply();
        }

        public bool GetAppFirstLaunch()
        {
            var prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            return prefs.GetBoolean("Helseboka_Launched", false);
        }

        public string GetData(SecureDataType dataType)
        {
            try
            {
                return SecureStorage.GetAsync(dataType.ToString()).Result;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return null;

            //var prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            //return prefs.GetString(dataType.ToString(), null);
        }

        public void SetAppFirstLaunch()
        {
            var prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            var editor = prefs.Edit();
            editor.PutBoolean("Helseboka_Launched", true);
            editor.Apply();
        }

        public void StoreData(string data, SecureDataType dataType)
        {
            try
            {
                SecureStorage.SetAsync(dataType.ToString(), data).Wait();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            //var prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            //var editor = prefs.Edit();
            //editor.PutString(dataType.ToString(), data);
            //editor.Apply();
        }
    }
}
