using System;
using System.Collections.Generic;

namespace XenditDev1._02.ModelsNew
{
    public partial class DetailQr
    {
        public int IdQr { get; set; }
        public int? IdTransaksiPayment { get; set; }
        public string IdXendit { get; set; }
        public decimal? Amount { get; set; }
        public string Description { get; set; }
        public string QrString { get; set; }
        public string CallbackUrl { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string MetaData { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
