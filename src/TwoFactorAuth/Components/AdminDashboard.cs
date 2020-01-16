using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Composing;
using Umbraco.Core.Dashboards;

namespace TwoFactorAuth.Components
{
    [Weight(10)]
    class AdminDashboard : IDashboard
    {
        public string[] Sections => new[] { "2fa" };

        public IAccessRule[] AccessRules => Array.Empty<IAccessRule>();

        public string Alias => "twoFactorAdminDashboard";

        public string View => "/App_Plugins/TwoFactor/Views/AdminDashboard.html";
    }
}
