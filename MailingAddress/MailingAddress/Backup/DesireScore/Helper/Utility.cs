using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace DesireScore.Helper
{
    public static class Utility
    {
        public static string GetScore(string address, string city, string listedPrice, string hoa)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    listedPrice = listedPrice.Replace(",","");
                    listedPrice = listedPrice.Replace("$","");

                    client.BaseAddress = new Uri("https://microsoft-apiappb8aa4223110d49739ba98efc32252162.azurewebsites.net/");
                    var result = client.GetAsync("/api/Score/Get/?Address1=" + address.Replace(" ", "+") + "&City=" + city + "&ListedPrice=" +
                                                     listedPrice + "&Hoa=" + hoa + "&State=CA").Result;

                    if (result.Content.ReadAsStringAsync().Result.Contains("An error has occurred"))
                    {
                        return "";
                    }
                    else
                    {
                        return result.Content.ReadAsStringAsync().Result;
                    }

                }
            }
            catch(Exception ex)
            {
                return "";
            }
        }
        public static List<int> PrepareListForHomeAddressSearch(string responseJson, string searchstring)
        {
            int len = searchstring.Length;
            var ret = new List<int>();
            int start = -len;
            while (true)
            {
                start = responseJson.IndexOf(searchstring, start + len);
                if (start == -1)
                {
                    ret.Add(start);
                    break;
                }
                else
                {
                    ret.Add(start);
                }
            }
            return ret;
        }
        
        public static string GetPriceforZPId(string zpid)
        {
            string jsondata = null;
            string price = null;
            string url = Constants.zillowDetailsPageUrlFirstPart + zpid +  Constants.zillowDetailsPageUrlSecondPart + zpid;

            using (WebClient wc = new WebClient())
            {
                try
                { 
                    jsondata = wc.DownloadString(url);
                }
                catch(Exception ex)
                {
                    jsondata = "";
                }
                
            }

            //just get 15 chars <meta itemprop="price" content="$1,300,000"> 
            //just get 15 chars, search for > and -2
            string searchstring = Constants.zillowDetailsPagePriceString;
            string afterReplacejsondata = jsondata.Replace(Constants.zillowDetailsPagePriceReplaceString1, "");
            int ind1 = afterReplacejsondata.IndexOf(searchstring);
            if (ind1 > 0)
            {
                price = afterReplacejsondata.Substring(ind1 + searchstring.Length, 15);
                price = price.Replace("from: ","");
                //price = price.Replace("\"", "");
                //price = price.Replace(">", "");
                if (price.IndexOf(Constants.zillowDetailsPagePriceReplaceString2) > 0)
                {
                    //int PriceStringLength = price.IndexOf(Constants.zillowDetailsPagePriceReplaceString2);
                    //price.Remove(PriceStringLength, price.Length - PriceStringLength);
                    int searchdoubleq = price.IndexOf("\"");
                    price = price.Remove(searchdoubleq, (price.Length - searchdoubleq));
                }

                return price;
            }
            return null;
        }

        public static string GetHOAForZPId(string zpid)
        {
            return StaticRandom.Instance.Next(150, 300).ToString();
        }
    }
}