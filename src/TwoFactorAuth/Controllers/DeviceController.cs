using Newtonsoft.Json;
using OtpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TwoFactorAuth.Logic;
using TwoFactorAuth.Models;
using TwoFactorAuth.ViewModels;
using Umbraco.Core.Scoping;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace TwoFactorAuth.Controllers
{
    [PluginController("2fa")]
    public class DeviceController : UmbracoAuthorizedApiController
    {
        private DeviceLogic deviceLogic;

        private int UserId => Security.CurrentUser.Id;

        public DeviceController(IScopeProvider scopeProvider)
        {
            deviceLogic = new DeviceLogic(scopeProvider);
        }

        [HttpGet]
        public IEnumerable<DeviceViewModel> GetTwoFactorModels()
        {
            var models = deviceLogic.GetTwoFactorModels(UserId, TwoFactorConstants.TotpProvider);
            return models
            .Select(x =>
            {
                var totp = new Totp(Base32Encoding.ToBytes(x.Secret));
                return new DeviceViewModel()
                {
                    Id = x.Id,
                    Code = totp.ComputeTotp(),
                    RemainingSeconds = totp.RemainingSeconds(),
                    Selected = false
                };
            });
        }

        [HttpPost]
        public async Task DeleteDevices(string input)
        {
            var devices = JsonConvert.DeserializeObject<IEnumerable<DeviceViewModel>>(input);
            var idsToDelete = devices.Where(x => x.Selected).Select(x => x.Id);
            await deviceLogic.DeleteDevicesAsync(idsToDelete, UserId);
        }

        [HttpPost]
        public async Task DisableMail()
        {
            var model = deviceLogic.GetTwoFactorModels(UserId, TwoFactorConstants.MailProvider).FirstOrDefault();
            if (model == null)
                return;
            await deviceLogic.DeleteDevicesAsync(new[] { model.Id }, UserId);
        }
    }
}
