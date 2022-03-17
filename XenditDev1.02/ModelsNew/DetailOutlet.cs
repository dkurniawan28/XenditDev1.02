using System;
using System.Collections.Generic;

namespace XenditDev1._02.ModelsNew
{
    public partial class DetailOutlet
    {
        public int IdDetailOutlet { get; set; }
        public string RetailOutletName { get; set; }
        public string PaymentCode { get; set; }
        public decimal? TransferAmount { get; set; }
        public string MerchantName { get; set; }
        public int? Status { get; set; }
        public int? IdTransaksiPayment { get; set; }

        public virtual TransaksiPayment IdTransaksiPaymentNavigation { get; set; }
    }
}
