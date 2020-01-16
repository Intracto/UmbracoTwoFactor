using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TwoFactorAuth.Logic;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace TwoFactorAuth.Controllers
{
    [PluginController("2fa")]
    public class UserController : UmbracoAuthorizedApiController
    {
        private UserLogic userLogic;

        private int UserId => Security.CurrentUser.Id;

        public UserController(IScopeProvider scopeProvider, IUserService userService)
        {
            userLogic = new UserLogic(scopeProvider, userService);
        }

        [HttpGet]
        public async Task<bool> AnyEnabled() => await userLogic.AnyEnabledAsync(UserId);

        [HttpGet]
        public async Task<bool> TypeEnabled(string type) => await userLogic.TypeEnabledAsync(type, UserId);

        [HttpGet]
        public async Task<bool> TypeAllowed(string type) => await userLogic.TypeAllowedAsync(type, Security.CurrentUser);

        [HttpGet]
        public async Task<bool> AnyAllowed() => await userLogic.AnyAllowedAsync(Security.CurrentUser);
    }
}
