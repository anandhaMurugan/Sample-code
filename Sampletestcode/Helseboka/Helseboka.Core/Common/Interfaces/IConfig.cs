using System;

namespace Helseboka.Core.Common.Interfaces
{
    public interface IConfig
    {
		String BaseURL { get; }
		String UserName { get; }
		String Password { get; }
    }

    public interface IBankIdConfig
    {
        String Bankid { get; }
        String Authentication { get; }
    }
}
