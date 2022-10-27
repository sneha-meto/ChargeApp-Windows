using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ChargeApp
{
    internal class DataService
    {

        static HttpClient client = new HttpClient();

        public DataService()
        {
            client.BaseAddress = new Uri("https://d007-111-92-76-192.in.ngrok.io/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task addChargeAsync(ChargeModel data)
        {
            JsonContent content = JsonContent.Create(data);
            var response = await client.PostAsync("charge",content);
        }

        public async Task<List<DischargeModel>> getChargeCycle()
        {
          
            var response = await client.GetAsync("charge/cycle");
            List < DischargeModel > data = await response.Content.ReadAsAsync <List<DischargeModel>>();

            return data;
        }

        public async Task<List<int>> getChargePattern()
        {

            //var response = await client.GetAsync("charge/pattern");
            //String data =await  response.Content.ReadAsStringAsync();
            //Debug.Print(data);
           
            HttpResponseMessage response = await client.GetAsync("charge/pattern");
            List<int> data = await response.Content.ReadAsAsync<List<int>>();
           
            return data;

        }


    }
}
