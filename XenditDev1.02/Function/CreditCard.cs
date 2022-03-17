using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XenditDev1._02.ModelsNew;

namespace XenditDev1._02.Function
{
    public class CreditCard
    {
        public String ChargeCC(RequestCC reqCC)
        {
            var client = new RestClient("https://api.xendit.co/credit_card_charges");
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
