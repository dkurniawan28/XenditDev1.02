using System;
using System.Collections.Generic;

namespace XenditDev1._02.ModelsNew
{
    public partial class RequestLog
    {
        public RequestLog()
        {
            DetailLogTransaksi = new HashSet<DetailLogTransaksi>();
            Transaksi = new HashSet<Transaksi>();
        }

        public int IdRequestLog { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string Url { get; set; }

        public virtual ICollection<DetailLogTransaksi> DetailLogTransaksi { get; set; }
        public virtual ICollection<Transaksi> Transaksi { get; set; }
    }
}
