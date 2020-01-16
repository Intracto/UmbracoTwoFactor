using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoFactorAuth.Models;
using Umbraco.Core.Scoping;

namespace TwoFactorAuth.Logic
{
    public class DeviceLogic
    {
        private IScopeProvider scopeProvider;

        public DeviceLogic(IScopeProvider scopeProvider)
        {
            this.scopeProvider = scopeProvider;
        }

        public IEnumerable<TwoFactorModel> GetTwoFactorModels(int userId, string type)
        {
            using (var scope = scopeProvider.CreateScope())
            {
                var db = scope.Database;
                return db.Query<TwoFactorModel>().Where(x => x.UserId == userId && x.Type == type).ToEnumerable();
            }
        }

        public async Task DeleteUserDevicesAsync(IEnumerable<int> userIds)
        {
            using (var scope = scopeProvider.CreateScope())
            {
                var db = scope.Database;
                await db.DeleteMany<TwoFactorModel>().Where(x => userIds.Contains(x.UserId)).ExecuteAsync();
                scope.Complete();
            }
        }

        public async Task DeleteDevicesAsync(IEnumerable<int> ids, int userId)
        {
            using (var scope = scopeProvider.CreateScope())
            {
                var db = scope.Database;
                await db.DeleteMany<TwoFactorModel>().Where(x => x.UserId == userId && ids.Contains(x.Id)).ExecuteAsync();
                scope.Complete();
            }
        }

        public async Task SaveDeviceAsync(string type, string secret, int userId)
        {
            var model = new TwoFactorModel
            {
                UserId = userId,
                Secret = secret,
                Type = type,
                Counter = 0
            };
            using (var scope = scopeProvider.CreateScope())
            {
                var db = scope.Database;
                await db.InsertAsync(model);
                scope.Complete();
            }
        }
    }
}
