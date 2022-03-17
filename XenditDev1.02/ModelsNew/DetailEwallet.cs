using System;
using System.Collections.Generic;

namespace XenditDev1._02.ModelsNew
{
    public partial class DetailEwallet
    {
        public int IdDetailEwallet { get; set; }
        public int? IdTransaksiPayment { get; set; }
        public string EwalletType { get; set; }
        public int? Status { get; set; }

        public virtual TransaksiPayment IdTransaksiPaymentNavigation { get; set; }
    }
}
