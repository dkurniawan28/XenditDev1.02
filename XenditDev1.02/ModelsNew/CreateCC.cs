using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XenditDev1._02.ModelsNew
{
    public class CreateCC
    {
        public class Metadata
        {
        }
        public string status { get; set; }
        public int authorized_amount { get; set; }
        public int capture_amount { get; set; }
        public string currency { get; set; }
        public string credit_card_token_id { get; set; }
        public string business_id { get; set; }
        public string merchant_id { get; set; }
        public string merchant_reference_code { get; set; }
        public string external_id { get; set; }
        public string eci { get; set; }
        public string charge_type { get; set; }
        public string masked_card_number { get; set; }
        public string card_brand { get; set; }
        public string card_type { get; set; }
        public string xid { get; set; }
        public string cavv { get; set; }
        public string descriptor { get; set; }
        public string authorization_id { get; set; }
        public string bank_reconciliation_id { get; set; }
        public Metadata metadata { get; set; }
        public string issuing_bank_name { get; set; }
        public string client_id { get; set; }
        public string cvn_code { get; set; }
        public string approval_code { get; set; }
        public DateTime created { get; set; }
        public string id { get; set; }
    }
}
