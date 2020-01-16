using OtpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoFactorAuth.Logic
{
    public class TotpLogic : OtpLogic
    {
        public bool ValidateTotp(string code, string secret)
        {
            if (string.IsNullOrEmpty(code))
                return false;
            return new Totp(Base32Encoding.ToBytes(secret)).VerifyTotp(code, out _, new VerificationWindow(previous: 1));
        }
    }
}
