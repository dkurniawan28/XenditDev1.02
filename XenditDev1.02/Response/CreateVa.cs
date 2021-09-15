using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XenditDev1._02.Response
{
    public class CreateVa
    {
        public string id { get; set; }
        public string external_id { get; set; }
        public string owner_id { get; set; }
        public string bank_code { get; set; }
        public string merchant_code { get; set; }
        public string name { get; set; }
        public string account_number { get; set; }
        public double expected_amount { get; set; }
        public bool is_single_use { get; set; }
        public bool is_closed { get; set; }
        public object description { get; set; }
        public string currency { get; set; }
        public string status { get; set; }
        public DateTime expiration_date { get; set; }
    }
}
