using System;
using System.Collections.Generic;

namespace XenditDev1._02.ModelsNew
{
    public partial class DetailLogTransaksi
    {
        public int IdDetailLogTransaksi { get; set; }
        public int? IdLogRequest { get; set; }
        public int? IdTransaksiPayment { get; set; }
        public string Description { get; set; }
        public DateTime? Timestamp { get; set; }

        public virtual RequestLog IdLogRequestNavigation { get; set; }
        public virtual TransaksiPayment IdTransaksiPaymentNavigation { get; set; }
    }
}
