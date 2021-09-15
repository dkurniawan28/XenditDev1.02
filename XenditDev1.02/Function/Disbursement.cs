using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xendit.ApiClient;
using Xendit.ApiClient.Constants;
using Xendit.ApiClient.Disbursement;

namespace XenditDev1._02.Function
{
    public class Disbursement
    {
        public string key = "xnd_development_e48fDUwdgdh8UIoYHmpk6Dvroi3DQcB0LUKg1fPxAk8GJCR388SM2fiOnmkmyS1";
        public XenditBatchDisbursementCreateResponse xenditDisbursement = new XenditBatchDisbursementCreateResponse();

        //public async Task<XenditBatchDisbursementCreateResponse> Disb()
        public async Task<XenditBatchDisbursementCreateResponse> Disb(string externalId, int amount, XenditDisbursementBankCode bank, string accName,
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

                    //ExternalId = "My Disb item Id 1",
                    //Amount = 10000,
                    //BankCode = XenditDisbursementBankCode.BRI_SYR,
                    //AccountHolderName = "Buye Loku",
                    //AccountNumber = "1234567890",
                    //Description = "Description for the item disbursement",
                    //EmailTo = new string[] { "buye.loku@example.com", "email2@example.com" }
                };


                xenditDisbursement = await xendit.Disbursement.CreateBatchAsync(new XenditBatchDisbursementCreateRequest
                {
                    Reference = ReffId,
                    Disbursements = new List<XenditBatchDisbursementCreateRequestItem> { item}
                });
            }
            catch (Exception ex)
            {

            }
            return xenditDisbursement;
        }
    }
}
