using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XenditDev1._02.ModelsNew;

namespace XenditDev1._02.Function
{
    public class UpdateDb
    {

        public void updateTransaksi(String extId, String paymentby)
        {

            TransaksiPayment transaksi = new TransaksiPayment();
            using (var db = new PaymentGatewayContext())
            {
                var result = db.TransaksiPayment.SingleOrDefault(b => b.ExternalId == extId);
                if (result != null)
                {
                    result.Status = "PAID";
                    result.PaymentBy = paymentby;
                    db.SaveChanges();
                }
            }
        }

        public void updateTransaksiPaymentCC(int IdTransaksiPayment)
        {

            TransaksiPayment transaksi = new TransaksiPayment();
            using (var db = new PaymentGatewayContext())
            {
                var result = db.TransaksiPayment.SingleOrDefault(b => b.IdTransaksiPayment == IdTransaksiPayment);
                if (result != null)
                {
                    result.Status = "PAID";
                    result.PaymentBy = "CC";
                    db.SaveChanges();
                }
            }
        }

        public void updateTransaksiInvoice(String extId, String paymentby,String Status)
        {

            TransaksiPayment transaksi = new TransaksiPayment();
            using (var db = new PaymentGatewayContext())
            {
                var result = db.TransaksiPayment.SingleOrDefault(b => b.ExternalId == extId);
                if (result != null)
                {
                    result.Status = Status;
                    result.PaymentBy = paymentby;
                    db.SaveChanges();
                }
            }
        }

        public void updateTransaksiDisbursement(String idXendit, String Status)
        {

            TransaksiPayment transaksi = new TransaksiPayment();
            using (var db = new PaymentGatewayContext())
            {
                var result = db.TransaksiPayment.SingleOrDefault(b => b.IdXendit == idXendit);
                if (result != null)
                {
                    result.Status = Status;
                    result.PaymentBy = "DISBURSEMENT";
                    db.SaveChanges();
                }
            }
        }

        public void updateTransaksiDisbursementaspan(String externalid, String Status)
        {

            TransaksiPayment transaksi = new TransaksiPayment();
            using (var db = new PaymentGatewayContext())
            {
                var result = db.TransaksiPayment.SingleOrDefault(b => b.ExternalId == externalid);
                if (result != null)
                {
                    result.Status = Status;
                    result.PaymentBy = "DISBURSEMENT";
                    db.SaveChanges();
                }
            }
        }

        public void updateTransaksi2(String idXendit, String paymentby)
        {

            TransaksiPayment transaksi = new TransaksiPayment();
            using (var db = new PaymentGatewayContext())
            {
                var result = db.TransaksiPayment.SingleOrDefault(b => b.IdXendit == idXendit);
                if (result != null)
                {
                    result.Status = "PAID";
                    result.PaymentBy = paymentby;
                    db.SaveChanges();
                }
            }
        }

        public void updateTransaksiOvoSukses(int id)
        {

            TransaksiPayment transaksi = new TransaksiPayment();
            using (var db = new PaymentGatewayContext())
            {
                var result = db.TransaksiPayment.SingleOrDefault(b => b.IdTransaksiPayment == id);
                if (result != null)
                {
                    result.Status = "PAID";
                    result.PaymentBy = "OVO";
                    db.SaveChanges();
                }
            }
        }

        public void updateTransaksiOvoFailed(int id)
        {

            TransaksiPayment transaksi = new TransaksiPayment();
            using (var db = new PaymentGatewayContext())
            {
                var result = db.TransaksiPayment.SingleOrDefault(b => b.IdTransaksiPayment == id);
                if (result != null)
                {
                    result.Status = "FAILED";
                    result.PaymentBy = "OVO";
                    db.SaveChanges();
                }
            }
        }
    }
}
