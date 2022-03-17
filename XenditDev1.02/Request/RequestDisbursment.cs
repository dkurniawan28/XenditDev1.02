using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XenditDev1._02.Request
{
    public class RequestDisbursment
    {

        public string external_id { get; set; }
        public int amount { get; set; }
        public string bank_code { get; set; }
        public string account_holder_name { get; set; }
        public string account_number { get; set; }
        public string description { get; set; }
    }
}
