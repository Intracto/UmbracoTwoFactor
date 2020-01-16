using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoFactorAuth.Models;
using Umbraco.Core.Models.Identity;
using Umbraco.Core.Scoping;

namespace TwoFactorAuth.Providers
{
    /// <summary>
    /// This the the provider that is used when a user doesnt have 2fa enabled but is forced to register before logging in
    /// </summary>
    public class RegisterProvider : IUserTokenProvider<BackOfficeIdentityUser, int>
    {
        private IScopeProvider scopeProvider;

        public RegisterProvider(IScopeProvider scopeProvider)
        {
            this.scopeProvider = scopeProvider;
        }

        public Task<bool> ValidateAsync(string purpose, string token, UserManager<BackOfficeIdentityUser, int> manager, BackOfficeIdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task<string> GenerateAsync(string purpose, UserManager<BackOfficeIdentityUser, int> manager, BackOfficeIdentityUser user)
        {
            return Task.FromResult("test123");
        }

        public async Task<bool> IsValidProviderForUserAsync(UserManager<BackOfficeIdentityUser, int> manager, BackOfficeIdentityUser user)
        {
            using (var scope = scopeProvider.CreateScope())
            {
                var db = scope.Database;
                return !await db.Query<TwoFactorModel>().Where(x => x.UserId == user.Id).AnyAsync();
            }
        }

        public Task NotifyAsync(string token, UserManager<BackOfficeIdentityUser, int> manager, BackOfficeIdentityUser user)
        {
            return Task.FromResult(token);
        }
    }
}
