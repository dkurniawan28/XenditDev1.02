using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xendit.ApiClient;
using Xendit.ApiClient.Constants;
using Xendit.ApiClient.VirtualAccount;

namespace XenditDev1._02.Controllers
{
    public class program
    {
       
        private async Task MainBusiness()
        {
            var xendit = new XenditClient("PUT YOUR API KEY HERE");

            // You can create non-specified fixed VA number by not providing `VirtualAccountNumber` property value.    
            var requestedVA = new XenditVACreateRequest
            {
                ExternalId = "VA_fixed-1234567890",
                Name = "Steve Woznike",
                BankCode = XenditVABankCode.MANDIRI,
                VirtualAccountNumber = "9999000002"
            };

            var va = await xendit.VirtualAccount.CreateAsync(requestedVA);
        }
    }
}
