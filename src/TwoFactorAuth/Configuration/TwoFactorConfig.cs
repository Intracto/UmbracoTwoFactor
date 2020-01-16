using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Xml.Linq;

namespace TwoFactorAuth.Configuration
{
    class TwoFactorConfig : ConfigurationSection
    {
        public static TwoFactorConfig GetConfig()
        {
            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Plugins/TwoFactor/TwoFactor.config");
            var config = XDocument.Load(path).Root;
            var mailConfig = config.Element("mailSettings");
            return new TwoFactorConfig
            {
                ApplicationName = (string)config.Element("appName"),
                PageSize = (int)config.Element("pageSize"),
                TimeoutTime = (int)config.Element("timeoutTime"),
                BrowserRememberTime = (int)config.Element("browserRememberTime"),
                MailAddress = (string)mailConfig.Element("fromAddress"),
                MailSubject = (string)mailConfig.Element("subject"),
                MailBody = (string)mailConfig.Element("message")
            };
        }

        public string ApplicationName { get; private set; }
        public int PageSize { get; private set; }
        public int TimeoutTime { get; private set; }
        public int BrowserRememberTime { get; private set; }
        public string MailAddress { get; private set; }
        public string MailSubject { get; private set; }
        public string MailBody { get; private set; }

    }
}
