using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XenditDev1._02.Function
{
    public class FunctionInvoice
    {
        public String CreateInv(string externalid,decimal amount,string pyermail,string desc)
        {
            var client = new RestClient("https://api.xendit.co/v2/invoices");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Basic eG5kX3Byb2R1Y3Rpb25fV1hWVEJ3eG03RXF3ZzdqR3B1TDY1VFVNZkRVNnN0OWtqSElXeU9qTmlLVTlOeUF3QjAxV3E1dzBZY1dLczo=");
            request.AddHeader("Cookie", "nlbi_2182539=Hu3vJTwHQiB1UybsjjCKbQAAAABWLr2tpOlC3tlmypHe1Esw; visid_incap_2182539=ZMmQfU3PSKG8NOR8aWLLjBx1ZWEAAAAAQUIPAAAAAABf3f2zatTrYCiYvIqdKWUs; incap_ses_1113_2182539=iiwaSoJsNGaxJA8iESxyD2ubZmEAAAAAYnKdCaIe4BUUVoZknnL5LQ==");
            request.AddParameter("application/json", "{\n\t\"external_id\":"+'"' +externalid+ '"'+
                ",\n\t\"amount\":" + amount+
                ",\n\t\"payer_email\":"+ '"' + pyermail+ '"'+
                ",\n\t\"description\":"+ '"' + desc+ '"' + "\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return response.Content;
        }
    }
}
