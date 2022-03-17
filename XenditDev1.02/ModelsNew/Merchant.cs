using System;
using System.Collections.Generic;

namespace XenditDev1._02.ModelsNew
{
    public partial class Merchant
    {
        public Merchant()
        {
            Transaksi = new HashSet<Transaksi>();
            TransaksiPayment = new HashSet<TransaksiPayment>();
        }

        public int IdMerchant { get; set; }
        public string NamaMerchant { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string UrlCallBack { get; set; }
        public string UrlWithdraw { get; set; }

        public virtual ICollection<Transaksi> Transaksi { get; set; }
        public virtual ICollection<TransaksiPayment> TransaksiPayment { get; set; }
    }
}
