using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using TwoFactorAuth.Configuration;
using TwoFactorAuth.Models;
using TwoFactorAuth.Providers;
using Umbraco.Core;
using Umbraco.Core.Configuration;
using Umbraco.Core.Configuration.UmbracoSettings;
using Umbraco.Core.Mapping;
using Umbraco.Core.Models.Identity;
using Umbraco.Core.Scoping;
using Umbraco.Core.Security;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Composing;
using Umbraco.Web.Security;

namespace TwoFactorAuth.UserHelpers
{
    /// <summary>
    /// This class is overwritten so we can implement the 2fa interface from Umbraco
    /// We add the 2fa validators in the Create method.
    /// We implement the TwoFactorView method so the
    /// </summary>
    public class CustomUserManager : BackOfficeUserManager, IUmbracoBackOfficeTwoFactorOptions
    {
        private IScopeProvider scopeProvider;

        public CustomUserManager(IUserStore<BackOfficeIdentityUser, int> store, IScopeProvider scopeProvider) : base(store)
        {
            this.scopeProvider = scopeProvider;
        }

        public static CustomUserManager Create(IdentityFactoryOptions<BackOfficeUserManager> options, IUserService userService, IMemberTypeService memberTypeService, IEntityService entityService, IExternalLoginService externalLoginService, MembershipProviderBase membershipProvider, UmbracoMapper mapper, IContentSection contentSectionConfig, IGlobalSettings globalSettings, IScopeProvider scopeProvider)
        {
            var manager = new CustomUserManager(new CustomUserStore(userService, memberTypeService, entityService, externalLoginService, globalSettings, membershipProvider, mapper, scopeProvider), scopeProvider);
            manager.InitUserManager(manager, membershipProvider, options.DataProtectionProvider, contentSectionConfig);
            manager.RegisterTwoFactorProvider(TwoFactorConstants.TotpProvider, new TotpProvider(scopeProvider, userService));
            manager.RegisterTwoFactorProvider(TwoFactorConstants.RegisterProvider, new RegisterProvider(scopeProvider));
            manager.RegisterTwoFactorProvider(TwoFactorConstants.MailProvider, new MailProvider(scopeProvider, userService));
            manager.EmailService = new EmailService(TwoFactorConfig.GetConfig().MailAddress, new EmailSender());
            return manager;
        }

        public string GetTwoFactorView(IOwinContext owinContext, UmbracoContext umbracoContext, string username)
        {
            var id = Store.FindByNameAsync(username).Result.Id;
            if (Required(id))
                return "/App_Plugins/TwoFactor/Views/ForceRegister.html";
            var cookie = GetTempCookie(id);
            umbracoContext.HttpContext.Response.Cookies.Add(cookie);
            return "/App_Plugins/TwoFactor/Views/Login.html";
        }

        // This method is different than the userStore method
        private bool Required(int userId)
        {
            using (var scope = scopeProvider.CreateScope())
            {
                return !scope.Database.Query<TwoFactorModel>().Where(x => x.UserId == userId).Any();
            }
        }

        private HttpCookie GetTempCookie(int userId)
        {
            var cookieVal = Guid.NewGuid();
            using (var scope = scopeProvider.CreateScope())
            {
                scope.Database.Save(new CookieModel
                {
                    CookieValue = cookieVal,
                    LoggedInAtUtc = DateTime.UtcNow,
                    UserId = userId,
                    Type = TwoFactorConstants.TempCookieName
                });
                scope.Complete();
            }
            var cookie = new HttpCookie(TwoFactorConstants.TempCookieName, cookieVal.ToString())
            {
                Expires = DateTime.UtcNow.AddMinutes(TwoFactorConstants.TempCookieTime),
                Path = "/",
                HttpOnly = true
            };
            return cookie;
        }

        public override bool SupportsUserTwoFactor => true;
    }
}
