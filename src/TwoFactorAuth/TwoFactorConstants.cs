using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoFactorAuth
{
    class TwoFactorConstants
    {
        public const int TempCookieTime = 1;
        public const string TempCookieName = "2fa_TEMP";
        public const string LongCookieName = "2fa_LONG";
        public const string SectionName = "2fa";
        public const string TotpProvider = "TOTP";
        public const string RegisterProvider = "Register";
        public const string MailProvider = "Mail";
    }
}
