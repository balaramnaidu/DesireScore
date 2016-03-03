using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace MailingAddress
{
    internal class Program
    {
        private static readonly string _localBaseUri = "http://localhost:50345";
        private static readonly string _releaseBaseUri = "https://microsoft-apiappb8aa4223110d49739ba98efc32252162.azurewebsites.net/";

        public static void Main(string[] args)
        {
            var baseUri = (args[4] != null && args[4].Trim().ToLower() == "local") ? _localBaseUri : _releaseBaseUri; 
            var score = GetDesirabilityScore(args[0], args[1], Convert.ToUInt32(args[2]), Convert.ToUInt32(args[3]), baseUri).DesirabilityScore;
            Console.WriteLine("Score: {0}\tBaseUri: {1}", score, baseUri);
        }

        private static PropertyInfo GetDesirabilityScore(string address1, string city, uint listedPrice, uint hoa, string baseUri)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                var result = client.GetAsync("/api/Score/Get/?Address1=" + address1.Replace(" ", "+") + "&City=" + city +
                                    "&ListedPrice=" + listedPrice + "&Hoa=" + hoa + "&State=CA" + "&zillowServiceId=X1-ZWz19u37vgjjez_8gte4").Result;

                var jsonAsString = result.Content.ReadAsStringAsync().Result;
                var output = JsonConvert.DeserializeObject<PropertyInfo>(jsonAsString);

                return output;
            }
        }
    }
}

//public static void Main(string[] args)
    //    {
    //        string address1 = "13638+Felson+St";
    //        string city = "Cerritos";
    //        string listedPrice = "354000";
    //        string hoa = "200";

    //        using (var client = new HttpClient())
    //        {
    //            client.BaseAddress = new Uri("https://microsoft-apiappb8aa4223110d49739ba98efc32252162.azurewebsites.net/");
    //            var result = client.GetAsync("/api/Score/Get/?Address1=" + address1.Replace(" ", "+") + "&City=" + city + "&ListedPrice=" + 
    //                                             listedPrice + "&Hoa=" + hoa + "&State=CA").Result;
    //            var resultContent = result.Content.ReadAsStringAsync().Result;
    //            Console.WriteLine(resultContent);
    //            Console.Read();
    //        }
    //    }

        //public static void Main(string[] args)
        //{
        //    HttpWebRequest http = (HttpWebRequest)HttpWebRequest.Create("http://www.zillow.com/homes/90241_rb/");
        //    HttpWebResponse response = (HttpWebResponse )http.GetResponse();
        //    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
        //    {
        //        string responseJson = sr.ReadToEnd();
        //        Regex regex = new Regex("<img rel=\"nofollow\" alt=\"");

        //        string searchstring = "<img rel=\"nofollow\" alt=\"";
        //        string searchstring1 = ", CA\"";

        //        List<int> ret = new List<int>();
        //        int len = searchstring.Length;
        //        int start = -len;
        //        while (true)
        //        {
        //            start = responseJson.IndexOf(searchstring, start + len);
        //            if (start == -1)
        //            {
        //                break;
        //            }
        //            else
        //            {
        //                ret.Add(start);
        //            }
        //        }
               
        //        foreach (int a in ret)
        //        {
        //            string startAddressFindString = responseJson.Substring(a,100).Replace(searchstring,"");
        //            int ind = startAddressFindString.IndexOf(searchstring1);
        //            if(ind>0)
        //                Console.WriteLine(startAddressFindString.Substring(0, ind));
        //        }
        //    }
        //    Console.Read();
        //}
//    }
//}
