using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Composing;
using Umbraco.Core.Migrations;
using Umbraco.Core.Migrations.Upgrade;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;
using Umbraco.Core.Logging;
using Umbraco.Web;
using TwoFactorAuth.Models;

namespace TwoFactorAuth.Components
{
    public class MigrationComponent : IComponent
    {
        private readonly IScopeProvider scopeProvider;
        private readonly IMigrationBuilder migrationBuilder;
        private readonly IKeyValueService keyValueService;
        private readonly ILogger logger;

        public MigrationComponent(IScopeProvider scopeProvider, IMigrationBuilder migrationBuilder, IKeyValueService keyValueService, ILogger logger)
        {
            this.scopeProvider = scopeProvider;
            this.migrationBuilder = migrationBuilder;
            this.keyValueService = keyValueService;
            this.logger = logger;
        }

        public void Initialize()
        {
            var upgrader = new Upgrader(new TwoFactorMigrationPlan());
            upgrader.Execute(scopeProvider, migrationBuilder, keyValueService, logger);
        }

        public void Terminate()
        {
        }
    }
}
