using System;

namespace Helseboka.Core.Common.Interfaces
{
    public interface ISerializer
    {
		(bool isSuccess, T result) Deserialize<T>(String jsonString) where T : class, new();
		(bool isSuccess, String jsonString) Serialize<T>(T dataObject) where T : class, new();
    }
}
