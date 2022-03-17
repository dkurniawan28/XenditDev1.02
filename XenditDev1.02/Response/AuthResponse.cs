using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XenditDev1.ResponseClass
{
    public class AuthResponse
    {
        public String access_token { get; set; }
        public String token_type { get; set; }
        public String expires_in { get; set; }
        public String scope { get; set; }
        public String Username { get; set; }
        public int Userid { get; set; }
      
    }
}