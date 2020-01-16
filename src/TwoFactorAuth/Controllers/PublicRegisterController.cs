using OtpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TwoFactorAuth.Configuration;
using TwoFactorAuth.Logic;
using TwoFactorAuth.Models;
using TwoFactorAuth.ViewModels;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.Security;
using Umbraco.Web.WebApi;

namespace TwoFactorAuth.Controllers
{
    [PluginController("2fa")]
    public class PublicRegisterController : UmbracoApiController
    {
        private BackOfficeSignInManager _signInManager; // do not use this field, use the property

        private IUserService userService;
        private TotpLogic totpLogic;
        private UserLogic userLogic;
        private DeviceLogic deviceLogic;

        private BackOfficeSignInManager SignInManager => _signInManager
            ?? (_signInManager = TryGetOwinContext().Result.GetBackOfficeSignInManager());

        public PublicRegisterController(IUserService userService, IScopeProvider scopeProvider)
        {
            this.userService = userService;
            totpLogic = new TotpLogic();
            userLogic = new UserLogic(scopeProvider, userService);
            deviceLogic = new DeviceLogic(scopeProvider);
        }

        [HttpGet]
        public string GenerateSecret() => totpLogic.GenerateSecret();

        [HttpGet]
        public async Task<QRViewModel> GenerateQR(string secret = null)
        {
            var userId = await GetVerifiedUserId();
            var mail = userId == int.MinValue ? "Could_not_find_mail" : GetUser(userId).Email;
            return new QRViewModel
            {
                Mail = mail,
                Secret = secret ?? GenerateSecret(),
                ApplicationName = ApplicationName
            };
        }

        [HttpPost]
        public bool ValidateTotp(string code, string secret) => totpLogic.ValidateTotp(code, secret);

        [HttpPost]
        public async Task SaveTwoFactor(string type, string secret)
        {
            var userId = await GetVerifiedUserId();
            // if the user id is the int.MinValue the user wasnt verified
            if (userId == int.MinValue)
                return;
            // We first need to check if this is the first device that is registered
            // we need to do this to make sure this api cant be abused to inject an extra device to bypass 2fa
            var first = !await userLogic.AnyEnabledAsync(userId);
            if (!first)
                return;

            // Here we actually save the device
            await deviceLogic.SaveDeviceAsync(type, secret, userId);
        }

        [HttpGet]
        public async Task<bool> TypeAllowed(string type)
        {
            return await userLogic.TypeAllowedAsync(type, GetUser(await GetVerifiedUserId()));
        }

        private async Task<int> GetVerifiedUserId() => await SignInManager.GetVerifiedUserIdAsync();

        private IUser GetUser(int userId) => userService.GetUserById(userId);

        private string ApplicationName => TwoFactorConfig.GetConfig().ApplicationName;
    }
}
