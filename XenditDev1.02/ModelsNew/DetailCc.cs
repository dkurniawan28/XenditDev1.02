using System;
using System.Collections.Generic;

namespace XenditDev1._02.ModelsNew
{
    public partial class DetailCc
    {
        public int IdDetailCc { get; set; }
        public int? IdTransaksiPayment { get; set; }
        public string IdXendit { get; set; }
        public string Status { get; set; }
        public decimal? AuthorizedAmount { get; set; }
        public decimal? CaptureAmount { get; set; }
        public string Currency { get; set; }
        public string CreditCardTokenId { get; set; }
        public string BusinessId { get; set; }
        public string MerchantId { get; set; }
        public string MerchantReferenceCode { get; set; }
        public string ExternalId { get; set; }
        public string Eci { get; set; }
        public string ChargeType { get; set; }
        public string MaskedCardNumber { get; set; }
        public string CardBrand { get; set; }
        public string CardType { get; set; }
        public string Xid { get; set; }
        public string Cavv { get; set; }
        public string Descriptor { get; set; }
        public string AuthorizationId { get; set; }
        public string BankReconciliationId { get; set; }
        public string Metadata { get; set; }
        public string IssuingBankName { get; set; }
        public string ClientId { get; set; }
        public string CvnCode { get; set; }
        public string ApprovalCode { get; set; }
        public DateTime? Created { get; set; }

        public virtual TransaksiPayment IdTransaksiPaymentNavigation { get; set; }
    }
}
