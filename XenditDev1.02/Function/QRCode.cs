using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XenditDev1._02.ModelsNew;

namespace XenditDev1._02.Function
{
    public class QRCode
    {

        public String CreateQR(RequestQR qr)
        {
            var client = new RestClient("https://api.xendit.co/qr_codes");
            client.Timeout = -1;
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(qr);
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Basic eG5kX3Byb2R1Y3Rpb25fV1hWVEJ3eG03RXF3ZzdqR3B1TDY1VFVNZkRVNnN0OWtqSElXeU9qTmlLVTlOeUF3QjAxV3E1dzBZY1dLczo=");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "nlbi_2182539=Hu3vJTwHQiB1UybsjjCKbQAAAABWLr2tpOlC3tlmypHe1Esw; visid_incap_2182539=ZMmQfU3PSKG8NOR8aWLLjBx1ZWEAAAAAQUIPAAAAAABf3f2zatTrYCiYvIqdKWUs; incap_ses_1113_2182539=Xl4zZKnKJROvRashESxyDx11ZWEAAAAAIiYLwbrUMu/dXEC3nQVElw==");
            request.AddParameter("application/json", jsonString, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            //CreateQR createQRs =(CreateQR) JsonConvert.DeserializeObject(response.Content);
            //var jsonStringCreate = Newtonsoft.Json.JsonConvert.SerializeObject(createQRs);
            //dynamic dynamicObjectdatum = JsonConvert.DeserializeObject(jsonStringCreate);

            return response.Content;
        }

        public String StatusQR(String extId)
        {
            var client = new RestClient("https://api.xendit.co/qr_codes/"+extId);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Basic eG5kX3Byb2R1Y3Rpb25fV1hWVEJ3eG03RXF3ZzdqR3B1TDY1VFVNZkRVNnN0OWtqSElXeU9qTmlLVTlOeUF3QjAxV3E1dzBZY1dLczo=");
            request.AddHeader("Cookie", "nlbi_2182539=Hu3vJTwHQiB1UybsjjCKbQAAAABWLr2tpOlC3tlmypHe1Esw; visid_incap_2182539=ZMmQfU3PSKG8NOR8aWLLjBx1ZWEAAAAAQUIPAAAAAABf3f2zatTrYCiYvIqdKWUs; incap_ses_1113_2182539=Xl4zZKnKJROvRashESxyDx11ZWEAAAAAIiYLwbrUMu/dXEC3nQVElw==");
            request.AddParameter("text/plain", "", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
          //  CreateQR createQR = (CreateQR)JsonConvert.DeserializeObject(response.Content);
            return response.Content;
        }
    }
}
