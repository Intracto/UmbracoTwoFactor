using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace TwoFactorAuth.Models
{
    [TableName("twoFactorPerms")]
    [PrimaryKey("groupId", AutoIncrement = false)]
    [ExplicitColumns]
    public class PermsModel
    {
        [Column("groupId")]
        [PrimaryKeyColumn(AutoIncrement = false, OnColumns = "groupId")]
        public int GroupId { get; set; }

        [Column("twoFactorRequired")]
        public bool Required { get; set; }

        [Column("canUseTotp")]
        public bool CanUseTotp { get; set; }

        [Column("canUseMail")]
        public bool CanUseMail { get; set; }
    }
}
