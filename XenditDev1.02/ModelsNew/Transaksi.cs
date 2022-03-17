using System;
using System.Collections.Generic;

namespace XenditDev1._02.ModelsNew
{
    public partial class Transaksi
    {
        public int IdTransaksi { get; set; }
        public decimal? Amount { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal? NominalCharge { get; set; }
        public int? Status { get; set; }
        public DateTime? Timestamp { get; set; }
        public int? IdMasterPayment { get; set; }
        public int? IdReqLog { get; set; }
        public int? IdUser { get; set; }

        public virtual RequestLog IdReqLogNavigation { get; set; }
        public virtual Merchant IdUserNavigation { get; set; }
    }
}
