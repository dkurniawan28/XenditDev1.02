using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XenditDev1._02.ModelsNew
{
    public class OvoRespon
    {


        public class ChannelProperties
        {
            public string mobile_number { get; set; }
        }

        public class Metadata
        {
            public string branch_area { get; set; }
            public string branch_city { get; set; }
        }

      
            public string id { get; set; }
            public string business_id { get; set; }
            public string reference_id { get; set; }
            public string status { get; set; }
            public string currency { get; set; }
            public int charge_amount { get; set; }
            public int capture_amount { get; set; }
            public object refunded_amount { get; set; }
            public string checkout_method { get; set; }
            public string channel_code { get; set; }
            public ChannelProperties channel_properties { get; set; }
            public object actions { get; set; }
            public bool is_redirect_required { get; set; }
            public string callback_url { get; set; }
            public DateTime created { get; set; }
            public DateTime updated { get; set; }
            public object void_status { get; set; }
            public object voided_at { get; set; }
            public bool capture_now { get; set; }
            public object customer_id { get; set; }
            public object payment_method_id { get; set; }
            public object failure_code { get; set; }
            public object basket { get; set; }
            public Metadata metadata { get; set; }
       
    }
}
