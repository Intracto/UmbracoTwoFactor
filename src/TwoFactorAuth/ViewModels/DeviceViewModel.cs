using OtpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoFactorAuth.ViewModels
{
    public class DeviceViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int RemainingSeconds { get; set; }
        public bool Selected { get; set; }
    }
}
