using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Composing;
using Umbraco.Core.Dashboards;
using Umbraco.Core;

namespace TwoFactorAuth.Components
{
    [Weight(101)]
    public class DeviceManagementDashboard : IDashboard
    {
        public string[] Sections => new[] { Constants.Applications.Content };

        public IAccessRule[] AccessRules => Array.Empty<IAccessRule>();

        public string Alias => "twofactorDeviceDashboard";

        public string View => "/App_Plugins/TwoFactor/Views/DeviceManagement.html";
    }
}
