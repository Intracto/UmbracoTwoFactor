using Newtonsoft.Json;
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
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using Umbraco.Web.WebApi.Filters;

namespace TwoFactorAuth.Controllers
{
    [PluginController("2fa")]
    [UmbracoApplicationAuthorize(TwoFactorConstants.SectionName)]
    public class AdminGroupController : UmbracoAuthorizedApiController
    {
        private GroupLogic groupLogic;

        public AdminGroupController(IUserService userService, IScopeProvider scopeProvider)
        {
            groupLogic = new GroupLogic(scopeProvider, userService);
        }

        [HttpGet]
        public async Task<IEnumerable<PermsViewModel>> GetGroups()
        {
            var groups = groupLogic.GetAllGroups();
            var permModels = await groupLogic.GetAllPermsAsync();
            return groups.Select(x =>
            {
                var model = permModels.Find(p => p.GroupId == x.Id);
                return new PermsViewModel
                {
                    GroupId = x.Id,
                    Name = x.Name,
                    Icon = x.Icon,
                    TotpAllowed = model?.CanUseTotp ?? false,
                    MailAllowed = model?.CanUseMail ?? false,
                    Required = model?.Required ?? false,
                    IsDirty = model == null
                };
            });
        }

        [HttpPost]
        public void SaveGroups(string input)
        {
            var groups = JsonConvert.DeserializeObject<IEnumerable<PermsViewModel>>(input);
            var dirtyGroups = groups.Where(x => x.IsDirty);
            groupLogic.SaveGroups(
                dirtyGroups.Select(x => new PermsModel
                {
                    CanUseTotp = x.TotpAllowed,
                    CanUseMail = x.MailAllowed,
                    GroupId = x.GroupId,
                    Required = x.Required
                }).ToList());
        }
    }
}
