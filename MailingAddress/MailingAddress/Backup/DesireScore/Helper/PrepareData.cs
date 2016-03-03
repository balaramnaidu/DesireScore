
using DesireScore.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace DesireScore.Helper
{
    public static class PrepareData
    {
        public static List<DesireScoreModel> GetData(string zipCode)
        {
            List<DesireScoreModel> resaddress = new List<DesireScoreModel>();
            if (string.IsNullOrEmpty(zipCode))
            {
                DesireScoreModel sc1 = new DesireScoreModel();
                sc1.ResAddress = Constants.Enterzipcity;
                sc1.Score = "";
                resaddress.Add(sc1);
                return resaddress;
            }

            string zpid = null;
            string responseJson = string.Empty;
            try
            {
                HttpWebRequest http = (HttpWebRequest)HttpWebRequest.Create(Constants.ZillowHomePage + zipCode + "_rb/");
                HttpWebResponse response = (HttpWebResponse)http.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    responseJson = sr.ReadToEnd();
                    Regex regex = new Regex(Constants.FindStartStringForAddress);
                    var searchstring = Constants.FindStartStringForAddress;
                    var searchstring1 = Constants.FindEndStringForAddress;
                    var searchZPIdString = " id=\"zpid_";

                    var ret = Utility.PrepareListForHomeAddressSearch(responseJson, searchstring);
                    var ret1 = Utility.PrepareListForHomeAddressSearch(responseJson, searchZPIdString);
                    if (ret.Count == 0)
                    {
                        DesireScoreModel sc2 = new DesireScoreModel();
                        sc2.ResAddress = Constants.NoResults;
                        sc2.Score = "";
                        resaddress.Add(sc2);
                    }

                    int inc = 0;
                    foreach (int a in ret)
                    {
                        string startAddressFindString = responseJson.Substring(a, 100).Replace(searchstring, "");
                        int ind = startAddressFindString.IndexOf(searchstring1);
                        int number = ret1[inc];
                        zpid = responseJson.Substring(number + searchZPIdString.Length, 8); //zpid is 8 chars

                        if (ind > 0)
                        {
                            DesireScoreModel sc2 = new DesireScoreModel();
                            sc2.ResAddress = startAddressFindString.Substring(0, ind);
                            sc2.Price = Utility.GetPriceforZPId(zpid); ;
                            sc2.HOA = Utility.GetHOAForZPId(zpid);
                            //sc2.Score = StaticRandom.Instance.Next(500, 1000).ToString();
                            sc2.Score = Utility.GetScore(sc2.ResAddress.Replace(" ", "+"), "", sc2.Price, sc2.HOA);
                            resaddress.Add(sc2);
                        }
                        inc++;
                    }
                    return resaddress;
                }
            }
            catch (Exception Ex)
            {
                DesireScoreModel sc1 = new DesireScoreModel();
                sc1.ResAddress = Constants.Unknown;
                sc1.Score = "";
                resaddress.Add(sc1);
                return resaddress;
            }
        }
    }
}