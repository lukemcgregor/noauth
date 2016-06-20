using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.OAuth2;

namespace NoAuth.Site.OAuth
{
    public class Client : IClientDescription
    {
        public ClientType ClientType { get; set; }

        public Uri DefaultCallback { get; set; }

        public bool HasNonEmptySecret { get; set; } = true;

        public bool IsCallbackAllowed(Uri callback)
        {
            return true;
        }

        public bool IsValidClientSecret(string secret)
        {
            return true;
        }
    }
}