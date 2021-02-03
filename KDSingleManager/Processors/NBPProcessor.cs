using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KDSingleManager.Processors
{
    public class NBPProcessor
    {
        HttpClient client;// = new HttpClient();

        public async Task<ExRate> getRate(string date)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            ExRate ex = new ExRate();
            DateTime dt = DateTime.Parse(date).AddDays(-1);
            string dtStr = dt.ToString("yyyy-MM-dd");

            string req = $"http://api.nbp.pl/api/exchangerates/rates/a/eur/{dtStr}";


            var response = await client.GetAsync(req);
            if (response.IsSuccessStatusCode)
            {
                ex = await response.Content.ReadAsAsync<ExRate>();
                return ex;
            }
            else
            {
                //for (; !response.IsSuccessStatusCode;)
                while (!response.IsSuccessStatusCode)
                {
                    {
                        req = $"http://api.nbp.pl/api/exchangerates/rates/a/eur/{dtStr}";

                        response = await client.GetAsync(req);
                        if (response.IsSuccessStatusCode)
                        {
                            ex = await response.Content.ReadAsAsync<ExRate>();
                            // return ex;
                        }
                        else
                        {
                            dtStr = DateTime.Parse(dtStr).AddDays(-1).ToShortDateString();
                        }
                    }
                }
            }


            return ex;
        }
    }

    public class ExRate
    {
        public string table { get; set; }
        public string currency { get; set; }
        public string code { get; set; }
        public Rate[] rates { get; set; }

        public float GetRate()
        {
            return this.rates[0].mid;
        }

        public string GetDate()
        {
            return this.rates[0].effectiveDate;
        }
    }

    public class Rate
    {
        public string no { get; set; }
        public string effectiveDate { get; set; }
        public float mid { get; set; }
    }

}
