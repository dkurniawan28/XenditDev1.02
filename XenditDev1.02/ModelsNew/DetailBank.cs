using System;
using System.Collections.Generic;

namespace XenditDev1._02.ModelsNew
{
    public partial class DetailBank
    {
        public int IdDetailBank { get; set; }
        public string BankCode { get; set; }
        public string CollectionType { get; set; }
        public string BankAccountNumber { get; set; }
        public decimal? TransferAmount { get; set; }
        public string BankBranch { get; set; }
        public string AccountHolderName { get; set; }
        public int? IdentityAmount { get; set; }
        public int? Status { get; set; }
        public int? IdTransaksiPayment { get; set; }

        public virtual TransaksiPayment IdTransaksiPaymentNavigation { get; set; }
    }
}
