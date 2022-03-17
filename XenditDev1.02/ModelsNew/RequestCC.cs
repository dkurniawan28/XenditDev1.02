using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XenditDev1._02.ModelsNew
{
    public class RequestCC
    {
        public string token_id { get; set; }
        public string external_id { get; set; }
        public string authentication_id { get; set; }
        public int amount { get; set; }
        public string card_cvn { get; set; }
        public string client_id { get; set; }
        public bool capture { get; set; }
    }
}
