using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DesireScore.Helper
{
    static class Constants
    {
        public const string Unknown = "Unknown Zip Code or City / Unknown to system";
        public const string Enterzipcity = "Enter the Zip Code or City";
        public const string NoResults = "No Results Found/ Unknown";
        public const string FindStartStringForAddress = "<img rel=\"nofollow\" alt=\"";
        public const string FindEndStringForAddress = ", CA\"";
        //public const string ZillowHomePage = "http://www.zillow.com/homes/";
        public const string ZillowHomePage = "http://www.zillow.com/homes/for_sale/{zipcode}/fsba,fsbo,fore,cmsn_lt/house,condo,apartment_duplex,townhouse_type/100000-1000000_price";
        public const string SearchStringZPId = "?zpid=";

        public const string zillowDetailsHOAStringSearchFirstPart = "HOA Fee: ";
        public const string zillowDetailsHOAStringSearchSecondPart = "mo";
        public const string zillowDetailsPageUrlFirstPart ="http://www.zillow.com/jsonp/Hdp.htm?zpid=";
        public const string zillowDetailsPageUrlSecondPart = "&fad=false&hc=false&lhdp=true&callback=YUI.Env.JSONP.handleHomeDetailPage";
        public const string zillowDetailsPagePriceString = "<meta itemprop=\"price\" content=\"";
        public const string zillowDetailsPagePriceReplaceString1 =  "\\";
        public const string zillowDetailsPagePriceReplaceString2 = ">";

        
    }
}