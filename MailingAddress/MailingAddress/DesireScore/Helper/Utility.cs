using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using DesireScore.Models;
using Newtonsoft.Json;

namespace DesireScore.Helper
{
    public static class Utility
    {
        public static PropertyInfo GetScore(string address, string city, string listedPrice, string hoa, string serviceId)
        {
            const string baseUrl = "https://microsoft-apiappb8aa4223110d49739ba98efc32252162.azurewebsites.net/";
            //const string baseUrl = "http://localhost:50345/";
            try
            {
                using (var client = new HttpClient())
                {
                    listedPrice = listedPrice.Replace(",","");
                    listedPrice = listedPrice.Replace("$","");

                    client.BaseAddress = new Uri(baseUrl);
                    var result = client.GetAsync("/api/Score/Get/?Address1=" + address.Replace(" ", "+") + "&City=" + city +
                                        "&ListedPrice=" + listedPrice + "&Hoa=" + hoa + "&State=CA" + "&zillowServiceId=" + serviceId).Result;

                    var jsonAsString = result.Content.ReadAsStringAsync().Result;
                    var output = JsonConvert.DeserializeObject<PropertyInfo>(jsonAsString);

                    return output; 
                }
            }
            catch(Exception ex)
            {
                return new PropertyInfo();
            }
        }

        public static List<int> PrepareListForHomeAddressSearch(string responseJson, string searchstring)
        {
            var len = searchstring.Length;
            var ret = new List<int>();
            var start = -len;
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

        public static string[] GetPriceforZPId(string zpid)
        {
            string[] dataArray = new string[2];
            string jsondata = null;
            string price = null;
            string url = Constants.zillowDetailsPageUrlFirstPart + zpid + Constants.zillowDetailsPageUrlSecondPart + zpid;

            using (WebClient wc = new WebClient())
            {
                try
                {
                    jsondata = wc.DownloadString(url);
                }
                catch (Exception ex)
                {
                    jsondata = "";
                }
            }

            string afterReplacejsondata = jsondata.Replace(Constants.zillowDetailsPagePriceReplaceString1, "");

            //just get 15 chars <meta itemprop="price" content="$1,300,000"> 
            //just get 15 chars, search for > and -2
            //Listing Prise
            string searchstring = Constants.zillowDetailsPagePriceString;
            int ind1 = afterReplacejsondata.IndexOf(searchstring);
            if (ind1 > 0)
            {
                price = afterReplacejsondata.Substring(ind1 + searchstring.Length, 15);
                price = price.Replace("from: ", "");
                if (price.IndexOf(Constants.zillowDetailsPagePriceReplaceString2) > 0)
                {
                    int searchdoubleq = price.IndexOf("\"");
                    price = price.Remove(searchdoubleq, (price.Length - searchdoubleq));
                }

                dataArray[0] = price; ;
            }

            //HOA
            string HOA = string.Empty;
            string searchHOAstring = Constants.zillowDetailsHOAStringSearchFirstPart;
            int index = afterReplacejsondata.IndexOf(searchHOAstring);
            if (index > 0)
            {
                HOA = afterReplacejsondata.Substring(index + searchHOAstring.Length, 15);
                if (HOA.IndexOf(Constants.zillowDetailsHOAStringSearchSecondPart) > 0)
                {
                    int searchdoubleq = HOA.IndexOf("mo");
                    HOA = HOA.Remove(searchdoubleq, (HOA.Length - searchdoubleq));
                    HOA = HOA.Replace("/", "");
                }

                if (string.IsNullOrEmpty(HOA))
                    dataArray[1] = StaticRandom.Instance.Next(150, 300).ToString();
                else
                    dataArray[1] = HOA;
            }

            return dataArray;
        }

        //public static string GetPriceforZpId(string zpid)
        //{
        //    string jsondata;
        //    var url = Constants.zillowDetailsPageUrlFirstPart + zpid +  Constants.zillowDetailsPageUrlSecondPart + zpid;

        //    using (var wc = new WebClient())
        //    {
        //        try
        //        { 
        //            jsondata = wc.DownloadString(url);
        //        }
        //        catch(Exception ex)
        //        {
        //            jsondata = "";
        //        }                
        //    }

        //    //just get 15 chars <meta itemprop="price" content="$1,300,000"> 
        //    //just get 15 chars, search for > and -2
        //    var searchstring = Constants.zillowDetailsPagePriceString;
        //    var afterReplacejsondata = jsondata.Replace(Constants.zillowDetailsPagePriceReplaceString1, "");
        //    var ind1 = afterReplacejsondata.IndexOf(searchstring);
        //    if (ind1 <= 0) return null;
        //    var price = afterReplacejsondata.Substring(ind1 + searchstring.Length, 15);
        //    price = price.Replace("from: ","");
        //    //price = price.Replace("\"", "");
        //    //price = price.Replace(">", "");
        //    if (price.IndexOf(Constants.zillowDetailsPagePriceReplaceString2) > 0)
        //    {
        //        //int PriceStringLength = price.IndexOf(Constants.zillowDetailsPagePriceReplaceString2);
        //        //price.Remove(PriceStringLength, price.Length - PriceStringLength);
        //        var searchdoubleq = price.IndexOf("\"");
        //        price = price.Remove(searchdoubleq, (price.Length - searchdoubleq));
        //    }

        //    return price;
        //}

        public static string GetHoaForZpId(string zpid)
        {
            return StaticRandom.Instance.Next(150, 300).ToString();
        }
    }
}