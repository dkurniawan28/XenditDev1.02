using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xendit.ApiClient;
using Xendit.ApiClient.Constants;
using Xendit.ApiClient.EWallet;

namespace XenditDev1._02.Function
{
    public class Ewallet
    {
        // public string key = "xnd_development_e48fDUwdgdh8UIoYHmpk6Dvroi3DQcB0LUKg1fPxAk8GJCR388SM2fiOnmkmyS1";
        public string key = "xnd_production_WXVTBwxm7Eqwg7jGpuL65TUMfDU6st9kjHIWyOjNiKU9NyAwB01Wq5w0YcWKs";
        XenditEWalletCreatePaymentResponse XenditWallet = new XenditEWalletCreatePaymentResponse();
        public async Task<XenditEWalletCreatePaymentResponse> Ovo(string externalId, decimal amount, string phone)

        {
            try
            {
                var xendit = new XenditClient(key);
                var ovoPayment = new XenditEWalletCreateOvoPaymentRequest
                {
                    ExternalId = externalId,
                    Amount = amount,
                    Phone = phone
                   


              
                };


                XenditWallet = await xendit.EWallet.CreateOvoPaymentAsync(ovoPayment);
            }
            catch (Exception ex)
            {

            }
            return XenditWallet;
        }

      

        public async Task<XenditEWalletCreatePaymentResponse> Dana(string externalId, decimal amount, string callbak, string redirek)

        {
            try
            {
                var xendit = new XenditClient(key);
                var danaPayment = new XenditEWalletCreateDanaPaymentRequest
                {
                    ExternalId = externalId,
                    Amount = amount,
                    CallbackUrl = callbak,
                    RedirectUrl=redirek




                };


                XenditWallet = await xendit.EWallet.CreateDanaPaymentAsync(danaPayment);
            }
            catch (Exception ex)
            {

            }
            return XenditWallet;
        }

        public async Task<XenditEWalletCreatePaymentResponse> LinkAja(string externalId, decimal amount, string callbak, string redirek,string phones, List<XenditEWalletCreateLinkAjaPaymentRequestItem> item)

        {
            XenditEWalletCreateLinkAjaPaymentRequestItem lisitem = new XenditEWalletCreateLinkAjaPaymentRequestItem();
            try
            {
                var xendit = new XenditClient(key);


                var linkAjaPaymentItems = new List<XenditEWalletCreateLinkAjaPaymentRequestItem>
                {
                    new XenditEWalletCreateLinkAjaPaymentRequestItem
                    {
                        Id = lisitem.Id,
                        Name = lisitem.Name,
                        Price = lisitem.Price,
                        Quantity = lisitem.Quantity
                    }
                };

                var linkAjaPayment = new XenditEWalletCreateLinkAjaPaymentRequest
                {
                    ExternalId =externalId,
                    Amount = amount,
                    CallbackUrl = callbak,
                    RedirectUrl =redirek,
                    Phone = phones,
                    Items = linkAjaPaymentItems
                };

                XenditWallet = await xendit.EWallet.CreateLinkAjaPaymentAsync(linkAjaPayment);


             
            }
            catch (Exception ex)
            {

            }
            return XenditWallet;
        }

        public async Task<XenditEWalletCreatePaymentResponse> GetPayment(string externalid, XenditEWalletType tipe)
        {
            try
            {
                var xendit = new XenditClient(key);
                XenditWallet = await xendit.EWallet.GetPaymentStatusAsync(externalid, tipe);
            }
            catch (Exception ex)
            {

            }
            return XenditWallet;
        }

       
    }
}
