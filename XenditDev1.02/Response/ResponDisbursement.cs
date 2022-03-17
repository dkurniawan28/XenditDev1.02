using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XenditDev1._02.Response
{
    public class ResponDisbursement
    {

        public string status { get; set; }
        public string user_id { get; set; }
        public string external_id { get; set; }
        public int amount { get; set; }
        public string bank_code { get; set; }
        public string account_holder_name { get; set; }
        public string disbursement_description { get; set; }
        public string id { get; set; }
    }
}
