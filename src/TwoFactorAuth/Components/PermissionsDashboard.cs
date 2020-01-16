using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Composing;
using Umbraco.Core.Dashboards;

namespace TwoFactorAuth.Components
{
    [Weight(20)]
    class PermissionsDashboard : IDashboard
    {
        public string[] Sections => new[] { "2fa" };

        public IAccessRule[] AccessRules => Array.Empty<IAccessRule>();

        public string Alias => "twoFactorPermissionsDashboard";

        public string View => "/App_Plugins/TwoFactor/Views/PermissionsDashboard.html";
    }
}
