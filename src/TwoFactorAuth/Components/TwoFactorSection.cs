using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models.Sections;

namespace TwoFactorAuth.Components
{
    class TwoFactorSection : ISection
    {
        public string Alias => TwoFactorConstants.SectionName;

        public string Name => TwoFactorConstants.SectionName;
    }
}
