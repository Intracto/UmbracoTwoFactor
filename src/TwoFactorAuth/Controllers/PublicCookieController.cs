using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TwoFactorAuth.Configuration;
using TwoFactorAuth.Logic;
using TwoFactorAuth.Models;
using Umbraco.Core.Scoping;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.Security;
using Umbraco.Web.WebApi;

namespace TwoFactorAuth.Controllers
{
    [PluginController("2fa")]
    public class PublicCookieController : UmbracoApiController
    {
        private BackOfficeSignInManager _signInManager; // do not use this field, use the property

        private CookieLogic cookieLogic;

        private BackOfficeSignInManager SignInManager => _signInManager
            ?? (_signInManager = TryGetOwinContext().Result.GetBackOfficeSignInManager());

        public PublicCookieController(IScopeProvider scopeProvider)
        {
            cookieLogic = new CookieLogic(scopeProvider);
        }


        /// <summary>
        /// This method will check if the user has the correct cookies to skip the second part of authentication. The user will be logged in if it has the correct cookies
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> HasAuthCookie()
        {
            // We first check if cookie authentication is turned off, we do this before any other logic because in the case of a security error you would be able to turn cookie authentication off in the config
            if (cookieLogic.CookieAuthIsDisabled)
                return false;

            // Here we get the verified user id, if the value is int.MinValue there is no user verified
            var userId = await GetVerifiedUesr();
            if (userId == int.MinValue)
                return false;

            // Here we try to get the 2 authentication cookies from the browser: 2fa_TEMP and 2fa_LONG. If we cant get the cookie we will set the value to an empty string
            var cookieSession = Request.Headers.GetCookies().FirstOrDefault()?.Cookies.Where(x => x.Name == TwoFactorConstants.LongCookieName).FirstOrDefault()?.Value ?? "";
            var tempCookie = Request.Headers.GetCookies().FirstOrDefault()?.Cookies.Where(x => x.Name == TwoFactorConstants.TempCookieName).FirstOrDefault()?.Value ?? "";

            // If one of the cookie values is empty we we're not able to find the cookie or the cookie was empty. In both cases the user should not be logged in
            if (cookieSession == "" || tempCookie == "")
                return false;

            // Here we compare both cookie values with cookie values in the database and check if the database contains the cookies with a valid time
            var validCookies = await cookieLogic.HasValidCookiesAsync(userId, cookieSession, tempCookie);
            if (validCookies)
                Security.PerformLogin(userId);
            return validCookies;
        }

        private async Task<int> GetVerifiedUesr() => await SignInManager.GetVerifiedUserIdAsync();
    }
}
