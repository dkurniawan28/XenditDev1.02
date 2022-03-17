using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Xendit.ApiClient.Constants;
using Xendit.ApiClient.EWallet;
using XenditDev1._02.Function;
using XenditDev1._02.ModelsNew;

namespace XenditDev1._02.Controllers
{
   // [Route("api/[controller]")]
    [ApiController]
    public class EwalletController : ControllerBase
    {
        PaymentGatewayContext entities = new PaymentGatewayContext();
        JObject json = JObject.Parse("{\r\n    \"Status\":\"Error\", \r\n\"StatusCode\":\"400\"\r\n,\"Description\":\"Duplicate Externalid\"\r\n}");

        XenditEWalletCreatePaymentResponse XenditWallet = new XenditEWalletCreatePaymentResponse();
        Ewallet Ewallet = new Ewallet();

        //[HttpPost]
        //[Route("api/Ovo")]
        //public async Task<IActionResult> Posts([FromBody] object value)
        //{


        //    string token = HttpContext.Session.GetString("token");
        //    var userid = HttpContext.Session.GetInt32("userid");
        //    int idm = userid.GetValueOrDefault();

        //    var jsonString = JsonConvert.DeserializeObject(value.ToString());
        //    jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
        //    OvoRequest Request_ = new OvoRequest();
        //    Request_ = JsonConvert.DeserializeObject<OvoRequest>(value.ToString());


        //    XenditEWalletCreatePaymentResponse respon = new XenditEWalletCreatePaymentResponse();
        //    Request.Headers.TryGetValue("Content-Type", out var ContentType_);
        //    Request.Headers.TryGetValue("Accept", out var Accept_);
        //    Request.Headers.TryGetValue("Authorization", out var Authorization_);
        //    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

        //    string authBearer = Authorization_.ToString().Replace("Bearer ", "");
        //    authBearer = authBearer.Replace("{", "");
        //    authBearer = authBearer.Replace("}", "");
        //    string auth = token;


        //    string externalid = "";

        //    try
        //    {
        //        externalid = Request_.ExternalId;
        //    }
        //    catch (System.Exception e)
        //    {
        //        externalid = "";
        //    }
        //    decimal amount = (decimal)Request_.Amount;

        //    string phone = Request_.Phone;

        //    ResponseStatusPayment resmessage = new ResponseStatusPayment();

        //    int idtransaksipayment = 0;
        //    if (authBearer == auth && externalid!="")
        //    {

        //        try
        //        {
        //            idtransaksipayment = entities.TransaksiPayment.Where(p => p.ExternalId == externalid).ToList()[0].IdTransaksiPayment;
        //        }
        //        catch (System.Exception e)
        //        {
        //            idtransaksipayment = 0;
        //        }
        //        XenditWallet = await Ewallet.Ovo(externalid, amount, phone);
        //        string sJSONResponsedatum = JsonConvert.SerializeObject(XenditWallet);

        //        dynamic dynamicObjectdatum = JsonConvert.DeserializeObject(sJSONResponsedatum);
        //        var dt = @"" + dynamicObjectdatum;

        //        var bd = (string)dt.Replace("\r\n", "");

        //        String content3 = bd;


        //        string sJSONResponsedatum2 = JsonConvert.SerializeObject(Request_);

        //        dynamic dynamicObjectdatum2 = JsonConvert.DeserializeObject(sJSONResponsedatum2);
        //        var dt2 = @"" + dynamicObjectdatum2;

        //        var bd2 = (string)dt2.Replace("\r\n", "");

        //        String content2 = bd2;

        //        DateTime Now = DateTime.Now;
        //        InsertDB insertDB_ = new InsertDB();
        //        int idreslog = insertDB_.InsertLog(content2, content3, "http://192.168.0.8/xenditapi/api/Ovo", Now);


        //        try
        //        {
        //            //int idtransaksipayment = insertDB_.InsertTransaksiPayment(XenditWallet.BusinessId, externalid, "", idm, "","", XenditWallet.Amount, "", "New OVO", XenditWallet.ExpirationDate,"", XenditWallet.Status.ToString(), 0, 0,Now, Now, "IDR", Now);
        //            //insertDB_.InsertDetailLogTransaksi(idreslog, idtransaksipayment, "New OVO", Now);
        //            DetailWalletPayment detail_ = new DetailWalletPayment();
        //            detail_.IdTransaksiPayment = idtransaksipayment;
        //            detail_.IdXendit = XenditWallet.ExternalId;
        //            detail_.ExternalId = XenditWallet.ExternalId;
        //            detail_.BusinessId = XenditWallet.BusinessId;
        //            detail_.Phone = XenditWallet.Phone;
        //            detail_.Amount = XenditWallet.Amount;
        //            detail_.Status = XenditWallet.Status.ToString();
        //            detail_.CheckoutUrl = XenditWallet.CheckoutUrl;
        //            detail_.TransactionDate = XenditWallet.TransactionDate;
        //            detail_.PaymentTimestamp = XenditWallet.PaymentTimestamp;
        //            detail_.ExpirationDate = XenditWallet.ExpirationDate;
        //            detail_.ExpiredAt = XenditWallet.ExpirationDate;
        //            detail_.Created = XenditWallet.CreatedAt;
        //            detail_.EwalletType = XenditWallet.EWalletType.ToString();
        //            entities.DetailWalletPayment.Add(detail_);
        //            entities.SaveChanges();
        //            return Ok(XenditWallet);
        //        }
        //        catch (System.Exception e)
        //        {
        //            return Ok(json);
        //        }

        //    }
        //    else
        //    {
        //        return Ok(json);
        //    }

        //}

        [HttpPost]
        [Route("api/Ovo")]
        public async Task<IActionResult> Posts([FromBody] object value)
        {


            PaymentGatewayContext entities = new PaymentGatewayContext();

            JObject json = JObject.Parse("{\r\n    \"Status\":\"Error\", \r\n\"StatusCode\":\"400\"\r\n,\"Description\":\"Duplicate Externalid\"\r\n}");

            string token = HttpContext.Session.GetString("token");
            var userid = HttpContext.Session.GetInt32("userid");
            int idm = userid.GetValueOrDefault();

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            RequestOvo requestCC = new RequestOvo();
            requestCC = JsonConvert.DeserializeObject<RequestOvo>(value.ToString());


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
                externalid = requestCC.reference_id;
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

                CreateOVO CreateOVO_ = new CreateOVO();


                String createovo = CreateOVO_.ChargeOVO(requestCC);
                OvoRespon jobj = JsonConvert.DeserializeObject<OvoRespon>(createovo);

                dynamic dynamicObjectdatum = JsonConvert.DeserializeObject(createovo);
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
                int idreslog = insertDB_.InsertLog(content2, content3, "http://192.168.0.8/xenditapi/api/Ovo", Now);






                try
                {
                    //int idtransaksipayment = insertDB_.InsertTransaksiPayment(XenditWallet.BusinessId, externalid, "", idm, "","", XenditWallet.Amount, "", "New OVO", XenditWallet.ExpirationDate,"", XenditWallet.Status.ToString(), 0, 0,Now, Now, "IDR", Now);
                    //insertDB_.InsertDetailLogTransaksi(idreslog, idtransaksipayment, "New OVO", Now);
                    DetailWalletPayment detail_ = new DetailWalletPayment();
                    detail_.IdTransaksiPayment = idtransaksipayment;
                    detail_.IdXendit = jobj.id;
                    detail_.ExternalId = jobj.reference_id;
                    detail_.BusinessId = jobj.business_id;
                    detail_.Phone = jobj.channel_properties.mobile_number;
                    detail_.Amount = jobj.charge_amount;
                    detail_.Status = jobj.status.ToString();
                    detail_.CheckoutUrl = jobj.callback_url;
                    detail_.TransactionDate = jobj.created;
                    detail_.PaymentTimestamp = jobj.created;
                    detail_.ExpirationDate = jobj.created;
                    detail_.ExpiredAt = jobj.created;
                    detail_.Created = jobj.created;
                    detail_.EwalletType = "OVO";
                    entities.DetailWalletPayment.Add(detail_);
                    entities.SaveChanges();
                    return Ok(jsonpar);
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
        [Route("api/Dana")]
        public async Task<IActionResult> Posts1([FromBody] object value)
        {


            string token = HttpContext.Session.GetString("token");
            var userid = HttpContext.Session.GetInt32("userid");
            int idm = userid.GetValueOrDefault();
            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            DanaRequest Request_ = new DanaRequest();
            Request_ = JsonConvert.DeserializeObject<DanaRequest>(value.ToString());


            XenditEWalletCreatePaymentResponse respon = new XenditEWalletCreatePaymentResponse();
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
                externalid = Request_.ExternalId;
            }
            catch (System.Exception e)
            {
                externalid = "";
            }
            decimal amount = (decimal)Request_.Amount;

            string callback = Request_.CallbackUrl;
            string redirek = Request_.RedirectUrl;
            ResponseStatusPayment resmessage = new ResponseStatusPayment();
            int idtransaksipayment = 0;

            if (authBearer == auth && externalid!="")
            {

                try
                {
                    idtransaksipayment = entities.TransaksiPayment.Where(p => p.ExternalId == externalid).ToList()[0].IdTransaksiPayment;
                }
                catch (System.Exception e)
                {
                    idtransaksipayment = 0;
                }
                XenditWallet = await Ewallet.Dana(externalid, amount,callback,redirek);

                string sJSONResponsedatum = JsonConvert.SerializeObject(XenditWallet);

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
                int idreslog = insertDB_.InsertLog(content2, content3, "http://192.168.0.8/xenditapi/api/Dana", Now);


                try
                {
                    DetailWalletPayment detail_ = new DetailWalletPayment();
                    detail_.IdTransaksiPayment = idtransaksipayment;
                    detail_.IdXendit = XenditWallet.ExternalId;
                    detail_.ExternalId = XenditWallet.ExternalId;
                    detail_.BusinessId = XenditWallet.BusinessId;
                    detail_.Phone = XenditWallet.Phone;
                    detail_.Amount = XenditWallet.Amount;
                    detail_.Status = XenditWallet.Status.ToString();
                    detail_.CheckoutUrl = XenditWallet.CheckoutUrl;
                    detail_.TransactionDate = XenditWallet.TransactionDate;
                    detail_.PaymentTimestamp = XenditWallet.PaymentTimestamp;
                    detail_.ExpirationDate = XenditWallet.ExpirationDate;
                    detail_.ExpiredAt = XenditWallet.ExpirationDate;
                    detail_.Created = XenditWallet.CreatedAt;
                    detail_.EwalletType = XenditWallet.EWalletType.ToString();
                    entities.DetailWalletPayment.Add(detail_);
                    entities.SaveChanges();
                    return Ok(XenditWallet);
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
        [Route("api/LinkAja")]
        public async Task<IActionResult> Posts2([FromBody] object value)
        {


            string token = HttpContext.Session.GetString("token");
            var userid = HttpContext.Session.GetInt32("userid");
            int idm = userid.GetValueOrDefault();
            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            LinkAjaRequest Request_ = new LinkAjaRequest();
            Request_ = JsonConvert.DeserializeObject<LinkAjaRequest>(value.ToString());


            XenditEWalletCreatePaymentResponse respon = new XenditEWalletCreatePaymentResponse();
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
                externalid = Request_.ExternalId;
            }
            catch (System.Exception e)
            {
                externalid = "";
            }
            decimal amount = (decimal)Request_.Amount;

            string callback = Request_.CallbackUrl;
            string redirek = Request_.RedirectUrl;
            string phone = Request_.Phone;
            List<XenditEWalletCreateLinkAjaPaymentRequestItem> item = Request_.Items;
            ResponseStatusPayment resmessage = new ResponseStatusPayment();
            int idtransaksipayment = 0;

            if (authBearer == auth && externalid!="")
            {

                try
                {
                    idtransaksipayment = entities.TransaksiPayment.Where(p => p.ExternalId == externalid).ToList()[0].IdTransaksiPayment;
                }
                catch (System.Exception e)
                {
                    idtransaksipayment = 0;
                }
                XenditWallet = await Ewallet.LinkAja(externalid, amount, callback, redirek,phone,item);
                string sJSONResponsedatum = JsonConvert.SerializeObject(XenditWallet);

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
                int idreslog = insertDB_.InsertLog(content2, content3, "http://192.168.0.8/xenditapi/api/LinkAja", Now);


                try
                {
                    DetailWalletPayment detail_ = new DetailWalletPayment();
                    detail_.IdTransaksiPayment = idtransaksipayment;
                    detail_.IdXendit = XenditWallet.ExternalId;
                    detail_.ExternalId = XenditWallet.ExternalId;
                    detail_.BusinessId = XenditWallet.BusinessId;
                    detail_.Phone = XenditWallet.Phone;
                    detail_.Amount = XenditWallet.Amount;
                    detail_.Status = XenditWallet.Status.ToString();
                    detail_.CheckoutUrl = XenditWallet.CheckoutUrl;
                    detail_.TransactionDate = XenditWallet.TransactionDate;
                    detail_.PaymentTimestamp = XenditWallet.PaymentTimestamp;
                    detail_.ExpirationDate = XenditWallet.ExpirationDate;
                    detail_.ExpiredAt = XenditWallet.ExpirationDate;
                    detail_.Created = XenditWallet.CreatedAt;
                    detail_.EwalletType = XenditWallet.EWalletType.ToString();
                    entities.DetailWalletPayment.Add(detail_);
                    entities.SaveChanges();
                    return Ok(XenditWallet);
                
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
        [Route("api/GetPaymentByTipe")]
        public async Task<IActionResult> Posts3([FromBody] object value)
        {


            string token = HttpContext.Session.GetString("token");

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            GetPaymentRequest Request_ = new GetPaymentRequest();
            Request_ = JsonConvert.DeserializeObject<GetPaymentRequest>(value.ToString());


            XenditEWalletCreatePaymentResponse respon = new XenditEWalletCreatePaymentResponse();
            Request.Headers.TryGetValue("Content-Type", out var ContentType_);
            Request.Headers.TryGetValue("Accept", out var Accept_);
            Request.Headers.TryGetValue("Authorization", out var Authorization_);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            string authBearer = Authorization_.ToString().Replace("Bearer ", "");
            authBearer = authBearer.Replace("{", "");
            authBearer = authBearer.Replace("}", "");
            string auth = token;


            string externalid = Request_.ExternalId;
            XenditEWalletType tipe = Request_.Tipe;

      
            ResponseStatusPayment resmessage = new ResponseStatusPayment();


            if (authBearer == auth)
            {
                XenditWallet = await Ewallet.GetPayment(externalid, tipe);

                string sJSONResponsedatum = JsonConvert.SerializeObject(XenditWallet);

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
                int idreslog = insertDB_.InsertLog(content2, content3, "http://192.168.0.8/xenditapi/api/GetPaymentByTipe", Now);
                return Ok(XenditWallet);
            }
            else
            {
                return Ok(json);
            }

         
        }

        [HttpGet("api/GetOvo/{id}")]
        public ActionResult<string> Get(string id)
        {
            string token = HttpContext.Session.GetString("token");
            var userid = HttpContext.Session.GetInt32("userid");
            int idm = userid.GetValueOrDefault();
            int idmerchan = 0;
            PaymentGatewayContext entities = new PaymentGatewayContext();

            JObject json = JObject.Parse("{\r\n    \"Status\":\"Error\", \r\n\"StatusCode\":\"400\"\r\n,\"Description\":\"Duplicate Externalid\"\r\n}");
            Request.Headers.TryGetValue("Content-Type", out var ContentType_);
            Request.Headers.TryGetValue("Accept", out var Accept_);
            Request.Headers.TryGetValue("Authorization", out var Authorization_);
           

            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            string authBearer = Authorization_.ToString().Replace("Bearer ", "");
            authBearer = authBearer.Replace("{", "");
            authBearer = authBearer.Replace("}", "");
            string auth = token;
            DateTime Now = DateTime.Now;
            InsertDB insertDb = new InsertDB();
            UpdateDb updateDb = new UpdateDb();
            if (authBearer == auth)
            {
                CreateOVO CreateOVO_ = new CreateOVO();
                String createovo = CreateOVO_.GetOVO(id);
                OvoRespon jobj = JsonConvert.DeserializeObject<OvoRespon>(createovo);
                string external_id = "";
                dynamic dynamicObjectdatum = JsonConvert.DeserializeObject(createovo);
                var dt = @"" + dynamicObjectdatum;

                JObject jsonpar = JObject.Parse(dt);

                var bd = (string)dt.Replace("\r\n", "");

                String content3 = bd;

                string status = jobj.status;
                int idtransaksipayment = 0;
                try
                {
                    idtransaksipayment = (int)entities.DetailWalletPayment.Where(p => p.IdXendit == id).ToList()[0].IdTransaksiPayment;
                }
                catch (System.Exception e)
                {
                    idtransaksipayment = 0;
                }

                try
                {
                    idmerchan = (int)entities.TransaksiPayment.Where(p => p.IdTransaksiPayment == idtransaksipayment).ToList()[0].IdMerchan;
                }
                catch (System.Exception e)
                {
                    idmerchan = 0;
                }

                try
                {
                    external_id = entities.TransaksiPayment.Where(p => p.IdTransaksiPayment == idtransaksipayment).ToList()[0].ExternalId;
                }
                catch (System.Exception e)
                {
                    external_id = "";
                }
                string statusfailed = "FAILED";
                string statussucces = "SUCCEEDED";
                ReqKamuBisas call2 = new ReqKamuBisas();
                
              

                if (idmerchan==1) {
                    if (status == "FAILED")
                    {
                        updateDb.updateTransaksiOvoFailed(idtransaksipayment);

                        insertDb.InsertLog("", content3, "http://192.168.0.8/xenditapi/api/GetOvo/" + id, Now);
                        return Ok(jsonpar);
                    }
                    else if (status == "SUCCEEDED" || status=="PAID" || status=="COMPLETED")
                    {
                        updateDb.updateTransaksiOvoSukses(idtransaksipayment);
                        insertDb.InsertLog("", content3, "http://192.168.0.8/xenditapi/api/GetOvo/" + id, Now);
                        return Ok(jsonpar);
                    }
                    else
                    {
                        insertDb.InsertLog("", content3, "http://192.168.0.8/xenditapi/api/GetOvo/" + id, Now);
                        return Ok(jsonpar);
                    }
                }
                else if (idmerchan==2)
                {
                    string urlsign = "http://asri.aspan.co.id/AspanAsuransi/Account/Login";

                    string username = "admin";
                    string password = "semogajaya@1993";
                    var clients = new RestClient(urlsign);

                    var request = new RestRequest(Method.POST);
                    JObject jObjectbody = new JObject();
                    jObjectbody.Add("username", username);
                    jObjectbody.Add("password", password);
                    request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

                    IRestResponse restResponse = clients.Execute(request);
                    string responses = restResponse.Content;
                    var jObject = JObject.Parse(restResponse.Content);
                    string cookvalue = "";
                    var sessionCookie = restResponse.Cookies.SingleOrDefault(x => x.Name == ".ASPXAUTH");
                    if (sessionCookie != null)
                    {
                        cookvalue = sessionCookie.Value;
                    }
                  

                    if (restResponse.StatusCode == HttpStatusCode.OK)
                    {
                        if (status == "FAILED")
                        {
                            call2.external_id = external_id;
                            call2.paymen_by = "OVO "+statusfailed;
                            call2.timestamp = Now;
                            call2.status = statusfailed;
                            string sJSONResponsedatum4 = JsonConvert.SerializeObject(call2);

                            dynamic dynamicObjectdatum4 = JsonConvert.DeserializeObject(sJSONResponsedatum4);


                            var dt4 = @"" + dynamicObjectdatum4;

                            var bd4 = (string)dt4.Replace("\r\n", "");

                            String content4 = bd4;
                            string url = "http://asri.aspan.co.id/AspanAsuransi/Services/View/AllTransactionVa/UpdateStatusVa";
                            var client = new RestClient(url);

                            var requestParam = new RestRequest(Method.POST);

                            requestParam.AddHeader("Content-Type", "application/json");
                            requestParam.AddHeader("Cookie", ".ASPXAUTH=" + cookvalue);
                            requestParam.AddParameter("application/json", content4, ParameterType.RequestBody);

                            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                            IRestResponse restResponseAspandigi = client.Execute(requestParam);

                            ResponseAspandigiss responstatus = new ResponseAspandigiss();

                            if (restResponseAspandigi.StatusCode == HttpStatusCode.OK)
                            {

                                string response = restResponseAspandigi.Content;
                                responstatus = JsonConvert.DeserializeObject<ResponseAspandigiss>(response);


                                string responCode = responstatus.StatusCode.ToString();

                                if (responCode == "200")
                                {
                                    
                                    insertDb.InsertLog("", content3, "http://192.168.0.8/xenditapi/api/GetOvo/" + id, Now);
                                    return Ok(jsonpar);
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
                        else if (status == "SUCCEEDED" || status == "PAID" || status == "COMPLETED")
                        {
                            call2.external_id = external_id;
                            call2.paymen_by = "OVO " + statussucces;
                            call2.timestamp = Now;
                            call2.status = statussucces;
                            string sJSONResponsedatum4 = JsonConvert.SerializeObject(call2);

                            dynamic dynamicObjectdatum4 = JsonConvert.DeserializeObject(sJSONResponsedatum4);


                            var dt4 = @"" + dynamicObjectdatum4;

                            var bd4 = (string)dt4.Replace("\r\n", "");

                            String content4 = bd4;
                            string url = "http://asri.aspan.co.id/AspanAsuransi/Services/View/AllTransactionVa/UpdateStatusVa";
                            var client = new RestClient(url);

                            var requestParam = new RestRequest(Method.POST);

                            requestParam.AddHeader("Content-Type", "application/json");
                            requestParam.AddHeader("Cookie", ".ASPXAUTH=" + cookvalue);
                            requestParam.AddParameter("application/json", content4, ParameterType.RequestBody);

                            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                            IRestResponse restResponseAspandigi = client.Execute(requestParam);

                            ResponseAspandigiss responstatus = new ResponseAspandigiss();

                            if (restResponseAspandigi.StatusCode == HttpStatusCode.OK)
                            {

                                string response = restResponseAspandigi.Content;
                                responstatus = JsonConvert.DeserializeObject<ResponseAspandigiss>(response);


                                string responCode = responstatus.StatusCode.ToString();

                                if (responCode == "200")
                                {

                                    insertDb.InsertLog("", content3, "http://192.168.0.8/xenditapi/api/GetOvo/" + id, Now);
                                    return Ok(jsonpar);
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
                        else
                        {
                            insertDb.InsertLog("", content3, "http://192.168.0.8/xenditapi/api/GetOvo/" + id, Now);
                            return Ok(jsonpar);
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
            else
            {
                return Ok(json);
            }

              
        }

       


    }



    public class OvoRequest
    {



        public string ExternalId { get; set; }


        public decimal Amount { get; set; }

       

        public string Phone { get; set; }

        



    }

    public class DanaRequest
    {



        public string ExternalId { get; set; }


        public decimal Amount { get; set; }

        public string CallbackUrl { get; set; }
        public string RedirectUrl { get; set; }

     

    }

    public class LinkAjaRequest
    {



        public string ExternalId { get; set; }


        public decimal Amount { get; set; }

        public string CallbackUrl { get; set; }
        public string RedirectUrl { get; set; }
        public string Phone { get; set; }
        public List<XenditEWalletCreateLinkAjaPaymentRequestItem> Items { get; set; }



    }

    public class GetPaymentRequest
    {



        public string ExternalId { get; set; }
        public XenditEWalletType Tipe { get; set; }


    }

    public class ResponseStatusPayment
    {
        public string responseCode { get; set; }
        public string responseMessage { get; set; }
        public XenditEWalletCreatePaymentResponse data { get; set; }
    }
    public class ResponseAspandigiss
    {

        public string Status { get; set; }
        public string StatusCode { get; set; }
    }

    public class ReqKamuBisas
    {
        public string external_id { get; set; }
        public string paymen_by { get; set; }
        public DateTime timestamp { get; set; }
        public string status { get; set; }
    }
}