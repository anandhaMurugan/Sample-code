using System;
using System.Globalization;

namespace Helseboka.Core.Common.Interfaces
{
    public interface IDeviceHandler
    {
		/// <summary>
        /// Gets the two char language code e.g. en, nb etc.
        /// </summary>
        /// <returns>The language code.</returns>
		String GetLanguageCode();

		bool IsTouchIDSupported();

		bool IsFaceIDSupported();
    }
}
