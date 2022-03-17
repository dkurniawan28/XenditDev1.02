using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XenditDev1._02.ModelsNew
{
    public class InvoiceResponse
    {
        public string id { get; set; }
        public string external_id { get; set; }
        public string user_id { get; set; }
        public string status { get; set; }
        public string merchant_name { get; set; }
        public string merchant_profile_picture_url { get; set; }
        public int amount { get; set; }
        public string payer_email { get; set; }
        public string description { get; set; }
        public DateTime expiry_date { get; set; }
        public string invoice_url { get; set; }
        public List<AvailableBank> available_banks { get; set; }
        public List<AvailableRetailOutlet> available_retail_outlets { get; set; }
        public List<AvailableEwallet> available_ewallets { get; set; }
        public bool should_exclude_credit_card { get; set; }
        public bool should_send_email { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
        public string currency { get; set; }

        public class AvailableBank
        {
            public string bank_code { get; set; }
            public string collection_type { get; set; }
            public string bank_account_number { get; set; }
            public int transfer_amount { get; set; }
            public string bank_branch { get; set; }
            public string account_holder_name { get; set; }
            public int identity_amount { get; set; }
        }

        public class AvailableRetailOutlet
        {
            public string retail_outlet_name { get; set; }
            public string payment_code { get; set; }
            public int transfer_amount { get; set; }
            public string merchant_name { get; set; }
        }

        public class AvailableEwallet
        {
            public string ewallet_type { get; set; }
        }
    }
}
