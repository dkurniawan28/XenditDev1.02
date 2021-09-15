﻿using Moq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xendit.ApiClient;
using Xendit.ApiClient.Abstracts;
using Xendit.ApiClient.Constants;
using Xendit.ApiClient.Models;
using Xendit.ApiClient.VirtualAccount;
using XenditDev1._02.Helpers;
using Xunit;
using Newtonsoft.Json;
using XenditDev1._02.Response;

namespace XenditDev1._02.Function
{
    public class VirtualAccount
    {
        public string key = "xnd_development_e48fDUwdgdh8UIoYHmpk6Dvroi3DQcB0LUKg1fPxAk8GJCR388SM2fiOnmkmyS1";
        public async Task VaAsync (string nama,int bank, string vaNumber, string externalId,int amount)
        {
            try
            {
                var xendit = new XenditClient(key);
                var codeBank = XenditVABankCode.MANDIRI;
                switch (bank)
                {
                    case 1:
                        // mandiri
                        codeBank = XenditVABankCode.MANDIRI;
                        break;
                    case 2:
                        // bca
                        codeBank = XenditVABankCode.BCA;
                        break;
                    case 3:
                        // permata
                        codeBank = XenditVABankCode.PERMATA;
                        break;
                    case 4:
                        // code BRI
                        codeBank = XenditVABankCode.BRI;
                        break;
                    case 5:
                        // code BNI
                        codeBank = XenditVABankCode.BNI;
                        break;
                }
                var requestedVA = new XenditVACreateRequest
                {
                    ExternalId = externalId,
                    BankCode = codeBank,
                    //VirtualAccountNumber = vaNumber,
                    ExpectedAmount = amount,
                    Name = nama
                    
                };
                var va = await xendit.VirtualAccount.CreateAsync(requestedVA);
            }
            catch (Exception ex)
            {

            }
            
        }


    }


}