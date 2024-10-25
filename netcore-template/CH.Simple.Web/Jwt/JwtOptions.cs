using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace CH.Simple.Web.Filters
{
    public class JwtOptions
    {
        public string SecurityKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }
}
