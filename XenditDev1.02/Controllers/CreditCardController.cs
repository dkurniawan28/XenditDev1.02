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
    public class CreditCardController : ControllerBase
    {
        [HttpPost]
        [Route("api/CreateCC")]
        public async Task<IActionResult> Posts([FromBody] object value)
        {
            PaymentGatewayContext entities = new PaymentGatewayContext();

            JObject json = JObject.Parse("{\r\n    \"Status\":\"Error\", \r\n\"StatusCode\":\"400\"\r\n,\"Description\":\"Duplicate Externalid\"\r\n}");

            string token = HttpContext.Session.GetString("token");
            var userid = HttpContext.Session.GetInt32("userid");
            int idm = userid.GetValueOrDefault();

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            RequestCC requestCC = new RequestCC();
            requestCC = JsonConvert.DeserializeObject<RequestCC>(value.ToString());


            //  XenditEWalletCreatePaymentResponse respon = new XenditEWalletCreatePaymentResponse();
            Request.Headers.TryGetValue("Content-Type", out var ContentType_);
            Request.Headers.TryGetValue("Accept", out var Accept_);
            Request.Headers.TryGetValue("Authorization", out var Authorization_);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            string authBearer = Authorization_.ToString().Replace("Bearer ", "");
            authBearer = authBearer.Replace("{", "");
            authBearer = authBearer.Replace("}", "");
            string auth = token;
            int idtransaksipayment = 0;

            string externalid = "";

            try
            {
                externalid = requestCC.external_id;
            }
            catch (System.Exception e)
            {
                externalid = "";
            }
            int amount = (int)requestCC.amount;



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

                CreditCard chargeCC = new CreditCard();


                String createCC = chargeCC.ChargeCC(requestCC);
                CreateCC jobj = JsonConvert.DeserializeObject<CreateCC>(createCC);



                dynamic dynamicObjectdatum = JsonConvert.DeserializeObject(createCC);
                var dt = @"" + dynamicObjectdatum;
                JObject jsonpar = JObject.Parse(dt);

                var bd = (string)dt.Replace("\r\n", "");

                String content3 = bd;

                string sJSONResponsedatum2 = JsonConvert.SerializeObject(requestCC);

                dynamic dynamicObjectdatum2 = JsonConvert.DeserializeObject(sJSONResponsedatum2);


                var dt2 = @"" + dynamicObjectdatum2;

                var bd2 = (string)dt2.Replace("\r\n", "");

                String content2 = bd2;

                DateTime Now = DateTime.Now;
                InsertDB insertDB_ = new InsertDB();
                UpdateDb updateDb = new UpdateDb();

                String statusCC = jobj.status;
                int idreslog = insertDB_.InsertLog(content2, content3, "http://192.168.0.8/xenditapi/api/CreateCC", Now);
                if (idtransaksipayment > 0 && statusCC == "CAPTURED")
                {

                    try
                    {
                        insertDB_.InsertDetailCC(idtransaksipayment, jobj.id, jobj.status, jobj.authorized_amount, jobj.capture_amount, jobj.currency, jobj.credit_card_token_id,
                            jobj.business_id, jobj.merchant_id, jobj.merchant_reference_code, jobj.external_id, jobj.eci, jobj.charge_type, jobj.masked_card_number, jobj.card_brand,
                            jobj.card_type, jobj.xid, jobj.cavv, jobj.descriptor, jobj.authorization_id, jobj.bank_reconciliation_id, jobj.issuing_bank_name, jobj.client_id, jobj.cvn_code,
                            jobj.approval_code, jobj.created, "{}");
                        updateDb.updateTransaksiPaymentCC(idtransaksipayment);


                        return Ok(jsonpar);
                    }
                    catch (System.Exception e)
                    {
                        return Ok(json);
                    }
                }
                else
                {
                    JObject json2 = JObject.Parse("{\r\n    \"Status\":\"Error\", \r\n\"StatusCode\":\"400\"\r\n,\"Description\":\"Failed Debit CC\"\r\n}");
                    return Ok(json2);
                }
            }
            else
            {
                return Ok(json);
            }
        }
    }
}