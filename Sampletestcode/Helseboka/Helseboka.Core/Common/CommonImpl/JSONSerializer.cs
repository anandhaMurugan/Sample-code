using System;
using Helseboka.Core.Common.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Serialization;
using Helseboka.Core.Common.Extension;
using Newtonsoft.Json.Converters;
using Helseboka.Core.Common.EnumDefinitions;

namespace Helseboka.Core.Common.CommonImpl
{
	public class JSONSerializer : ISerializer
    {
        //Adding special case for String serialization and deserialization.

        private JsonSerializerSettings DefaultSettings
        {
            get
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.ContractResolver = new LowercaseContractResolver();
                settings.Converters.Add(new DayOfWeekConverter());
                settings.Converters.Add(new TimeOfDayConverter());
                settings.Converters.Add(new AppointmentStatusSonverter());
                settings.Converters.Add(new AlarmStatusSonverter());
                settings.NullValueHandling = NullValueHandling.Ignore;

                return settings;
            }
        }

		public (bool isSuccess, T result) Deserialize<T>(String jsonString) where T : class, new()
		{
			if (typeof(T) == typeof(String))
            {
				return (true, jsonString as T);
            }
            else
            {
				try
				{
                    T jsonObject = JsonConvert.DeserializeObject<T>(jsonString, DefaultSettings);
					return (true, jsonObject);
				}
				catch (JsonException)
				{
					return (false, null);
				}
            }
		}

        public (bool isSuccess, String jsonString) Serialize<T>(T dataObject) where T : class, new()
		{
			if (dataObject is String)
            {
				return (true, dataObject as String);
            }
            else
            {
				try
				{
                    String jsonString = JsonConvert.SerializeObject(dataObject, DefaultSettings);
					return (true, jsonString);
				}
				catch (JsonException)
				{
					return (false, null);
				}
            }
		}

        public static String LogObject(Object obj)
        {
            if (obj == null)
            {
                return "null";
            }

            try
            {
                return JsonConvert.SerializeObject(obj, Formatting.Indented);
            }
            catch
            {
                return obj.ToString();
            }
        }

        public static String PrettyPrint(String json)
        {
            if (String.IsNullOrEmpty(json))
            {
                return String.Empty;
            }

            try
            {
                return JsonConvert.SerializeObject(JsonConvert.DeserializeObject(json), Formatting.Indented);
            }
            catch
            {
                return json;
            }
        }
	}

    public class LowercaseContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(String propertyName)
        {
            return propertyName.FirstCharacterToLower();
        }
    }

    public class DayOfWeekConverter : JsonConverter<DayOfWeek>
    {
        public override DayOfWeek ReadJson(JsonReader reader, Type objectType, DayOfWeek existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (Enum.TryParse<DayOfWeek>(reader.Value as String, true, out var result))
            {
                return result;
            }
            else
            {
                return DayOfWeek.Sunday;
            }
        }

        public override void WriteJson(JsonWriter writer, DayOfWeek value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString().ToUpper());
        }
    }

    // TODO: Need to find out a easy way to subclass StringEnumConverter and handle all caps enums.
    // We can copy the original source code https://github.com/JamesNK/Newtonsoft.Json/blob/master/Src/Newtonsoft.Json/Converters/StringEnumConverter.cs
    // and modify for all caps. It needs to be copied some utils class too. Moreover updating it and maintaining with current release will be a pain.

    public class TimeOfDayConverter : JsonConverter<TimeOfDay>
    {
        public override TimeOfDay ReadJson(JsonReader reader, Type objectType, TimeOfDay existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (Enum.TryParse<TimeOfDay>(reader.Value as String, true, out var result))
            {
                return result;
            }
            else
            {
                return TimeOfDay.Early;
            }
        }

        public override void WriteJson(JsonWriter writer, TimeOfDay value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString().ToUpper());
        }
    }

    public class AppointmentStatusSonverter : JsonConverter<AppointmentStatus>
    {
        public override AppointmentStatus ReadJson(JsonReader reader, Type objectType, AppointmentStatus existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (Enum.TryParse<AppointmentStatus>(reader.Value as String, true, out var result))
            {
                return result;
            }
            else
            {
                return AppointmentStatus.Cancelled;
            }
        }

        public override void WriteJson(JsonWriter writer, AppointmentStatus value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString().ToUpper());
        }
    }

    public class AlarmStatusSonverter : JsonConverter<AlarmStatus>
    {
        public override AlarmStatus ReadJson(JsonReader reader, Type objectType, AlarmStatus existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (Enum.TryParse<AlarmStatus>(reader.Value as String, true, out var result))
            {
                return result;
            }
            else
            {
                return AlarmStatus.Pending;
            }
        }

        public override void WriteJson(JsonWriter writer, AlarmStatus value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString().ToUpper());
        }
    }

}
