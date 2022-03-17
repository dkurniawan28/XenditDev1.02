using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XenditDev1._02.ModelsNew
{
    public class CallBack
    {
        public class VaPaid
        {
            
            public DateTime updated { get; set; }
            public DateTime created { get; set; }
            public string payment_id { get; set; }
            public string callback_virtual_account_id { get; set; }
            public string owner_id { get; set; }
            public string external_id { get; set; }
            public string account_number { get; set; }
            public string bank_code { get; set; }
            public int amount { get; set; }
            public DateTime transaction_timestamp { get; set; }
            public string merchant_code { get; set; }
            public string id { get; set; }
       

        }

        public class CreatedVA
        {
              
            public string id { get; set; }
            public string owner_id { get; set; }
            public string external_id { get; set; }
            public string merchant_code { get; set; }
            public string account_number { get; set; }
            public string bank_code { get; set; }
            public string name { get; set; }
            public bool is_closed { get; set; }
            public DateTime expiration_date { get; set; }
            public bool is_single_use { get; set; }
            public string status { get; set; }
            public DateTime created { get; set; }
            public DateTime updated { get; set; }
          

        }

        public class Distbursment
        {
            public string id { get; set; }
            public string uuid { get; set; }
            public int amount { get; set; }
            public string entity { get; set; }
            public string status { get; set; }
            public DateTime created { get; set; }
            public DateTime updated { get; set; }
            public string user_id { get; set; }
            public string bank_code { get; set; }
            public string external_id { get; set; }
            public string failure_code { get; set; }
            public string account_number { get; set; }
            public DateTime status_updated { get; set; }
            public string xendit_fee_user_id { get; set; }
            public string account_holder_name { get; set; }
            public string disbursement_description { get; set; }
            public bool should_prioritize_disbursement { get; set; }

        }


        public class BatchDistbursment
        {
            public List<Disbursement> disbursements { get; set; }
            public int total_disbursed_count { get; set; }
            public int total_disbursed_amount { get; set; }
            public int total_error_count { get; set; }
            public int total_error_amount { get; set; }
            public string id { get; set; }
            public string status { get; set; }
            public string approver_id { get; set; }
            public DateTime approved_at { get; set; }
            public int total_uploaded_count { get; set; }
            public int total_uploaded_amount { get; set; }
            public string reference { get; set; }
            public string user_id { get; set; }
            public DateTime created { get; set; }
            public DateTime updated { get; set; }
       
            public class Disbursement
            {
                public string bank_account_number { get; set; }
                public string bank_account_name { get; set; }
                public string email_to { get; set; }
                public string id { get; set; }
                public string status { get; set; }
                public string bank_reference { get; set; }
                public string valid_name { get; set; }
                public string external_id { get; set; }
                public string description { get; set; }
                public string bank_code { get; set; }
                public int amount { get; set; }
                public DateTime created { get; set; }
                public DateTime updated { get; set; }
            }
        }

        public class RetailOutlet
        {
            public string id { get; set; }
            public string external_id { get; set; }
            public string prefix { get; set; }
            public string payment_code { get; set; }
            public string retail_outlet_name { get; set; }
            public string name { get; set; }
            public int amount { get; set; }
            public string status { get; set; }
            public DateTime transaction_timestamp { get; set; }
            public string payment_id { get; set; }
            public string fixed_payment_code_payment_id { get; set; }
            public string fixed_payment_code_id { get; set; }
            public string owner_id { get; set; }
         
        }

        public class DirectdebitPaid
        {
            public string events { get; set; }
            public string id { get; set; }
            public string business_id { get; set; }
            public string reference_id { get; set; }
            public string payment_method_id { get; set; }
            public string channel_code { get; set; }
            public string currency { get; set; }
            public int amount { get; set; }
            public string description { get; set; }
            public string status { get; set; }
            public object metadata { get; set; }
            public DateTime timestamp { get; set; }
            public DateTime created { get; set; }
            public DateTime updated { get; set; }
          
        }

        public class RefundDebit
        {
            public string @event { get; set; }
            public DateTime created { get; set; }
            public string business_id { get; set; }
            public Data data { get; set; }
         
            public class Metadata

            {
                public string payment_reference { get; set; }
            }

            public class Data
            {
                public string id { get; set; }
                public string direct_debit_id { get; set; }
                public int amount { get; set; }
                public string currency { get; set; }
                public string status { get; set; }
                public string reason { get; set; }
                public object failure_code { get; set; }
                public Metadata metadata { get; set; }
            }
        }
        public class ExpiredDebit
        {
            public string @event { get; set; }
            public string id { get; set; }
            public string business_id { get; set; }
            public string customer_id { get; set; }
            public string expiration_timestamp { get; set; }
            public DateTime timestamp { get; set; }
        }

        public class InvoicesPaid
        {
            public string id { get; set; }
            public string external_id { get; set; }
            public string user_id { get; set; }
            public bool is_high { get; set; }
            public string payment_method { get; set; }
            public string status { get; set; }
            public string merchant_name { get; set; }
            public int amount { get; set; }
            public int paid_amount { get; set; }
            public string bank_code { get; set; }
            public DateTime paid_at { get; set; }
            public string payer_email { get; set; }
            public string description { get; set; }
            public int adjusted_received_amount { get; set; }
            public int fees_paid_amount { get; set; }
            public DateTime updated { get; set; }
            public DateTime created { get; set; }
            public string currency { get; set; }
            public string payment_channel { get; set; }
            public string payment_destination { get; set; }
           
        }

        public class OvoPaid
        {
            public string id { get; set; }
            public string @event { get; set; }
            public string phone { get; set; }
            public int amount { get; set; }
            public string status { get; set; }
            public DateTime created { get; set; }
            public string business_id { get; set; }
            public string external_id { get; set; }
            public string ewallet_type { get; set; }
       
        }

        public class EwalletPaid
        {
            public Data data { get; set; }
            public string @event { get; set; }
            public string created { get; set; }
            public string business_id { get; set; }
          
            public class Actions
            {
                public string mobile_web_checkout_url { get; set; }
                public string desktop_web_checkout_url { get; set; }
                public string mobile_deeplink_checkout_url { get; set; }
            }

            public class Metadata
            {
                public string branch_code { get; set; }
            }

            public class ChannelProperties
            {
                public string success_redirect_url { get; set; }
            }

            public class Data
            {
                public string id { get; set; }
                public object basket { get; set; }
                public string status { get; set; }
                public Actions actions { get; set; }
                public DateTime created { get; set; }
                public DateTime updated { get; set; }
                public string currency { get; set; }
                public Metadata metadata { get; set; }
                public object voided_at { get; set; }
                public bool capture_now { get; set; }
                public object customer_id { get; set; }
                public string callback_url { get; set; }
                public string channel_code { get; set; }
                public object failure_code { get; set; }
                public string reference_id { get; set; }
                public int charge_amount { get; set; }
                public int capture_amount { get; set; }
                public string checkout_method { get; set; }
                public object payment_method_id { get; set; }
                public ChannelProperties channel_properties { get; set; }
                public bool is_redirect_required { get; set; }
               
            }
        }
    }

   
}
