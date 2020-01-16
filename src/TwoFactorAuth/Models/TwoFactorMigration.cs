using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Migrations;

namespace TwoFactorAuth.Models
{
    public class TwoFactorMigrationPlan : MigrationPlan
    {
        public TwoFactorMigrationPlan()
            : base("CustomDatabaseTable")
        {
            From(string.Empty)
                .To<TwoFactorMigration>("2fa-migration");
        }
    }

    public class TwoFactorMigration : MigrationBase
    {
        public TwoFactorMigration(IMigrationContext context) : base(context) { }

        public override void Migrate()
        {
            if (!TableExists("twoFactor"))
            {
                Create.Table<TwoFactorModel>().Do();
                Create.ForeignKey().FromTable("twoFactor").ForeignColumn("userId").ToTable("umbracoUser").PrimaryColumn("id").Do();
            }
            if (!TableExists("twoFactorPerms"))
            {
                Create.Table<PermsModel>().Do();
                Create.ForeignKey().FromTable("twoFactorPerms").ForeignColumn("groupId").ToTable("umbracoUserGroup").PrimaryColumn("id").Do();
            }
            if (!TableExists("twoFactorCookies"))
            {
                Create.Table<CookieModel>().Do();
                Create.ForeignKey().FromTable("twoFactorCookies").ForeignColumn("userId").ToTable("umbracoUser").PrimaryColumn("id").Do();
            }
        }
    }
}