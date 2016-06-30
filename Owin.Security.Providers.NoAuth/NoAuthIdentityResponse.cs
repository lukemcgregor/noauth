using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owin.Security.Providers.NoAuth
{
    public class NoAuthIdentityResponse
    {
        public string Id { get; set; }
        public List<Tuple<string, string>> Claims { get; set; }
    }
}

