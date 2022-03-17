using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XenditDev1._02.ModelsNew
{
    public class RequestOvo
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

     
            public string reference_id { get; set; }
            public string currency { get; set; }
            public int amount { get; set; }
            public string checkout_method { get; set; }
            public string channel_code { get; set; }
            public ChannelProperties channel_properties { get; set; }
         //   public Metadata metadata { get; set; }
        
    }
}
