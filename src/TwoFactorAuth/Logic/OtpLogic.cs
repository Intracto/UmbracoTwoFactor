using OtpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoFactorAuth.Logic
{
    public abstract class OtpLogic
    {
        public string GenerateSecret() => Base32Encoding.ToString(KeyGeneration.GenerateRandomKey());
    }
}
