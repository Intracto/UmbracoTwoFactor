using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TwoFactorAuth.Configuration;
using TwoFactorAuth.Models;
using Umbraco.Core.Scoping;

namespace TwoFactorAuth.Logic
{
    public class CookieLogic
    {
        private IScopeProvider scopeProvider;

        public CookieLogic(IScopeProvider scopeProvider)
        {
            this.scopeProvider = scopeProvider;
        }

        private int BrowserCookieMinutes => TwoFactorConfig.GetConfig().BrowserRememberTime;

        public bool CookieAuthIsDisabled => BrowserCookieMinutes <= 0;

        private async Task CleanupCookiesAsync(IScope scope)
        {
            var db = scope.Database;
            // In the cookie tables in the database we look for all records that are older than the expiry time for the cookie and delete them to keep the database clean
            var longTask = db.DeleteMany<CookieModel>().Where(x => x.Type == TwoFactorConstants.LongCookieName && x.LoggedInAtUtc < DateTime.UtcNow.AddMinutes(-BrowserCookieMinutes * 2)).ExecuteAsync();
            var tempTask = db.DeleteMany<CookieModel>().Where(x => x.Type == TwoFactorConstants.TempCookieName && x.LoggedInAtUtc < DateTime.UtcNow.AddMinutes(-TwoFactorConstants.TempCookieTime * 2)).ExecuteAsync();

            await Task.WhenAll(longTask, tempTask);
        }

        public async Task<CookieHeaderValue> SetCookieAsync(int userId)
        {
            // Here we generate a new cookie
            // The value is a Guid (Globally unique identifier) which can be used to validate the cookie
            // The name is set in the constants file and is used to get the cookie during authentication
            // The expire time of the cookie is set by using the conigured amount of minutes the cookie is valid
            var value = Guid.NewGuid();
            var cookie = new CookieHeaderValue(TwoFactorConstants.LongCookieName, value.ToString())
            {
                Expires = DateTime.UtcNow.AddMinutes(BrowserCookieMinutes),
                Path = "/",
                HttpOnly = true
            };

            using (var scope = scopeProvider.CreateScope())
            {
                var db = scope.Database;
                var cleanupTask = CleanupCookiesAsync(scope);

                // Here we save the cookie that we created earlier in the database so it can be used for validation
                db.Save(new CookieModel()
                {
                    CookieValue = value,
                    LoggedInAtUtc = DateTime.UtcNow,
                    UserId = userId,
                    Type = TwoFactorConstants.LongCookieName
                });

                // Here we await the database cleaning task to make sure the async operations are done before we complete the scope
                await cleanupTask;

                scope.Complete();
            }

            return cookie;
        }

        public async Task<bool> HasValidCookiesAsync(int userId, string longCookieVal, string tempCookieVal)
        {
            // Here we compare both cookie values with cookie values in the database and check if the database contains the cookies with a valid time
            using (var scope = scopeProvider.CreateScope())
            {

                // We check for every cookie in the database that has a login time that is newer than the current time minus the configured cookie time
                // We check for all those records if they have the same user id as the user trying to login and if they have the same cookie value as the cookie value we found earlier
                var hasCookie = IsValidCookieAsync(scope, TwoFactorConstants.LongCookieName, userId, longCookieVal, BrowserCookieMinutes);

                // We check for every cookie in the database that has a login time that is newer than the current time minus the temporary cookie time which is set in the constant file
                // We check for all those records if they have the same user id as the user trying to login and if they have the same cookie value as the cookie value we found earlier
                var authenticated = IsValidCookieAsync(scope, TwoFactorConstants.TempCookieName, userId, tempCookieVal, TwoFactorConstants.TempCookieTime);

                var cleanupTask = CleanupCookiesAsync(scope);

                // We await all database tasks so we can be sure that the scope can be closed
                await Task.WhenAll(hasCookie, authenticated, cleanupTask);

                scope.Complete();

                // If both cookie values were valid against the values in the database the user has valid cookies
                return await hasCookie && await authenticated;
            }
        }

        /// <summary>
        /// Checks if a cookie of a type is still valid in the database with a given timespan in minutes
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="type"></param>
        /// <param name="userId"></param>
        /// <param name="value"></param>
        /// <param name="allowedMinutes"></param>
        /// <returns></returns>
        private async Task<bool> IsValidCookieAsync(IScope scope, string type, int userId, string value, int allowedMinutes) =>
            await scope.Database.Query<CookieModel>()
                .Where(x => x.Type == type && x.UserId == userId && x.CookieValue == Guid.Parse(value) && x.LoggedInAtUtc >= DateTime.UtcNow.AddMinutes(-allowedMinutes))
                .AnyAsync();
    }
}
