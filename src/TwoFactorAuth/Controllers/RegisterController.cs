using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TwoFactorAuth.Configuration;
using TwoFactorAuth.Logic;
using TwoFactorAuth.ViewModels;
using Umbraco.Core.Scoping;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace TwoFactorAuth.Controllers
{
    [PluginController("2fa")]
    public class RegisterController : UmbracoAuthorizedApiController
    {
        private DeviceLogic deviceLogic;
        private TotpLogic totpLogic;

        private int UserId => Security.CurrentUser.Id;

        public RegisterController(IScopeProvider scopeProvider)
        {
            deviceLogic = new DeviceLogic(scopeProvider);
            totpLogic = new TotpLogic();
        }

        [HttpGet]
        public string GenerateSecret() => totpLogic.GenerateSecret();

        [HttpGet]
        public QRViewModel GenerateQR(string secret = null) => new QRViewModel
        {
            Mail = Security.CurrentUser.Email,
            Secret = secret ?? GenerateSecret(),
            ApplicationName = ApplicationName
        };

        [HttpPost]
        public bool ValidateTotp(string code, string secret) => totpLogic.ValidateTotp(code, secret);

        [HttpPost]
        public async Task SaveTwoFactor(string type, string secret) => await deviceLogic.SaveDeviceAsync(type, secret, UserId);

        private string ApplicationName => TwoFactorConfig.GetConfig().ApplicationName;
    }
}
