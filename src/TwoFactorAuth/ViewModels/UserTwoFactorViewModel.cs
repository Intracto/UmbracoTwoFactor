using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoFactorAuth.ViewModels
{
    public class UserTwoFactorViewModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public bool Enabled { get; set; }
        public bool Checked { get; set; }
    }
}
