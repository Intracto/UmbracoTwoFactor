using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models.Identity;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace TwoFactorAuth.Models
{
    [TableName("twoFactor")]
    [PrimaryKey("id", AutoIncrement = true)]
    [ExplicitColumns]
    public class TwoFactorModel
    {
        [Column("id")]
        [PrimaryKeyColumn(AutoIncrement = true, OnColumns = "id")]
        public int Id { get; set; }

        [Column("userId")]
        public int UserId { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("secret")]
        public string Secret { get; set; }

        [Column("counter")]
        public int Counter { get; set; }
    }
}
