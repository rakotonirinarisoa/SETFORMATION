using System;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace apptab.Controllers
{
    public class FtpController : Controller
    {
        private readonly SOFTCONNECTSIIG db = new SOFTCONNECTSIIG();

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public ActionResult FtpCreate()
        {
            ViewBag.Controller = "Paramétrage FTP";

            return View();
        }

        [HttpPost]
        public ActionResult DetailsFtp(SI_USERS suser, int iProjet)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = iProjet;

                var crpto = db.OPA_FTP.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null);
                if (crpto != null)
                {
                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = crpto, settings }));
                }
                else
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Veuillez créer un nouveau FTP. " }, settings));
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        [HttpPost]
        public JsonResult UpdateFtp(SI_USERS suser, OPA_FTP param, int iProjet)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            String uploadUrl = String.Format("{0}/{1}/", "ftp://" + param.HOTE, param.PATH);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uploadUrl);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(param.IDENTIFIANT, param.FTPPWD);
            request.Proxy = null;
            request.KeepAlive = true;
            request.UseBinary = true;

            try
            {
                int IdS = iProjet;
                var SExist = db.OPA_FTP.FirstOrDefault(a => a.IDPROJET == IdS && a.DELETIONDATE == null);

                try
                { request.GetResponse(); }
                catch { return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Configuration FTP non valide. " }, settings)); }

                if (SExist != null)
                {
                    if (SExist.HOTE != param.HOTE || SExist.IDENTIFIANT != param.IDENTIFIANT || SExist.FTPPWD != param.FTPPWD || SExist.PATH != param.PATH /*|| SExist.PORT != param.PORT*/)
                    {
                        SExist.HOTE = param.HOTE;
                        SExist.IDENTIFIANT = param.IDENTIFIANT;
                        SExist.FTPPWD = param.FTPPWD;
                        SExist.PATH = param.PATH;

                        db.SaveChanges();

                        var H = db.HOPA_FTP.FirstOrDefault(a => a.IDPARENT == SExist.ID && a.DELETIONDATE == null);
                        if (H != null)
                        {
                            H.DELETIONDATE = DateTime.Now;
                            db.SaveChanges();
                        }

                        var newElemH = new HOPA_FTP()
                        {
                            HOTE = param.HOTE,
                            IDENTIFIANT = param.IDENTIFIANT,
                            FTPPWD = param.FTPPWD,
                            PATH = param.PATH,
                            IDPROJET = IdS,
                            CREATIONDATE = DateTime.Now,
                            IDUSER = exist.ID,
                            IDPARENT = SExist.ID
                        };
                        db.HOPA_FTP.Add(newElemH);
                        db.SaveChanges();
                    }

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = param }, settings));
                }
                else
                {
                    var newPara = new OPA_FTP()
                    {
                        HOTE = param.HOTE,
                        IDENTIFIANT = param.IDENTIFIANT,
                        FTPPWD = param.FTPPWD,
                        PATH = param.PATH,
                        IDPROJET = IdS,
                        CREATIONDATE = DateTime.Now,
                        IDUSER = exist.ID
                    };

                    db.OPA_FTP.Add(newPara);
                    db.SaveChanges();

                    var isElemH = db.OPA_FTP.FirstOrDefault(a => a.IDPROJET == IdS && a.HOTE == param.HOTE && a.IDENTIFIANT == param.IDENTIFIANT && a.FTPPWD == param.FTPPWD && a.PATH == param.PATH && a.DELETIONDATE == null);
                    var newElemH = new HOPA_FTP()
                    {
                        HOTE = param.HOTE,
                        IDENTIFIANT = param.IDENTIFIANT,
                        FTPPWD = param.FTPPWD,
                        PATH = param.PATH,
                        IDPROJET = IdS,
                        CREATIONDATE = isElemH.CREATIONDATE,
                        IDUSER = isElemH.IDUSER,
                        IDPARENT = isElemH.ID
                    };
                    db.HOPA_FTP.Add(newElemH);
                    db.SaveChanges();

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = param }, settings));
                }
            }
            catch (Exception)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Erreur d'enregistrement de l'information. " }, settings));
            }
        }
    }
}
