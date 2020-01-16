using Owin;
using System;
using TwoFactorAuth.Configuration;
using TwoFactorAuth.UserHelpers;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Configuration;
using Umbraco.Core.Configuration.UmbracoSettings;
using Umbraco.Core.Models.Identity;
using Umbraco.Core.Scoping;
using Umbraco.Core.Security;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Security;

namespace TwoFactorAuth.Components
{
    class UserComponent : IComponent
    {
        private readonly IGlobalSettings globalSettings;
        private readonly IUmbracoSettingsSection umbracoSettingsSection;
        private readonly IRuntimeState runtimeState;
        private readonly IUserService userService;
        private readonly IMemberTypeService memberTypeService;
        private readonly IEntityService entityService;
        private readonly IExternalLoginService externalLoginService;
        private readonly IScopeProvider scopeProvider;

        public UserComponent(IGlobalSettings globalSettings, IUmbracoSettingsSection umbracoSettingsSection, IRuntimeState runtimeState, IUserService userService, IMemberTypeService memberTypeService, IEntityService entityService, IExternalLoginService externalLoginService, IScopeProvider scopeProvider)
        {
            this.globalSettings = globalSettings;
            this.umbracoSettingsSection = umbracoSettingsSection;
            this.runtimeState = runtimeState;
            this.userService = userService;
            this.memberTypeService = memberTypeService;
            this.entityService = entityService;
            this.externalLoginService = externalLoginService;
            this.scopeProvider = scopeProvider;
        }

        public void Initialize()
        {
            UmbracoDefaultOwinStartup.MiddlewareConfigured += (_, e) => ConfigureManager(e.AppBuilder);
        }

        private void ConfigureManager(IAppBuilder app)
        {
            app.UseTwoFactorSignInCookie(Constants.Security.BackOfficeTwoFactorAuthenticationType, TimeSpan.FromMinutes(SignInCookieTime));
            app.ConfigureUserManagerForUmbracoBackOffice<BackOfficeUserManager, BackOfficeIdentityUser>(
            runtimeState,
            globalSettings,
            (options, context) =>
            {
                var membershipProvider = MembershipProviderExtensions.GetUsersMembershipProvider().AsUmbracoMembershipProvider();
                var userManager = CustomUserManager.Create(
                    options,
                    userService,
                    memberTypeService,
                    entityService,
                    externalLoginService,
                    membershipProvider,
                    Current.Mapper,
                    umbracoSettingsSection.Content,
                    globalSettings,
                    scopeProvider
                    );
                return userManager;
            });
        }

        private int SignInCookieTime => TwoFactorConfig.GetConfig().TimeoutTime;

        public void Terminate()
        {
        }
    }
}
