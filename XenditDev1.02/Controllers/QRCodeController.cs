using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XenditDev1._02.Function;
using XenditDev1._02.ModelsNew;

namespace XenditDev1._02.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class QRCodeController : ControllerBase
    {
        PaymentGatewayContext entities = new PaymentGatewayContext();


        [HttpPost]
        [Route("api/CreateQR")]
        public async Task<IActionResult> Posts([FromBody] object value)
        {


            JObject json = JObject.Parse("{\r\n    \"Status\":\"Error\", \r\n\"StatusCode\":\"400\"\r\n,\"Description\":\"Duplicate Externalid\"\r\n}");
            JObject json2 = JObject.Parse("{\r\n    \"Status\":\"Error\", \r\n\"StatusCode\":\"400\"\r\n,\"Description\":\"Wrong Request\"\r\n}");
            string token = HttpContext.Session.GetString("token");
            var userid = HttpContext.Session.GetInt32("userid");
            int idm = userid.GetValueOrDefault();

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            RequestQR Request_ = new RequestQR();
            Request_ = JsonConvert.DeserializeObject<RequestQR>(value.ToString());

            Request.Headers.TryGetValue("Content-Type", out var ContentType_);
            Request.Headers.TryGetValue("Accept", out var Accept_);
            Request.Headers.TryGetValue("Authorization", out var Authorization_);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            string authBearer = Authorization_.ToString().Replace("Bearer ", "");
            authBearer = authBearer.Replace("{", "");
            authBearer = authBearer.Replace("}", "");
            string auth = token;


            string externalid = "";

            try
            {
                externalid = Request_.external_id;
            }
            catch (System.Exception e)
            {
                externalid = "";
            }

          

            int amount = (int)Request_.amount;

            string tipe = Request_.type;
            string callback_url = Request_.callback_url;

            CreateQR createQRs = new CreateQR();
            int idtransaksipayment = 0;
            if (authBearer == auth && externalid != "")
            {
                try
                {
                    idtransaksipayment = entities.TransaksiPayment.Where(p => p.ExternalId == externalid).ToList()[0].IdTransaksiPayment;
                }
                catch (System.Exception e)
                {
                    idtransaksipayment = 0;
                }

                QRCode QRCode_ = new QRCode();

               
                    String createQRcode = QRCode_.CreateQR(Request_);
                CreateQR jobj = JsonConvert.DeserializeObject<CreateQR>(createQRcode);

                dynamic dynamicObjectdatum = JsonConvert.DeserializeObject(createQRcode);
                var dt = @"" + dynamicObjectdatum;

                JObject jsonpar = JObject.Parse(dt);

                var bd = (string)dt.Replace("\r\n", "");

                String content3 = bd;
                
                string sJSONResponsedatum2 = JsonConvert.SerializeObject(Request_);

                dynamic dynamicObjectdatum2 = JsonConvert.DeserializeObject(sJSONResponsedatum2);


                var dt2 = @"" + dynamicObjectdatum2;

                var bd2 = (string)dt2.Replace("\r\n", "");

                String content2 = bd2;

                DateTime Now = DateTime.Now;
                InsertDB insertDB_ = new InsertDB();
                int idreslog = insertDB_.InsertLog(content2, content3, "http://192.168.0.8/xenditapi/api/CreateQR", Now);
                string callback = Convert.ToString(jobj.callback_url);

                if (idtransaksipayment>0) {

                    try
                    {
                        //  int idtransaksipayment = insertDB_.InsertTransaksiPayment(jobj.id, externalid, "", idm, "", "", jobj.amount,"", jobj.description,Now, "", jobj.status.ToString(),0, 0,jobj.created,jobj.updated, "IDR", Now);
                        //  insertDB_.InsertDetailLogTransaksi(idreslog, idtransaksipayment, "New QR", Now);

                        insertDB_.InsertDetailLQr(idtransaksipayment, jobj.id, jobj.amount, jobj.description, jobj.qr_string, callback, jobj.type, jobj.status, jobj.created, jobj.updated, "{}");


                        return Ok(jsonpar);
                    }
                    catch (System.Exception e)
                    {
                        return Ok(json2);
                    }

                }
                else
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
        [Route("api/GetQR")]
        public async Task<IActionResult> Posts1([FromBody] object value)
        {


            JObject json = JObject.Parse("{\r\n    \"Status\":\"Error\", \r\n\"StatusCode\":\"400\"\r\n,\"Description\":\"Duplicate Externalid\"\r\n}");
            JObject json2 = JObject.Parse("{\r\n    \"Status\":\"Error\", \r\n\"StatusCode\":\"400\"\r\n,\"Description\":\"Wrong Request\"\r\n}");
            string token = HttpContext.Session.GetString("token");
            var userid = HttpContext.Session.GetInt32("userid");
            int idm = userid.GetValueOrDefault();

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            RequestQR Request_ = new RequestQR();
            Request_ = JsonConvert.DeserializeObject<RequestQR>(value.ToString());


            //  XenditEWalletCreatePaymentResponse respon = new XenditEWalletCreatePaymentResponse();
            Request.Headers.TryGetValue("Content-Type", out var ContentType_);
            Request.Headers.TryGetValue("Accept", out var Accept_);
            Request.Headers.TryGetValue("Authorization", out var Authorization_);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            string authBearer = Authorization_.ToString().Replace("Bearer ", "");
            authBearer = authBearer.Replace("{", "");
            authBearer = authBearer.Replace("}", "");
            string auth = token;


            string externalid = "";

            try
            {
                externalid = Request_.external_id;
            }
            catch (System.Exception e)
            {
                externalid = "";
            }
            int amount = (int)Request_.amount;

            string tipe = Request_.type;
            string callback_url = Request_.callback_url;

            RequestQR Requestqr = new RequestQR();
            Requestqr.external_id = externalid;
            Requestqr.type = tipe;
            Requestqr.callback_url = callback_url;
            Requestqr.amount = amount;
           
            CreateQR createQRs = new CreateQR();

            if (authBearer == auth && externalid != "")
            {

                QRCode QRCode_ = new QRCode();


                String createQRcode = QRCode_.StatusQR(externalid);


                dynamic dynamicObjectdatum = JsonConvert.DeserializeObject(createQRcode);
                var dt = @"" + dynamicObjectdatum;

                JObject jsonpar = JObject.Parse(dt);

                var bd = (string)dt.Replace("\r\n", "");

                String content3 = bd;

                var xx = createQRcode.ToList();

                string sJSONResponsedatum2 = JsonConvert.SerializeObject(Request_);

                dynamic dynamicObjectdatum2 = JsonConvert.DeserializeObject(sJSONResponsedatum2);


                var dt2 = @"" + dynamicObjectdatum2;

                var bd2 = (string)dt2.Replace("\r\n", "");

                String content2 = bd2;

                DateTime Now = DateTime.Now;
                InsertDB insertDB_ = new InsertDB();
                int idreslog = insertDB_.InsertLog(content2, content3, "http://192.168.0.8/xenditapi/api/GetQR", Now);
               
                    return Ok(jsonpar);
                
               

            }
            else
            {

                return Ok(json);
            }

        }

    }
}