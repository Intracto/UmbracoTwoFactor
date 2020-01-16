using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoFactorAuth.Components;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace TwoFactorAuth.Composers
{
    public class BackendComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Components()
                .Append<MigrationComponent>();
            composition.Components()
                .Append<UserComponent>();
        }
    }
}
