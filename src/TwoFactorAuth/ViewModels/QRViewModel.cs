using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoFactorAuth.ViewModels
{
    public class QRViewModel
    {
        public string Mail { get; set; }
        public string Secret { get; set; }
        public string ApplicationName { get; set; }
    }
}
