using System;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace apptab.Controllers
{
    public class GetFileFController : Controller
    {
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        // GET: GetFileF
        public ActionResult Index()
        {
            return View();
        }
        [Route("GetFile")]
        [HttpPost]
        public ActionResult GetFile(SI_USERS suser)
        {
            try
            {
                string pth = AppDomain.CurrentDomain.BaseDirectory + "\\DeploieF\\UrlOrigine.txt";
                //UrlOrigine
                StreamReader sr = new StreamReader(pth);
                string line = sr.ReadToEnd();
                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = line }, settings));
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Error" }, settings));
        }
    }
}
