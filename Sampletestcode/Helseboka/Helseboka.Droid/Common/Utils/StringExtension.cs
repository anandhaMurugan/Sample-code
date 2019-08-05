using System;
using Android.Content;

namespace Helseboka.Droid.Common.Utils
{
    public static class StringExtension
    {
        public static int GetStringResourceId(this String key)
        {
            return (int)typeof(Resource.String).GetField(key).GetValue(null);
        }

        public static String Translate(this string key, Context context)
        {
            return context.Resources.GetString(key.GetStringResourceId());
        }
    }
}
