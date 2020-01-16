using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoFactorAuth.Logic;
using TwoFactorAuth.Models;
using Umbraco.Core.Configuration;
using Umbraco.Core.Mapping;
using Umbraco.Core.Models.Identity;
using Umbraco.Core.Scoping;
using Umbraco.Core.Security;
using Umbraco.Core.Services;
using Umbraco.Web.Composing;

namespace TwoFactorAuth.UserHelpers
{
    /// <summary>
    /// This class is overwritten so we can implement the GetTwoFactorEnabled method, this is used by asp.net to check if a user can use 2fa
    /// </summary>
    public class CustomUserStore : BackOfficeUserStore
    {
        private UserLogic userLogic;

        public CustomUserStore(IUserService userService, IMemberTypeService memberTypeService, IEntityService entityService, IExternalLoginService externalLoginService, IGlobalSettings globalSettings, MembershipProviderBase usersMembershipProvider, UmbracoMapper mapper, IScopeProvider scopeProvider) : base(userService, memberTypeService, entityService, externalLoginService, globalSettings, usersMembershipProvider, mapper)
        {
            userLogic = new UserLogic(scopeProvider, userService);
        }

        public override async Task<bool> GetTwoFactorEnabledAsync(BackOfficeIdentityUser user)
        {
            if (!await userLogic.AnyAllowedAsync(user))
                return false;
            if (await userLogic.RequiredAsync(user))
                return true;
            return await userLogic.AnyEnabledAsync(user.Id);
        }

        public override Task SetTwoFactorEnabledAsync(BackOfficeIdentityUser user, bool enabled)
        {
            return Task.FromResult("");
        }
    }
}
