using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XenditDev1._02.ModelsNew
{
    public class ResponseChargeCC
    {
        public class Promotion
        {
            public string reference_id { get; set; }
            public string original_amount { get; set; }
        }

        public class Installment
        {
            public int count { get; set; }
            public string interval { get; set; }
        }

        public DateTime created { get; set; }
        public string status { get; set; }
        public string business_id { get; set; }
        public int authorized_amount { get; set; }
        public string external_id { get; set; }
        public string merchant_id { get; set; }
        public string merchant_reference_code { get; set; }
        public string card_type { get; set; }
        public string masked_card_number { get; set; }
        public string charge_type { get; set; }
        public string card_brand { get; set; }
        public string bank_reconciliation_id { get; set; }
        public string eci { get; set; }
        public int capture_amount { get; set; }
        public string descriptor { get; set; }
        public string id { get; set; }
        public string mid_label { get; set; }
        public Promotion promotion { get; set; }
        public Installment installment { get; set; }

    }
}
