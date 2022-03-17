using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XenditDev1._02.ModelsNew
{
    public class CreateQR
    {
        public string id { get; set; }
        public string external_id { get; set; }
        public int amount { get; set; }
        public string description { get; set; }
        public string qr_string { get; set; }
        public string callback_url { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
        public object metadata { get; set; }
    }
}
