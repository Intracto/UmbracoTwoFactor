using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoFactorAuth.Components;
using Umbraco.Core.Composing;
using Umbraco.Web;

namespace TwoFactorAuth.Composers
{
    public class ViewComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Sections().Append<TwoFactorSection>();
            composition.Dashboards().Add<RegisterDashboard>();
            composition.Dashboards().Add<DeviceManagementDashboard>();
            composition.Dashboards().Add<AdminDashboard>();
            composition.Dashboards().Add<PermissionsDashboard>();
        }
    }
}
