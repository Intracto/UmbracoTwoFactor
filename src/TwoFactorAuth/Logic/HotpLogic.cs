using OtpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Scoping;

namespace TwoFactorAuth.Logic
{
    public class HotpLogic : OtpLogic
    {
        public bool ValidateHotp(string code, string secret, int counter)
        {
            if (string.IsNullOrEmpty(code))
                return false;
            return new Hotp(Base32Encoding.ToBytes(secret)).VerifyHotp(code, counter);
        }

        public string GenerateHotp(string secret, int counter)
        {
            return new Hotp(Base32Encoding.ToBytes(secret)).ComputeHOTP(counter);
        }
    }
}
