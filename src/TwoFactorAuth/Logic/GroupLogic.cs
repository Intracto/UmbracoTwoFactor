using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoFactorAuth.Configuration;
using TwoFactorAuth.Models;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;

namespace TwoFactorAuth.Logic
{
    public class GroupLogic
    {
        private IScopeProvider scopeProvider;
        private IUserService userService;

        public GroupLogic(IScopeProvider scopeProvider, IUserService userService)
        {
            this.scopeProvider = scopeProvider;
            this.userService = userService;
        }

        public void SaveGroups(List<PermsModel> groups)
        {
            using (var scope = scopeProvider.CreateScope())
            {
                var db = scope.Database;
                groups.ForEach(x => db.Save(x));
                scope.Complete();
            }
        }

        public IEnumerable<IUserGroup> GetAllGroups() => userService.GetAllUserGroups();

        public async Task<List<PermsModel>> GetAllPermsAsync()
        {
            using(var scope = scopeProvider.CreateScope())
            {
                var db = scope.Database;
                return await db.Query<PermsModel>().ToListAsync();
            }
        }
    }
}
