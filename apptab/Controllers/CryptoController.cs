using System;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace apptab.Controllers
{
    public class CryptoController : Controller
    {
        private readonly SOFTCONNECTSIIG db = new SOFTCONNECTSIIG();

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public ActionResult CryptoCreate()
        {
            ViewBag.Controller = "Mot de passe fichier";

            return View();
        }

        [HttpPost]
        public ActionResult DetailsCrypto(SI_USERS suser, int iProjet)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = iProjet;

                var crpto = db.OPA_CRYPTO.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null);
                if (crpto != null)
                {
                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = crpto, settings }));
                }
                else
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Veuillez créer un nouveau mot de passe fichier. " }, settings));
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        [HttpPost]
        public JsonResult UpdateCrypto(SI_USERS suser, OPA_CRYPTO param, int iProjet)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int IdS = iProjet;
                var SExist = db.OPA_CRYPTO.FirstOrDefault(a => a.IDPROJET == IdS && a.DELETIONDATE == null);

                if (SExist != null)
                {
                    if (SExist.CRYPTOPWD != param.CRYPTOPWD)
                    {
                        SExist.CRYPTOPWD = param.CRYPTOPWD;

                        db.SaveChanges();

                        var H = db.HOPA_CRYPTO.FirstOrDefault(a => a.IDPARENT == SExist.ID && a.DELETIONDATE == null);
                        if (H != null)
                        {
                            H.DELETIONDATE = DateTime.Now;
                            db.SaveChanges();
                        }

                        var newElemH = new HOPA_CRYPTO()
                        {
                            CRYPTOPWD = param.CRYPTOPWD,
                            IDPROJET = IdS,
                            CREATIONDATE = DateTime.Now,
                            IDUSER = exist.ID,
                            IDPARENT = SExist.ID
                        };
                        db.HOPA_CRYPTO.Add(newElemH);
                        db.SaveChanges();
                    }

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = param }, settings));
                }
                else
                {
                    var newPara = new OPA_CRYPTO()
                    {
                        CRYPTOPWD = param.CRYPTOPWD,
                        IDPROJET = IdS,
                        CREATIONDATE = DateTime.Now,
                        IDUSER = exist.ID
                    };

                    db.OPA_CRYPTO.Add(newPara);
                    db.SaveChanges();

                    var isElemH = db.OPA_CRYPTO.FirstOrDefault(a => a.IDPROJET == IdS && a.CRYPTOPWD == param.CRYPTOPWD && a.DELETIONDATE == null);
                    var newElemH = new HOPA_CRYPTO()
                    {
                        CRYPTOPWD = isElemH.CRYPTOPWD,
                        IDPROJET = IdS,
                        CREATIONDATE = isElemH.CREATIONDATE,
                        IDUSER = isElemH.IDUSER,
                        IDPARENT = isElemH.ID
                    };
                    db.HOPA_CRYPTO.Add(newElemH);
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
