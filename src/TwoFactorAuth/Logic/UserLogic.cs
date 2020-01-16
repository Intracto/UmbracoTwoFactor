using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoFactorAuth.Configuration;
using TwoFactorAuth.Models;
using Umbraco.Core.Models.Identity;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;

namespace TwoFactorAuth.Logic
{
    public class UserLogic
    {
        private IScopeProvider scopeProvider;
        private IUserService userService;

        public UserLogic(IScopeProvider scopeProvider, IUserService userService)
        {
            this.scopeProvider = scopeProvider;
            this.userService = userService;
        }

        private int PageSize => TwoFactorConfig.GetConfig().PageSize;

        public int Pages => (int)Math.Ceiling((decimal)userService.GetCount(MemberCountType.All) / PageSize);

        public IEnumerable<IUser> GetUsers(int page) => userService.GetAll(page - 1, PageSize, out _);

        public IEnumerable<TwoFactorModel> GetTwoFactorModels(IEnumerable<int> userIds)
        {
            using (var scope = scopeProvider.CreateScope())
            {
                var db = scope.Database;
                return db.Query<TwoFactorModel>().Where(x => userIds.Contains(x.UserId)).ToEnumerable();
            }
        }

        public async Task<bool> AnyAllowedAsync(IUser user) => await AnyAllowedAsync(user.Groups.Select(x => x.Id));

        public async Task<bool> AnyAllowedAsync(BackOfficeIdentityUser user) => await AnyAllowedAsync(user.Groups.Select(x => x.Id));

        private async Task<bool> AnyAllowedAsync(IEnumerable<int> groupIds)
        {
            using (var scope = scopeProvider.CreateScope())
            {
                var db = scope.Database;
                return await db.Query<PermsModel>().Where(x => groupIds.Contains(x.GroupId) && (x.CanUseTotp || x.CanUseMail)).AnyAsync(); // When more types are added they need to be included here
            }
        }

        public async Task<bool> TypeAllowedAsync(string type, IUser user) => await TypeAllowedAsync(type, user.Groups.Select(x => x.Id));

        public async Task<bool> TypeAllowedAsync(string type, BackOfficeIdentityUser user) => await TypeAllowedAsync(type, user.Groups.Select(x => x.Id));

        private async Task<bool> TypeAllowedAsync(string type, IEnumerable<int> groupIds)
        {
            using (var scope = scopeProvider.CreateScope())
            {
                var db = scope.Database;
                switch (type)
                {
                    case TwoFactorConstants.TotpProvider:
                        return await db.Query<PermsModel>().Where(x => groupIds.Contains(x.GroupId) && x.CanUseTotp).AnyAsync();
                    case TwoFactorConstants.MailProvider:
                        return await db.Query<PermsModel>().Where(x => groupIds.Contains(x.GroupId) && x.CanUseMail).AnyAsync();
                    default: return false;
                }
            }
        }

        public async Task<bool> AnyEnabledAsync(int userId)
        {
            using (var scope = scopeProvider.CreateScope())
            {
                var db = scope.Database;
                return await db.Query<TwoFactorModel>().Where(x => x.UserId == userId).AnyAsync();
            }
        }

        public async Task<bool> TypeEnabledAsync(string type, int userId)
        {
            using (var scope = scopeProvider.CreateScope())
            {
                var db = scope.Database;
                return await db.Query<TwoFactorModel>().Where(x => x.UserId == userId && x.Type == type).AnyAsync();
            }
        }

        public async Task<bool> RequiredAsync(IUser user) => await RequiredAsync(user.Groups.Select(x => x.Id));

        public async Task<bool> RequiredAsync(BackOfficeIdentityUser user) => await RequiredAsync(user.Groups.Select(x => x.Id));

        private async Task<bool> RequiredAsync(IEnumerable<int> groupIds)
        {
            using (var scope = scopeProvider.CreateScope())
            {
                var db = scope.Database;
                return await db.Query<PermsModel>().Where(x => groupIds.Contains(x.GroupId) && x.Required).AnyAsync();
            }
        }
    }
}
