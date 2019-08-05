using System;
using System.Diagnostics;
using Helseboka.Core.Common.Interfaces;

namespace Helseboka.Core.Common.CommonImpl
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
#if DEBUG
            Debug.WriteLine(message);
#endif
        }
    }
}
