using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xendit.ApiClient.Constants;
using Xendit.ApiClient.VirtualAccount;
using XenditDev1._02.Function;
using XenditDev1._02.ModelsNew;

namespace XenditDev1._02.Controllers
{
   // [Route("api/[controller]")]
    [ApiController]
    public class VaController : ControllerBase
    {

        JObject json = JObject.Parse("{\r\n    \"Status\":\"Error\", \r\n\"StatusCode\":\"400\"\r\n,\"Description\":\"Duplicate Externalid\"\r\n}");

        public XenditVACreateResponse xenditva = new XenditVACreateResponse();
        VirtualAccount VirtualAccount = new VirtualAccount();
        PaymentGatewayContext entities = new PaymentGatewayContext();
        [HttpPost]
        [Route("api/CreateVa")]
        public async Task<IActionResult> Posts([FromBody] object value)
        {


            string token = HttpContext.Session.GetString("token");
            
            var userid = HttpContext.Session.GetInt32("userid");
            int idm = userid.GetValueOrDefault();
            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            VaRequest Request_ = new VaRequest();
            Request_ = JsonConvert.DeserializeObject<VaRequest>(value.ToString());


            XenditVACreateResponse respon = new XenditVACreateResponse();
            Request.Headers.TryGetValue("Content-Type", out var ContentType_);
            Request.Headers.TryGetValue("Accept", out var Accept_);
            Request.Headers.TryGetValue("Authorization", out var Authorization_);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            string authBearer = Authorization_.ToString().Replace("Bearer ", "");
            authBearer = authBearer.Replace("{", "");
            authBearer = authBearer.Replace("}", "");
            string auth = token;
            int bank = 0;
            var codeBank = XenditVABankCode.MANDIRI;
            switch (bank)
            {
                case 1:
                    // mandiri
                    codeBank = XenditVABankCode.MANDIRI;
                    break;
                case 2:
                    // bca
                    codeBank = XenditVABankCode.BCA;
                    break;
                case 3:
                    // permata
                    codeBank = XenditVABankCode.PERMATA;
                    break;
                case 4:
                    // code BRI
                    codeBank = XenditVABankCode.BRI;
                    break;
                case 5:
                    // code BNI
                    codeBank = XenditVABankCode.BNI;
                    break;
            }

            string externalid = "";
            string nama = Request_.Name;
            try
            {
                externalid = Request_.ExternalId;
            }
            catch (System.Exception e)
            {
                externalid = "";
            }
            int banks = (int)Request_.Bank;
            string vanumber = Request_.VirtualAccountNumber;
            decimal amount = (decimal)Request_.Amount;
            ResponseStatusVa resmessage = new ResponseStatusVa();


            if (authBearer == auth && externalid !="")
            {
                xenditva = await VirtualAccount.VaAsync(nama,banks, vanumber,externalid, amount);

                string sJSONResponsedatum = JsonConvert.SerializeObject(xenditva);

                dynamic dynamicObjectdatum = JsonConvert.DeserializeObject(sJSONResponsedatum);
                var dt = @"" + dynamicObjectdatum;

                var bd = (string)dt.Replace("\r\n", "");

                String content3 = bd;


                string sJSONResponsedatum2 = JsonConvert.SerializeObject(Request_);

                dynamic dynamicObjectdatum2 = JsonConvert.DeserializeObject(sJSONResponsedatum2);
                var dt2 = @"" + dynamicObjectdatum2;

                var bd2 = (string)dt2.Replace("\r\n", "");

                String content2 = bd2;

                DateTime Now = DateTime.Now;
                InsertDB insertDB_ = new InsertDB();
                int idreslog = insertDB_.InsertLog(content2, content3, "https://localhost:44379/api/CreateVa", Now);



                try
                {
                    int idtransaksipayment = insertDB_.InsertTransaksiPayment(xenditva.Id, externalid, "", idm, xenditva.Name, "", xenditva.ExpectedAmount, "", xenditva.Description, xenditva.ExpirationDate, "", xenditva.Status.ToString(), 0, 0, Now, Now, xenditva.Currency, Now);
                    insertDB_.InsertDetailLogTransaksi(idreslog, idtransaksipayment, xenditva.Description, Now);
                    return Ok(xenditva);
                }
                catch (System.Exception e)
                {
                    return Ok(json);
                }

              
            }
            else
            {
                return Ok(json);
            }

           
        }

        [HttpPost]
        [Route("api/getVA")]
        public async Task<IActionResult> Posts2([FromBody] object value)
        {


            string token = HttpContext.Session.GetString("token");

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            VaRequestid Request_ = new VaRequestid();
            Request_ = JsonConvert.DeserializeObject<VaRequestid>(value.ToString());


            XenditVACreateResponse respon = new XenditVACreateResponse();
            Request.Headers.TryGetValue("Content-Type", out var ContentType_);
            Request.Headers.TryGetValue("Accept", out var Accept_);
            Request.Headers.TryGetValue("Authorization", out var Authorization_);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            string authBearer = Authorization_.ToString().Replace("Bearer ", "");
            authBearer = authBearer.Replace("{", "");
            authBearer = authBearer.Replace("}", "");
            string auth = token;
           
            ResponseStatusVa resmessage = new ResponseStatusVa();
          


            string id = "";

            try
            {
                id = Request_.Id;
            }
            catch (System.Exception e)
            {
                id = "";
            }

            if (authBearer == auth && id!="")
            {
                xenditva = await VirtualAccount.Vacek(id);

                string sJSONResponsedatum = JsonConvert.SerializeObject(xenditva);

                dynamic dynamicObjectdatum = JsonConvert.DeserializeObject(sJSONResponsedatum);
                var dt = @"" + dynamicObjectdatum;

                var bd = (string)dt.Replace("\r\n", "");

                String content3 = bd;


                string sJSONResponsedatum2 = JsonConvert.SerializeObject(Request_);

                dynamic dynamicObjectdatum2 = JsonConvert.DeserializeObject(sJSONResponsedatum2);
                var dt2 = @"" + dynamicObjectdatum2;

                var bd2 = (string)dt2.Replace("\r\n", "");

                String content2 = bd2;

                DateTime Now = DateTime.Now;
                InsertDB insertDB_ = new InsertDB();
                int idreslog = insertDB_.InsertLog(content2, content3, "https://localhost:44379/api/getVA", Now);

                return Ok(xenditva);
            }
            else
            {
                return Ok(json);
            }

          
        }

        [HttpPost]
        [Route("api/ExpiredVa")]
        public async Task<IActionResult> Posts3([FromBody] object value)
        {


            string token = HttpContext.Session.GetString("token");

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            VaRequestid Request_ = new VaRequestid();
            Request_ = JsonConvert.DeserializeObject<VaRequestid>(value.ToString());


            XenditVACreateResponse respon = new XenditVACreateResponse();
            Request.Headers.TryGetValue("Content-Type", out var ContentType_);
            Request.Headers.TryGetValue("Accept", out var Accept_);
            Request.Headers.TryGetValue("Authorization", out var Authorization_);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            string authBearer = Authorization_.ToString().Replace("Bearer ", "");
            authBearer = authBearer.Replace("{", "");
            authBearer = authBearer.Replace("}", "");
            string auth = token;

            ResponseStatusVa resmessage = new ResponseStatusVa();
            string id = "";

            try
            {
                id = Request_.Id;
            }
            catch (System.Exception e)
            {
                id = "";
            }

            if (authBearer == auth && id!="")
            {
                xenditva = await VirtualAccount.VaExpired(id);
                string sJSONResponsedatum = JsonConvert.SerializeObject(xenditva);

                dynamic dynamicObjectdatum = JsonConvert.DeserializeObject(sJSONResponsedatum);
                var dt = @"" + dynamicObjectdatum;

                var bd = (string)dt.Replace("\r\n", "");

                String content3 = bd;


                string sJSONResponsedatum2 = JsonConvert.SerializeObject(Request_);

                dynamic dynamicObjectdatum2 = JsonConvert.DeserializeObject(sJSONResponsedatum2);
                var dt2 = @"" + dynamicObjectdatum2;

                var bd2 = (string)dt2.Replace("\r\n", "");

                String content2 = bd2;

                DateTime Now = DateTime.Now;
                InsertDB insertDB_ = new InsertDB();
                int idreslog = insertDB_.InsertLog(content2, content3, "https://localhost:44379/api/getVA", Now);

                return Ok(xenditva);
            }
            else
            {
                return Ok(json);
            }

         
        }
        public class VaRequest
        {

          
            public string ExternalId { get; set; }
            public int Bank { get; set; }
            public string Name { get; set; }
            public decimal Amount { get; set; }
            public string VirtualAccountNumber { get; set; }

        }

        public class VaRequestid
        {
            public string Id { get; set; }
        }


        public class ResponseStatusVa
        {
            public string responseCode { get; set; }
            public string responseMessage { get; set; }
            public XenditVACreateResponse data { get; set; }
        }
    }
}