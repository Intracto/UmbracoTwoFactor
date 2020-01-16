using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoFactorAuth.Configuration;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Dashboards;

namespace TwoFactorAuth.Components
{
    [Weight(100)]
    public class RegisterDashboard : IDashboard
    {
        public string[] Sections => new[] { Constants.Applications.Content };

        public IAccessRule[] AccessRules => Array.Empty<IAccessRule>();

        public string Alias => "twofactorRegisterDashboard";

        public string View => "/App_Plugins/TwoFactor/Views/RegisterDashboard.html";
    }
}
