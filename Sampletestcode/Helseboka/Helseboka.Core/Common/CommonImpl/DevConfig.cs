using System;
using Helseboka.Core.Common.Interfaces;

namespace Helseboka.Core.Common.CommonImpl
{
	//Environment configs
    public class DevDevConfig : IConfig
    {
        public string BaseURL => "https://frontend-dev.helseboka.no/";

        public string UserName => "apiuser";

        public string Password => "tZ5gS6n92T6q9FceBS8dUuEYndhz4pfkB7wgJRCuL47qc2xT8S";
    }

    public class TestDevConfig : IConfig
    {
        public string BaseURL => "https://frontend-test.helseboka.no/";

        public string UserName => "apiuser";

        public string Password => "tZ5gS6n92T6q9FceBS8dUuEYndhz4pfkB7wgJRCuL47qc2xT8S";
    }

    public class StagDevConfig : IConfig
    {
        public string BaseURL => "https://frontend-staging.helseboka.no/";

        public string UserName => "apiuser";

        public string Password => "tZ5gS6n92T6q9FceBS8dUuEYndhz4pfkB7wgJRCuL47qc2xT8S";
    }

    public class DevConfig : IConfig
    {
        public string BaseURL => "https://frontend.helseboka.no/";

        public string UserName => "apiuser";

        public string Password => "tZ5gS6n92T6q9FceBS8dUuEYndhz4pfkB7wgJRCuL47qc2xT8S";
    }

    //BankID-Configs
    public class PreProdBankIdConfig : IBankIdConfig
    {
        public string Bankid => "bankid-preprod";

        public string Authentication => "authentication-test";

    }

    public class ProdBankIdConfig : IBankIdConfig
    {
        public string Bankid => "bankid";

        public string Authentication => "authentication";
    }

}
