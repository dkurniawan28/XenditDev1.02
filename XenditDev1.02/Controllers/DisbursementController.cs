using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Xendit.ApiClient;
using Xendit.ApiClient.Constants;
using Xendit.ApiClient.Disbursement;
using XenditDev1._02.Function;
using XenditDev1._02.ModelsNew;
using XenditDev1._02.Request;
using XenditDev1._02.Response;
using XenditDev1.Controllers;
using XenditDev1.ResponseClass;

namespace XenditDev1._02.Controllers
{
  
 
    [ApiController]
    public class DisbursementController : ControllerBase
    {
        Disbursement disb = new Disbursement();
        PaymentGatewayContext entities = new PaymentGatewayContext();

        public XenditDisbursementCreateResponse xenditDisbursement = new XenditDisbursementCreateResponse();
        public XenditBatchDisbursementCreateResponse xenditBatchDisbursement = new XenditBatchDisbursementCreateResponse();

        JObject json = JObject.Parse("{\r\n    \"Status\":\"Error\", \r\n\"StatusCode\":\"400\"\r\n,\"Description\":\"Duplicate Externalid\"\r\n}");

      
        [HttpPost]
        [Route("api/Disbursement")]
        public async Task<IActionResult> Posts([FromBody] object value)
        {


            JObject json = JObject.Parse("{\r\n    \"Status\":\"Error\", \r\n\"StatusCode\":\"400\"\r\n,\"Description\":\"Duplicate Externalid\"\r\n}");
            JObject json2 = JObject.Parse("{\r\n    \"Status\":\"Error\", \r\n\"StatusCode\":\"400\"\r\n,\"Description\":\"Wrong Request\"\r\n}");
            JObject json3 = JObject.Parse("{\r\n    \"Status\":\"Error\", \r\n\"StatusCode\":\"400\"\r\n,\"Description\":\"Dalam Perbaikan\"\r\n}");
            string token = HttpContext.Session.GetString("token");
            var userid = HttpContext.Session.GetInt32("userid");
            int idm = userid.GetValueOrDefault();

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            RequestDisbursment Request_ = new RequestDisbursment();
            Request_ = JsonConvert.DeserializeObject<RequestDisbursment>(value.ToString());

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

        

           
            int idtransaksipayment = 0;

            if (idm !=2) {
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

                    Disbursement Disbursement_ = new Disbursement();


                    String createDisbursement = Disbursement_.ChargeDisbursement(Request_);
                    ResponDisbursement jobj = JsonConvert.DeserializeObject<ResponDisbursement>(createDisbursement);

                    dynamic dynamicObjectdatum = JsonConvert.DeserializeObject(createDisbursement);
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
                    int idreslog = insertDB_.InsertLog(content2, content3, "http://192.168.0.8/xenditapi/api/Disbursement", Now);




                    try
                    {
                        idtransaksipayment = insertDB_.InsertTransaksiPayment(jobj.id, externalid, jobj.user_id, idm, jobj.account_holder_name, "", jobj.amount, "", jobj.disbursement_description, Now, "", jobj.status.ToString(), 0, 0, Now, Now, "IDR", Now);
                        insertDB_.InsertDetailLogTransaksi(idreslog, idtransaksipayment, xenditDisbursement.Description, Now);
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
                return Ok(json3);
            }

        }

        [HttpPost]
        [Route("api/BatchDisbursement")]
        public async Task<IActionResult> Posts1([FromBody] object value)
        {


            string token = HttpContext.Session.GetString("token");

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            BatchDisbursmenRequest Request_ = new BatchDisbursmenRequest();
            Request_ = JsonConvert.DeserializeObject<BatchDisbursmenRequest>(value.ToString());


            XenditBatchDisbursementCreateResponse respon = new XenditBatchDisbursementCreateResponse();
            Request.Headers.TryGetValue("Content-Type", out var ContentType_);
            Request.Headers.TryGetValue("Accept", out var Accept_);
            Request.Headers.TryGetValue("Authorization", out var Authorization_);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            string authBearer = Authorization_.ToString().Replace("Bearer ", "");
            authBearer = authBearer.Replace("{", "");
            authBearer = authBearer.Replace("}", "");
            string auth = token;


            string externalid = Request_.ExternalId;
            int amount = (int)Request_.Amount;
            XenditDisbursementBankCode bank = Request_.BankCode;
            string acccname = Request_.AccountHolderName;
            string accnumber = Request_.AccountNumber;
            string desc = Request_.Description;
            string[] emailto = Request_.Emailto;
            string refid = Request_.refid;
            ResponseStatusBatch resmessage = new ResponseStatusBatch();


            if (authBearer == auth)
            {
                xenditBatchDisbursement = await disb.BatchDisb(externalid, amount, bank, acccname, accnumber, desc,emailto,refid);
                resmessage.responseCode = "200";
                resmessage.responseMessage = "Success";
                resmessage.data = xenditBatchDisbursement;
            }
            else
            {
                resmessage.responseCode = "401";
                resmessage.responseMessage = "UnAutorizhed";
                resmessage.data = null;
            }

            return Ok(resmessage);
        }


        [HttpPost]
        [Route("api/DisbursementById")]
        public async Task<IActionResult> Posts2([FromBody] object value)
        {


            string token = HttpContext.Session.GetString("token");

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            DisbursmenRequestid Request_ = new DisbursmenRequestid();
            Request_ = JsonConvert.DeserializeObject<DisbursmenRequestid>(value.ToString());


            XenditDisbursementCreateResponse respon = new XenditDisbursementCreateResponse();
            Request.Headers.TryGetValue("Content-Type", out var ContentType_);
            Request.Headers.TryGetValue("Accept", out var Accept_);
            Request.Headers.TryGetValue("Authorization", out var Authorization_);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            string authBearer = Authorization_.ToString().Replace("Bearer ", "");
            authBearer = authBearer.Replace("{", "");
            authBearer = authBearer.Replace("}", "");
            string auth = token;


            string id ="";

          
            try
            {
                id = Request_.Id;
            }
            catch (System.Exception e)
            {
                id = "";
            }

            ResponseStatus resmessage = new ResponseStatus();


            if (authBearer == auth && id!="")
            {
             
                xenditDisbursement = await disb.GetById(id);
                string sJSONResponsedatum = JsonConvert.SerializeObject(xenditDisbursement);

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
                int idreslog = insertDB_.InsertLog(content2, content3, "http://192.168.0.8/xenditapi/api/DisbursementById", Now);

                return Ok(xenditDisbursement);
            }
            else
            {
                return Ok(json);
            }

        
        }

        [HttpPost]
        [Route("api/DisbursementByExternalid")]
        public async Task<IActionResult> Posts3([FromBody] object value)
        {


            string token = HttpContext.Session.GetString("token");

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            DisbursmenRequestexternalid Request_ = new DisbursmenRequestexternalid();
            Request_ = JsonConvert.DeserializeObject<DisbursmenRequestexternalid>(value.ToString());


            XenditDisbursementCreateResponse respon = new XenditDisbursementCreateResponse();
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
            ResponseStatus resmessage = new ResponseStatus();


            if (authBearer == auth && externalid!="")
            {

                xenditDisbursement = await disb.GetByExternalId(externalid);
                string sJSONResponsedatum = JsonConvert.SerializeObject(xenditDisbursement);

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
                int idreslog = insertDB_.InsertLog(content2, content3, "http://192.168.0.8/xenditapi/api/DisbursementByExternalid", Now);

                return Ok(xenditDisbursement);
            }
            else
            {
                return Ok(json);
            }

         
        }

    }






    public class DisbursmenRequest
    {



        public string external_id { get; set; }
        public int amount { get; set; }
        public string bank_code { get; set; }
        public string account_holder_name { get; set; }
        public string account_number { get; set; }
        public string description { get; set; }


    }
    public class DisbursmenRequestid
    {



        public string Id { get; set; }



    }

public class DisbursmenRequestexternalid
{



    public string ExternalId { get; set; }



}

public class BatchDisbursmenRequest
    {



        public string ExternalId { get; set; }


        public decimal Amount { get; set; }

        public XenditDisbursementBankCode BankCode { get; set; }


        public string AccountHolderName { get; set; }
        public string AccountNumber { get; set; }

        public string Description { get; set; }

        public string[] Emailto { get; set; }
        public string refid { get; set; }



    }

    public class ResponseStatus
    {
        public string responseCode { get; set; }
        public string responseMessage { get; set; }
        public XenditDisbursementCreateResponse data { get; set; }
    }

    public class ResponseStatusBatch
    {
        public string responseCode { get; set; }
        public string responseMessage { get; set; }
        public XenditBatchDisbursementCreateResponse data { get; set; }
    }


}