using DesireScore.Helper;
using DesireScore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DesireScore.Controllers
{
    public class HomeController : Controller
    {
        List<DesireScoreModel> resaddress = new List<DesireScoreModel>();
        static HttpClient client = new HttpClient();

        public ActionResult Index(string zipCode)
        {
            ViewBag.Message = "";
            ViewBag.Collection = PrepareData.GetData(zipCode);
            return View();
        }
               
        public ActionResult About()
        {
            ViewBag.Message = "";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "";

            return View();
        }
        
        private async void ListBook(int id)
        {
            JsonWebClient client = new JsonWebClient();
            //var resp = await client.DoRequestAsync("https://microsoft-apiappb8aa4223110d49739ba98efc32252162.azurewebsites.net/api/Score/Get/?ListedPrice=154000&ZAmount=276000&ZRent=4500&Hoa=630");
            //string result = resp.ReadToEnd();
        }  
    }
}
