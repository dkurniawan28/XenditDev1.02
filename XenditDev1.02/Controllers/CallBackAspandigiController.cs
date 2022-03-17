using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using XenditDev1._02.Function;
using XenditDev1._02.ModelsNew;

namespace XenditDev1._02.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class CallBackAspandigiController : ControllerBase
    {

        JObject sukses = JObject.Parse("{\r\n    \"Status\":\"Sukses\", \r\n\"StatusCode\":\"200\"\r\n}");
        JObject gagal = JObject.Parse("{\r\n    \"Status\":\"Gagal\", \r\n\"StatusCode\":\"400\"\r\n}");
        PaymentGatewayContext entities = new PaymentGatewayContext();

        [HttpPost]
        [Route("api/callback/aspanovopaid")]
        public async Task<IActionResult> AspanOvopaid([FromBody] object value)
        {
            string token = HttpContext.Session.GetString("token");

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            int idmerchan = 0;
            CallBack.OvoPaid call = new CallBack.OvoPaid();
            try
            {
                call = JsonConvert.DeserializeObject<CallBack.OvoPaid>(value.ToString());
                ReqAspandigi call2 = new ReqAspandigi();
                DateTime Now = DateTime.Now;
                call2.external_id = call.external_id;
                call2.paymen_by = "OVO";
                call2.timestamp = Now;
                call2.status = call.status;
                string sJSONResponsedatum2 = JsonConvert.SerializeObject(call);

                dynamic dynamicObjectdatum2 = JsonConvert.DeserializeObject(sJSONResponsedatum2);


                var dt2 = @"" + dynamicObjectdatum2;

                var bd2 = (string)dt2.Replace("\r\n", "");

                String content2 = bd2;

                string sJSONResponsedatum4 = JsonConvert.SerializeObject(call2);

                dynamic dynamicObjectdatum4 = JsonConvert.DeserializeObject(sJSONResponsedatum4);


                var dt4 = @"" + dynamicObjectdatum4;

                var bd4 = (string)dt4.Replace("\r\n", "");

                String content4 = bd4;


                try
                {
                    idmerchan = (int)entities.TransaksiPayment.Where(p => p.ExternalId == call.external_id).ToList()[0].IdMerchan;
                }
                catch (System.Exception e)
                {
                    idmerchan = 0;
                }
                String LinkCallback = entities.Merchant.Where(p => p.IdMerchant == idmerchan).ToList()[0].UrlCallBack;
                InsertDB insertDb = new InsertDB();
                UpdateDb updateDb = new UpdateDb();

                string extId = call.external_id;
                string paymentby = "OVO";

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
                string status = call2.status;

                if (restResponse.StatusCode == HttpStatusCode.OK)
                {
                    string url = LinkCallback;
                    var client = new RestClient(url);

                    var requestParam = new RestRequest(Method.POST);

                    requestParam.AddHeader("Content-Type", "application/json");
                    requestParam.AddHeader("Cookie", ".ASPXAUTH=" + cookvalue);
                    requestParam.AddParameter("application/json", content4, ParameterType.RequestBody);

                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                    IRestResponse restResponseAspandigi = client.Execute(requestParam);

                    ResponseAspandigi responstatus = new ResponseAspandigi();

                    if (restResponseAspandigi.StatusCode == HttpStatusCode.OK)
                    {

                        string response = restResponseAspandigi.Content;
                        responstatus = JsonConvert.DeserializeObject<ResponseAspandigi>(response);


                        string responCode = responstatus.StatusCode.ToString();

                        if (responCode == "200")
                        {

                            if (status == "EXPIRED")
                            {

                                updateDb.updateTransaksiInvoice(extId, paymentby, "EXPIRED");
                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/aspanovopaid", Now);
                                return Ok(sukses);
                            }
                            else if (status == "PENDING")
                            {

                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/aspanovopaid", Now);
                                return Ok(sukses);
                            }
                           
                            else 
                            {
                                updateDb.updateTransaksiInvoice(extId, paymentby, "PAID");
                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/aspanovopaid", Now);
                                return Ok(sukses);
                            }

                        }
                        else
                        {
                            return Ok(gagal);
                        }



                    }
                    else
                    {
                        return Ok(gagal);
                    }
                }
                else
                {
                    return Ok(gagal);
                }

            }
            catch (Exception ex)
            {
                return Ok(gagal);
            }


        }

        [HttpPost]
        [Route("api/callback/aspanvapaid")]
        public async Task<IActionResult> VaPaidAspans([FromBody] object value)
        {
            string token = HttpContext.Session.GetString("token");

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            int idmerchan = 0;

            CallBack.VaPaid call = new CallBack.VaPaid();
            try
            {
                call = JsonConvert.DeserializeObject<CallBack.VaPaid>(value.ToString());

                ReqAspandigi call2 = new ReqAspandigi();
                DateTime Now = DateTime.Now;
                call2.external_id = call.external_id;
                call2.paymen_by = "VA" + " " + call.bank_code;
                call2.timestamp = Now;

                string sJSONResponsedatum2 = JsonConvert.SerializeObject(call);

                dynamic dynamicObjectdatum2 = JsonConvert.DeserializeObject(sJSONResponsedatum2);


                var dt2 = @"" + dynamicObjectdatum2;

                var bd2 = (string)dt2.Replace("\r\n", "");

                String content2 = bd2;
                string sJSONResponsedatum4 = JsonConvert.SerializeObject(call2);

                dynamic dynamicObjectdatum4 = JsonConvert.DeserializeObject(sJSONResponsedatum4);


                var dt4 = @"" + dynamicObjectdatum4;

                var bd4 = (string)dt4.Replace("\r\n", "");

                String content4 = bd4;
                try
                {
                    idmerchan = (int)entities.TransaksiPayment.Where(p => p.ExternalId == call.external_id).ToList()[0].IdMerchan;
                }
                catch (System.Exception e)
                {
                    idmerchan = 0;
                }

                String LinkCallback = entities.Merchant.Where(p => p.IdMerchant == idmerchan).ToList()[0].UrlCallBack;

                InsertDB insertDb = new InsertDB();
                UpdateDb updateDb = new UpdateDb();

                string extId = call.external_id;
                string paymentby = "VA" + " " + call.bank_code;

                // updateDb.updateTransaksi(extId, paymentby);

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
                string status = call2.status;

                if (restResponse.StatusCode == HttpStatusCode.OK)
                {
                    string url = LinkCallback;
                    var client = new RestClient(url);

                    var requestParam = new RestRequest(Method.POST);

                    requestParam.AddHeader("Content-Type", "application/json");
                    requestParam.AddHeader("Cookie", ".ASPXAUTH=" + cookvalue);

                    requestParam.AddParameter("application/json", content4, ParameterType.RequestBody);

                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                    IRestResponse restResponseAspandigi = client.Execute(requestParam);

                    ResponseAspandigi responstatus = new ResponseAspandigi();

                    if (restResponseAspandigi.StatusCode == HttpStatusCode.OK)
                    {

                        string response = restResponseAspandigi.Content;
                        responstatus = JsonConvert.DeserializeObject<ResponseAspandigi>(response);


                        string responCode = responstatus.StatusCode.ToString();

                        if (responCode == "200")
                        {
                            updateDb.updateTransaksi(extId, paymentby);
                            insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/aspanvapaid", Now);
                            return Ok(sukses);
                        }
                        else
                        {
                            return Ok(gagal);
                        }

                    }
                    else
                    {
                        return Ok(gagal);
                    }

                }
                else
                {
                    return Ok(gagal);
                }

                 


            }
            catch (Exception ex)
            {
                return Ok(gagal);
            }


        }

        [HttpPost]
        [Route("api/callback/aspanvacreated")]
        public async Task<IActionResult> AspanVaCreated([FromBody] object value)
        {
            string token = HttpContext.Session.GetString("token");

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            int idmerchan = 0;

            CallBack.CreatedVA call = new CallBack.CreatedVA();
            try
            {
                call = JsonConvert.DeserializeObject<CallBack.CreatedVA>(value.ToString());
                ReqAspandigi call2 = new ReqAspandigi();
                DateTime Now = DateTime.Now;
                call2.external_id = call.external_id;
                call2.paymen_by = "VA" + " " + call.bank_code;
                call2.timestamp = Now;
                string sJSONResponsedatum2 = JsonConvert.SerializeObject(call);

                dynamic dynamicObjectdatum2 = JsonConvert.DeserializeObject(sJSONResponsedatum2);


                var dt2 = @"" + dynamicObjectdatum2;

                var bd2 = (string)dt2.Replace("\r\n", "");

                String content2 = bd2;

                string sJSONResponsedatum4 = JsonConvert.SerializeObject(call2);

                dynamic dynamicObjectdatum4 = JsonConvert.DeserializeObject(sJSONResponsedatum4);


                var dt4 = @"" + dynamicObjectdatum4;

                var bd4 = (string)dt4.Replace("\r\n", "");

                String content4 = bd4;

                try
                {
                    idmerchan = (int)entities.TransaksiPayment.Where(p => p.ExternalId == call.external_id).ToList()[0].IdMerchan;
                }
                catch (System.Exception e)
                {
                    idmerchan = 0;
                }

                String LinkCallback = entities.Merchant.Where(p => p.IdMerchant == idmerchan).ToList()[0].UrlCallBack;
                InsertDB insertDb = new InsertDB();
                UpdateDb updateDb = new UpdateDb();

                string extId = call.external_id;
                string paymentby = "VA" + " " + call.bank_code;

               // updateDb.updateTransaksi(extId, paymentby);

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
                string status = call2.status;

                if (restResponse.StatusCode == HttpStatusCode.OK)
                {

                    string url = LinkCallback;
                    var client = new RestClient(url);

                    var requestParam = new RestRequest(Method.POST);

                    requestParam.AddHeader("Content-Type", "application/json");
                    requestParam.AddHeader("Cookie", ".ASPXAUTH=" + cookvalue);
                    requestParam.AddParameter("application/json", content4, ParameterType.RequestBody);

                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                    IRestResponse restResponseAspandigi = client.Execute(requestParam);

                    ResponseAspandigi responstatus = new ResponseAspandigi();

                    if (restResponseAspandigi.StatusCode == HttpStatusCode.OK)
                    {

                        string response = restResponseAspandigi.Content;
                        responstatus = JsonConvert.DeserializeObject<ResponseAspandigi>(response);


                        string responCode = responstatus.StatusCode.ToString();

                        if (responCode == "200")
                        {
                            insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/aspanvacreated", Now);
                            return Ok(sukses);
                        }
                        else
                        {
                            return Ok(gagal);
                        }

                    }
                    else
                    {
                        return Ok(gagal);
                    }
                }
                else
                {
                    return Ok(gagal);
                }
            }
            catch (Exception ex)
            {
                return Ok(gagal);
            }


        }

        [HttpPost]
        [Route("api/callback/aspaninvpaid")]
        public async Task<IActionResult> InvoicePaids([FromBody] object value)
        {
            string token = HttpContext.Session.GetString("token");

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            int idmerchan = 0;

            CallBack.InvoicesPaid call = new CallBack.InvoicesPaid();
            try
            {
                call = JsonConvert.DeserializeObject<CallBack.InvoicesPaid>(value.ToString());
                ReqAspandigi call2 = new ReqAspandigi();
                DateTime Now = DateTime.Now;
                call2.external_id = call.external_id;
                call2.paymen_by = "INVOICE" + " " + call.bank_code;
                call2.timestamp = Now;
                call2.status = call.status;
                string sJSONResponsedatum2 = JsonConvert.SerializeObject(call);

                dynamic dynamicObjectdatum2 = JsonConvert.DeserializeObject(sJSONResponsedatum2);


                var dt2 = @"" + dynamicObjectdatum2;

                var bd2 = (string)dt2.Replace("\r\n", "");

                String content2 = bd2;


                string sJSONResponsedatum4 = JsonConvert.SerializeObject(call2);

                dynamic dynamicObjectdatum4 = JsonConvert.DeserializeObject(sJSONResponsedatum4);


                var dt4 = @"" + dynamicObjectdatum4;

                var bd4 = (string)dt4.Replace("\r\n", "");

                String content4 = bd4;

                try
                {
                    idmerchan = (int)entities.TransaksiPayment.Where(p => p.ExternalId == call.external_id).ToList()[0].IdMerchan;
                }
                catch (System.Exception e)
                {
                    idmerchan = 0;
                }
                String LinkCallback = entities.Merchant.Where(p => p.IdMerchant == idmerchan).ToList()[0].UrlCallBack;
                InsertDB insertDb = new InsertDB();
                UpdateDb updateDb = new UpdateDb();

                string extId = call.external_id;
                string paymentby = "INVOICE" + " " + call.bank_code;


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
                    

                    string url = LinkCallback;
                    var client = new RestClient(url);

                    var requestParam = new RestRequest(Method.POST);

                    requestParam.AddHeader("Content-Type", "application/json");
                    requestParam.AddHeader("Cookie", ".ASPXAUTH="+ cookvalue);
                    requestParam.AddParameter("application/json", content4, ParameterType.RequestBody);

                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                    IRestResponse restResponseAspandigi = client.Execute(requestParam);
                    string status = call2.status;

                    ResponseAspandigi responstatus = new ResponseAspandigi();

                    if (restResponseAspandigi.StatusCode == HttpStatusCode.OK)
                    {

                        string response = restResponseAspandigi.Content;
                        responstatus = JsonConvert.DeserializeObject<ResponseAspandigi>(response);


                        string responCode = responstatus.StatusCode.ToString();

                        if (responCode == "200")
                        {
                            if (status == "EXPIRED")
                            {
                                updateDb.updateTransaksiInvoice(extId, paymentby, "EXPIRED");
                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/aspaninvpaid", Now);
                                return Ok(sukses);
                            }
                            else if (status == "PENDING")
                            {

                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/aspaninvpaid", Now);
                                return Ok(sukses);
                            }
                            else
                            {
                                updateDb.updateTransaksiInvoice(extId, paymentby, "PAID");
                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/aspaninvpaid", Now);
                                return Ok(sukses);
                            }
                        }
                        else
                        {

                            return Ok(gagal);
                        }

                    }
                    else
                    {

                        return Ok(gagal);
                    }

                }
                else
                {

                    return Ok(gagal);


                }

              
            }
            catch (Exception ex)
            {
                return Ok(gagal);
            }


        }

       

        [HttpPost]
        [Route("api/callback/aspandistbursmentpaid")]
        public async Task<IActionResult> AspandigiDistPaid([FromBody] object value)
        {
            string token = HttpContext.Session.GetString("token");

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            int idmerchan = 0;
            CallBack.Distbursment call = new CallBack.Distbursment();
            try
            {
                call = JsonConvert.DeserializeObject<CallBack.Distbursment>(value.ToString());


                ReqAspandigiDisbursement call2 = new ReqAspandigiDisbursement();
                DateTime Now = DateTime.Now;
                call2.external_id = call.external_id;

                call2.status = call.status;
                string sJSONResponsedatum2 = JsonConvert.SerializeObject(call);

                dynamic dynamicObjectdatum2 = JsonConvert.DeserializeObject(sJSONResponsedatum2);


                var dt2 = @"" + dynamicObjectdatum2;

                var bd2 = (string)dt2.Replace("\r\n", "");

                String content2 = bd2;
                string sJSONResponsedatum4 = JsonConvert.SerializeObject(call2);

                dynamic dynamicObjectdatum4 = JsonConvert.DeserializeObject(sJSONResponsedatum4);


                var dt4 = @"" + dynamicObjectdatum4;

                var bd4 = (string)dt4.Replace("\r\n", "");

                String content4 = bd4;

                try
                {
                    idmerchan = (int)entities.TransaksiPayment.Where(p => p.ExternalId == call.external_id).ToList()[0].IdMerchan;
                }
                catch (System.Exception e)
                {
                    idmerchan = 0;
                }
                String LinkCallback = entities.Merchant.Where(p => p.IdMerchant == idmerchan).ToList()[0].UrlWithdraw;
                InsertDB insertDb = new InsertDB();
                UpdateDb updateDb = new UpdateDb();

                string externalid = call.external_id;
                string paymentby = "DISBURSEMENT";

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

                    string url = LinkCallback;
                    var client = new RestClient(url);

                    var requestParam = new RestRequest(Method.POST);

                    requestParam.AddHeader("Content-Type", "application/json");
                    requestParam.AddHeader("Cookie", ".ASPXAUTH=" + cookvalue);
                    requestParam.AddParameter("application/json", content4, ParameterType.RequestBody);

                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                    IRestResponse restResponseAspandigi = client.Execute(requestParam);
                    string status = call2.status;

                    ResponseAspandigi responstatus = new ResponseAspandigi();


                    if (restResponseAspandigi.StatusCode == HttpStatusCode.OK)
                    {

                        string response = restResponseAspandigi.Content;
                        responstatus = JsonConvert.DeserializeObject<ResponseAspandigi>(response);


                        string responCode = responstatus.StatusCode.ToString();

                        if (responCode == "200")
                        {

                            if (status == "FAILED")
                            {

                                updateDb.updateTransaksiDisbursementaspan(externalid, "FAILED");
                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/aspandistbursmentpaid", Now);
                                return Ok(sukses);
                            }
                            else if (status == "PENDING")
                            {

                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/aspandistbursmentpaid", Now);
                                return Ok(sukses);
                            }
                            else
                            {
                                updateDb.updateTransaksiDisbursementaspan(externalid, "PAID");
                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/aspandistbursmentpaid", Now);
                                return Ok(sukses);
                            }
                        }
                        else
                        {

                            return Ok(gagal);
                        }

                    }
                    else
                    {

                        return Ok(gagal);
                    }

                }
                else
                {
                    return Ok(gagal);
                }


            }
            catch (Exception ex)
            {
                return Ok(gagal);
            }


        }

       

        [HttpPost]
        [Route("api/callback/aspanretailpaid")]
        public async Task<IActionResult> RetailPaids([FromBody] object value)
        {
            string token = HttpContext.Session.GetString("token");

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            int idmerchan = 0;
            CallBack.RetailOutlet call = new CallBack.RetailOutlet();
            try
            {
                call = JsonConvert.DeserializeObject<CallBack.RetailOutlet>(value.ToString());
                ReqAspandigi call2 = new ReqAspandigi();
                DateTime Now = DateTime.Now;
                call2.external_id = call.external_id;
                call2.paymen_by = "RETAIL OUTLET";
                call2.timestamp = Now;
                call2.status = call.status;
                string sJSONResponsedatum2 = JsonConvert.SerializeObject(call);

                dynamic dynamicObjectdatum2 = JsonConvert.DeserializeObject(sJSONResponsedatum2);


                var dt2 = @"" + dynamicObjectdatum2;

                var bd2 = (string)dt2.Replace("\r\n", "");

                String content2 = bd2;
                string sJSONResponsedatum4 = JsonConvert.SerializeObject(call2);

                dynamic dynamicObjectdatum4 = JsonConvert.DeserializeObject(sJSONResponsedatum4);


                var dt4 = @"" + dynamicObjectdatum4;

                var bd4 = (string)dt4.Replace("\r\n", "");

                String content4 = bd4;

                try
                {
                    idmerchan = (int)entities.TransaksiPayment.Where(p => p.ExternalId == call.external_id).ToList()[0].IdMerchan;
                }
                catch (System.Exception e)
                {
                    idmerchan = 0;
                }
                String LinkCallback = entities.Merchant.Where(p => p.IdMerchant == idmerchan).ToList()[0].UrlCallBack;
                InsertDB insertDb = new InsertDB();
                UpdateDb updateDb = new UpdateDb();

                //string idXendit = call.id;
                string extId = call.external_id;
                string paymentby = "RETAIL OUTLET";

                // updateDb.updateTransaksi2(idXendit, paymentby);

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
                string status = call2.status;

                if (restResponse.StatusCode == HttpStatusCode.OK)
                {

                    string url = LinkCallback;
                    var client = new RestClient(url);

                    var requestParam = new RestRequest(Method.POST);

                    requestParam.AddHeader("Content-Type", "application/json");

                    requestParam.AddParameter("application/json", content4, ParameterType.RequestBody);

                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                    IRestResponse restResponseAspandigi = client.Execute(requestParam);

                    ResponseAspandigi responstatus = new ResponseAspandigi();

                    if (restResponseAspandigi.StatusCode == HttpStatusCode.OK)
                    {

                        string response = restResponseAspandigi.Content;
                        responstatus = JsonConvert.DeserializeObject<ResponseAspandigi>(response);


                        string responCode = responstatus.StatusCode.ToString();

                        
                        if (responCode == "200")
                        {
                            if (status == "EXPIRED")
                            {
                                updateDb.updateTransaksiInvoice(extId, paymentby, "EXPIRED");
                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/aspanretailpaid", Now);
                                return Ok(sukses);
                            }
                            else if (status == "PENDING")
                            {

                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/aspanretailpaid", Now);
                                return Ok(sukses);
                            }
                            else
                            {
                                updateDb.updateTransaksiInvoice(extId, paymentby, "PAID");
                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/aspanretailpaid", Now);
                                return Ok(sukses);
                            }
                        }
                        else
                        {

                            return Ok(gagal);
                        }


                    }
                    else
                    {
                        return Ok(gagal);
                    }
                }
                else
                {
                    return Ok(gagal);
                }
            }
            catch (Exception ex)
            {
                return Ok(gagal);
            }


        }

        //[HttpPost]
        //[Route("api/callback/qrpaid")]
        //public async Task<IActionResult> QrPaid([FromBody] object value)
        //{
        //    string token = HttpContext.Session.GetString("token");

        //    var jsonString = JsonConvert.DeserializeObject(value.ToString());
        //    jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
        //    CallBackQR callBackQR = new CallBackQR();

        //    try
        //    {
        //        callBackQR = JsonConvert.DeserializeObject<CallBackQR>(value.ToString());

        //        DateTime Now = DateTime.Now;

        //        string sJSONResponsedatum2 = JsonConvert.SerializeObject(callBackQR);

        //        dynamic dynamicObjectdatum2 = JsonConvert.DeserializeObject(sJSONResponsedatum2);


        //        var dt2 = @"" + dynamicObjectdatum2;

        //        var bd2 = (string)dt2.Replace("\r\n", "");

        //        String content2 = bd2;
        //        string sJSONResponsedatum4 = JsonConvert.SerializeObject(sukses);

        //        dynamic dynamicObjectdatum4 = JsonConvert.DeserializeObject(sJSONResponsedatum4);


        //        var dt4 = @"" + dynamicObjectdatum4;

        //        var bd4 = (string)dt4.Replace("\r\n", "");

        //        String content4 = bd4;

        //        InsertDB insertDb = new InsertDB();
        //        UpdateDb updateDb = new UpdateDb();

        //        string extId = callBackQR.external_id;
        //        string paymentby = "QRCode";

        //        updateDb.updateTransaksi(extId, paymentby);

        //        insertDb.InsertLog(content2, content4, "http://192.168.0.8/xenditapi/api/callback/qrpaid", Now);
        //        return Ok(sukses);

        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(gagal);
        //    }
        //}

        [HttpPost]
        [Route("api/callback/aspanewalletpaid")]
        public async Task<IActionResult> EwalletPaids([FromBody] object value)
        {
            string token = HttpContext.Session.GetString("token");

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            int idmerchan = 0;
            CallBack.EwalletPaid call = new CallBack.EwalletPaid();
            try
            {
                call = JsonConvert.DeserializeObject<CallBack.EwalletPaid>(value.ToString());
                ReqAspandigi call2 = new ReqAspandigi();
                DateTime Now = DateTime.Now;
                call2.external_id = call.data.id;
                call2.paymen_by = "EWALLET";
                call2.timestamp = Now;
                string sJSONResponsedatum2 = JsonConvert.SerializeObject(call);

                dynamic dynamicObjectdatum2 = JsonConvert.DeserializeObject(sJSONResponsedatum2);


                var dt2 = @"" + dynamicObjectdatum2;

                var bd2 = (string)dt2.Replace("\r\n", "");

                String content2 = bd2;
                string sJSONResponsedatum4 = JsonConvert.SerializeObject(call2);

                dynamic dynamicObjectdatum4 = JsonConvert.DeserializeObject(sJSONResponsedatum4);


                var dt4 = @"" + dynamicObjectdatum4;

                var bd4 = (string)dt4.Replace("\r\n", "");

                String content4 = bd4;
                try
                {
                    idmerchan = (int)entities.TransaksiPayment.Where(p => p.IdXendit == call.data.id).ToList()[0].IdMerchan;
                }
                catch (System.Exception e)
                {
                    idmerchan = 0;
                }
                String LinkCallback = entities.Merchant.Where(p => p.IdMerchant == idmerchan).ToList()[0].UrlCallBack;
                InsertDB insertDb = new InsertDB();
                UpdateDb updateDb = new UpdateDb();

                string idXendit = call.data.id;
                string paymentby = "EWALLET";

                //  updateDb.updateTransaksi2(idXendit, paymentby);


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
                string status = call2.status;

                if (restResponse.StatusCode == HttpStatusCode.OK)
                {
                    string url = LinkCallback;
                    var client = new RestClient(url);

                    var requestParam = new RestRequest(Method.POST);

                    requestParam.AddHeader("Content-Type", "application/json");
                    requestParam.AddHeader("Cookie", ".ASPXAUTH=" + cookvalue);
                    requestParam.AddParameter("application/json", content4, ParameterType.RequestBody);

                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                    IRestResponse restResponseAspandigi = client.Execute(requestParam);

                    ResponseAspandigi responstatus = new ResponseAspandigi();

                    if (restResponseAspandigi.StatusCode == HttpStatusCode.OK)
                    {

                        string response = restResponseAspandigi.Content;
                        responstatus = JsonConvert.DeserializeObject<ResponseAspandigi>(response);


                        string responCode = responstatus.StatusCode.ToString();

                        if (responCode == "200")
                        {
                            updateDb.updateTransaksi2(idXendit, paymentby);
                            insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/aspanewalletpaid", Now);
                            return Ok(sukses);
                        }
                        else
                        {
                            return Ok(gagal);
                        }

                    }
                    else
                    {
                        return Ok(gagal);
                    }
                }
                else
                {
                    return Ok(gagal);
                }
            }
            catch (Exception ex)
            {
                return Ok(gagal);
            }


        }


    }

    public class ReqAspandigi
    {
        public string external_id { get; set; }
        public string paymen_by { get; set; }
        public DateTime timestamp { get; set; }
        public string status { get; set; }
    }

    public class ReqAspandigiOther
    {
        public string external_id { get; set; }
        public string paymen_by { get; set; }
        public DateTime timestamp { get; set; }
       
    }

    public class ReqAspandigiDisbursement
    {
        public string external_id { get; set; }
        public string status { get; set; }

    }

    public class ResponseAspandigi
    {
        
        public string Status { get; set; }
        public string StatusCode { get; set; }
    }

    public class CallBackQRAspandigi
    {

        public string external_id { get; set; }
    }

}