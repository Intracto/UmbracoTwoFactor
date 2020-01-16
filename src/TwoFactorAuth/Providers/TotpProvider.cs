using Microsoft.AspNet.Identity;
using OtpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TwoFactorAuth.Logic;
using TwoFactorAuth.Models;
using Umbraco.Core.Models.Identity;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;
using Umbraco.Web.Composing;

namespace TwoFactorAuth.Providers
{
    /// <summary>
    /// This class is used by asp.net.
    /// Because it implements this interface it can be used by asp.net.
    /// This allows use to use standard function that already exist in Umbraco.
    /// </summary>
    public class TotpProvider : IUserTokenProvider<BackOfficeIdentityUser, int>
    {
        private TotpLogic totpLogic;
        private UserLogic userLogic;
        private DeviceLogic deviceLogic;

        public TotpProvider(IScopeProvider scopeProvider, IUserService userService)
        {
            totpLogic = new TotpLogic();
            userLogic = new UserLogic(scopeProvider, userService);
            deviceLogic = new DeviceLogic(scopeProvider);
        }

        public Task<bool> ValidateAsync(string purpose, string token, UserManager<BackOfficeIdentityUser, int> manager, BackOfficeIdentityUser user)
        {
            var models = deviceLogic.GetTwoFactorModels(user.Id, TwoFactorConstants.TotpProvider);
            var secrets = models.Select(x => x.Secret);
            return Task.FromResult(secrets.Any(secret => totpLogic.ValidateTotp(token, secret)));
        }

        public Task<string> GenerateAsync(string purpose, UserManager<BackOfficeIdentityUser, int> manager, BackOfficeIdentityUser user)
        {
            return Task.FromResult((string)null);
        }

        public async Task<bool> IsValidProviderForUserAsync(UserManager<BackOfficeIdentityUser, int> manager, BackOfficeIdentityUser user)
        {
            return await userLogic.TypeAllowedAsync(TwoFactorConstants.TotpProvider, user)
                && await userLogic.TypeEnabledAsync(TwoFactorConstants.TotpProvider, user.Id);
        }

        public Task NotifyAsync(string token, UserManager<BackOfficeIdentityUser, int> manager, BackOfficeIdentityUser user)
        {
            return Task.FromResult(true);
        }
    }
}
