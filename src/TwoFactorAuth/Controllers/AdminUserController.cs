using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TwoFactorAuth.Logic;
using TwoFactorAuth.ViewModels;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using Umbraco.Web.WebApi.Filters;

namespace TwoFactorAuth.Controllers
{
    [PluginController("2fa")]
    [UmbracoApplicationAuthorize(TwoFactorConstants.SectionName)]
    public class AdminUserController : UmbracoAuthorizedApiController
    {
        private UserLogic userLogic;
        private DeviceLogic deviceLogic;

        public AdminUserController(IUserService userService, IScopeProvider scopeProvider)
        {
            userLogic = new UserLogic(scopeProvider, userService);
            deviceLogic = new DeviceLogic(scopeProvider);
        }

        [HttpGet]
        public int Pages() => userLogic.Pages;

        [HttpGet]
        public IEnumerable<UserTwoFactorViewModel> GetAllInfo(int page = 1)
        {
            page = page < 1 || page > Pages() ? 1 : page;
            var users = userLogic.GetUsers(page);
            return users.Select(x => new UserTwoFactorViewModel
            {
                UserId = x.Id,
                Name = x.Name,
                Mail = x.Email,
                Enabled = userLogic.AnyEnabledAsync(x.Id).Result,
                Checked = false
            });
        }

        [HttpPost]
        public async Task DeleteTwoFactor(string input)
        {
            var vms = JsonConvert.DeserializeObject<IEnumerable<UserTwoFactorViewModel>>(input);
            var idsToDelete = vms.Where(x => x.Checked)
                                 .Select(x => x.UserId);
            await deviceLogic.DeleteUserDevicesAsync(idsToDelete);
        }
    }
}
