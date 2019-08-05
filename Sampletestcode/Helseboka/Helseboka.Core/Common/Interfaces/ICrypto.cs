using System;
namespace Helseboka.Core.Common.Interfaces
{
    public interface ICrypto
    {
		String Hash(String password);

		bool Verify(String password, String hash);
    }
}
