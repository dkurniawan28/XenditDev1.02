using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xendit.ApiClient.Constants;
using Xendit.ApiClient.Invoice;
using XenditDev1._02.Function;
using XenditDev1._02.ModelsNew;
using XenditDev1._02.Response;

namespace XenditDev1._02.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {

        JObject json = JObject.Parse("{\r\n    \"Status\":\"Error\", \r\n\"StatusCode\":\"400\"\r\n,\"Description\":\"Failed\"\r\n}");


        IEnumerable<XenditInvoiceCreateResponse> XenditInvoice2;
        Invoice Invoice = new Invoice();
        PaymentGatewayContext entities = new PaymentGatewayContext();

        [HttpPost]
        [Route("api/Invoice")]
        public async Task<IActionResult> Posts([FromBody] object value)
        {

            InvoiceResponse XenditInvoice = new InvoiceResponse();

            string token = HttpContext.Session.GetString("token");
            var userid = HttpContext.Session.GetInt32("userid");
            int idm = userid.GetValueOrDefault();
            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            InvoiceRequest Request_ = new InvoiceRequest();
            Request_ = JsonConvert.DeserializeObject<InvoiceRequest>(value.ToString());

            XenditInvoiceCreateResponse respon = new XenditInvoiceCreateResponse();
            Request.Headers.TryGetValue("Content-Type", out var ContentType_);
            Request.Headers.TryGetValue("Accept", out var Accept_);
            Request.Headers.TryGetValue("Authorization", out var Authorization_);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            string authBearer = Authorization_.ToString().Replace("Bearer ", "");
            authBearer = authBearer.Replace("{", "");
            authBearer = authBearer.Replace("}", "");
            string auth = token;
            string externalid = "";
            decimal amount = 0;
            string pyermail = "";
            try
            {
                 externalid = Request_.external_id;
            }
            catch (System.Exception e)
            {
                externalid = "";
            }

            try
            {
                amount = (decimal)Request_.amount;
            }
            catch (System.Exception e)
            {
                amount = 0;
            }

            try
            {
                pyermail = Request_.payer_email;
            }
            catch (System.Exception e)
            {
                pyermail = "";
            }


            string email = "";
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(pyermail);
            if (match.Success)
                email = pyermail;
            else
                email = "kusnur9@gmail.com";
            string desc = Request_.description;
            ResponseStatusInvoice resmessage = new ResponseStatusInvoice();

            FunctionInvoice functionInvoice = new FunctionInvoice(); 

            if (authBearer == auth && externalid!="")
            {

                //XenditInvoice = await Invoice.CreateInvoices(externalid, amount, pyermail,desc);
                String resp = functionInvoice.CreateInv(externalid, amount, email,desc);
                XenditInvoice = JsonConvert.DeserializeObject<InvoiceResponse>(resp);

                string sJSONResponsedatum = JsonConvert.SerializeObject(resp);

                dynamic dynamicObjectdatum = JsonConvert.DeserializeObject(sJSONResponsedatum);
                var dt = @"" + dynamicObjectdatum;
                JObject jsonpar = JObject.Parse(dt);

                var bd = (string)dt.Replace("\r\n", "");

              String  content3 = bd;


                string sJSONResponsedatum2 = JsonConvert.SerializeObject(Request_);

                dynamic dynamicObjectdatum2 = JsonConvert.DeserializeObject(sJSONResponsedatum2);
                var dt2 = @"" + dynamicObjectdatum2;

                var bd2 = (string)dt2.Replace("\r\n", "");

                String content2 = bd2;

                DateTime Now = DateTime.Now;
                InsertDB insertDB_ = new InsertDB();
                int idreslog = 0;
                try
                {
                    idreslog = insertDB_.InsertLog(content2, content3, "http://192.168.0.8/xenditapi/api/Invoice", Now);
                }
                catch (System.Exception e)
                {
                 
                    idreslog = 0;
                }

                string idxendit = XenditInvoice.id;
                bool testBool = XenditInvoice.should_exclude_credit_card;
                bool testBool2 = XenditInvoice.should_send_email;
                int ShouldExcludeCreditCard = testBool ? 1 : 0;
                int ShouldSendEmail = testBool ? 1 : 0;
                DateTime expiry = XenditInvoice.expiry_date.AddHours(7);
                try
                {
                    int idtransaksipayment = insertDB_.InsertTransaksiPayment(idxendit, XenditInvoice.external_id, XenditInvoice.external_id, idm, XenditInvoice.merchant_name, XenditInvoice.merchant_profile_picture_url, XenditInvoice.amount, XenditInvoice.payer_email, XenditInvoice.description, expiry, XenditInvoice.invoice_url, XenditInvoice.status.ToString(), ShouldExcludeCreditCard, ShouldSendEmail, XenditInvoice.created.AddHours(7), XenditInvoice.updated.AddHours(7), XenditInvoice.currency, Now);
                    insertDB_.InsertDetailLogTransaksi(idreslog, idtransaksipayment, XenditInvoice.description, Now);

                    var list = XenditInvoice.available_banks.ToList();
                    int counbank = list.Count;

                    for (int i = 0; i < counbank; i++)
                    {
                        int identiti = Convert.ToInt32(list[i].identity_amount);
                        insertDB_.InsertDetailBank(list[i].bank_code, list[i].collection_type, list[i].bank_account_number, list[i].transfer_amount, list[i].bank_branch, list[i].account_holder_name, identiti, 0, idtransaksipayment);
                    }

                    try
                    {
                        var listoutlet = XenditInvoice.available_retail_outlets.ToList();
                        int counoutlet = listoutlet.Count;

                        for (int i = 0; i < counoutlet; i++)
                        {

                            insertDB_.InsertDetailOutlet(listoutlet[i].retail_outlet_name, listoutlet[i].payment_code, listoutlet[i].transfer_amount, listoutlet[i].merchant_name, 0, idtransaksipayment);
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                   
                    var listwallet = XenditInvoice.available_ewallets.ToList();
                    int counwallet = listwallet.Count;
                    for (int i = 0; i < counwallet; i++)
                    {

                        insertDB_.InsertDetailWallet(listwallet[i].ewallet_type,0,idtransaksipayment);
                    }
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
        [Route("api/InvoiceById")]
        public async Task<IActionResult> Posts2([FromBody] object value)
        {


            XenditInvoiceCreateResponse XenditInvoice = new XenditInvoiceCreateResponse();
            string token = HttpContext.Session.GetString("token");

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            InvoiceRequestid Request_ = new InvoiceRequestid();
            Request_ = JsonConvert.DeserializeObject<InvoiceRequestid>(value.ToString());


            XenditInvoiceCreateResponse respon = new XenditInvoiceCreateResponse();
            Request.Headers.TryGetValue("Content-Type", out var ContentType_);
            Request.Headers.TryGetValue("Accept", out var Accept_);
            Request.Headers.TryGetValue("Authorization", out var Authorization_);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            string authBearer = Authorization_.ToString().Replace("Bearer ", "");
            authBearer = authBearer.Replace("{", "");
            authBearer = authBearer.Replace("}", "");
            string auth = token;


            string id = "";

            try
            {
                id = Request_.idinvoice;
            }
            catch (System.Exception e)
            {
                id = "";
            }

            ResponseStatusInvoice resmessage = new ResponseStatusInvoice();


            if (authBearer == auth && id!="")
            {
                XenditInvoice = await Invoice.GetInvoiceById(id);

                string sJSONResponsedatum = JsonConvert.SerializeObject(XenditInvoice);

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
                int idreslog = insertDB_.InsertLog(content2, content3, "http://192.168.0.8/xenditapi/api/InvoiceById", Now);

                return Ok(XenditInvoice);
            }
            else
            {
                return Ok(json);
            }

            
        }

        [HttpPost]
        [Route("api/InvoiceAll")]
        public async Task<IActionResult> Posts3([FromBody] object value)
        {


            string token = HttpContext.Session.GetString("token");

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            InvoiceRequestAll Request_ = new InvoiceRequestAll();
            Request_ = JsonConvert.DeserializeObject<InvoiceRequestAll>(value.ToString());


            XenditInvoiceCreateResponse respon = new XenditInvoiceCreateResponse();
            Request.Headers.TryGetValue("Content-Type", out var ContentType_);
            Request.Headers.TryGetValue("Accept", out var Accept_);
            Request.Headers.TryGetValue("Authorization", out var Authorization_);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            string authBearer = Authorization_.ToString().Replace("Bearer ", "");
            authBearer = authBearer.Replace("{", "");
            authBearer = authBearer.Replace("}", "");
            string auth = token;
            XenditInvoiceStatus sts = new XenditInvoiceStatus();

            int limit = Request_.Limit;
            List<XenditInvoiceStatus> status = new List<XenditInvoiceStatus> { XenditInvoiceStatus.SETTLED, XenditInvoiceStatus.EXPIRED, XenditInvoiceStatus.PENDING, XenditInvoiceStatus.PAID };
            XenditInvoiceOptions options = new XenditInvoiceOptions();
            options.Limit = limit;
            options.Statuses = status;
            ResponseStatusInvoice resmessage = new ResponseStatusInvoice();


            if (authBearer == auth)
            {
                XenditInvoice2 = await Invoice.GetAllInvoice(options);

                resmessage.responseCode = "200";
                resmessage.responseMessage = "Success";
                //resmessage.data = XenditInvoice;
                return Ok(resmessage);
            }
            else
            {
             
                return Ok(json);
            }

          
        }

        [HttpPost]
        [Route("api/GetListTransaksi")]
        public async Task<IActionResult> Posts4([FromBody] object value)
        {


            XenditInvoiceCreateResponse XenditInvoice = new XenditInvoiceCreateResponse();
            string token = HttpContext.Session.GetString("token");

            List<ItemBankResponse> listbanks = new List<ItemBankResponse>();

            List<ItemOutletResponse> listoutlets = new List<ItemOutletResponse>();

            List<ItemEwalletRespon> listwallete = new List<ItemEwalletRespon>();

            var jsonString = JsonConvert.DeserializeObject(value.ToString());
            jsonString = JsonConvert.SerializeObject(jsonString, Formatting.None);
            ReqExternalid Request_ = new ReqExternalid();
            Request_ = JsonConvert.DeserializeObject<ReqExternalid>(value.ToString());


            XenditInvoiceCreateResponse respon = new XenditInvoiceCreateResponse();
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
                externalid = Request_.externalid;
            }
            catch (System.Exception e)
            {
                externalid = "";
            }

            ResponseStatusInvoice resmessage = new ResponseStatusInvoice();


            if (authBearer == auth && externalid != "")
            {

                var listtransaksi = entities.TransaksiPayment.Where(p=>p.ExternalId==externalid).ToList();
                int idtransaksipayment = listtransaksi[0].IdTransaksiPayment;
                int countr = listtransaksi.Count;

                if (countr>0)
                {


                    DateTime date = listtransaksi[0].ExpiryDate.GetValueOrDefault();
                    string t1 = date.ToString("yyyy-MM-dd HH:mm:ss");
                    DateTime date2 = listtransaksi[0].Created.GetValueOrDefault();
                    string t2 = date2.ToString("yyyy-MM-dd HH:mm:ss");
                    DateTime date3 = listtransaksi[0].Updated.GetValueOrDefault();
                    string t3 = date3.ToString("yyyy-MM-dd HH:mm:ss");
                    DateTime date4 = listtransaksi[0].Timestamp.GetValueOrDefault();
                    string t4 = date4.ToString("yyyy-MM-dd HH:mm:ss");
                    var listtbank = entities.DetailBank.Where(p => p.IdTransaksiPayment == idtransaksipayment).ToList();
                    var listtoutlet= entities.DetailOutlet.Where(p => p.IdTransaksiPayment == idtransaksipayment).ToList();
                    var listtwallet = entities.DetailEwallet.Where(p => p.IdTransaksiPayment == idtransaksipayment).ToList();

                    for (int i=0;i<listtbank.Count;i++)
                    {
                        listbanks.Add(new ItemBankResponse() { bank_code = listtbank[i].BankCode, collection_type = listtbank[i].CollectionType,
                        bank_account_number=listtbank[i].BankAccountNumber,transfer_amount=(int)listtbank[0].TransferAmount.GetValueOrDefault(),bank_branch=listtbank[i].BankBranch,
                        account_holder_name=listtbank[i].AccountHolderName,identity_amount=(int)listtbank[i].IdentityAmount,status=(int)listtbank[i].Status});
                    }

                    for (int i = 0; i < listtoutlet.Count; i++)
                    {
                        listoutlets.Add(new ItemOutletResponse()
                        {
                            retail_outlet_name = listtoutlet[i].RetailOutletName,
                            payment_code = listtoutlet[i].PaymentCode,
                            transfer_amount =(int) listtoutlet[i].TransferAmount.GetValueOrDefault(),
                            merchant_name = listtoutlet[i].MerchantName,
                            status = (int)listtoutlet[i].Status
                        });
                    }

                    for (int i = 0; i < listtwallet.Count; i++)
                    {
                        listwallete.Add(new ItemEwalletRespon()
                        {
                            ewallet_type = listtwallet[i].EwalletType,
                           
                            status = (int)listtwallet[i].Status
                        });
                    }

                    TransaksiResponse TransaksiResponse_ = new TransaksiResponse();
                    TransaksiResponse_.IdTransaksiPayment = idtransaksipayment;
                    TransaksiResponse_.IdXendit = listtransaksi[0].IdXendit;
                    TransaksiResponse_.IdMerchan = listtransaksi[0].IdMerchan.GetValueOrDefault();
                    TransaksiResponse_.external_id = listtransaksi[0].ExternalId;
                    TransaksiResponse_.user_id = listtransaksi[0].UserId;
                    TransaksiResponse_.status = listtransaksi[0].Status;
                    TransaksiResponse_.merchant_name = listtransaksi[0].MerchantName;
                    TransaksiResponse_.merchant_profile_picture_url = listtransaksi[0].MerchantProfilePictureUrl;
                    TransaksiResponse_.amount = listtransaksi[0].Amount.GetValueOrDefault();
                    TransaksiResponse_.payer_email = listtransaksi[0].PayerEmail;
                    TransaksiResponse_.description = listtransaksi[0].Description;
                    TransaksiResponse_.expiry_date = t1;
                    TransaksiResponse_.invoice_url = listtransaksi[0].InvoiceUrl;
                    TransaksiResponse_.should_exclude_credit_card = Convert.ToBoolean(listtransaksi[0].ShouldExcludeCreditCard);
                    TransaksiResponse_.should_send_email = Convert.ToBoolean(listtransaksi[0].ShouldSendEmail);
                    TransaksiResponse_.created = t2;
                    TransaksiResponse_.updated = t3;
                    TransaksiResponse_.currency = listtransaksi[0].Currency;
                    TransaksiResponse_.Timestamp = t4;
                    TransaksiResponse_.available_banks = listbanks;
                    TransaksiResponse_.available_retail_outlets = listoutlets;
                    TransaksiResponse_.available_ewallets = listwallete;

                    string sJSONResponsedatum = JsonConvert.SerializeObject(TransaksiResponse_);

                    dynamic dynamicObjectdatum = JsonConvert.DeserializeObject(sJSONResponsedatum);
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
                    int idreslog = insertDB_.InsertLog(content2, content3, "http://192.168.0.8/xenditapi/api/GetListTransaksi", Now);

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

        public class InvoiceRequest
        {
           
            public string external_id { get; set; }
            public decimal amount { get; set; }
            public string payer_email { get; set; }
            public string description { get; set; }
        }

        public class ResponseStatusInvoice
        {
            public string responseCode { get; set; }
            public string responseMessage { get; set; }
            public XenditInvoiceCreateResponse data { get; set; }
        }

        public class InvoiceRequestid
        {
            public string idinvoice { get; set; }
        }

        public class ReqExternalid
        {
            public string externalid { get; set; }
        }


        public class InvoiceRequestAll
        {
            public int Limit { get; set; }
          
    }
    }
}