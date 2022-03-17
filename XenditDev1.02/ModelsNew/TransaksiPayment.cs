using System;
using System.Collections.Generic;

namespace XenditDev1._02.ModelsNew
{
    public partial class TransaksiPayment
    {
        public TransaksiPayment()
        {
            DetailBank = new HashSet<DetailBank>();
            DetailCc = new HashSet<DetailCc>();
            DetailEwallet = new HashSet<DetailEwallet>();
            DetailLogTransaksi = new HashSet<DetailLogTransaksi>();
            DetailOutlet = new HashSet<DetailOutlet>();
        }

        public int IdTransaksiPayment { get; set; }
        public string IdXendit { get; set; }
        public string ExternalId { get; set; }
        public string UserId { get; set; }
        public int? IdMerchan { get; set; }
        public string MerchantName { get; set; }
        public string MerchantProfilePictureUrl { get; set; }
        public decimal? Amount { get; set; }
        public string PayerEmail { get; set; }
        public string Description { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string InvoiceUrl { get; set; }
        public string Status { get; set; }
        public int? ShouldExcludeCreditCard { get; set; }
        public int? ShouldSendEmail { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public string Currency { get; set; }
        public DateTime? Timestamp { get; set; }
        public string PaymentBy { get; set; }

        public virtual Merchant IdMerchantNavigation { get; set; }
        public virtual ICollection<DetailBank> DetailBank { get; set; }
        public virtual ICollection<DetailCc> DetailCc { get; set; }
        public virtual ICollection<DetailEwallet> DetailEwallet { get; set; }
        public virtual ICollection<DetailLogTransaksi> DetailLogTransaksi { get; set; }
        public virtual ICollection<DetailOutlet> DetailOutlet { get; set; }
    }
}
