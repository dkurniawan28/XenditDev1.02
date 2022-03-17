using System;
using System.Collections.Generic;

namespace XenditDev1._02.ModelsNew
{
    public partial class DetailWalletPayment
    {
        public int IdDetailWalletPayment { get; set; }
        public int? IdTransaksiPayment { get; set; }
        public string IdXendit { get; set; }
        public string ExternalId { get; set; }
        public string BusinessId { get; set; }
        public string Phone { get; set; }
        public decimal? Amount { get; set; }
        public string Status { get; set; }
        public string CheckoutUrl { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? PaymentTimestamp { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public DateTime? Created { get; set; }
        public string EwalletType { get; set; }
    }
}
