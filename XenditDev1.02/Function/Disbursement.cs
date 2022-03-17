using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xendit.ApiClient;
using Xendit.ApiClient.Constants;
using Xendit.ApiClient.Disbursement;
using Xendit.ApiClient.Models;
using XenditDev1._02.Request;

namespace XenditDev1._02.Function
{
    public class Disbursement
    {
        // public string key = "xnd_development_e48fDUwdgdh8UIoYHmpk6Dvroi3DQcB0LUKg1fPxAk8GJCR388SM2fiOnmkmyS1";
        public string key = "xnd_production_WXVTBwxm7Eqwg7jGpuL65TUMfDU6st9kjHIWyOjNiKU9NyAwB01Wq5w0YcWKs";
        public XenditBatchDisbursementCreateResponse xenditBatchDisbursement = new XenditBatchDisbursementCreateResponse();
        public XenditDisbursementCreateResponse xenditDisbursement= new XenditDisbursementCreateResponse();
        public IEnumerable<XenditDisbursementBank> xenditDisbursementBank;

        //public async Task<XenditBatchDisbursementCreateResponse> Disb()

        
        public async Task<XenditBatchDisbursementCreateResponse> BatchDisb(string externalId, int amount, XenditDisbursementBankCode bank, string accName,
        string accNumber, string Desc, string[] emailNotif,string ReffId)

        {
            try
            {
                var xendit = new XenditClient(key);
                var item = new XenditBatchDisbursementCreateRequestItem
                {
                    ExternalId = externalId,
                    Amount = amount,
                    BankCode = bank,
                    AccountHolderName = accName,
                    AccountNumber = accNumber,
                    Description = Desc,
                    EmailTo = emailNotif

                   
                };


                xenditBatchDisbursement = await xendit.Disbursement.CreateBatchAsync(new XenditBatchDisbursementCreateRequest
                {
                    Reference = ReffId,
                    Disbursements = new List<XenditBatchDisbursementCreateRequestItem> { item}
                });
            }
            catch (Exception ex)
            {

            }
            return xenditBatchDisbursement;
        }



        public async Task<XenditDisbursementCreateResponse> Disbursmen(string externalId, int amount, XenditDisbursementBankCode bank, string accName,
        string accNumber, string Desc)

        {
           
            try
            {
                var xendit = new XenditClient(key);
                var item = new XenditDisbursementCreateRequest
                {
                    ExternalId = externalId,
                    Amount = amount,
                    BankCode = bank,
                    AccountHolderName = accName,
                    AccountNumber = accNumber
                  

                   
                };


                xenditDisbursement = await xendit.Disbursement.CreateAsync(item);
            }
            catch (Exception ex)
            {

            }
            return xenditDisbursement;
        }

        public async Task<XenditDisbursementCreateResponse> GetByExternalId(string externalid)
        {
            try
            {
                var xendit = new XenditClient(key);
                xenditDisbursement= await xendit.Disbursement.GetByExternalIdAsync(externalid);
            }
            catch (Exception ex)
            {

            }
            return xenditDisbursement;
        }

        public async Task<XenditDisbursementCreateResponse> GetById(string id)
        {
            try
            {
                var xendit = new XenditClient(key);
                xenditDisbursement = await xendit.Disbursement.GetByIdAsync(id);
            }
            catch (Exception ex)
            {

            }
            return xenditDisbursement;
        }

        public async Task<IEnumerable<XenditDisbursementBank>> GetAvailableBank()
        {
            try
            {
                var xendit = new XenditClient(key);
                xenditDisbursementBank = await xendit.Disbursement.GetAvailableBanksAsync();
            }
            catch (Exception ex)
            {

            }
            return xenditDisbursementBank;
        }


        public String ChargeDisbursement(RequestDisbursment reqCC)
        {
            var client = new RestClient("https://api.xendit.co/disbursements");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(reqCC);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Basic eG5kX3Byb2R1Y3Rpb25fV1hWVEJ3eG03RXF3ZzdqR3B1TDY1VFVNZkRVNnN0OWtqSElXeU9qTmlLVTlOeUF3QjAxV3E1dzBZY1dLczo=");
            request.AddHeader("Cookie", "nlbi_2182539=Hu3vJTwHQiB1UybsjjCKbQAAAABWLr2tpOlC3tlmypHe1Esw; visid_incap_2182539=ZMmQfU3PSKG8NOR8aWLLjBx1ZWEAAAAAQUIPAAAAAABf3f2zatTrYCiYvIqdKWUs; incap_ses_1113_2182539=3ZX8IUo38XTcwK8hESxyD59/ZWEAAAAAjwFplEpzPltXdF9SrBCKhg==");
            request.AddParameter("application/json", jsonString, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            //   ResponseChargeCC respCC= (ResponseChargeCC)JsonConvert.DeserializeObject(response.Content);
            return response.Content;
        }
    }
}
