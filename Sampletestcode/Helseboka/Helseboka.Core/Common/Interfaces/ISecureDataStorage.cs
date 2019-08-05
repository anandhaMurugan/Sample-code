using System;
namespace Helseboka.Core.Common.Interfaces
{
	public enum SecureDataType
	{
		PIN,
        LoginMode,
        APIKey
	}
    public interface ISecureDataStorage
    {
		void StoreData(String data, SecureDataType dataType);

		String GetData(SecureDataType dataType);

		void DeleteData(SecureDataType dataType);

		void DeleteAll();

		void SetAppFirstLaunch();

		bool GetAppFirstLaunch();
    }
}
