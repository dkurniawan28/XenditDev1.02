using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xendit.ApiClient;
using Xendit.ApiClient.Constants;
using Xendit.ApiClient.Invoice;

namespace XenditDev1._02.Function
{
    public class Invoice
    {
        // public string key = "xnd_development_e48fDUwdgdh8UIoYHmpk6Dvroi3DQcB0LUKg1fPxAk8GJCR388SM2fiOnmkmyS1";
        public string key = "xnd_production_WXVTBwxm7Eqwg7jGpuL65TUMfDU6st9kjHIWyOjNiKU9NyAwB01Wq5w0YcWKs";
        XenditInvoiceCreateResponse XenditInvoice = new XenditInvoiceCreateResponse();
        IEnumerable<XenditInvoiceCreateResponse> XenditInvoice1;


        public async Task<XenditInvoiceCreateResponse> CreateInvoices(string externalId, decimal amount, string payeremail,string description)

        {
            try
            {
                var xendit = new XenditClient(key);
                var invoiceitem = new XenditInvoiceCreateRequest
                {
                    ExternalId = externalId,
                    Amount = amount,
                    PayerEmail = payeremail,
                    Description=description

                };


                XenditInvoice = await xendit.Invoice.CreateAsync(invoiceitem);
            }
            catch (Exception ex)
            {

            }
            return XenditInvoice;
        }

        public async Task<XenditInvoiceCreateResponse> GetInvoiceById(string idinvoice)
        {
            try
            {
                var xendit = new XenditClient(key);
                XenditInvoice = await xendit.Invoice.GetAsync(idinvoice);
            }
            catch (Exception ex)
            {

            }
            return XenditInvoice;
        }

        public async Task<XenditInvoiceCreateResponse> Expired(string idinvoice)
        {
            try
            {
                var xendit = new XenditClient(key);
                XenditInvoice = await xendit.Invoice.ExpireAsync(idinvoice);
            }
            catch (Exception ex)
            {

            }
            return XenditInvoice;
        }

        public async Task<IEnumerable<XenditInvoiceCreateResponse>> GetAllInvoice(XenditInvoiceOptions option)
        {

            List<XenditInvoiceStatus> liststatus = new List<XenditInvoiceStatus> { XenditInvoiceStatus.SETTLED, XenditInvoiceStatus.EXPIRED, XenditInvoiceStatus.PENDING, XenditInvoiceStatus .PAID};
            
            try
            {
                var xendit = new XenditClient(key);
                var options = new XenditInvoiceOptions
                {
                    Limit = 3,
                    Statuses = liststatus
                };

              XenditInvoice1 = await xendit.Invoice.GetAllAsync(options);
            }
            catch (Exception ex)
            {

            }
            return XenditInvoice1;
        }
    }
}
