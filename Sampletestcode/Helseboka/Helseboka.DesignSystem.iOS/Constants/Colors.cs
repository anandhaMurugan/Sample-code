using System;
using UIKit;

namespace Helseboka.DesignSystem.iOS.Constants
{
    public static class Colors
    {
        private static UIColor FromHEX(int hexValue)
        {
            return UIColor.FromRGB(
                ((hexValue & 0xFF0000) >> 16) / 255.0f,
                ((hexValue & 0xFF00) >> 8) / 255.0f,
                (hexValue & 0xFF) / 255.0f
            );
        }

        /// <summary>
        /// Primary brand color purple
        /// </summary>
        public static readonly UIColor Purple = FromHEX(0x6508BF);

        /// <summary>
        /// Primary brand color turquoise
        /// </summary>
        public static readonly UIColor Turquoise = FromHEX(0x47EAEA);

        /// <summary>
        /// The app neutral Grey 600.
        /// </summary>
        public static readonly UIColor Grey600 = FromHEX(0x6B6B6B);

        /// <summary>
        /// The app neutral Grey 350.
        /// </summary>
        public static readonly UIColor Grey350 = FromHEX(0xD8D8D8);

        /// <summary>
        /// The app neutral Grey 250.
        /// </summary>
        public static readonly UIColor Grey250 = FromHEX(0xE9E9E9);
    }

}
