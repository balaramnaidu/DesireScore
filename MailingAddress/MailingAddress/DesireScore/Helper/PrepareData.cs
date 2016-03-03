
using DesireScore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace DesireScore.Helper
{
    public static class PrepareData
    {
        public static List<DesireScoreModel> GetData(string zipCode, string serviceId)
        {
            var resaddress = new List<DesireScoreModel>();
            if (string.IsNullOrEmpty(zipCode))
            {
                var sc1 = new DesireScoreModel { ResAddress = Constants.Enterzipcity, Score = "" };
                resaddress.Add(sc1);

                return resaddress;
            }

            try
            {
                //var http = (HttpWebRequest) WebRequest.Create(Constants.ZillowHomePage + zipCode + "_rb/");
                var http = (HttpWebRequest) HttpWebRequest.Create(Constants.ZillowHomePage.Replace("{zipcode}", zipCode));
                var response = (HttpWebResponse) http.GetResponse();
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    var responseJson = sr.ReadToEnd();
                    var regex = new Regex(Constants.FindStartStringForAddress);
                    const string searchstring = Constants.FindStartStringForAddress;
                    const string searchstring1 = Constants.FindEndStringForAddress;
                    const string searchZpIdString = " id=\"zpid_";

                    var ret = Utility.PrepareListForHomeAddressSearch(responseJson, searchstring);
                    var ret1 = Utility.PrepareListForHomeAddressSearch(responseJson, searchZpIdString);
                    if (ret.Count == 0)
                    {
                        var sc2 = new DesireScoreModel { ResAddress = Constants.NoResults, Score = "" };
                        resaddress.Add(sc2);
                    }

                    var inc = 0;
                    foreach (var a in ret)
                    {
                        var startAddressFindString = responseJson.Substring(a, 100).Replace(searchstring, "");
                        var ind = startAddressFindString.IndexOf(searchstring1);
                        var number = ret1[inc];
                        var zpid = responseJson.Substring(number + searchZpIdString.Length, 8);

                        if (ind > 0)
                        {
                            var sc2 = new DesireScoreModel
                            {
                                ResAddress = startAddressFindString.Substring(0, ind),
                                Price = Utility.GetPriceforZPId(zpid)[0],
                                HOA = Utility.GetPriceforZPId(zpid)[1]
                            };
                            //sc2.Score = StaticRandom.Instance.Next(500, 1000).ToString();
                            var propInfo = Utility.GetScore(sc2.ResAddress.Replace(" ", "+"), "", sc2.Price, sc2.HOA, serviceId);
                            sc2.Score = propInfo.DesirabilityScore.ToString();
                            sc2.Bathrooms = propInfo.Bathrooms;
                            sc2.Bedrooms = propInfo.Bedrooms;
                            sc2.HomeType = propInfo.HomeType;
                            sc2.FinishedSize = propInfo.FinishedSize;
                            sc2.LotSize = propInfo.LotSize;
                            sc2.YearBuilt = propInfo.YearBuilt;
                            sc2.ZRent = propInfo.ZRent;
                            sc2.Zestimate = propInfo.ZAmount;
                            sc2.HomeDetailsLink = propInfo.HomeDetailsLink;

                            resaddress.Add(sc2);
                        }
                        if (inc > 5) break; 
                        inc++;
                    }
                    return resaddress;
                }
            }
            catch (Exception Ex)
            {
                var sc1 = new DesireScoreModel { ResAddress = Constants.Unknown, Score = "" };
                resaddress.Add(sc1);

                return resaddress;
            }
        }
    }
}