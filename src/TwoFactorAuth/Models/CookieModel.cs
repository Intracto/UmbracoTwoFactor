using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace TwoFactorAuth.Models
{
    [TableName("twoFactorCookies")]
    [PrimaryKey("cookieValue", AutoIncrement = false)]
    [ExplicitColumns]
    public class CookieModel
    {
        [Column("cookieValue")]
        [PrimaryKeyColumn(AutoIncrement = false, OnColumns = "cookieValue")]
        public Guid CookieValue { get; set; }

        [Column("userId")]
        public int UserId { get; set; }

        [Column("loggedInAtUtc")]
        public DateTime LoggedInAtUtc { get; set; }

        [Column("type")]
        public string Type { get; set; }
    }
}
