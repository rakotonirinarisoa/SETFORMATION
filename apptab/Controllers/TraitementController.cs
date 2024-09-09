using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net.Mail;
using Extensions.DateTime;

namespace apptab.Controllers
{
    public class TraitementController : Controller
    {
        private readonly SOFTCONNECTSIIG db = new SOFTCONNECTSIIG();
        private readonly SOFTCONNECTOM tom = new SOFTCONNECTOM();

        private static int idF = 0;
        private static string Numeroliquidations = "";
        private static string EstAvance = "";

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        //Traitement mandats PROJET//
        public ActionResult TraitementPROJET()
        {
            ViewBag.Controller = "Tris des engagements par le RAF";

            return View();
        }

        //GET ALL PROJET//
        [HttpPost]
        public ActionResult GetAllPROJET(SI_USERS suser)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                var test = db.SI_USERS.Where(x => x.LOGIN == exist.LOGIN && x.PWD == exist.PWD && x.DELETIONDATE == null).FirstOrDefault();
                if (test.ROLE == (int)Role.SAdministrateur)
                {
                    var user = db.SI_PROJETS.Select(a => new
                    {
                        PROJET = a.PROJET,
                        ID = a.ID,
                        DELETIONDATE = a.DELETIONDATE,
                    }).Where(a => a.DELETIONDATE == null).ToList();

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = user }, settings));
                }
                else
                {
                    if (test.IDPROJET != 0)
                    {
                        var user = db.SI_PROJETS.Select(a => new
                        {
                            PROJET = a.PROJET,
                            ID = a.ID,
                            DELETIONDATE = a.DELETIONDATE,
                        }).Where(a => a.DELETIONDATE == null && a.ID == test.IDPROJET).ToList();

                        return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = user }, settings));
                    }
                    else
                    {
                        var user = (from usr in db.SI_PROJETS
                                    join prj in db.SI_MAPUSERPROJET on usr.ID equals prj.IDPROJET
                                    where prj.IDUS == test.ID && usr.DELETIONDATE == null
                                    select new
                                    {
                                        PROJET = usr.PROJET,
                                        ID = usr.ID,
                                        DELETIONDATE = usr.DELETIONDATE,
                                    }).ToList();

                        return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = user }, settings));
                    }
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        //[HttpPost]
        //public ActionResult GetIsProjet(SI_USERS suser)
        //{
        //    var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
        //    if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

        //    var isProjet = "";

        //    if (db.SI_PROJETS.Any(a => a.ID == exist.IDPROJET && a.DELETIONDATE == null))
        //    {
        //        isProjet = db.SI_PROJETS.FirstOrDefault(a => a.ID == exist.IDPROJET && a.DELETIONDATE == null).PROJET;
        //    }

        //    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = isProjet }, settings));
        //}

        [HttpPost]
        public ActionResult DetailsInfoPro(SI_USERS suser, int iProjet)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = iProjet;

                int proj = 0;
                if (db.SI_PROJETS.FirstOrDefault(a => a.ID == crpt && a.DELETIONDATE == null) != null)
                {
                    proj = db.SI_PROJETS.FirstOrDefault(a => a.ID == crpt && a.DELETIONDATE == null).ID;
                }

                if (proj != 0)
                {
                    return Json(JsonConvert.SerializeObject(new
                    {
                        type = "success",
                        msg = "message",
                        data = new
                        {
                            PROJ = proj
                        }
                    }, settings));
                }
                else
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "message" }, settings));
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        [HttpPost]
        public async Task<JsonResult> Generation(SI_USERS suser, DateTime DateDebut, DateTime DateFin, int iProjet)
        {
            var exist = await db.SI_USERS.FirstOrDefaultAsync(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = iProjet;
                //Check si le projet est mappé à une base de données TOM²PRO//
                if (db.SI_MAPPAGES.FirstOrDefault(a => a.IDPROJET == crpt) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Le projet n'est pas mappé à une base de données TOM²PRO. " }, settings));

                int retarDate = 0;
                if (db.SI_DELAISTRAITEMENT.Any(a => a.IDPROJET == crpt && a.DELETIONDATE == null))
                    retarDate = db.SI_DELAISTRAITEMENT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null).DELRAF.Value;

                SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
                SOFTCONNECTOM tom = new SOFTCONNECTOM();

                List<DATATRPROJET> list = new List<DATATRPROJET>();

                //Check si la correspondance des états est OK//
                var numCaEtapAPP = db.SI_PARAMETAT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null);
                if (numCaEtapAPP == null) return Json(JsonConvert.SerializeObject(new { type = "PEtat", msg = "Veuillez paramétrer la correspondance des états. " }, settings));
                //TEST si les états dans les paramètres dans cohérents avec ceux de TOM²PRO//
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.DEF) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état DEF n'est pas paramétré sur TOM²PRO. " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.TEF) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état TEF n'est pas paramétré sur TOM²PRO. " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.BE) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état BE n'est pas paramétré sur TOM²PRO. " }, settings));

                if (tom.CPTADMIN_FLIQUIDATION.Any(a => a.DATELIQUIDATION >= DateDebut && a.DATELIQUIDATION <= DateFin))
                {
                    foreach (var x in tom.CPTADMIN_FLIQUIDATION.Where(a => a.DATELIQUIDATION >= DateDebut && a.DATELIQUIDATION <= DateFin).OrderBy(a => a.DATELIQUIDATION).ToList())
                    {
                        decimal MTN = 0;
                        decimal MTNPJ = 0;
                        var PCOP = "";

                        //Get total MTN dans CPTADMIN_MLIQUIDATION pour vérification du SOMMES MTN M = SOMMES MTN MPJ//
                        if (tom.CPTADMIN_MLIQUIDATION.Any(a => a.IDLIQUIDATION == x.ID))
                        {
                            foreach (var y in tom.CPTADMIN_MLIQUIDATION.Where(a => a.IDLIQUIDATION == x.ID).ToList())
                            {
                                MTN += y.MONTANTLOCAL.Value;

                                if (String.IsNullOrEmpty(PCOP))
                                    PCOP = y.POSTE;
                            }
                        }

                        //TEST SI SOMMES MTN M = SOMMES MTN MPJ//
                        var IDString = x.ID.ToString();
                        if (tom.TP_MPIECES_JUSTIFICATIVES.Any(a => a.NUMERO_FICHE == IDString && a.MODULLE == "LIQUIDATION"))
                        {
                            foreach (var y in tom.TP_MPIECES_JUSTIFICATIVES.Where(a => a.NUMERO_FICHE == IDString && a.MODULLE == "LIQUIDATION").ToList())
                            {
                                MTNPJ += y.MONTANT.Value;
                            }
                        }

                        //MathRound 3 satria kely kokoa ny marge d'erreur no le 2//
                        if (Math.Round(MTN, 3) == Math.Round(MTNPJ, 3))
                        {
                            //Check si F a déjà passé les 3 étapes (DEF, TEF et BE) pour avoir les dates => BE étape finale//
                            var canBe = true;
                            if (tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.DEF) == null)
                                canBe = false;
                            if (tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.TEF) == null)
                                canBe = false;
                            if (tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.BE) == null)
                                canBe = false;

                            //TEST que F n'est pas encore traité ou F a été annulé// ETAT annulé = 2//
                            if (canBe)
                            {
                                if (!db.SI_TRAITPROJET.Any(a => a.No == x.ID && a.IDPROJET == crpt) || db.SI_TRAITPROJET.Any(a => a.No == x.ID && a.ETAT == 2 && a.IDPROJET == crpt))
                                {
                                    var titulaire = "";
                                    if (tom.RTIERS.Any(a => a.COGE == x.COGEBENEFICIAIRE && a.AUXI == x.AUXIBENEFICIAIRE))
                                        titulaire = tom.RTIERS.FirstOrDefault(a => a.COGE == x.COGEBENEFICIAIRE && a.AUXI == x.AUXIBENEFICIAIRE).NOM;

                                    var soa = (from soas in db.SI_SOAS
                                               join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                               where prj.IDPROJET == crpt && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                               select new
                                               {
                                                   soas.SOA
                                               });

                                    bool isLate = false;
                                    DateTime DD = tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.BE).DATECA.Value.Date;
                                    if (DD.AddBusinessDays(retarDate).Date < DateTime.Now/* && ((int)DateTime.Now.DayOfWeek) != 6 && ((int)DateTime.Now.DayOfWeek) != 0*/)
                                        isLate = true;

                                    list.Add(new DATATRPROJET
                                    {
                                        No = x.ID,
                                        REF = x.NUMEROCA,
                                        OBJ = x.DESCRIPTION,
                                        TITUL = titulaire,
                                        MONT = Math.Round(MTN, 2).ToString(),
                                        COMPTE = x.COGEBENEFICIAIRE,
                                        DATE = x.DATELIQUIDATION.Value.Date,
                                        PCOP = PCOP,
                                        DATEDEF = tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.DEF).DATECA,
                                        DATETEF = tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.TEF).DATECA,
                                        DATEBE = tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.BE).DATECA,
                                        SOA = soa.FirstOrDefault().SOA,
                                        PROJET = db.SI_PROJETS.Where(a => a.ID == crpt && a.DELETIONDATE == null).FirstOrDefault().PROJET,
                                        isLATE = isLate
                                    });
                                }
                            }
                        }
                    }
                }

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = list }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        [HttpPost]
        public async Task<JsonResult> GenerationLOAD(SI_USERS suser, int iProjet)
        {
            var exist = await db.SI_USERS.FirstOrDefaultAsync(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = iProjet;
                //Check si le projet est mappé à une base de données TOM²PRO//
                if (db.SI_MAPPAGES.FirstOrDefault(a => a.IDPROJET == crpt) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Le projet n'est pas mappé à une base de données TOM²PRO. " }, settings));

                int retarDate = 0;
                if (db.SI_DELAISTRAITEMENT.Any(a => a.IDPROJET == crpt && a.DELETIONDATE == null))
                    retarDate = db.SI_DELAISTRAITEMENT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null).DELRAF.Value;

                SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
                SOFTCONNECTOM tom = new SOFTCONNECTOM();

                List<DATATRPROJET> list = new List<DATATRPROJET>();

                //Check si la correspondance des états est OK//
                var numCaEtapAPP = db.SI_PARAMETAT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null);
                if (numCaEtapAPP == null) return Json(JsonConvert.SerializeObject(new { type = "PEtat", msg = "Veuillez paramétrer la correspondance des états. " }, settings));
                //TEST si les états dans les paramètres dans cohérents avec ceux de TOM²PRO//
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.DEF) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état DEF n'est pas paramétré sur TOM²PRO. " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.TEF) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état TEF n'est pas paramétré sur TOM²PRO. " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.BE) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état BE n'est pas paramétré sur TOM²PRO. " }, settings));

                if (tom.CPTADMIN_FLIQUIDATION.Any())
                {
                    foreach (var x in tom.CPTADMIN_FLIQUIDATION.OrderBy(a => a.DATELIQUIDATION).ToList())
                    {
                        decimal MTN = 0;
                        decimal MTNPJ = 0;
                        var PCOP = "";

                        //Get total MTN dans CPTADMIN_MLIQUIDATION pour vérification du SOMMES MTN M = SOMMES MTN MPJ//
                        if (tom.CPTADMIN_MLIQUIDATION.Any(a => a.IDLIQUIDATION == x.ID))
                        {
                            foreach (var y in tom.CPTADMIN_MLIQUIDATION.Where(a => a.IDLIQUIDATION == x.ID).ToList())
                            {
                                MTN += y.MONTANTLOCAL.Value;

                                if (String.IsNullOrEmpty(PCOP))
                                    PCOP = y.POSTE;
                            }
                        }

                        //TEST SI SOMMES MTN M = SOMMES MTN MPJ//
                        var IDString = x.ID.ToString();
                        if (tom.TP_MPIECES_JUSTIFICATIVES.Any(a => a.NUMERO_FICHE == IDString && a.MODULLE == "LIQUIDATION"))
                        {
                            foreach (var y in tom.TP_MPIECES_JUSTIFICATIVES.Where(a => a.NUMERO_FICHE == IDString && a.MODULLE == "LIQUIDATION").ToList())
                            {
                                MTNPJ += y.MONTANT.Value;
                            }
                        }

                        //MathRound 3 satria kely kokoa ny marge d'erreur no le 2//
                        if (Math.Round(MTN, 3) == Math.Round(MTNPJ, 3))
                        {
                            //Check si F a déjà passé les 3 étapes (DEF, TEF et BE) pour avoir les dates => BE étape finale//
                            var canBe = true;
                            if (tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.DEF) == null)
                                canBe = false;
                            if (tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.TEF) == null)
                                canBe = false;
                            if (tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.BE) == null)
                                canBe = false;

                            //TEST que F n'est pas encore traité ou F a été annulé// ETAT annulé = 2//
                            if (canBe)
                            {
                                if (!db.SI_TRAITPROJET.Any(a => a.No == x.ID && a.IDPROJET == crpt) || db.SI_TRAITPROJET.Any(a => a.No == x.ID && a.ETAT == 2 && a.IDPROJET == crpt))
                                {
                                    var titulaire = "";
                                    if (tom.RTIERS.Any(a => a.COGE == x.COGEBENEFICIAIRE && a.AUXI == x.AUXIBENEFICIAIRE))
                                        titulaire = tom.RTIERS.FirstOrDefault(a => a.COGE == x.COGEBENEFICIAIRE && a.AUXI == x.AUXIBENEFICIAIRE).NOM;

                                    var soa = (from soas in db.SI_SOAS
                                               join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                               where prj.IDPROJET == crpt && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                               select new
                                               {
                                                   soas.SOA
                                               });

                                    bool isLate = false;
                                    DateTime DD = tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.BE).DATECA.Value.Date;
                                    if (DD.AddBusinessDays(retarDate).Date < DateTime.Now/* && ((int)DateTime.Now.DayOfWeek) != 6 && ((int)DateTime.Now.DayOfWeek) != 0*/)
                                        isLate = true;

                                    list.Add(new DATATRPROJET
                                    {
                                        No = x.ID,
                                        REF = x.NUMEROCA,
                                        OBJ = x.DESCRIPTION,
                                        TITUL = titulaire,
                                        MONT = Math.Round(MTN, 2).ToString(),
                                        COMPTE = x.COGEBENEFICIAIRE,
                                        DATE = x.DATELIQUIDATION.Value.Date,
                                        PCOP = PCOP,
                                        DATEDEF = tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.DEF).DATECA,
                                        DATETEF = tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.TEF).DATECA,
                                        DATEBE = tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.BE).DATECA,
                                        SOA = soa.FirstOrDefault().SOA,
                                        PROJET = db.SI_PROJETS.Where(a => a.ID == crpt && a.DELETIONDATE == null).FirstOrDefault().PROJET,
                                        isLATE = isLate
                                    });
                                }
                            }
                        }
                    }
                }

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = list }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        //Traitement mandats ORDSEC//
        public ActionResult TraitementORDSEC()
        {
            ViewBag.Controller = "Validation des engagements par ORDESEC";

            return View();
        }

        [HttpPost]
        public JsonResult GenerationSIIG(SI_USERS suser, DateTime DateDebut, DateTime DateFin, int iProjet)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = iProjet;

                int retarDate = 0;
                if (db.SI_DELAISTRAITEMENT.Any(a => a.IDPROJET == crpt && a.DELETIONDATE == null))
                    retarDate = db.SI_DELAISTRAITEMENT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null).DELTV.Value;

                //Check si le projet est mappé à une base de données TOM²PRO//
                if (db.SI_MAPPAGES.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Le projet n'est pas mappé à une base de données TOM²PRO. " }, settings));

                SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
                SOFTCONNECTOM tom = new SOFTCONNECTOM();

                List<DATATRPROJET> list = new List<DATATRPROJET>();

                //Check si la correspondance des états est OK//
                var numCaEtapAPP = db.SI_PARAMETAT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null);
                if (numCaEtapAPP == null) return Json(JsonConvert.SerializeObject(new { type = "PEtat", msg = "Veuillez paramétrer la correspondance des états. " }, settings));
                //TEST si les états dans les paramètres dans cohérents avec ceux de TOM²PRO//
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.DEF) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état DEF n'est pas paramétré sur TOM²PRO. " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.TEF) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état TEF n'est pas paramétré sur TOM²PRO. " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.BE) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état BE n'est pas paramétré sur TOM²PRO. " }, settings));

                if (db.SI_TRAITPROJET.Any(a => a.IDPROJET == crpt && a.DATEMANDAT >= DateDebut && a.DATEMANDAT <= DateFin && a.ETAT == 0))
                {
                    foreach (var x in db.SI_TRAITPROJET.Where(a => a.IDPROJET == crpt && a.DATEMANDAT >= DateDebut && a.DATEMANDAT <= DateFin && a.ETAT == 0).OrderBy(a => a.DATECRE).OrderBy(a => a.DATEMANDAT).ToList())
                    {
                        var soa = (from soas in db.SI_SOAS
                                   join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                   where prj.IDPROJET == crpt && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                   select new
                                   {
                                       soas.SOA
                                   });

                        bool isLate = false;
                        if (x.DATECRE.Value.AddBusinessDays(retarDate).Date < DateTime.Now/* && ((int)DateTime.Now.DayOfWeek) != 6 && ((int)DateTime.Now.DayOfWeek) != 0*/)
                            isLate = true;

                        list.Add(new DATATRPROJET
                        {
                            No = x.No,
                            REF = x.REF,
                            OBJ = x.OBJ,
                            TITUL = x.TITUL,
                            MONT = Data.Cipher.Decrypt(x.MONT, "Oppenheimer").ToString(),
                            COMPTE = x.COMPTE,
                            DATE = x.DATEMANDAT.Value.Date,
                            PCOP = x.PCOP,
                            DATEDEF = x.DATEDEF.Value.Date,
                            DATETEF = x.DATETEF.Value.Date,
                            DATEBE = x.DATEBE.Value.Date,
                            LIEN = db.SI_USERS.FirstOrDefault(a => a.ID == x.IDUSERCREATE).LOGIN,
                            DATECREATION = x.DATECRE.Value.Date,
                            SOA = soa.FirstOrDefault().SOA,
                            PROJET = db.SI_PROJETS.Where(a => a.ID == crpt && a.DELETIONDATE == null).FirstOrDefault().PROJET,
                            isLATE = isLate
                        });
                    }
                }

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = list.OrderByDescending(a => a.isLATE).ToList() }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        //GENERATION SIIGLOAD//
        [HttpPost]
        public JsonResult GenerationSIIGLOAD(SI_USERS suser, int iProjet)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = iProjet;

                int retarDate = 0;
                if (db.SI_DELAISTRAITEMENT.Any(a => a.IDPROJET == crpt && a.DELETIONDATE == null))
                    retarDate = db.SI_DELAISTRAITEMENT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null).DELTV.Value;

                //Check si le projet est mappé à une base de données TOM²PRO//
                if (db.SI_MAPPAGES.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Le projet n'est pas mappé à une base de données TOM²PRO. " }, settings));

                SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
                SOFTCONNECTOM tom = new SOFTCONNECTOM();

                List<DATATRPROJET> list = new List<DATATRPROJET>();

                //Check si la correspondance des états est OK//
                var numCaEtapAPP = db.SI_PARAMETAT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null);
                if (numCaEtapAPP == null) return Json(JsonConvert.SerializeObject(new { type = "PEtat", msg = "Veuillez paramétrer la correspondance des états. " }, settings));
                //TEST si les états dans les paramètres dans cohérents avec ceux de TOM²PRO//
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.DEF) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état DEF n'est pas paramétré sur TOM²PRO. " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.TEF) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état TEF n'est pas paramétré sur TOM²PRO. " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.BE) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état BE n'est pas paramétré sur TOM²PRO. " }, settings));

                if (db.SI_TRAITPROJET.FirstOrDefault(a => a.IDPROJET == crpt && a.ETAT == 0) != null)
                {
                    foreach (var x in db.SI_TRAITPROJET.Where(a => a.IDPROJET == crpt && a.ETAT == 0).OrderBy(a => a.DATECRE).OrderBy(a => a.DATEMANDAT).ToList())
                    {
                        var soa = (from soas in db.SI_SOAS
                                   join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                   where prj.IDPROJET == crpt && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                   select new
                                   {
                                       soas.SOA
                                   });

                        bool isLate = false;
                        if (x.DATECRE.Value.AddBusinessDays(retarDate).Date < DateTime.Now/* && ((int)DateTime.Now.DayOfWeek) != 6 && ((int)DateTime.Now.DayOfWeek) != 0*/)
                            isLate = true;

                        list.Add(new DATATRPROJET
                        {
                            No = x.No,
                            REF = x.REF,
                            OBJ = x.OBJ,
                            TITUL = x.TITUL,
                            MONT = Data.Cipher.Decrypt(x.MONT, "Oppenheimer").ToString(),
                            COMPTE = x.COMPTE,
                            DATE = x.DATEMANDAT.Value.Date,
                            PCOP = x.PCOP,
                            DATEDEF = x.DATEDEF.Value.Date,
                            DATETEF = x.DATETEF.Value.Date,
                            DATEBE = x.DATEBE.Value.Date,
                            LIEN = db.SI_USERS.FirstOrDefault(a => a.ID == x.IDUSERCREATE).LOGIN,
                            DATECREATION = x.DATECRE.Value.Date,
                            SOA = soa.FirstOrDefault().SOA,
                            PROJET = db.SI_PROJETS.Where(a => a.ID == crpt && a.DELETIONDATE == null).FirstOrDefault().PROJET,
                            isLATE = isLate
                        });
                    }
                    //listORDER = list.OrderByDescending(a => a.isLATE).ToList();
                }

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = list.OrderByDescending(a => a.isLATE).ToList() }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        [HttpPost]
        public JsonResult GetCheckedEcritureF(SI_USERS suser, DateTime DateDebut, DateTime DateFin, string listCompte, int iProjet)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            int countTraitement = 0;
            int crpt = iProjet;
            var lien = "http://srvapp.softwell.cloud/softconnectsiig/";

            var ProjetIntitule = db.SI_PROJETS.Where(a => a.ID == crpt && a.DELETIONDATE == null).FirstOrDefault().PROJET;

            var listCompteS = listCompte.Split(',');
            foreach (var SAV in listCompteS)
            {
                try
                {
                    SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
                    SOFTCONNECTOM tom = new SOFTCONNECTOM();
                    var FSauv = new SI_TRAITPROJET();

                    List<DATATRPROJET> list = new List<DATATRPROJET>();

                    Guid elem = Guid.Parse(SAV);
                    if (db.SI_TRAITPROJET.FirstOrDefault(a => a.No == elem && a.ETAT == 2 && a.IDPROJET == crpt) != null)
                    {
                        var ismod = db.SI_TRAITPROJET.FirstOrDefault(a => a.No == elem && a.IDPROJET == crpt);
                        ismod.ETAT = 0;
                        ismod.DATECRE = DateTime.Now;
                        ismod.DATEANNUL = null;
                        ismod.IDUSERANNUL = null;

                        db.SaveChanges();
                    }
                    else
                    {
                        decimal MTN = 0;
                        var PCOP = "";
                        if (tom.CPTADMIN_FLIQUIDATION.Any(a => a.ID == elem))
                        {
                            if (tom.CPTADMIN_MLIQUIDATION.Any(a => a.IDLIQUIDATION == elem))
                            {
                                foreach (var y in tom.CPTADMIN_MLIQUIDATION.Where(a => a.IDLIQUIDATION == elem).ToList())
                                {
                                    //Get total MTN dans CPTADMIN_MLIQUIDATION pour vérification du SOMMES MTN M = SOMMES MTN MPJ//
                                    MTN += y.MONTANTLOCAL.Value;

                                    if (string.IsNullOrEmpty(PCOP))
                                        PCOP = y.POSTE;
                                }
                            }

                            var FF = tom.CPTADMIN_FLIQUIDATION.FirstOrDefault(a => a.ID == elem);

                            var numCaEtapAPP = db.SI_PARAMETAT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null);

                            var titulaire = "";
                            if (tom.RTIERS.Any(a => a.COGE == FF.COGEBENEFICIAIRE && a.AUXI == FF.AUXIBENEFICIAIRE))
                                titulaire = tom.RTIERS.FirstOrDefault(a => a.COGE == FF.COGEBENEFICIAIRE && a.AUXI == FF.AUXIBENEFICIAIRE).NOM;

                            var newT = new SI_TRAITPROJET()
                            {
                                IDPROJET = crpt,
                                No = FF.ID,
                                REF = FF.NUMEROCA,
                                OBJ = FF.DESCRIPTION,
                                TITUL = titulaire,
                                MONT = Data.Cipher.Encrypt((Math.Round(MTN, 2)).ToString(), "Oppenheimer"),
                                COMPTE = FF.COGEBENEFICIAIRE,
                                DATEMANDAT = FF.DATELIQUIDATION.Value.Date,
                                PCOP = PCOP,
                                DATEDEF = tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == FF.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.DEF).DATECA,
                                DATETEF = tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == FF.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.TEF).DATECA,
                                DATEBE = tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == FF.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.BE).DATECA,
                                DATECRE = DateTime.Now,
                                ETAT = 0,
                                IDUSERCREATE = exist.ID
                            };

                            db.SI_TRAITPROJET.Add(newT);
                            db.SaveChanges();
                        }
                    }
                    countTraitement++;
                }
                catch (Exception e)
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
                }
            }

            //SEND MAIL ALERT et NOTIFICATION//
            string MailAdresse = "serviceinfo@softwell.mg";
            string mdpMail = "09eYpçç0601";

            using (System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage())
            {
                SmtpClient smtp = new SmtpClient("smtpauth.moov.mg");
                smtp.UseDefaultCredentials = true;

                mail.From = new MailAddress(MailAdresse);

                mail.To.Add(MailAdresse);
                if (db.SI_MAIL.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null).MAILTE != null)
                {
                    string[] separators = { ";" };

                    var Tomail = mail;
                    if (Tomail != null)
                    {
                        string listUser = db.SI_MAIL.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null).MAILTE;
                        string[] mailListe = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var mailto in mailListe)
                        {
                            mail.To.Add(mailto);
                        }
                    }
                }

                mail.Subject = "Attente validation pièces du projet " + ProjetIntitule;
                mail.IsBodyHtml = true;
                mail.Body = "Madame, Monsieur,<br/><br>" + "Nous vous informons que vous avez " + countTraitement + " pièces en attente de validation pour le compte du projet " + ProjetIntitule + ".<br/><br>" +
                    "Nous vous remercions de cliquer <a href='" + lien + "'>(ici)</a> pour accéder à la plate-forme SOFT-SIIG CONNECT.<br/><br>" + "Cordialement";

                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential(MailAdresse, mdpMail);
                smtp.EnableSsl = true;

                try { smtp.Send(mail); }
                catch (Exception) { }
            }

            return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Traitement avec succès. ", data = "" }, settings));
        }

        [HttpPost]
        public JsonResult GetCheckedEcritureORDSEC(SI_USERS suser, string listCompte, int iProjet)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            int countTraitement = 0;
            int crpt = iProjet;
            var lien = "http://srvapp.softwell.cloud/softconnectsiig/";

            var ProjetIntitule = db.SI_PROJETS.Where(a => a.ID == crpt && a.DELETIONDATE == null).FirstOrDefault().PROJET;

            var listCompteS = listCompte.Split(',');
            foreach (var SAV in listCompteS)
            {
                try
                {
                    SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
                    SOFTCONNECTOM tom = new SOFTCONNECTOM();

                    List<DATATRPROJET> list = new List<DATATRPROJET>();

                    Guid isSAV = Guid.Parse(SAV);
                    if (db.SI_TRAITPROJET.FirstOrDefault(a => a.IDPROJET == crpt && a.No == isSAV) != null)
                    {
                        var isModified = db.SI_TRAITPROJET.FirstOrDefault(a => a.IDPROJET == crpt && a.No == isSAV);
                        isModified.ETAT = 1;
                        isModified.DATEVALIDATION = DateTime.Now;
                        isModified.DATEANNUL = null;
                        isModified.IDUSERANNUL = null;
                        isModified.IDUSERVALIDATE = exist.ID;
                        db.SaveChanges();

                        countTraitement++;
                    }
                }
                catch (Exception e)
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
                }
            }

            //SEND MAIL ALERT et NOTIFICATION//
            string MailAdresse = "serviceinfo@softwell.mg";
            string mdpMail = "09eYpçç0601";

            using (System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage())
            {
                SmtpClient smtp = new SmtpClient("smtpauth.moov.mg");
                smtp.UseDefaultCredentials = true;

                mail.From = new MailAddress(MailAdresse);

                mail.To.Add(MailAdresse);
                if (db.SI_MAIL.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null).MAILTV != null)
                {
                    string[] separators = { ";" };

                    var Tomail = mail;
                    if (Tomail != null)
                    {
                        string listUser = db.SI_MAIL.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null).MAILTV;
                        string[] mailListe = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var mailto in mailListe)
                        {
                            mail.To.Add(mailto);
                        }
                    }
                }

                mail.Subject = "Validation pièces du projet " + ProjetIntitule;
                mail.IsBodyHtml = true;
                mail.Body = "Madame, Monsieur,<br/><br>" + "Nous vous informons que vous avez " + countTraitement + " pièces validées pour le compte du projet " + ProjetIntitule + " et en attente de transfert vers SIIGFP.<br/><br>" +
                    "Nous vous remercions de cliquer <a href='" + lien + "'>(ici)</a> pour accéder à la plate-forme SOFT-SIIG CONNECT.<br/><br>" + "Cordialement";

                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential(MailAdresse, mdpMail);
                smtp.EnableSsl = true;

                try { smtp.Send(mail); }
                catch (Exception) { }
            }

            return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Traitement avec succès. ", data = "" }, settings));
        }

        [HttpPost]
        public async Task<JsonResult> ModalF(SI_USERS suser, string IdF, int iProjet)
        {
            var exist = await db.SI_USERS.FirstOrDefaultAsync(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = iProjet;

                SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
                SOFTCONNECTOM tom = new SOFTCONNECTOM();

                List<DATATRPROJET> list = new List<DATATRPROJET>();

                if (tom.TP_MPIECES_JUSTIFICATIVES.FirstOrDefault(a => a.NUMERO_FICHE == IdF && a.MODULLE == "LIQUIDATION") != null)
                {
                    foreach (var x in tom.TP_MPIECES_JUSTIFICATIVES.Where(a => a.NUMERO_FICHE == IdF && a.TYPEPIECE != "DEF" && a.TYPEPIECE != "TEF" && a.TYPEPIECE != "BE" && a.MODULLE == "LIQUIDATION").OrderBy(a => a.RANG).ToList())
                    {
                        var idFGuid = Guid.Parse(IdF);
                        DateTime dpj = tom.CPTADMIN_FLIQUIDATION.Where(a => a.ID == idFGuid).FirstOrDefault().DATELIQUIDATION.Value;
                        list.Add(new DATATRPROJET
                        {
                            REF = x.TYPEPIECE != null ? x.TYPEPIECE : "",
                            OBJ = x.RANG != null ? x.RANG.ToString() : "",
                            TITUL = x.NOMBRE != null ? x.NOMBRE.ToString() : "",
                            DATE = dpj,
                            MONT = x.MONTANT != null ? Math.Round(x.MONTANT.Value, 2).ToString() : "0",
                            LIEN = !String.IsNullOrEmpty(x.LIEN) ? x.LIEN : ""
                        });
                    }
                }

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = list }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ModalD(SI_USERS suser, Guid IdF, int iProjet)
        {
            var exist = await db.SI_USERS.FirstOrDefaultAsync(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = iProjet;

                SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
                SOFTCONNECTOM tom = new SOFTCONNECTOM();

                List<DATATRPROJET> list = new List<DATATRPROJET>();

                if (tom.CPTADMIN_MLIQUIDATION.FirstOrDefault(a => a.IDLIQUIDATION == IdF) != null)
                {
                    foreach (var x in tom.CPTADMIN_MLIQUIDATION.Where(a => a.IDLIQUIDATION == IdF).ToList())
                    {
                        list.Add(new DATATRPROJET
                        {
                            REF = x.LIBELLE != null ? x.LIBELLE : "",
                            OBJ = x.COGE != null ? x.COGE.ToString() : "",
                            TITUL = x.POSTE != null ? x.POSTE.ToString() : "",
                            MONT = x.MONTANTLOCAL != null ? Math.Round(x.MONTANTLOCAL.Value, 2).ToString() : "0",
                        });
                    }
                }

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = list }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ModalLIAS(SI_USERS suser, string IdF, int iProjet)
        {
            var exist = await db.SI_USERS.FirstOrDefaultAsync(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = iProjet;

                SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
                SOFTCONNECTOM tom = new SOFTCONNECTOM();

                //List<DATATRPROJET> list = new List<DATATRPROJET>();
                var newElemH = new DATATRPROJET()
                {
                    REF = "",
                    OBJ = "",
                    TITUL = ""
                };

                if (tom.TP_MPIECES_JUSTIFICATIVES.FirstOrDefault(a => a.NUMERO_FICHE == IdF && (a.TYPEPIECE == "DEF" || a.TYPEPIECE == "TEF" || a.TYPEPIECE == "BE") && a.MODULLE == "LIQUIDATION") != null)
                {
                    var def = "";
                    if (tom.TP_MPIECES_JUSTIFICATIVES.FirstOrDefault(a => a.NUMERO_FICHE == IdF && a.TYPEPIECE == "DEF" && a.MODULLE == "LIQUIDATION") != null)
                        def = tom.TP_MPIECES_JUSTIFICATIVES.FirstOrDefault(a => a.NUMERO_FICHE == IdF && a.TYPEPIECE == "DEF" && a.MODULLE == "LIQUIDATION").LIEN;
                    var tef = "";
                    if (tom.TP_MPIECES_JUSTIFICATIVES.FirstOrDefault(a => a.NUMERO_FICHE == IdF && a.TYPEPIECE == "TEF" && a.MODULLE == "LIQUIDATION") != null)
                        tef = tom.TP_MPIECES_JUSTIFICATIVES.FirstOrDefault(a => a.NUMERO_FICHE == IdF && a.TYPEPIECE == "TEF" && a.MODULLE == "LIQUIDATION").LIEN;
                    var be = "";
                    if (tom.TP_MPIECES_JUSTIFICATIVES.FirstOrDefault(a => a.NUMERO_FICHE == IdF && a.TYPEPIECE == "BE" && a.MODULLE == "LIQUIDATION") != null)
                        be = tom.TP_MPIECES_JUSTIFICATIVES.FirstOrDefault(a => a.NUMERO_FICHE == IdF && a.TYPEPIECE == "BE" && a.MODULLE == "LIQUIDATION").LIEN;

                    newElemH = new DATATRPROJET()
                    {
                        REF = String.IsNullOrEmpty(def) ? "" : def,
                        OBJ = String.IsNullOrEmpty(tef) ? "" : tef,
                        TITUL = String.IsNullOrEmpty(be) ? "" : be
                    };
                }

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = newElemH }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }


        [HttpPost]
        public JsonResult GetIsMotif()
        {
            try
            {
                List<DATATRPROJET> list = new List<DATATRPROJET>();

                if (db.SI_MOTIF.Any())
                {
                    string[] separators = { "," };

                    var Tomail = db.SI_MOTIF.FirstOrDefault().MOTIFTRAIT;
                    if (Tomail != null)
                    {
                        string listUser = Tomail.ToString();
                        string[] mailListe = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var mailto in mailListe)
                        {
                            list.Add(new DATATRPROJET
                            {
                                REF = mailto
                            });
                        }
                    }
                }

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = list }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        [HttpPost]
        public JsonResult AnnulationMandat(SI_USERS suser, Guid IdF, string Comm, string Motif, int iProjet)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int IdS = iProjet;

                var lien = "http://srvapp.softwell.cloud/softconnectsiig/";

                var ProjetIntitule = db.SI_PROJETS.Where(a => a.ID == IdS && a.DELETIONDATE == null).FirstOrDefault().PROJET;

                if (db.SI_TRAITPROJET.FirstOrDefault(a => a.No == IdF && a.IDPROJET == IdS) != null)
                {
                    var ismod = db.SI_TRAITPROJET.FirstOrDefault(a => a.No == IdF && a.IDPROJET == IdS);
                    ismod.ETAT = 2;
                    //ismod.DATECRE = DateTime.Now;
                    ismod.DATEANNUL = DateTime.Now;
                    ismod.IDUSERANNUL = exist.ID;

                    db.SaveChanges();
                }

                var newElemH = new SI_TRAITANNUL()
                {
                    No = IdF,
                    DATEANNUL = DateTime.Now,
                    MOTIF = Motif,
                    COMMENTAIRE = Comm,
                    IDPROJET = IdS,
                    IDUSER = exist.ID
                };
                db.SI_TRAITANNUL.Add(newElemH);
                db.SaveChanges();

                //SEND MAIL ALERT et NOTIFICATION//
                string MailAdresse = "serviceinfo@softwell.mg";
                string mdpMail = "09eYpçç0601";

                using (System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage())
                {
                    SmtpClient smtp = new SmtpClient("smtpauth.moov.mg");
                    smtp.UseDefaultCredentials = true;

                    mail.From = new MailAddress(MailAdresse);

                    mail.To.Add(MailAdresse);
                    if (db.SI_MAIL.FirstOrDefault(a => a.IDPROJET == IdS && a.DELETIONDATE == null).MAILREJET != null)
                    {
                        string[] separators = { ";" };

                        var Tomail = mail;
                        if (Tomail != null)
                        {
                            string listUser = db.SI_MAIL.FirstOrDefault(a => a.IDPROJET == IdS && a.DELETIONDATE == null).MAILREJET;
                            string[] mailListe = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var mailto in mailListe)
                            {
                                mail.To.Add(mailto);
                            }
                        }
                    }

                    mail.Subject = "Rejet pièce du projet " + ProjetIntitule;
                    mail.IsBodyHtml = true;
                    mail.Body = "Madame, Monsieur,<br/><br>" + "Nous vous informons que vous avez une pièce rejetée pour le compte du projet " + ProjetIntitule + ".<br/><br>" +
                        "Nous vous remercions de cliquer <a href='" + lien + "'>(ici)</a> pour accéder à la plate-forme SOFT-SIIG CONNECT.<br/><br>" + "Cordialement";

                    smtp.Port = 587;
                    smtp.Credentials = new System.Net.NetworkCredential(MailAdresse, mdpMail);
                    smtp.EnableSsl = true;

                    try { smtp.Send(mail); }
                    catch (Exception) { }
                }

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Rejet avec succès. ", data = Comm }, settings));
            }
            catch (Exception)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Erreur d'enregistrement de l'information. " }, settings));
            }
        }

        //Traitement mandats ORDSECOTHER//
        public ActionResult TraitementORDSECOTHER()
        {
            ViewBag.Controller = "Dépenses initiées";

            return View();
        }

        //GENERATION SIIGLOADOTHER//
        [HttpPost]
        public JsonResult GenerationSIIGLOADOTHER(SI_USERS suser, int iProjet)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = iProjet;
                //Check si le projet est mappé à une base de données TOM²PRO//
                if (db.SI_MAPPAGES.FirstOrDefault(a => a.IDPROJET == crpt) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Le projet n'est pas mappé à une base de données TOM²PRO. " }, settings));


                SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
                SOFTCONNECTOM tom = new SOFTCONNECTOM();

                List<DATATRPROJET> list = new List<DATATRPROJET>();

                //Check si la correspondance des états est OK//
                var numCaEtapAPP = db.SI_PARAMETAT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null);
                if (numCaEtapAPP == null) return Json(JsonConvert.SerializeObject(new { type = "PEtat", msg = "Veuillez paramétrer la correspondance des états. " }, settings));
                //TEST si les états dans les paramètres dans cohérents avec ceux de TOM²PRO//
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.DEF) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état DEF n'est pas paramétré sur TOM²PRO. " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.TEF) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état TEF n'est pas paramétré sur TOM²PRO. " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.BE) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état BE n'est pas paramétré sur TOM²PRO. " }, settings));

                if (tom.CPTADMIN_FLIQUIDATION.Any())
                {
                    foreach (var x in tom.CPTADMIN_FLIQUIDATION.OrderBy(a => a.DATELIQUIDATION).ToList())
                    {
                        decimal MTN = 0;
                        decimal MTNPJ = 0;
                        var PCOP = "";

                        //Get total MTN dans CPTADMIN_MLIQUIDATION pour vérification du SOMMES MTN M = SOMMES MTN MPJ//
                        if (tom.CPTADMIN_MLIQUIDATION.Any(a => a.IDLIQUIDATION == x.ID))
                        {
                            foreach (var y in tom.CPTADMIN_MLIQUIDATION.Where(a => a.IDLIQUIDATION == x.ID).ToList())
                            {
                                MTN += y.MONTANTLOCAL.Value;

                                if (String.IsNullOrEmpty(PCOP))
                                    PCOP = y.POSTE;
                            }
                        }

                        //TEST SI SOMMES MTN M = SOMMES MTN MPJ//
                        var IDString = x.ID.ToString();
                        if (tom.TP_MPIECES_JUSTIFICATIVES.Any(a => a.NUMERO_FICHE == IDString && a.MODULLE == "LIQUIDATION"))
                        {
                            foreach (var y in tom.TP_MPIECES_JUSTIFICATIVES.Where(a => a.NUMERO_FICHE == IDString && a.MODULLE == "LIQUIDATION").ToList())
                            {
                                MTNPJ += y.MONTANT.Value;
                            }
                        }

                        //MathRound 3 satria kely kokoa ny marge d'erreur no le 2//
                        if (Math.Round(MTN, 3) == Math.Round(MTNPJ, 3))
                        {
                            //Check si F a déjà passé les 3 étapes (DEF, TEF et BE) pour avoir les dates => BE étape finale//
                            var canBeDEF = true;
                            var canBeTEF = true;
                            var canBeBE = true;
                            if (tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.DEF) == null)
                                canBeDEF = false;
                            if (tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.TEF) == null)
                                canBeTEF = false;
                            if (tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.BE) == null)
                                canBeBE = false;

                            //TEST que F n'est pas encore traité ou F a été annulé// ETAT annulé = 2//
                            if (canBeDEF == false || canBeTEF == false || canBeBE == false)
                            {
                                var titulaire = "";
                                if (tom.RTIERS.Any(a => a.COGE == x.COGEBENEFICIAIRE && a.AUXI == x.AUXIBENEFICIAIRE))
                                    titulaire = tom.RTIERS.FirstOrDefault(a => a.COGE == x.COGEBENEFICIAIRE && a.AUXI == x.AUXIBENEFICIAIRE).NOM;

                                DateTime? DATEDEF = null;
                                if (tom.CPTADMIN_TRAITEMENT.Any(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.DEF))
                                    DATEDEF = tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.DEF).DATECA;
                                DateTime? DATETEF = null;
                                if (tom.CPTADMIN_TRAITEMENT.Any(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.TEF))
                                    DATETEF = tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.TEF).DATECA;
                                DateTime? DATEBE = null;
                                if (tom.CPTADMIN_TRAITEMENT.Any(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.BE))
                                    DATEBE = tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.BE).DATECA;

                                var soa = (from soas in db.SI_SOAS
                                           join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                           where prj.IDPROJET == crpt && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                           select new
                                           {
                                               soas.SOA
                                           });

                                list.Add(new DATATRPROJET
                                {
                                    No = x.ID,
                                    REF = x.NUMEROCA,
                                    OBJ = x.DESCRIPTION,
                                    TITUL = titulaire,
                                    MONT = Math.Round(MTN, 2).ToString(),
                                    COMPTE = x.COGEBENEFICIAIRE,
                                    DATE = x.DATELIQUIDATION.Value.Date,
                                    PCOP = PCOP,
                                    DATEDEF = DATEDEF,
                                    DATETEF = DATETEF,
                                    DATEBE = DATEBE,
                                    SOA = soa.FirstOrDefault().SOA,
                                    PROJET = db.SI_PROJETS.Where(a => a.ID == crpt && a.DELETIONDATE == null).FirstOrDefault().PROJET
                                });
                            }
                        }
                    }
                }

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = list }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        //GENERATION SIIGLOADOTHER//
        [HttpPost]
        public JsonResult GenerationSIIGOTHER(SI_USERS suser, DateTime DateDebut, DateTime DateFin, int iProjet)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = iProjet;
                //Check si le projet est mappé à une base de données TOM²PRO//
                if (db.SI_MAPPAGES.FirstOrDefault(a => a.IDPROJET == crpt) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Le projet n'est pas mappé à une base de données TOM²PRO. " }, settings));

                SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
                SOFTCONNECTOM tom = new SOFTCONNECTOM();

                List<DATATRPROJET> list = new List<DATATRPROJET>();

                //Check si la correspondance des états est OK//
                var numCaEtapAPP = db.SI_PARAMETAT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null);
                if (numCaEtapAPP == null) return Json(JsonConvert.SerializeObject(new { type = "PEtat", msg = "Veuillez paramétrer la correspondance des états. " }, settings));
                //TEST si les états dans les paramètres dans cohérents avec ceux de TOM²PRO//
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.DEF) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état DEF n'est pas paramétré sur TOM²PRO. " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.TEF) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état TEF n'est pas paramétré sur TOM²PRO. " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.BE) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état BE n'est pas paramétré sur TOM²PRO. " }, settings));

                if (tom.CPTADMIN_FLIQUIDATION.Any(a => a.DATELIQUIDATION >= DateDebut && a.DATELIQUIDATION <= DateFin))
                {
                    foreach (var x in tom.CPTADMIN_FLIQUIDATION.Where(a => a.DATELIQUIDATION >= DateDebut && a.DATELIQUIDATION <= DateFin).OrderBy(a => a.DATELIQUIDATION).ToList())
                    {
                        decimal MTN = 0;
                        decimal MTNPJ = 0;
                        var PCOP = "";

                        //Get total MTN dans CPTADMIN_MLIQUIDATION pour vérification du SOMMES MTN M = SOMMES MTN MPJ//
                        if (tom.CPTADMIN_MLIQUIDATION.Any(a => a.IDLIQUIDATION == x.ID))
                        {
                            foreach (var y in tom.CPTADMIN_MLIQUIDATION.Where(a => a.IDLIQUIDATION == x.ID).ToList())
                            {
                                MTN += y.MONTANTLOCAL.Value;

                                if (String.IsNullOrEmpty(PCOP))
                                    PCOP = y.POSTE;
                            }
                        }

                        //TEST SI SOMMES MTN M = SOMMES MTN MPJ//
                        var IDString = x.ID.ToString();
                        if (tom.TP_MPIECES_JUSTIFICATIVES.Any(a => a.NUMERO_FICHE == IDString && a.MODULLE == "LIQUIDATION"))
                        {
                            foreach (var y in tom.TP_MPIECES_JUSTIFICATIVES.Where(a => a.NUMERO_FICHE == IDString && a.MODULLE == "LIQUIDATION").ToList())
                            {
                                MTNPJ += y.MONTANT.Value;
                            }
                        }

                        //MathRound 3 satria kely kokoa ny marge d'erreur no le 2//
                        if (Math.Round(MTN, 3) == Math.Round(MTNPJ, 3))
                        {
                            //Check si F a déjà passé les 3 étapes (DEF, TEF et BE) pour avoir les dates => BE étape finale//
                            var canBeDEF = true;
                            var canBeTEF = true;
                            var canBeBE = true;
                            if (tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.DEF) == null)
                                canBeDEF = false;
                            if (tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.TEF) == null)
                                canBeTEF = false;
                            if (tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.BE) == null)
                                canBeBE = false;

                            //TEST que F n'est pas encore traité ou F a été annulé// ETAT annulé = 2//
                            if (canBeDEF == false || canBeTEF == false || canBeBE == false)
                            {
                                var titulaire = "";
                                if (tom.RTIERS.Any(a => a.COGE == x.COGEBENEFICIAIRE && a.AUXI == x.AUXIBENEFICIAIRE))
                                    titulaire = tom.RTIERS.FirstOrDefault(a => a.COGE == x.COGEBENEFICIAIRE && a.AUXI == x.AUXIBENEFICIAIRE).NOM;

                                DateTime? DATEDEF = null;
                                if (tom.CPTADMIN_TRAITEMENT.Any(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.DEF))
                                    DATEDEF = tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.DEF).DATECA;
                                DateTime? DATETEF = null;
                                if (tom.CPTADMIN_TRAITEMENT.Any(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.TEF))
                                    DATETEF = tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.TEF).DATECA;
                                DateTime? DATEBE = null;
                                if (tom.CPTADMIN_TRAITEMENT.Any(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.BE))
                                    DATEBE = tom.CPTADMIN_TRAITEMENT.FirstOrDefault(a => a.NUMEROCA == x.NUMEROCA && a.NUMCAETAPE == numCaEtapAPP.BE).DATECA;

                                var soa = (from soas in db.SI_SOAS
                                           join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                           where prj.IDPROJET == crpt && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                           select new
                                           {
                                               soas.SOA
                                           });

                                list.Add(new DATATRPROJET
                                {
                                    No = x.ID,
                                    REF = x.NUMEROCA,
                                    OBJ = x.DESCRIPTION,
                                    TITUL = titulaire,
                                    MONT = Math.Round(MTN, 2).ToString(),
                                    COMPTE = x.COGEBENEFICIAIRE,
                                    DATE = x.DATELIQUIDATION.Value.Date,
                                    PCOP = PCOP,
                                    DATEDEF = DATEDEF,
                                    DATETEF = DATETEF,
                                    DATEBE = DATEBE,
                                    SOA = soa.FirstOrDefault().SOA,
                                    PROJET = db.SI_PROJETS.Where(a => a.ID == crpt && a.DELETIONDATE == null).FirstOrDefault().PROJET
                                });
                            }
                        }
                    }
                }

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = list }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        //GENERATION SIIGLOAD SEND//
        [HttpPost]
        public JsonResult GenerationSIIGLOADSEND(SI_USERS suser, int iProjet)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = iProjet;

                int retarDate = 0;
                if (db.SI_DELAISTRAITEMENT.Any(a => a.IDPROJET == crpt && a.DELETIONDATE == null))
                    retarDate = db.SI_DELAISTRAITEMENT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null).DELENVOISIIGFP.Value;

                //Check si le projet est mappé à une base de données TOM²PRO//
                if (db.SI_MAPPAGES.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Le projet n'est pas mappé à une base de données TOM²PRO. " }, settings));

                SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
                SOFTCONNECTOM tom = new SOFTCONNECTOM();

                List<DATATRPROJET> list = new List<DATATRPROJET>();

                //Check si la correspondance des états est OK//
                var numCaEtapAPP = db.SI_PARAMETAT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null);
                if (numCaEtapAPP == null) return Json(JsonConvert.SerializeObject(new { type = "PEtat", msg = "Veuillez paramétrer la correspondance des états. " }, settings));
                //TEST si les états dans les paramètres dans cohérents avec ceux de TOM²PRO//
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.DEF) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état DEF n'est pas paramétré sur TOM²PRO. " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.TEF) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état TEF n'est pas paramétré sur TOM²PRO. " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.BE) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état BE n'est pas paramétré sur TOM²PRO. " }, settings));

                if (db.SI_TRAITPROJET.FirstOrDefault(a => a.IDPROJET == crpt && a.ETAT == 1) != null)
                {
                    foreach (var x in db.SI_TRAITPROJET.Where(a => a.IDPROJET == crpt && a.ETAT == 1).OrderBy(a => a.DATECRE).OrderBy(a => a.DATEMANDAT).ToList())
                    {
                        var soa = (from soas in db.SI_SOAS
                                   join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                   where prj.IDPROJET == crpt && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                   select new
                                   {
                                       soas.SOA
                                   });

                        bool isLate = false;
                        if (x.DATEVALIDATION.Value.AddBusinessDays(retarDate).Date < DateTime.Now/* && ((int)DateTime.Now.DayOfWeek) != 6 && ((int)DateTime.Now.DayOfWeek) != 0*/)
                            isLate = true;

                        list.Add(new DATATRPROJET
                        {
                            No = x.No,
                            REF = x.REF,
                            OBJ = x.OBJ,
                            TITUL = x.TITUL,
                            MONT = Data.Cipher.Decrypt(x.MONT, "Oppenheimer").ToString(),
                            COMPTE = x.COMPTE,
                            DATE = x.DATEMANDAT.Value.Date,
                            PCOP = x.PCOP,
                            DATEDEF = x.DATEDEF.Value.Date,
                            DATETEF = x.DATETEF.Value.Date,
                            DATEBE = x.DATEBE.Value.Date,
                            LIEN = db.SI_USERS.FirstOrDefault(a => a.ID == x.IDUSERVALIDATE).LOGIN,
                            DATECREATION = x.DATEVALIDATION.Value.Date,
                            SOA = soa.FirstOrDefault().SOA,
                            PROJET = db.SI_PROJETS.Where(a => a.ID == crpt && a.DELETIONDATE == null).FirstOrDefault().PROJET,
                            isLATE = isLate
                        });
                    }
                    //listORDER = list.OrderByDescending(a => a.isLATE).ToList();
                }

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = list.OrderByDescending(a => a.isLATE).ToList() }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        public ActionResult TraitementSENDSIIGFP()
        {
            ViewBag.Controller = "Transfert SIIGFP";

            return View();
        }

        [HttpPost]
        public JsonResult GenerationSIIGENVOI(SI_USERS suser, DateTime DateDebut, DateTime DateFin, int iProjet)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = iProjet;

                int retarDate = 0;
                if (db.SI_DELAISTRAITEMENT.Any(a => a.IDPROJET == crpt && a.DELETIONDATE == null))
                    retarDate = db.SI_DELAISTRAITEMENT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null).DELENVOISIIGFP.Value;

                //Check si le projet est mappé à une base de données TOM²PRO//
                if (db.SI_MAPPAGES.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Le projet n'est pas mappé à une base de données TOM²PRO. " }, settings));

                SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
                SOFTCONNECTOM tom = new SOFTCONNECTOM();

                List<DATATRPROJET> list = new List<DATATRPROJET>();

                //Check si la correspondance des états est OK//
                var numCaEtapAPP = db.SI_PARAMETAT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null);
                if (numCaEtapAPP == null) return Json(JsonConvert.SerializeObject(new { type = "PEtat", msg = "Veuillez paramétrer la correspondance des états. " }, settings));
                //TEST si les états dans les paramètres dans cohérents avec ceux de TOM²PRO//
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.DEF) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état DEF n'est pas paramétré sur TOM²PRO. " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.TEF) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état TEF n'est pas paramétré sur TOM²PRO. " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.BE) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état BE n'est pas paramétré sur TOM²PRO. " }, settings));

                if (db.SI_TRAITPROJET.Any(a => a.IDPROJET == crpt && a.DATEMANDAT >= DateDebut && a.DATEMANDAT <= DateFin && a.ETAT == 1))
                {
                    foreach (var x in db.SI_TRAITPROJET.Where(a => a.IDPROJET == crpt && a.DATEMANDAT >= DateDebut && a.DATEMANDAT <= DateFin && a.ETAT == 1).OrderBy(a => a.DATECRE).OrderBy(a => a.DATEMANDAT).ToList())
                    {
                        var soa = (from soas in db.SI_SOAS
                                   join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                   where prj.IDPROJET == crpt && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                   select new
                                   {
                                       soas.SOA
                                   });

                        bool isLate = false;
                        if (x.DATEVALIDATION.Value.AddBusinessDays(retarDate).Date < DateTime.Now/* && ((int)DateTime.Now.DayOfWeek) != 6 && ((int)DateTime.Now.DayOfWeek) != 0*/)
                            isLate = true;

                        list.Add(new DATATRPROJET
                        {
                            No = x.No,
                            REF = x.REF,
                            OBJ = x.OBJ,
                            TITUL = x.TITUL,
                            MONT = Data.Cipher.Decrypt(x.MONT, "Oppenheimer").ToString(),
                            COMPTE = x.COMPTE,
                            DATE = x.DATEMANDAT.Value.Date,
                            PCOP = x.PCOP,
                            DATEDEF = x.DATEDEF.Value.Date,
                            DATETEF = x.DATETEF.Value.Date,
                            DATEBE = x.DATEBE.Value.Date,
                            LIEN = db.SI_USERS.FirstOrDefault(a => a.ID == x.IDUSERVALIDATE).LOGIN,
                            DATECREATION = x.DATEVALIDATION.Value.Date,
                            SOA = soa.FirstOrDefault().SOA,
                            PROJET = db.SI_PROJETS.Where(a => a.ID == crpt && a.DELETIONDATE == null).FirstOrDefault().PROJET,
                            isLATE = isLate
                        });
                    }
                }

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = list.OrderByDescending(a => a.isLATE).ToList() }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        [HttpPost]
        public JsonResult GetCheckedEcritureORDSECSEND(SI_USERS suser, string listCompte, int iProjet)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            int countTraitement = 0;
            int crpt = iProjet;
            var lien = "http://srvapp.softwell.cloud/softconnectsiig/";

            var ProjetIntitule = db.SI_PROJETS.Where(a => a.ID == crpt && a.DELETIONDATE == null).FirstOrDefault().PROJET;

            var listCompteS = listCompte.Split(',');
            foreach (var SAV in listCompteS)
            {
                try
                {
                    SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
                    SOFTCONNECTOM tom = new SOFTCONNECTOM();

                    List<DATATRPROJET> list = new List<DATATRPROJET>();

                    Guid isSAV = Guid.Parse(SAV);
                    if (db.SI_TRAITPROJET.FirstOrDefault(a => a.IDPROJET == crpt && a.No == isSAV) != null)
                    {
                        //SEND SIIGFP//


                        var isModified = db.SI_TRAITPROJET.FirstOrDefault(a => a.IDPROJET == crpt && a.No == isSAV);
                        isModified.ETAT = 3;
                        isModified.DATENVOISIIGFP = DateTime.Now;
                        isModified.IDUSERENVOISIIGFP = exist.ID;
                        db.SaveChanges();

                        countTraitement++;
                    }
                }
                catch (Exception e)
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
                }
            }

            //SEND MAIL ALERT et NOTIFICATION//
            string MailAdresse = "serviceinfo@softwell.mg";
            string mdpMail = "09eYpçç0601";

            using (System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage())
            {
                SmtpClient smtp = new SmtpClient("smtpauth.moov.mg");
                smtp.UseDefaultCredentials = true;

                mail.From = new MailAddress(MailAdresse);

                mail.To.Add(MailAdresse);
                if (db.SI_MAIL.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null).MAILSIIG != null)
                {
                    string[] separators = { ";" };

                    var Tomail = mail;
                    if (Tomail != null)
                    {
                        string listUser = db.SI_MAIL.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null).MAILSIIG;
                        string[] mailListe = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var mailto in mailListe)
                        {
                            mail.To.Add(mailto);
                        }
                    }
                }

                mail.Subject = "Validation pièces du projet " + ProjetIntitule;
                mail.IsBodyHtml = true;
                mail.Body = "Madame, Monsieur,<br/><br>" + "Nous vous informons que vous avez " + countTraitement + " pièces transférées pour le compte du projet " + ProjetIntitule + " et en attente du traitement de SIIGFP.<br/><br>" +
                    "Nous vous remercions de cliquer <a href='" + lien + "'>(ici)</a> pour accéder à la plate-forme SOFT-SIIG CONNECT.<br/><br>" + "Cordialement";

                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential(MailAdresse, mdpMail);
                smtp.EnableSsl = true;

                try { smtp.Send(mail); }
                catch (Exception) { }
            }

            return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Traitement avec succès. ", data = "" }, settings));
        }


        //GENERATION PAIEMENT//
        public ActionResult GenerationPAIEMENTIndex()
        {
            ViewBag.Controller = "Liste des engagements ou avances par paiement";

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> SetGlobalStates(SI_USERS suser, int IdF, string numeroliquidations, string estAvance)
        {
            var exist = await db.SI_USERS.FirstOrDefaultAsync(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null);

            if (exist == null)
            {
                return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));
            }

            idF = IdF;
            Numeroliquidations = numeroliquidations;
            EstAvance = estAvance;

            return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = "" }, settings));
        }

        [HttpPost]
        public JsonResult GenerationPAIEMENT(SI_USERS suser)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            if (idF == 0 || Numeroliquidations == "" || EstAvance == "")
            {
                return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));
            }

            try
            {
                int crpt = idF;
                bool isAvance = bool.Parse(EstAvance);

                int retarDate = 0;
                if (db.SI_DELAISTRAITEMENT.Any(a => a.IDPROJET == crpt && a.DELETIONDATE == null))
                    retarDate = db.SI_DELAISTRAITEMENT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null).DELTV.Value;

                //Check si le projet est mappé à une base de données TOM²PRO//
                if (db.SI_MAPPAGES.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Le projet n'est pas mappé à une base de données TOM²PRO. " }, settings));

                SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
                SOFTCONNECTOM tom = new SOFTCONNECTOM();

                List<DATATRPROJET> list = new List<DATATRPROJET>();

                //Check si la correspondance des états est OK//
                var numCaEtapAPP = db.SI_PARAMETAT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null);
                if (numCaEtapAPP == null) return Json(JsonConvert.SerializeObject(new { type = "PEtat", msg = "Veuillez paramétrer la correspondance des états. " }, settings));
                //TEST si les états dans les paramètres dans cohérents avec ceux de TOM²PRO//
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.DEF) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état DEF n'est pas paramétré sur TOM²PRO. " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.TEF) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état TEF n'est pas paramétré sur TOM²PRO. " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.BE) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état BE n'est pas paramétré sur TOM²PRO. " }, settings));

                if (isAvance)
                {
                    if (db.SI_TRAITAVANCE.FirstOrDefault(a => a.IDPROJET == crpt && a.ETAT == 1 && a.REF == Numeroliquidations) != null)
                    {
                        foreach (var x in db.SI_TRAITAVANCE.Where(a => a.IDPROJET == crpt && a.ETAT == 1 && a.REF == Numeroliquidations).OrderBy(a => a.DATECRE).OrderBy(a => a.DATEMANDAT).ToList())
                        {
                            var soa = (from soas in db.SI_SOAS
                                       join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                       where prj.IDPROJET == crpt && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                       select new
                                       {
                                           soas.SOA
                                       });

                            bool isLate = false;
                            if (x.DATECRE.Value.AddBusinessDays(retarDate).Date < DateTime.Now/* && ((int)DateTime.Now.DayOfWeek) != 6 && ((int)DateTime.Now.DayOfWeek) != 0*/)
                                isLate = true;

                            list.Add(new DATATRPROJET
                            {
                                No = x.No,
                                REF = x.REF,
                                OBJ = x.OBJ,
                                TITUL = x.TITUL,
                                MONT = Data.Cipher.Decrypt(x.MONT, "Oppenheimer").ToString(),
                                COMPTE = x.COMPTE,
                                DATE = x.DATEMANDAT.Value.Date,
                                PCOP = x.PCOP,
                                DATEDEF = x.DATEDEF.Value.Date,
                                DATETEF = x.DATETEF.Value.Date,
                                DATEBE = x.DATEBE.Value.Date,
                                LIEN = db.SI_USERS.FirstOrDefault(a => a.ID == x.IDUSERCREATE).LOGIN,
                                DATECREATION = x.DATECRE.Value.Date,
                                SOA = soa.FirstOrDefault().SOA,
                                PROJET = db.SI_PROJETS.Where(a => a.ID == crpt && a.DELETIONDATE == null).FirstOrDefault().PROJET,
                                isLATE = isLate,
                                isAvance = true
                            });
                        }
                    }
                }
                else
                {
                    if (db.SI_TRAITPROJET.FirstOrDefault(a => a.IDPROJET == crpt && a.ETAT == 1 && a.REF == Numeroliquidations) != null)
                    {
                        foreach (var x in db.SI_TRAITPROJET.Where(a => a.IDPROJET == crpt && a.ETAT == 1 && a.REF == Numeroliquidations).OrderBy(a => a.DATECRE).OrderBy(a => a.DATEMANDAT).ToList())
                        {
                            var soa = (from soas in db.SI_SOAS
                                       join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                       where prj.IDPROJET == crpt && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                       select new
                                       {
                                           soas.SOA
                                       });

                            bool isLate = false;
                            if (x.DATECRE.Value.AddBusinessDays(retarDate).Date < DateTime.Now/* && ((int)DateTime.Now.DayOfWeek) != 6 && ((int)DateTime.Now.DayOfWeek) != 0*/)
                                isLate = true;

                            list.Add(new DATATRPROJET
                            {
                                No = x.No,
                                REF = x.REF,
                                OBJ = x.OBJ,
                                TITUL = x.TITUL,
                                MONT = Data.Cipher.Decrypt(x.MONT, "Oppenheimer").ToString(),
                                COMPTE = x.COMPTE,
                                DATE = x.DATEMANDAT.Value.Date,
                                PCOP = x.PCOP,
                                DATEDEF = x.DATEDEF.Value.Date,
                                DATETEF = x.DATETEF.Value.Date,
                                DATEBE = x.DATEBE.Value.Date,
                                LIEN = db.SI_USERS.FirstOrDefault(a => a.ID == x.IDUSERCREATE).LOGIN,
                                DATECREATION = x.DATECRE.Value.Date,
                                SOA = soa.FirstOrDefault().SOA,
                                PROJET = db.SI_PROJETS.Where(a => a.ID == crpt && a.DELETIONDATE == null).FirstOrDefault().PROJET,
                                isLATE = isLate,
                                isAvance = false
                            });
                        }
                    }
                }

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = list.OrderByDescending(a => a.isLATE).ToList() }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ModalFRFR(SI_USERS suser, string IdF)
        {
            var exist = await db.SI_USERS.FirstOrDefaultAsync(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = idF;
                bool isAvance = bool.Parse(EstAvance);

                SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
                SOFTCONNECTOM tom = new SOFTCONNECTOM();

                List<DATATRPROJET> list = new List<DATATRPROJET>();

                if (isAvance)
                {
                    if (tom.TP_MPIECES_JUSTIFICATIVES.FirstOrDefault(a => a.NUMERO_FICHE == IdF && a.MODULLE == "CPTADMINAVANCE") != null)
                    {
                        foreach (var x in tom.TP_MPIECES_JUSTIFICATIVES.Where(a => a.NUMERO_FICHE == IdF && a.TYPEPIECE != "DEF" && a.TYPEPIECE != "TEF" && a.TYPEPIECE != "BE" && a.MODULLE == "CPTADMINAVANCE").OrderBy(a => a.RANG).ToList())
                        {
                            var idFGuid = Guid.Parse(IdF);
                            DateTime dpj = tom.CPTADMIN_FAVANCE.Where(a => a.ID == idFGuid).FirstOrDefault().DATEAVANCE.Value;
                            list.Add(new DATATRPROJET
                            {
                                REF = x.TYPEPIECE != null ? x.TYPEPIECE : "",
                                OBJ = x.RANG != null ? x.RANG.ToString() : "",
                                TITUL = x.NOMBRE != null ? x.NOMBRE.ToString() : "",
                                DATE = dpj,
                                MONT = x.MONTANT != null ? Math.Round(x.MONTANT.Value, 2).ToString() : "0",
                                LIEN = !String.IsNullOrEmpty(x.LIEN) ? x.LIEN : ""
                            });
                        }
                    }
                }
                else
                {
                    if (tom.TP_MPIECES_JUSTIFICATIVES.FirstOrDefault(a => a.NUMERO_FICHE == IdF && a.MODULLE == "LIQUIDATION") != null)
                    {
                        foreach (var x in tom.TP_MPIECES_JUSTIFICATIVES.Where(a => a.NUMERO_FICHE == IdF && a.TYPEPIECE != "DEF" && a.TYPEPIECE != "TEF" && a.TYPEPIECE != "BE" && a.MODULLE == "LIQUIDATION").OrderBy(a => a.RANG).ToList())
                        {
                            var idFGuid = Guid.Parse(IdF);
                            DateTime dpj = tom.CPTADMIN_FLIQUIDATION.Where(a => a.ID == idFGuid).FirstOrDefault().DATELIQUIDATION.Value;
                            list.Add(new DATATRPROJET
                            {
                                REF = x.TYPEPIECE != null ? x.TYPEPIECE : "",
                                OBJ = x.RANG != null ? x.RANG.ToString() : "",
                                TITUL = x.NOMBRE != null ? x.NOMBRE.ToString() : "",
                                DATE = dpj,
                                MONT = x.MONTANT != null ? Math.Round(x.MONTANT.Value, 2).ToString() : "0",
                                LIEN = !String.IsNullOrEmpty(x.LIEN) ? x.LIEN : ""
                            });
                        }
                    }
                }

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = list }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ModalDRFR(SI_USERS suser, Guid IdF)
        {
            var exist = await db.SI_USERS.FirstOrDefaultAsync(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = idF;
                bool isAvance = bool.Parse(EstAvance);

                SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
                SOFTCONNECTOM tom = new SOFTCONNECTOM();

                List<DATATRPROJET> list = new List<DATATRPROJET>();

                if (isAvance)
                {
                    if (tom.CPTADMIN_MAVANCE.FirstOrDefault(a => a.IDAVANCE == IdF) != null)
                    {
                        foreach (var x in tom.CPTADMIN_MAVANCE.Where(a => a.IDAVANCE == IdF).ToList())
                        {
                            list.Add(new DATATRPROJET
                            {
                                REF = x.LIBELLE != null ? x.LIBELLE : "",
                                OBJ = x.COGE != null ? x.COGE.ToString() : "",
                                TITUL = x.POSTE != null ? x.POSTE.ToString() : "",
                                MONT = x.MONTANTLOCAL != null ? Math.Round(x.MONTANTLOCAL.Value, 2).ToString() : "0",
                            });
                        }
                    }
                }
                else
                {
                    if (tom.CPTADMIN_MLIQUIDATION.FirstOrDefault(a => a.IDLIQUIDATION == IdF) != null)
                    {
                        foreach (var x in tom.CPTADMIN_MLIQUIDATION.Where(a => a.IDLIQUIDATION == IdF).ToList())
                        {
                            list.Add(new DATATRPROJET
                            {
                                REF = x.LIBELLE != null ? x.LIBELLE : "",
                                OBJ = x.COGE != null ? x.COGE.ToString() : "",
                                TITUL = x.POSTE != null ? x.POSTE.ToString() : "",
                                MONT = x.MONTANTLOCAL != null ? Math.Round(x.MONTANTLOCAL.Value, 2).ToString() : "0",
                            });
                        }
                    }
                }

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = list }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ModalLIASRFR(SI_USERS suser, string IdF)
        {
            var exist = await db.SI_USERS.FirstOrDefaultAsync(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = idF;
                bool isAvance = bool.Parse(EstAvance);

                SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
                SOFTCONNECTOM tom = new SOFTCONNECTOM();

                var newElemH = new DATATRPROJET()
                {
                    REF = "",
                    OBJ = "",
                    TITUL = ""
                };

                if (isAvance)
                {
                    if (tom.TP_MPIECES_JUSTIFICATIVES.FirstOrDefault(a => a.NUMERO_FICHE == IdF && (a.TYPEPIECE == "DEF" || a.TYPEPIECE == "TEF" || a.TYPEPIECE == "BE") && a.MODULLE == "CPTADMINAVANCE") != null)
                    {
                        var def = "";
                        if (tom.TP_MPIECES_JUSTIFICATIVES.FirstOrDefault(a => a.NUMERO_FICHE == IdF && a.TYPEPIECE == "DEF" && a.MODULLE == "CPTADMINAVANCE") != null)
                            def = tom.TP_MPIECES_JUSTIFICATIVES.FirstOrDefault(a => a.NUMERO_FICHE == IdF && a.TYPEPIECE == "DEF" && a.MODULLE == "CPTADMINAVANCE").LIEN;
                        var tef = "";
                        if (tom.TP_MPIECES_JUSTIFICATIVES.FirstOrDefault(a => a.NUMERO_FICHE == IdF && a.TYPEPIECE == "TEF" && a.MODULLE == "CPTADMINAVANCE") != null)
                            tef = tom.TP_MPIECES_JUSTIFICATIVES.FirstOrDefault(a => a.NUMERO_FICHE == IdF && a.TYPEPIECE == "TEF" && a.MODULLE == "CPTADMINAVANCE").LIEN;
                        var be = "";
                        if (tom.TP_MPIECES_JUSTIFICATIVES.FirstOrDefault(a => a.NUMERO_FICHE == IdF && a.TYPEPIECE == "BE" && a.MODULLE == "CPTADMINAVANCE") != null)
                            be = tom.TP_MPIECES_JUSTIFICATIVES.FirstOrDefault(a => a.NUMERO_FICHE == IdF && a.TYPEPIECE == "BE" && a.MODULLE == "CPTADMINAVANCE").LIEN;

                        newElemH = new DATATRPROJET()
                        {
                            REF = String.IsNullOrEmpty(def) ? "" : def,
                            OBJ = String.IsNullOrEmpty(tef) ? "" : tef,
                            TITUL = String.IsNullOrEmpty(be) ? "" : be
                        };
                    }
                }
                else
                {
                    if (tom.TP_MPIECES_JUSTIFICATIVES.FirstOrDefault(a => a.NUMERO_FICHE == IdF && (a.TYPEPIECE == "DEF" || a.TYPEPIECE == "TEF" || a.TYPEPIECE == "BE") && a.MODULLE == "LIQUIDATION") != null)
                    {
                        var def = "";
                        if (tom.TP_MPIECES_JUSTIFICATIVES.FirstOrDefault(a => a.NUMERO_FICHE == IdF && a.TYPEPIECE == "DEF" && a.MODULLE == "LIQUIDATION") != null)
                            def = tom.TP_MPIECES_JUSTIFICATIVES.FirstOrDefault(a => a.NUMERO_FICHE == IdF && a.TYPEPIECE == "DEF" && a.MODULLE == "LIQUIDATION").LIEN;
                        var tef = "";
                        if (tom.TP_MPIECES_JUSTIFICATIVES.FirstOrDefault(a => a.NUMERO_FICHE == IdF && a.TYPEPIECE == "TEF" && a.MODULLE == "LIQUIDATION") != null)
                            tef = tom.TP_MPIECES_JUSTIFICATIVES.FirstOrDefault(a => a.NUMERO_FICHE == IdF && a.TYPEPIECE == "TEF" && a.MODULLE == "LIQUIDATION").LIEN;
                        var be = "";
                        if (tom.TP_MPIECES_JUSTIFICATIVES.FirstOrDefault(a => a.NUMERO_FICHE == IdF && a.TYPEPIECE == "BE" && a.MODULLE == "LIQUIDATION") != null)
                            be = tom.TP_MPIECES_JUSTIFICATIVES.FirstOrDefault(a => a.NUMERO_FICHE == IdF && a.TYPEPIECE == "BE" && a.MODULLE == "LIQUIDATION").LIEN;

                        newElemH = new DATATRPROJET()
                        {
                            REF = String.IsNullOrEmpty(def) ? "" : def,
                            OBJ = String.IsNullOrEmpty(tef) ? "" : tef,
                            TITUL = String.IsNullOrEmpty(be) ? "" : be
                        };
                    }
                }

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = newElemH }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }
    }
}
