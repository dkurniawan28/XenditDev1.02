using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Xendit.ApiClient;
using Xendit.ApiClient.Constants;
using Xendit.ApiClient.Invoice;
using Xendit.ApiClient.VirtualAccount;
using Xendit.ApiClient.Disbursement;
using XenditDev1._02.Function;
using XenditDev1._02.Response;
using Xendit.ApiClient.EWallet;

namespace XenditDev1._02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAsync()
        {
            VirtualAccount va = new VirtualAccount();
            Disbursement disb = new Disbursement();
            Ewallet Ewallet = new Ewallet();
            Invoice Invoice = new Invoice();
            XenditVACreateResponse xenditVA = new XenditVACreateResponse();
          
            XenditBatchDisbursementCreateResponse xenditBachDisb = new XenditBatchDisbursementCreateResponse();
            XenditDisbursementCreateResponse xenditDisb = new XenditDisbursementCreateResponse();
            XenditEWalletCreatePaymentResponse xenditwallet = new XenditEWalletCreatePaymentResponse();
            XenditInvoiceCreateResponse xinditInvoice = new XenditInvoiceCreateResponse();
            List<XenditEWalletCreateLinkAjaPaymentRequestItem> listitem = new List<XenditEWalletCreateLinkAjaPaymentRequestItem>();
            XenditEWalletCreateLinkAjaPaymentRequestItem items = new XenditEWalletCreateLinkAjaPaymentRequestItem();
            XenditInvoiceOptions option = new XenditInvoiceOptions();
            List<XenditInvoiceStatus> lisstatus = new List<XenditInvoiceStatus> { XenditInvoiceStatus.SETTLED, XenditInvoiceStatus.EXPIRED };

            IEnumerable<XenditInvoiceCreateResponse> xinditInvoice2 ;

            option.Limit = 5;
            option.Statuses = lisstatus;


            listitem.Add(new XenditEWalletCreateLinkAjaPaymentRequestItem() { Id = "1", Name = "Chikibal",Price=6000,Quantity=5 });
            listitem.Add(new XenditEWalletCreateLinkAjaPaymentRequestItem() { Id = "2", Name = "chitos", Price = 6000, Quantity = 7 });


            xenditVA = await va.VaAsync("ded", 1, "9999000015", "VA_fixed-" + DateTime.Now, 5000000);

            xenditVA = await va.Vacek(xenditVA.Id);

           xenditVA = await va.VaExpired(xenditVA.Id);

            string[] email;
            email = new string[2] { "dkurniawan28@gmail.com", "jono@dsd.com"};


            xenditBachDisb = await disb.BatchDisb("dedy test", 500000, XenditDisbursementBankCode.BCA, "dedy kurniawan", "1234587", "tester dedy", email, "122233");
            xenditwallet = await Ewallet.Ovo("ovo-ewallet-12345", 25000, "085810450098");
            xenditwallet = await Ewallet.GetPayment("ovo-ewallet-12345", XenditEWalletType.OVO);

            xenditwallet = await Ewallet.Dana("dana-ewallet-12345", 25000, "https://www.youtube.com/", "https://www.youtube.com/");
            xenditwallet = await Ewallet.GetPayment("dana-ewallet-12345", XenditEWalletType.DANA);
            xenditwallet = await Ewallet.LinkAja( "linkaja-ewallet-12345", 3000, "https://www.youtube.com/", "https://www.youtube.com/", "085810450090", listitem);
            xenditwallet = await Ewallet.GetPayment("linkaja-ewallet-12345", XenditEWalletType.LINKAJA);

            xinditInvoice = await Invoice.CreateInvoices("My-Invoice-1234567890",20000, "dkurniawan28@gmail.com","testing");
            xinditInvoice = await Invoice.GetInvoiceById(xinditInvoice.Id);
            xinditInvoice = await Invoice.Expired(xinditInvoice.Id);
            xinditInvoice2 = await Invoice.GetAllInvoice(option);
            xenditDisb = await disb.Disbursmen("kusnur", 500000, XenditDisbursementBankCode.BCA, "dedy kurniawan", "123765", "tester dedy");
            return new string[] {"" };
        }



        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }
}
