using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XenditDev1._02.Response
{
    public class TransaksiResponse
    {

        public int IdTransaksiPayment { get; set; }
        public string IdXendit { get; set; }
        public string external_id { get; set; }
        public string user_id { get; set; }
        public int IdMerchan { get; set; }
        public string merchant_name { get; set; }
        public string merchant_profile_picture_url { get; set; }
        public decimal amount { get; set; }
        public string payer_email { get; set; }
        public string description { get; set; }
        public string expiry_date { get; set; }
        public string invoice_url { get; set; }
        public string status { get; set; }
        public bool should_exclude_credit_card { get; set; }
        public bool should_send_email { get; set; }
        public string created { get; set; }
        public string updated { get; set; }
        public string currency { get; set; }
        public string Timestamp { get; set; }
        public List<ItemBankResponse> available_banks { get; set; }
        public List<ItemOutletResponse> available_retail_outlets { get; set; }
        public List<ItemEwalletRespon> available_ewallets { get; set; }
    }


    public class ItemBankResponse
    {

        public string bank_code { get; set; }
        public string collection_type { get; set; }
        public string bank_account_number { get; set; }
        public int transfer_amount { get; set; }
        public string bank_branch { get; set; }
        public string account_holder_name { get; set; }
        public int identity_amount { get; set; }
        public int status { get; set; }
    }

    public class ItemOutletResponse
    {

        public string retail_outlet_name { get; set; }
        public string payment_code { get; set; }
        public int transfer_amount { get; set; }
        public string merchant_name { get; set; }
        public int status { get; set; }
    }

    public class ItemEwalletRespon
    {

        public string ewallet_type { get; set; }
        public int status { get; set; }
    }
}
