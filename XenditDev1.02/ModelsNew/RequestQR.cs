using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XenditDev1._02.ModelsNew
{
    public class RequestQR
    {
        
        public string external_id { get; set; }
        public string type { get; set; }
        public string callback_url { get; set; }
        public int amount { get; set; }
    }
}

