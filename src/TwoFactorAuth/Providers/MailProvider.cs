using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoFactorAuth.Configuration;
using TwoFactorAuth.Logic;
using Umbraco.Core.Models.Identity;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;

namespace TwoFactorAuth.Providers
{
    public class MailProvider : IUserTokenProvider<BackOfficeIdentityUser, int>
    {
        private IScopeProvider scopeProvider;
        private HotpLogic hotpLogic;
        private UserLogic userLogic;
        private DeviceLogic deviceLogic;

        public MailProvider(IScopeProvider scopeProvider, IUserService userService)
        {
            this.scopeProvider = scopeProvider;
            hotpLogic = new HotpLogic();
            userLogic = new UserLogic(scopeProvider, userService);
            deviceLogic = new DeviceLogic(scopeProvider);
        }

        public Task<bool> ValidateAsync(string purpose, string token, UserManager<BackOfficeIdentityUser, int> manager, BackOfficeIdentityUser user)
        {
            var model = deviceLogic.GetTwoFactorModels(user.Id, TwoFactorConstants.MailProvider).First();
            var valid = hotpLogic.ValidateHotp(token, model.Secret, model.Counter);
            if (valid)
            {
                model.Counter++;
                using (var scope = scopeProvider.CreateScope())
                {
                    scope.Database.Save(model);
                    scope.Complete();
                }
            }
            return Task.FromResult(valid);
        }

        public Task<string> GenerateAsync(string purpose, UserManager<BackOfficeIdentityUser, int> manager, BackOfficeIdentityUser user)
        {
            var model = deviceLogic.GetTwoFactorModels(user.Id, TwoFactorConstants.MailProvider).First();
            return Task.FromResult(hotpLogic.GenerateHotp(model.Secret, model.Counter));
        }

        public async Task<bool> IsValidProviderForUserAsync(UserManager<BackOfficeIdentityUser, int> manager, BackOfficeIdentityUser user)
        {
            return await userLogic.TypeAllowedAsync(TwoFactorConstants.MailProvider, user)
                && await userLogic.TypeEnabledAsync(TwoFactorConstants.MailProvider, user.Id);
        }

        public async Task NotifyAsync(string token, UserManager<BackOfficeIdentityUser, int> manager, BackOfficeIdentityUser user)
        {
            var config = TwoFactorConfig.GetConfig();
            await manager.SendEmailAsync(user.Id, config.MailSubject, string.Format(config.MailBody, token));
        }
    }
}
