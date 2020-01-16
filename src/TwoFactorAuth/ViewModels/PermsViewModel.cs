using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoFactorAuth.ViewModels
{
    public class PermsViewModel
    {
        public int GroupId { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
        public bool TotpAllowed { get; set; }
        public bool MailAllowed { get; set; }
        public bool Required { get; set; }
        public bool IsDirty { get; set; }
    }
}
