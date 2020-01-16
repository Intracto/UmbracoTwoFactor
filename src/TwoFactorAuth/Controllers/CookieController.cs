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
using Umbraco.Core.Persistence;
using Umbraco.Core.Scoping;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace TwoFactorAuth.Controllers
{
    [PluginController("2fa")]
    public class CookieController : UmbracoAuthorizedApiController
    {
        private CookieLogic cookieLogic;

        private int UserId => Security.CurrentUser.Id;

        public CookieController(IScopeProvider scopeProvider)
        {
            cookieLogic = new CookieLogic(scopeProvider);
        }

        /// <summary>
        /// This method will give the user a cookie that allows it to skip the second part of authenication
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> SetCookie()
        {
            // First we create a new response and check if cookie auth is enabled, if it isn't we instantly return because we don't need a cookie
            HttpResponseMessage response = new HttpResponseMessage();
            if (cookieLogic.CookieAuthIsDisabled)
                return response;

            // We set a cookie in the logic and add it to the response
            var cookie = await cookieLogic.SetCookieAsync(UserId);
            response.Headers.AddCookies(new[] { cookie });
            return response;
        }
    }
}
