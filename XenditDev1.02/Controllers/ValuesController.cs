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

            XenditVACreateResponse xenditVA = new XenditVACreateResponse();
            XenditBatchDisbursementCreateResponse xenditDisb = new XenditBatchDisbursementCreateResponse();

            xenditVA= await va.VaAsync("ded", 1, "9999000015", "VA_fixed-" + DateTime.Now, 5000000);

            xenditVA = await va.Vacek(xenditVA.Id);

            xenditVA = await va.VaExpired(xenditVA.Id);

            string[] email;
            email = new string[2] { "dkurniawan28@gmail.com", "jono@dsd.com"};


            xenditDisb= await disb.Disb("dedy test",500000,XenditDisbursementBankCode.BCA,"dedy kurniawan","12345678","tester dedy",email,"122232333");

            return new string[] { xenditDisb.Reference };
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
