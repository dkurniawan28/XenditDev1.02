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
using XenditDev1._02.Function;
using XenditDev1._02.ModelsNew;
using System.Linq;

namespace XenditDev1._02.Controllers
{
    // [Route("api/[controller]")]


    [ApiController]
    public class CallBackController : ControllerBase
    {

        JObject sukses = JObject.Parse("{\r\n    \"Status\":\"Sukses\", \r\n\"StatusCode\":\"200\"\r\n}");
        JObject gagal = JObject.Parse("{\r\n    \"Status\":\"Gagal\", \r\n\"StatusCode\":\"400\"\r\n}");
        PaymentGatewayContext entities = new PaymentGatewayContext();
        [HttpPost]
        [Route("api/callback/vapaid")]
        public async Task<IActionResult> VaPaid([FromBody] object value)
        {
            string token = HttpContext.Session.GetString("token");

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            int idmerchan = 0;

            CallBack.VaPaid call = new CallBack.VaPaid();
            try
            {
                call = JsonConvert.DeserializeObject<CallBack.VaPaid>(value.ToString());

                ReqKamuBisa call2 = new ReqKamuBisa();
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

                //  updateDb.updateTransaksi(extId, paymentby);


                if (idmerchan == 1)
                {
                    string url = LinkCallback;
                    var client = new RestClient(url);

                    var requestParam = new RestRequest(Method.POST);

                    requestParam.AddHeader("Content-Type", "application/json");

                    requestParam.AddParameter("application/json", content4, ParameterType.RequestBody);

                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                    IRestResponse restResponseKamubisa = client.Execute(requestParam);

                    ResponseKamubisa responstatus = new ResponseKamubisa();

                    if (restResponseKamubisa.StatusCode == HttpStatusCode.OK)
                    {

                        string response = restResponseKamubisa.Content;
                        responstatus = JsonConvert.DeserializeObject<ResponseKamubisa>(response);


                        string responCode = responstatus.Code.ToString();

                        if (responCode == "200")
                        {
                            updateDb.updateTransaksi(extId, paymentby);
                            insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/vapaid", Now);
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
                else if (idmerchan == 2)
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
                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/vapaid", Now);
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

                else if (idmerchan == 3)
                {
                    string urlsign = "http://asri.aspan.co.id/BondingAspan/Account/Login";

                    string username = "adminbonding";
                    string password = "admin1234";
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
                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/vapaid", Now);
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
                else if (idmerchan == 4)
                {
                    string urlsign = "http://asri.aspan.co.id/BrokerDigi/Account/Login";

                    string username = "unitbroker";
                    string password = "unitbroker1234";
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
                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/vapaid", Now);
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
        [Route("api/callback/vacreated")]
        public async Task<IActionResult> VaCreated([FromBody] object value)
        {
            string token = HttpContext.Session.GetString("token");

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            int idmerchan = 0;

            CallBack.CreatedVA call = new CallBack.CreatedVA();
            try
            {
                call = JsonConvert.DeserializeObject<CallBack.CreatedVA>(value.ToString());
                ReqKamuBisa call2 = new ReqKamuBisa();
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


                if (idmerchan == 1)
                {
                    string url = LinkCallback;
                    var client = new RestClient(url);

                    var requestParam = new RestRequest(Method.POST);

                    requestParam.AddHeader("Content-Type", "application/json");

                    requestParam.AddParameter("application/json", content4, ParameterType.RequestBody);

                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                    IRestResponse restResponseKamubisa = client.Execute(requestParam);

                    ResponseKamubisa responstatus = new ResponseKamubisa();

                    if (restResponseKamubisa.StatusCode == HttpStatusCode.OK)
                    {

                        string response = restResponseKamubisa.Content;
                        responstatus = JsonConvert.DeserializeObject<ResponseKamubisa>(response);


                        string responCode = responstatus.Code.ToString();

                        if (responCode == "200")
                        {
                            updateDb.updateTransaksi(extId, paymentby);
                            insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/vapaid", Now);
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
                else if (idmerchan == 2)
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

                else if (idmerchan == 3)
                {
                    string urlsign = "http://asri.aspan.co.id/BondingAspan/Account/Login";

                    string username = "adminbonding";
                    string password = "admin1234";
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
                else if (idmerchan == 4)
                {
                    string urlsign = "http://asri.aspan.co.id/BrokerDigi/Account/Login";

                    string username = "unitbroker";
                    string password = "unitbroker1234";
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
        [Route("api/callback/invpaid")]
        public async Task<IActionResult> InvoicePaid([FromBody] object value)
        {
            string token = HttpContext.Session.GetString("token");

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            int idmerchan = 0;

            CallBack.InvoicesPaid call = new CallBack.InvoicesPaid();
            try
            {
                call = JsonConvert.DeserializeObject<CallBack.InvoicesPaid>(value.ToString());
                ReqKamuBisa call2 = new ReqKamuBisa();
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


                if (idmerchan == 1)
                {



                    string url = LinkCallback;
                    var client = new RestClient(url);

                    var requestParam = new RestRequest(Method.POST);

                    requestParam.AddHeader("Content-Type", "application/json");

                    requestParam.AddParameter("application/json", content4, ParameterType.RequestBody);

                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                    IRestResponse restResponseKamubisa = client.Execute(requestParam);
                    string status = call.status;

                    ResponseKamubisa responstatus = new ResponseKamubisa();

                    if (restResponseKamubisa.StatusCode == HttpStatusCode.OK)
                    {

                        string response = restResponseKamubisa.Content;
                        responstatus = JsonConvert.DeserializeObject<ResponseKamubisa>(response);


                        string responCode = responstatus.Code.ToString();

                        if (responCode == "200")
                        {
                            if (status == "EXPIRED")
                            {
                                updateDb.updateTransaksiInvoice(extId, paymentby, "EXPIRED");
                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/invpaid", Now);
                                return Ok(sukses);
                            }
                            else if (status == "PENDING")
                            {

                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/invpaid", Now);
                                return Ok(sukses);
                            }
                            else
                            {
                                updateDb.updateTransaksiInvoice(extId, paymentby, "PAID");
                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/invpaid", Now);
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
                else if (idmerchan == 2)
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


                        string url = LinkCallback;
                        var client = new RestClient(url);

                        var requestParam = new RestRequest(Method.POST);

                        requestParam.AddHeader("Content-Type", "application/json");
                        requestParam.AddHeader("Cookie", ".ASPXAUTH=" + cookvalue);
                        requestParam.AddParameter("application/json", content4, ParameterType.RequestBody);

                        ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                        IRestResponse restResponseKamubisa = client.Execute(requestParam);
                        string status = call2.status;

                        ResponseAspandigis responstatus = new ResponseAspandigis();


                        if (restResponseKamubisa.StatusCode == HttpStatusCode.OK)
                        {

                            string response = restResponseKamubisa.Content;
                            responstatus = JsonConvert.DeserializeObject<ResponseAspandigis>(response);


                            string responCode = responstatus.StatusCode.ToString();

                            if (responCode == "200")
                            {
                                if (status == "EXPIRED")
                                {
                                    updateDb.updateTransaksiInvoice(extId, paymentby, "EXPIRED");
                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/invpaid", Now);
                                    return Ok(sukses);
                                }
                                else if (status == "PENDING")
                                {

                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/invpaid", Now);
                                    return Ok(sukses);
                                }
                                else
                                {
                                    updateDb.updateTransaksiInvoice(extId, paymentby, "PAID");
                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/invpaid", Now);
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
                else if (idmerchan == 3)
                {

                    string urlsign = "http://asri.aspan.co.id/BondingAspan/Account/Login";

                    string username = "adminbonding";
                    string password = "admin1234";
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

                        IRestResponse restResponseKamubisa = client.Execute(requestParam);
                        string status = call2.status;

                        ResponseAspandigis responstatus = new ResponseAspandigis();


                        if (restResponseKamubisa.StatusCode == HttpStatusCode.OK)
                        {

                            string response = restResponseKamubisa.Content;
                            responstatus = JsonConvert.DeserializeObject<ResponseAspandigis>(response);


                            string responCode = responstatus.StatusCode.ToString();

                            if (responCode == "200")
                            {
                                if (status == "EXPIRED")
                                {
                                    updateDb.updateTransaksiInvoice(extId, paymentby, "EXPIRED");
                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/invpaid", Now);
                                    return Ok(sukses);
                                }
                                else if (status == "PENDING")
                                {

                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/invpaid", Now);
                                    return Ok(sukses);
                                }
                                else
                                {
                                    updateDb.updateTransaksiInvoice(extId, paymentby, "PAID");
                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/invpaid", Now);
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
                else if (idmerchan == 4)
                {

                    string urlsign = "http://asri.aspan.co.id/AspanAsuransi/Account/Login";

                    string username = "unitbroker";
                    string password = "unitbroker1234";
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

                        IRestResponse restResponseKamubisa = client.Execute(requestParam);
                        string status = call2.status;

                        ResponseAspandigis responstatus = new ResponseAspandigis();


                        if (restResponseKamubisa.StatusCode == HttpStatusCode.OK)
                        {

                            string response = restResponseKamubisa.Content;
                            responstatus = JsonConvert.DeserializeObject<ResponseAspandigis>(response);


                            string responCode = responstatus.StatusCode.ToString();

                            if (responCode == "200")
                            {
                                if (status == "EXPIRED")
                                {
                                    updateDb.updateTransaksiInvoice(extId, paymentby, "EXPIRED");
                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/invpaid", Now);
                                    return Ok(sukses);
                                }
                                else if (status == "PENDING")
                                {

                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/invpaid", Now);
                                    return Ok(sukses);
                                }
                                else
                                {
                                    updateDb.updateTransaksiInvoice(extId, paymentby, "PAID");
                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/invpaid", Now);
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
        [Route("api/callback/ovopaid")]
        public async Task<IActionResult> OvoPaid([FromBody] object value)
        {
            string token = HttpContext.Session.GetString("token");

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            int idmerchan = 0;
            CallBack.OvoPaid call = new CallBack.OvoPaid();
            try
            {
                call = JsonConvert.DeserializeObject<CallBack.OvoPaid>(value.ToString());
                ReqKamuBisa call2 = new ReqKamuBisa();
                DateTime Now = DateTime.Now;
                call2.external_id = call.external_id;
                call2.paymen_by = "OVO";
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
                string paymentby = "OVO";

                //updateDb.updateTransaksi(extId, paymentby);

                if (idmerchan == 1)
                {

                    string url = LinkCallback;
                    var client = new RestClient(url);

                    var requestParam = new RestRequest(Method.POST);

                    requestParam.AddHeader("Content-Type", "application/json");

                    requestParam.AddParameter("application/json", content4, ParameterType.RequestBody);

                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                    IRestResponse restResponseKamubisa = client.Execute(requestParam);

                    ResponseKamubisa responstatus = new ResponseKamubisa();
                    string status = call.status;

                    if (restResponseKamubisa.StatusCode == HttpStatusCode.OK)
                    {

                        string response = restResponseKamubisa.Content;
                        responstatus = JsonConvert.DeserializeObject<ResponseKamubisa>(response);


                        string responCode = responstatus.Code.ToString();



                        if (responCode == "200")
                        {
                            if (status == "EXPIRED")
                            {
                                updateDb.updateTransaksiInvoice(extId, paymentby, "EXPIRED");
                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/ovopaid", Now);
                                return Ok(sukses);
                            }
                            else if (status == "PENDING")
                            {

                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/ovopaid", Now);
                                return Ok(sukses);
                            }
                            else
                            {
                                updateDb.updateTransaksiInvoice(extId, paymentby, "PAID");
                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/ovopaid", Now);
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
                else if (idmerchan == 2)
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
                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/ovopaid", Now);
                                    return Ok(sukses);
                                }
                                else if (status == "PENDING")
                                {

                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/ovopaid", Now);
                                    return Ok(sukses);
                                }

                                else
                                {
                                    updateDb.updateTransaksiInvoice(extId, paymentby, "PAID");
                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/ovopaid", Now);
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
                else if (idmerchan == 3)
                {
                    string urlsign = "http://asri.aspan.co.id/BondingAspan/Account/Login";

                    string username = "adminbonding";
                    string password = "admin1234";
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
                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/ovopaid", Now);
                                    return Ok(sukses);
                                }
                                else if (status == "PENDING")
                                {

                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/ovopaid", Now);
                                    return Ok(sukses);
                                }

                                else
                                {
                                    updateDb.updateTransaksiInvoice(extId, paymentby, "PAID");
                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/ovopaid", Now);
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
                else if (idmerchan == 4)
                {
                    string urlsign = "http://asri.aspan.co.id/BrokerDigi/Account/Login";

                    string username = "unitbroker";
                    string password = "unitbroker1234";
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
                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/ovopaid", Now);
                                    return Ok(sukses);
                                }
                                else if (status == "PENDING")
                                {

                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/ovopaid", Now);
                                    return Ok(sukses);
                                }

                                else
                                {
                                    updateDb.updateTransaksiInvoice(extId, paymentby, "PAID");
                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/ovopaid", Now);
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
        [Route("api/callback/distbursmentpaid")]
        public async Task<IActionResult> DistPaid([FromBody] object value)
        {
            string token = HttpContext.Session.GetString("token");

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            int idmerchan = 0;
            CallBack.Distbursment call = new CallBack.Distbursment();
            try
            {
                call = JsonConvert.DeserializeObject<CallBack.Distbursment>(value.ToString());


                ReqKamuBisaDisbursement call2 = new ReqKamuBisaDisbursement();
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
                string paymentby = "DISBURSEMENT " + call.bank_code;

                //updateDb.updateTransaksi2(idXendit, paymentby);


                if (idmerchan == 1)
                {

                    string url = LinkCallback;
                    var client = new RestClient(url);

                    var requestParam = new RestRequest(Method.POST);

                    requestParam.AddHeader("Content-Type", "application/json");

                    requestParam.AddParameter("application/json", content4, ParameterType.RequestBody);

                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                    IRestResponse restResponseKamubisa = client.Execute(requestParam);

                    ResponseKamubisa responstatus = new ResponseKamubisa();


                    string status = call2.status;
                    if (restResponseKamubisa.StatusCode == HttpStatusCode.OK)
                    {

                        string response = restResponseKamubisa.Content;
                        responstatus = JsonConvert.DeserializeObject<ResponseKamubisa>(response);


                        string responCode = responstatus.Code.ToString();

                        if (responCode == "200")
                        {

                            if (status == "FAILED")
                            {



                                updateDb.updateTransaksiDisbursementaspan(externalid, "FAILED");

                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/distbursmentpaid", Now);
                                return Ok(sukses);
                            }
                            else if (status == "PENDING")
                            {

                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/distbursmentpaid", Now);
                                return Ok(sukses);
                            }
                            else if (status == "PAID")
                            {
                                updateDb.updateTransaksiDisbursementaspan(externalid, "PAID");

                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/distbursmentpaid", Now);
                                return Ok(sukses);
                            }
                            else 
                            {
                                updateDb.updateTransaksiDisbursementaspan(externalid, "COMPLETED");

                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/distbursmentpaid", Now);
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
                else if (idmerchan == 2)
                {
                    //string urlsign = "http://asri.aspan.co.id/AspanAsuransi/Account/Login";


                    //string username = "admin";
                    //string password = "semogajaya@1993";
                    //var clients = new RestClient(urlsign);

                    //var request = new RestRequest(Method.POST);
                    //JObject jObjectbody = new JObject();
                    //jObjectbody.Add("username", username);
                    //jObjectbody.Add("password", password);
                    //request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

                    //IRestResponse restResponse = clients.Execute(request);
                    //string responses = restResponse.Content;
                    //var jObject = JObject.Parse(restResponse.Content);
                    //string cookvalue = "";
                    //var sessionCookie = restResponse.Cookies.SingleOrDefault(x => x.Name == ".ASPXAUTH");
                    //if (sessionCookie != null)
                    //{
                    //    cookvalue = sessionCookie.Value;
                    //}
                    //if (restResponse.StatusCode == HttpStatusCode.OK)
                    //{

                    //    string url = LinkCallback;
                    //    var client = new RestClient(url);

                    //    var requestParam = new RestRequest(Method.POST);

                    //    requestParam.AddHeader("Content-Type", "application/json");
                    //    requestParam.AddHeader("Cookie", ".ASPXAUTH=" + cookvalue);
                    //    requestParam.AddParameter("application/json", content4, ParameterType.RequestBody);

                    //    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                    //    IRestResponse restResponseAspandigi = client.Execute(requestParam);
                    //    string status = call2.status;

                    //    ResponseAspandigi responstatus = new ResponseAspandigi();


                    //    if (restResponseAspandigi.StatusCode == HttpStatusCode.OK)
                    //    {

                    //        string response = restResponseAspandigi.Content;
                    //        responstatus = JsonConvert.DeserializeObject<ResponseAspandigi>(response);


                    //        string responCode = responstatus.StatusCode.ToString();

                    //        if (responCode == "200")
                    //        {

                    //            if (status == "FAILED")
                    //            {

                    //                updateDb.updateTransaksiDisbursementaspan(externalid, "FAILED");
                    //                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/distbursmentpaid", Now);
                    //                return Ok(sukses);
                    //            }
                    //            else if (status == "PENDING")
                    //            {

                    //                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/distbursmentpaid", Now);
                    //                return Ok(sukses);
                    //            }
                    //            else if (status == "PAID")
                    //            {
                    //                updateDb.updateTransaksiDisbursementaspan(externalid, "PAID");
                    //                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/distbursmentpaid", Now);
                    //                return Ok(sukses);
                    //            }
                    //            else 
                    //            {
                    //                updateDb.updateTransaksiDisbursementaspan(externalid, "COMPLETED");
                    //                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/distbursmentpaid", Now);
                    //                return Ok(sukses);
                    //            }

                    //        }
                    //        else
                    //        {

                    //            return Ok(gagal);
                    //        }

                    //    }
                    //    else
                    //    {

                    //        return Ok(gagal);
                    //    }

                    //}
                    //else
                    //{
                    //    return Ok(gagal);
                    //}
                    return Ok(gagal);
                }

                else if (idmerchan == 3)
                {
                    string urlsign = "http://asri.aspan.co.id/BondingAspan/Account/Login";


                    string username = "adminbonding";
                    string password = "admin1234";
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
                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/distbursmentpaid", Now);
                                    return Ok(sukses);
                                }
                                else if (status == "PENDING")
                                {

                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/distbursmentpaid", Now);
                                    return Ok(sukses);
                                }
                                else if (status == "PAID")
                                {
                                    updateDb.updateTransaksiDisbursementaspan(externalid, "PAID");
                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/distbursmentpaid", Now);
                                    return Ok(sukses);
                                }
                                else 
                                {
                                    updateDb.updateTransaksiDisbursementaspan(externalid, "COMPLETED");
                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/distbursmentpaid", Now);
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
                else if (idmerchan == 4)
                {
                    string urlsign = "http://asri.aspan.co.id/BrokerDigi/Account/Login";


                    string username = "unitbroker";
                    string password = "unitbroker1234";
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
                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/distbursmentpaid", Now);
                                    return Ok(sukses);
                                }
                                else if (status == "PENDING")
                                {

                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/distbursmentpaid", Now);
                                    return Ok(sukses);
                                }
                                else if (status == "PAID")
                                {
                                    updateDb.updateTransaksiDisbursementaspan(externalid, "PAID");
                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/distbursmentpaid", Now);
                                    return Ok(sukses);
                                }
                                else
                                {
                                    updateDb.updateTransaksiDisbursementaspan(externalid, "COMPLETED");
                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/distbursmentpaid", Now);
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
        [Route("api/callback/batcdistpaid")]
        public async Task<IActionResult> BacthDistPaid([FromBody] object value)
        {
            string token = HttpContext.Session.GetString("token");

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            int idmerchan = 0;
            CallBack.BatchDistbursment call = new CallBack.BatchDistbursment();
            try
            {
                call = JsonConvert.DeserializeObject<CallBack.BatchDistbursment>(value.ToString());
                ReqKamuBisa call2 = new ReqKamuBisa();
                DateTime Now = DateTime.Now;
                call2.external_id = call.id;
                call2.paymen_by = "BATCH DISBURSEMENT";
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
                    idmerchan = (int)entities.TransaksiPayment.Where(p => p.IdXendit == call.id).ToList()[0].IdMerchan;
                }
                catch (System.Exception e)
                {
                    idmerchan = 0;
                }
                String LinkCallback = entities.Merchant.Where(p => p.IdMerchant == idmerchan).ToList()[0].UrlCallBack;
                InsertDB insertDb = new InsertDB();
                UpdateDb updateDb = new UpdateDb();

                string idXendit = call.id;
                string paymentby = "BATCH DISBURSEMENT";

                updateDb.updateTransaksi2(idXendit, paymentby);



                string url = LinkCallback;
                var client = new RestClient(url);

                var requestParam = new RestRequest(Method.POST);

                requestParam.AddHeader("Content-Type", "application/json");

                requestParam.AddParameter("application/json", content4, ParameterType.RequestBody);

                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                IRestResponse restResponseKamubisa = client.Execute(requestParam);

                ResponseKamubisa responstatus = new ResponseKamubisa();

                if (restResponseKamubisa.StatusCode == HttpStatusCode.OK)
                {

                    string response = restResponseKamubisa.Content;
                    responstatus = JsonConvert.DeserializeObject<ResponseKamubisa>(response);


                    string responCode = responstatus.Code.ToString();

                    if (responCode == "200")
                    {
                        insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/batcdistpaid", Now);
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
            catch (Exception ex)
            {
                return Ok(gagal);
            }


        }

        [HttpPost]
        [Route("api/callback/retailpaid")]
        public async Task<IActionResult> RetailPaid([FromBody] object value)
        {
            string token = HttpContext.Session.GetString("token");

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            int idmerchan = 0;
            CallBack.RetailOutlet call = new CallBack.RetailOutlet();
            try
            {
                call = JsonConvert.DeserializeObject<CallBack.RetailOutlet>(value.ToString());
                ReqKamuBisa call2 = new ReqKamuBisa();
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

                // string idXendit = call.id;
                string extId = call.external_id;
                string paymentby = "RETAIL OUTLET";

                // updateDb.updateTransaksi2(idXendit, paymentby);


                if (idmerchan == 1)
                {
                    string url = LinkCallback;
                    var client = new RestClient(url);

                    var requestParam = new RestRequest(Method.POST);

                    requestParam.AddHeader("Content-Type", "application/json");

                    requestParam.AddParameter("application/json", content4, ParameterType.RequestBody);

                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                    IRestResponse restResponseKamubisa = client.Execute(requestParam);

                    ResponseKamubisa responstatus = new ResponseKamubisa();
                    string status = call2.status;
                    if (restResponseKamubisa.StatusCode == HttpStatusCode.OK)
                    {

                        string response = restResponseKamubisa.Content;
                        responstatus = JsonConvert.DeserializeObject<ResponseKamubisa>(response);


                        string responCode = responstatus.Code.ToString();

                        if (responCode == "200")
                        {
                            if (status == "EXPIRED")
                            {
                                updateDb.updateTransaksiInvoice(extId, paymentby, "EXPIRED");
                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/retailpaid", Now);
                                return Ok(sukses);
                            }
                            else if (status == "PENDING")
                            {

                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/retailpaid", Now);
                                return Ok(sukses);
                            }
                            else
                            {
                                updateDb.updateTransaksiInvoice(extId, paymentby, "PAID");
                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/retailpaid", Now);
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
                else if (idmerchan == 2)
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
                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/retailpaid", Now);
                                    return Ok(sukses);
                                }
                                else if (status == "PENDING")
                                {

                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/retailpaid", Now);
                                    return Ok(sukses);
                                }
                                else
                                {
                                    updateDb.updateTransaksiInvoice(extId, paymentby, "PAID");
                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/retailpaid", Now);
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
                else if (idmerchan == 3)
                {
                    string urlsign = "http://asri.aspan.co.id/BondingAspan/Account/Login";

                    string username = "adminbonding";
                    string password = "admin1234";
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
                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/retailpaid", Now);
                                    return Ok(sukses);
                                }
                                else if (status == "PENDING")
                                {

                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/retailpaid", Now);
                                    return Ok(sukses);
                                }
                                else
                                {
                                    updateDb.updateTransaksiInvoice(extId, paymentby, "PAID");
                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/retailpaid", Now);
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
                else if (idmerchan == 4)
                {
                    string urlsign = "http://asri.aspan.co.id/BrokerDigi/Account/Login";

                    string username = "unitbroker";
                    string password = "unitbroker1234";
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
                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/retailpaid", Now);
                                    return Ok(sukses);
                                }
                                else if (status == "PENDING")
                                {

                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/retailpaid", Now);
                                    return Ok(sukses);
                                }
                                else
                                {
                                    updateDb.updateTransaksiInvoice(extId, paymentby, "PAID");
                                    insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/retailpaid", Now);
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
        [Route("api/callback/qrpaid")]
        public async Task<IActionResult> QrPaid([FromBody] object value)
        {
            string token = HttpContext.Session.GetString("token");

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            int idmerchan = 0;
            CallBackQR callBackQR = new CallBackQR();

            try
            {
                callBackQR = JsonConvert.DeserializeObject<CallBackQR>(value.ToString());

                DateTime Now = DateTime.Now;

                string sJSONResponsedatum2 = JsonConvert.SerializeObject(callBackQR);

                dynamic dynamicObjectdatum2 = JsonConvert.DeserializeObject(sJSONResponsedatum2);


                var dt2 = @"" + dynamicObjectdatum2;

                var bd2 = (string)dt2.Replace("\r\n", "");

                String content2 = bd2;
                string sJSONResponsedatum4 = JsonConvert.SerializeObject(sukses);

                dynamic dynamicObjectdatum4 = JsonConvert.DeserializeObject(sJSONResponsedatum4);


                var dt4 = @"" + dynamicObjectdatum4;

                var bd4 = (string)dt4.Replace("\r\n", "");

                String content4 = bd4;

                InsertDB insertDb = new InsertDB();
                UpdateDb updateDb = new UpdateDb();

                string extId = callBackQR.external_id;
                string paymentby = "QRCode";

                try
                {
                    idmerchan = (int)entities.TransaksiPayment.Where(p => p.ExternalId == extId).ToList()[0].IdMerchan;
                }
                catch (System.Exception e)
                {
                    idmerchan = 0;
                }

                String LinkCallback = entities.Merchant.Where(p => p.IdMerchant == idmerchan).ToList()[0].UrlCallBack;

                if (idmerchan == 1)
                {
                    string url = LinkCallback;
                    var client = new RestClient(url);

                    var requestParam = new RestRequest(Method.POST);

                    requestParam.AddHeader("Content-Type", "application/json");

                    requestParam.AddParameter("application/json", content4, ParameterType.RequestBody);

                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                    IRestResponse restResponseKamubisa = client.Execute(requestParam);

                    ResponseKamubisa responstatus = new ResponseKamubisa();

                    if (restResponseKamubisa.StatusCode == HttpStatusCode.OK)
                    {

                        string response = restResponseKamubisa.Content;
                        responstatus = JsonConvert.DeserializeObject<ResponseKamubisa>(response);


                        string responCode = responstatus.Code.ToString();

                        if (responCode == "200")
                        {

                            updateDb.updateTransaksiInvoice(extId, paymentby, "COMPLETED");
                            insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/qrpaid", Now);
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

                else if (idmerchan == 2)
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

                                updateDb.updateTransaksiInvoice(extId, paymentby, "COMPLETED");
                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/qrpaid", Now);
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
                else if (idmerchan == 3)
                {
                    string urlsign = "http://asri.aspan.co.id/BondingAspan/Account/Login";

                    string username = "adminbonding";
                    string password = "admin1234";
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

                                updateDb.updateTransaksiInvoice(extId, paymentby, "COMPLETED");
                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/qrpaid", Now);
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
                else if (idmerchan == 4)
                {
                    string urlsign = "http://asri.aspan.co.id/BrokerDigi/Account/Login";

                    string username = "unitbroker";
                    string password = "unitbroker1234";
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

                                updateDb.updateTransaksiInvoice(extId, paymentby, "COMPLETED");
                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/qrpaid", Now);
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
        [Route("api/callback/ewalletpaid")]
        public async Task<IActionResult> EwalletPaid([FromBody] object value)
        {
            string token = HttpContext.Session.GetString("token");

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            int idmerchan = 0;
            CallBack.EwalletPaid call = new CallBack.EwalletPaid();
            try
            {
                call = JsonConvert.DeserializeObject<CallBack.EwalletPaid>(value.ToString());
                ReqKamuBisa call2 = new ReqKamuBisa();
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

                // updateDb.updateTransaksi2(idXendit, paymentby);

                if (idmerchan == 1)
                {

                    string url = LinkCallback;
                    var client = new RestClient(url);

                    var requestParam = new RestRequest(Method.POST);

                    requestParam.AddHeader("Content-Type", "application/json");

                    requestParam.AddParameter("application/json", content4, ParameterType.RequestBody);

                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                    IRestResponse restResponseKamubisa = client.Execute(requestParam);

                    ResponseKamubisa responstatus = new ResponseKamubisa();

                    if (restResponseKamubisa.StatusCode == HttpStatusCode.OK)
                    {

                        string response = restResponseKamubisa.Content;
                        responstatus = JsonConvert.DeserializeObject<ResponseKamubisa>(response);


                        string responCode = responstatus.Code.ToString();

                        if (responCode == "200")
                        {
                            updateDb.updateTransaksi2(idXendit, paymentby);
                            insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/ewalletpaid", Now);
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
                else if (idmerchan == 2)
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
                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/ewalletpaid", Now);
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
                else if (idmerchan == 3)
                {
                    string urlsign = "http://asri.aspan.co.id/BondingAspan/Account/Login";

                    string username = "adminbonding";
                    string password = "admin1234";
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
                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/ewalletpaid", Now);
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
                else if (idmerchan == 4)
                {
                    string urlsign = "http://asri.aspan.co.id/BrokerDigi/Account/Login";

                    string username = "unitbroker";
                    string password = "unitbroker1234";
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
                                insertDb.InsertLog(content2, response, "http://192.168.0.8/xenditapi/api/callback/ewalletpaid", Now);
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

    public class ReqKamuBisa
    {
        public string external_id { get; set; }
        public string paymen_by { get; set; }
        public DateTime timestamp { get; set; }
        public string status { get; set; }
    }

    public class ReqKamuBisaDisbursement
    {
        public string external_id { get; set; }

        public string status { get; set; }
    }

    public class ResponseKamubisa
    {
        public string Code { get; set; }
        public string Status { get; set; }
        public string Messages { get; set; }
    }

    public class ResponseAspandigis
    {

        public string Status { get; set; }
        public string StatusCode { get; set; }
    }

    public class CallBackQR
    {

        public string external_id { get; set; }
    }
}