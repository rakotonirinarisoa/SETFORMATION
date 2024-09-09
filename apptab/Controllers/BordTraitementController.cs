using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using apptab.Data.Entities;
using apptab.Data;
using Newtonsoft.Json;
using System.Web.UI.WebControls;

namespace apptab.Controllers
{
    public class BordTraitementController : Controller
    {
        private readonly SOFTCONNECTSIIG db = new SOFTCONNECTSIIG();
        private readonly SOFTCONNECTOM tom = new SOFTCONNECTOM();

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        //GET ALL PROJET//
        [HttpPost]
        public ActionResult GetAllPROJET(SI_USERS suser)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                var test = db.SI_USERS.Where(x => x.LOGIN == exist.LOGIN && x.PWD == exist.PWD && x.DELETIONDATE == null).FirstOrDefault();
                var proj = new List<int>();

                if (test.ROLE == (int)Role.SAdministrateur)
                {
                    var user = db.SI_PROJETS.Select(a => new
                    {
                        PROJET = a.PROJET,
                        ID = a.ID,
                        DELETIONDATE = a.DELETIONDATE,
                    }).Where(a => a.DELETIONDATE == null).ToList();

                    foreach (var x in user)
                    {
                        proj.Add(x.ID);
                    }

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = new { List = user, PROJET = proj } }, settings));
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

                        foreach (var x in user)
                        {
                            proj.Add(x.ID);
                        }

                        return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = new { List = user, PROJET = proj } }, settings));
                    }
                    else
                    {
                        var user = (from usr in db.SI_PROJETS
                                    join prj in db.SI_MAPUSERPROJET on usr.ID equals prj.IDPROJET
                                    where prj.IDUS == test.ID && usr.DELETIONDATE == null
                                    select new SI_PROJETS
                                    {
                                        PROJET = usr.PROJET,
                                        ID = usr.ID,
                                        DELETIONDATE = usr.DELETIONDATE,
                                    }).ToList();

                        foreach (var x in user)
                        {
                            proj.Add(x.ID);
                        }

                        return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = new { List = user, PROJET = proj } }, settings));
                    }
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ModalF(SI_USERS suser, string IdF, string projet)
        {
            var exist = await db.SI_USERS.FirstOrDefaultAsync(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = db.SI_PROJETS.FirstOrDefault(a => a.PROJET == projet && a.DELETIONDATE == null).ID;

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
                            REF = x.TYPEPIECE,
                            OBJ = x.RANG.ToString(),
                            TITUL = x.NOMBRE.ToString(),
                            DATE = dpj,
                            MONT = Math.Round(x.MONTANT.Value, 2).ToString(),
                            LIEN = x.LIEN
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
        public async Task<JsonResult> ModalD(SI_USERS suser, Guid IdF, string projet)
        {
            var exist = await db.SI_USERS.FirstOrDefaultAsync(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = db.SI_PROJETS.FirstOrDefault(a => a.PROJET == projet && a.DELETIONDATE == null).ID;

                SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
                SOFTCONNECTOM tom = new SOFTCONNECTOM();

                List<DATATRPROJET> list = new List<DATATRPROJET>();

                if (tom.CPTADMIN_MLIQUIDATION.FirstOrDefault(a => a.IDLIQUIDATION == IdF) != null)
                {
                    foreach (var x in tom.CPTADMIN_MLIQUIDATION.Where(a => a.IDLIQUIDATION == IdF).ToList())
                    {
                        list.Add(new DATATRPROJET
                        {
                            REF = x.LIBELLE,
                            OBJ = x.COGE.ToString(),
                            TITUL = x.POSTE.ToString(),
                            MONT = Math.Round(x.MONTANTLOCAL.Value, 2).ToString(),
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
        public async Task<JsonResult> ModalLIAS(SI_USERS suser, string IdF, string projet)
        {
            var exist = await db.SI_USERS.FirstOrDefaultAsync(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = db.SI_PROJETS.FirstOrDefault(a => a.PROJET == projet && a.DELETIONDATE == null).ID;

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

        private async Task<string> GetAgent(int? userId)
        {
            if (userId == null)
            {
                return "";
            }

            var agent = await db.SI_USERS.Where(user => user.ID == userId && user.DELETIONDATE == null).Select(user => user.LOGIN).FirstOrDefaultAsync();

            if (agent == null)
            {
                return "";
            }

            return agent;
        }

        //Liste des engagements, avances et paiements//
        public ActionResult BordListeEngaPaie()
        {
            ViewBag.Controller = "Liste des engagements, avances et des paiements";

            return View();
        }

        //Genere liste des engagements et paiements//
        [HttpPost]
        public JsonResult GenereLISTE(SI_USERS suser, string listProjet, DateTime DateDebut, DateTime DateFin)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                List<TxLISTETRAIT> list = new List<TxLISTETRAIT>();
                string[] separators = { "," };
                var pro = listProjet;
                if (pro != null)
                {
                    string listUser = pro.ToString();
                    string[] lst = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var idP in lst)
                    {
                        int crpt = int.Parse(idP);

                        //int retarDate = 0;
                        //if (db.SI_DELAISTRAITEMENT.Any(a => a.IDPROJET == crpt && a.DELETIONDATE == null))
                        //    retarDate = db.SI_DELAISTRAITEMENT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null).DELTV.Value;

                        //Check si le projet est mappé à une base de données TOM²PRO//
                        if (db.SI_MAPPAGES.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null) == null)
                            return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Le projet n'est pas mappé à une base de données TOM²PRO. " }, settings));

                        SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
                        SOFTCONNECTOM tom = new SOFTCONNECTOM();

                        //Check si la correspondance des états est OK//
                        var numCaEtapAPP = db.SI_PARAMETAT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null);
                        if (numCaEtapAPP == null) return Json(JsonConvert.SerializeObject(new { type = "PEtat", msg = "Veuillez paramétrer la correspondance des états. " }, settings));
                        //TEST si les états dans les paramètres dans cohérents avec ceux de TOM²PRO//
                        if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.DEF) == null)
                            return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état DEF n'est pas paramétré sur TOM²PRO (Liquidation). " }, settings));
                        if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.TEF) == null)
                            return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état TEF n'est pas paramétré sur TOM²PRO (Liquidation). " }, settings));
                        if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.BE) == null)
                            return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état BE n'est pas paramétré sur TOM²PRO (Liquidation). " }, settings));
                        if (tom.CPTADMIN_CHAINETRAITEMENT_AVANCE.FirstOrDefault(a => a.NUM == numCaEtapAPP.DEFA) == null)
                            return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état DEF n'est pas paramétré sur TOM²PRO (Avance). " }, settings));
                        if (tom.CPTADMIN_CHAINETRAITEMENT_AVANCE.FirstOrDefault(a => a.NUM == numCaEtapAPP.TEFA) == null)
                            return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état TEF n'est pas paramétré sur TOM²PRO (Avance). " }, settings));
                        if (tom.CPTADMIN_CHAINETRAITEMENT_AVANCE.FirstOrDefault(a => a.NUM == numCaEtapAPP.BEA) == null)
                            return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état BE n'est pas paramétré sur TOM²PRO (Avance). " }, settings));
                    }

                    foreach (var idP in lst)
                    {
                        int crpt = int.Parse(idP);

                        if (db.SI_TRAITPROJET.FirstOrDefault(a => a.IDPROJET == crpt && a.ETAT != 2 && a.DATEMANDAT >= DateDebut && a.DATEMANDAT <= DateFin) != null)
                        {
                            foreach (var x in db.SI_TRAITPROJET.Where(a => a.IDPROJET == crpt && a.ETAT != 2 && a.DATEMANDAT >= DateDebut && a.DATEMANDAT <= DateFin).OrderBy(a => a.DATEMANDAT).OrderBy(a => a.DATECRE).ToList())
                            {
                                var soa = (from soas in db.SI_SOAS
                                           join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                           where prj.IDPROJET == crpt && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                           select new
                                           {
                                               soas.SOA
                                           }).FirstOrDefault();

                                list.Add(new TxLISTETRAIT
                                {
                                    No = x.No,
                                    REF = x.REF,
                                    BENEF = x.TITUL,
                                    DATENGAGEMENT = x.DATEMANDAT != null ? x.DATEMANDAT : null,
                                    MONTENGAGEMENT = Data.Cipher.Decrypt(x.MONT, "Oppenheimer").ToString(),
                                    DATEPAIE = DateTime.Now.Date,
                                    MONTPAIE = Data.Cipher.Decrypt(x.MONT, "Oppenheimer").ToString(),
                                    SOA = soa != null ? soa.SOA : "",
                                    PROJET = db.SI_PROJETS.Where(a => a.ID == crpt && a.DELETIONDATE == null).FirstOrDefault().PROJET,
                                    TYPE = "Engagement",
                                });
                            }
                        }
                        if (db.SI_TRAITAVANCE.FirstOrDefault(a => a.IDPROJET == crpt && a.ETAT != 2 && a.DATEMANDAT >= DateDebut && a.DATEMANDAT <= DateFin) != null)
                        {
                            foreach (var x in db.SI_TRAITAVANCE.Where(a => a.IDPROJET == crpt && a.ETAT != 2 && a.DATEMANDAT >= DateDebut && a.DATEMANDAT <= DateFin).OrderBy(a => a.DATEMANDAT).OrderBy(a => a.DATECRE).ToList())
                            {
                                var soa = (from soas in db.SI_SOAS
                                           join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                           where prj.IDPROJET == crpt && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                           select new
                                           {
                                               soas.SOA
                                           }).FirstOrDefault();

                                list.Add(new TxLISTETRAIT
                                {
                                    No = x.No,
                                    REF = x.REF,
                                    BENEF = x.TITUL,
                                    DATENGAGEMENT = x.DATEMANDAT != null ? x.DATEMANDAT : null,
                                    MONTENGAGEMENT = Data.Cipher.Decrypt(x.MONT, "Oppenheimer").ToString(),
                                    DATEPAIE = DateTime.Now.Date,
                                    MONTPAIE = Data.Cipher.Decrypt(x.MONT, "Oppenheimer").ToString(),
                                    SOA = soa != null ? soa.SOA : "",
                                    PROJET = db.SI_PROJETS.Where(a => a.ID == crpt && a.DELETIONDATE == null).FirstOrDefault().PROJET,
                                    TYPE = "Avance",
                                });
                            }
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

        //Statut des engagements//
        public ActionResult StatutEngagements()
        {
            ViewBag.Controller = "Statut des engagements et avances";

            return View();
        }

        //Genere Statut des engagements et avances//
        [HttpPost]
        public JsonResult GenereSTATLISTE(SI_USERS suser, string listProjet, DateTime DateDebut, DateTime DateFin)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                List<TxLISTETRAIT> list = new List<TxLISTETRAIT>();
                string[] separators = { "," };
                var pro = listProjet;
                if (pro != null)
                {
                    string listUser = pro.ToString();
                    string[] lst = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var idP in lst)
                    {
                        int crpt = int.Parse(idP);

                        //int retarDate = 0;
                        //if (db.SI_DELAISTRAITEMENT.Any(a => a.IDPROJET == crpt && a.DELETIONDATE == null))
                        //    retarDate = db.SI_DELAISTRAITEMENT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null).DELENVOISIIGFP.Value;

                        //Check si le projet est mappé à une base de données TOM²PRO//
                        if (db.SI_MAPPAGES.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null) == null)
                            return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Le projet n'est pas mappé à une base de données TOM²PRO. " }, settings));

                        SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
                        SOFTCONNECTOM tom = new SOFTCONNECTOM();

                        //Check si la correspondance des états est OK//
                        var numCaEtapAPP = db.SI_PARAMETAT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null);
                        if (numCaEtapAPP == null) return Json(JsonConvert.SerializeObject(new { type = "PEtat", msg = "Veuillez paramétrer la correspondance des états. " }, settings));
                        //TEST si les états dans les paramètres dans cohérents avec ceux de TOM²PRO//
                        if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.DEF) == null)
                            return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état DEF n'est pas paramétré sur TOM²PRO (Liquidation). " }, settings));
                        if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.TEF) == null)
                            return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état TEF n'est pas paramétré sur TOM²PRO (Liquidation). " }, settings));
                        if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.BE) == null)
                            return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état BE n'est pas paramétré sur TOM²PRO (Liquidation). " }, settings));
                        if (tom.CPTADMIN_CHAINETRAITEMENT_AVANCE.FirstOrDefault(a => a.NUM == numCaEtapAPP.DEFA) == null)
                            return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état DEF n'est pas paramétré sur TOM²PRO (Avance). " }, settings));
                        if (tom.CPTADMIN_CHAINETRAITEMENT_AVANCE.FirstOrDefault(a => a.NUM == numCaEtapAPP.TEFA) == null)
                            return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état TEF n'est pas paramétré sur TOM²PRO (Avance). " }, settings));
                        if (tom.CPTADMIN_CHAINETRAITEMENT_AVANCE.FirstOrDefault(a => a.NUM == numCaEtapAPP.BEA) == null)
                            return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état BE n'est pas paramétré sur TOM²PRO (Avance). " }, settings));
                    }

                    foreach (var idP in lst)
                    {
                        int crpt = int.Parse(idP);

                        if (db.SI_TRAITPROJET.FirstOrDefault(a => a.IDPROJET == crpt && a.ETAT != 2 && a.DATEMANDAT >= DateDebut && a.DATEMANDAT <= DateFin) != null)
                        {
                            foreach (var x in db.SI_TRAITPROJET.Where(a => a.IDPROJET == crpt && a.ETAT != 2 && a.DATEMANDAT >= DateDebut && a.DATEMANDAT <= DateFin).OrderBy(a => a.DATEMANDAT).OrderBy(a => a.DATECRE).ToList())
                            {
                                var soa = (from soas in db.SI_SOAS
                                           join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                           where prj.IDPROJET == crpt && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                           select new
                                           {
                                               soas.SOA
                                           }).FirstOrDefault();

                                list.Add(new TxLISTETRAIT
                                {
                                    No = x.No,
                                    REF = x.REF,
                                    BENEF = x.TITUL,
                                    //DATENGAGEMENT = x.DATEMANDAT.Value.Date,
                                    MONTENGAGEMENT = Data.Cipher.Decrypt(x.MONT, "Oppenheimer").ToString(),

                                    DATETRANSFERTRAF = x.DATECRE != null ? x.DATECRE : null,
                                    DATEVALORDSEC = x.DATEVALIDATION != null ? x.DATEVALIDATION : null,
                                    DATESENDSIIG = x.DATENVOISIIGFP != null ? x.DATENVOISIIGFP : null,
                                    DATESIIGFP = x.DATESIIG != null ? x.DATESIIG : null,

                                    SOA = soa != null ? soa.SOA : "",
                                    PROJET = db.SI_PROJETS.Where(a => a.ID == crpt && a.DELETIONDATE == null).FirstOrDefault().PROJET,
                                    TYPE = "Engagement"
                                });
                            }
                        }
                        if (db.SI_TRAITAVANCE.FirstOrDefault(a => a.IDPROJET == crpt && a.ETAT != 2 && a.DATEMANDAT >= DateDebut && a.DATEMANDAT <= DateFin) != null)
                        {
                            foreach (var x in db.SI_TRAITAVANCE.Where(a => a.IDPROJET == crpt && a.ETAT != 2 && a.DATEMANDAT >= DateDebut && a.DATEMANDAT <= DateFin).OrderBy(a => a.DATEMANDAT).OrderBy(a => a.DATECRE).ToList())
                            {
                                var soa = (from soas in db.SI_SOAS
                                           join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                           where prj.IDPROJET == crpt && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                           select new
                                           {
                                               soas.SOA
                                           }).FirstOrDefault();

                                list.Add(new TxLISTETRAIT
                                {
                                    No = x.No,
                                    REF = x.REF,
                                    BENEF = x.TITUL,
                                    //DATENGAGEMENT = x.DATEMANDAT.Value.Date,
                                    MONTENGAGEMENT = Data.Cipher.Decrypt(x.MONT, "Oppenheimer").ToString(),

                                    DATETRANSFERTRAF = x.DATECRE != null ? x.DATECRE : null,
                                    DATEVALORDSEC = x.DATEVALIDATION != null ? x.DATEVALIDATION : null,
                                    DATESENDSIIG = x.DATENVOISIIGFP != null ? x.DATENVOISIIGFP : null,
                                    DATESIIGFP = x.DATESIIG != null ? x.DATESIIG : null,

                                    SOA = soa != null ? soa.SOA : "",
                                    PROJET = db.SI_PROJETS.Where(a => a.ID == crpt && a.DELETIONDATE == null).FirstOrDefault().PROJET,
                                    TYPE = "Avance"
                                });
                            }
                        }
                    }
                }

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = list.ToList() }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        //Engagements et avances rejetés//
        public ActionResult EngagementsREJETE()
        {
            ViewBag.Controller = "Liste des engagements et avances rejetés";

            return View();
        }

        //Genere Liste Engagements et avances rejetés//
        [HttpPost]
        public async Task<JsonResult> GenereREJETE(SI_USERS suser, string listProjet, DateTime DateDebut, DateTime DateFin)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                List<TxLISTETRAIT> list = new List<TxLISTETRAIT>();
                string[] separators = { "," };
                var pro = listProjet;
                if (pro != null)
                {
                    string listUser = pro.ToString();
                    string[] lst = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var idP in lst)
                    {
                        int crpt = int.Parse(idP);

                        //Check si le projet est mappé à une base de données TOM²PRO//
                        if (db.SI_MAPPAGES.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null) == null)
                            return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Le projet n'est pas mappé à une base de données TOM²PRO. " }, settings));

                        SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
                        SOFTCONNECTOM tom = new SOFTCONNECTOM();

                        //Check si la correspondance des états est OK//
                        var numCaEtapAPP = db.SI_PARAMETAT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null);
                        if (numCaEtapAPP == null) return Json(JsonConvert.SerializeObject(new { type = "PEtat", msg = "Veuillez paramétrer la correspondance des états. " }, settings));
                        //TEST si les états dans les paramètres dans cohérents avec ceux de TOM²PRO//
                        if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.DEF) == null)
                            return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état DEF n'est pas paramétré sur TOM²PRO (Liquidation). " }, settings));
                        if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.TEF) == null)
                            return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état TEF n'est pas paramétré sur TOM²PRO (Liquidation). " }, settings));
                        if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.BE) == null)
                            return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état BE n'est pas paramétré sur TOM²PRO (Liquidation). " }, settings));
                        if (tom.CPTADMIN_CHAINETRAITEMENT_AVANCE.FirstOrDefault(a => a.NUM == numCaEtapAPP.DEFA) == null)
                            return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état DEF n'est pas paramétré sur TOM²PRO (Avance). " }, settings));
                        if (tom.CPTADMIN_CHAINETRAITEMENT_AVANCE.FirstOrDefault(a => a.NUM == numCaEtapAPP.TEFA) == null)
                            return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état TEF n'est pas paramétré sur TOM²PRO (Avance). " }, settings));
                        if (tom.CPTADMIN_CHAINETRAITEMENT_AVANCE.FirstOrDefault(a => a.NUM == numCaEtapAPP.BEA) == null)
                            return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état BE n'est pas paramétré sur TOM²PRO (Avance). " }, settings));
                    }

                    foreach (var idP in lst)
                    {
                        int crpt = int.Parse(idP);

                        if (db.SI_TRAITPROJET.FirstOrDefault(a => a.IDPROJET == crpt && a.ETAT == 2 && a.DATEMANDAT >= DateDebut && a.DATEMANDAT <= DateFin) != null)
                        {
                            foreach (var x in db.SI_TRAITPROJET.Where(a => a.IDPROJET == crpt && a.ETAT == 2 && a.DATEMANDAT >= DateDebut && a.DATEMANDAT <= DateFin).OrderBy(a => a.DATEMANDAT).OrderBy(a => a.DATECRE).ToList())
                            {
                                var soa = (from soas in db.SI_SOAS
                                           join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                           where prj.IDPROJET == crpt && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                           select new
                                           {
                                               soas.SOA
                                           }).FirstOrDefault();

                                var isRejet = (from user in db.SI_USERS
                                               join rejet in db.SI_TRAITANNUL on user.ID equals rejet.IDUSER
                                               where rejet.IDPROJET == crpt && rejet.No == x.No
                                               orderby rejet.DATEANNUL descending
                                               select new
                                               {
                                                   IDUSER = rejet.IDUSER,
                                                   DATEREJE = rejet.DATEANNUL,
                                                   MOTIF = rejet.MOTIF,
                                                   COMMENTAIRE = rejet.COMMENTAIRE
                                               }).FirstOrDefault();

                                list.Add(new TxLISTETRAIT
                                {
                                    No = x.No,
                                    REF = x.REF,
                                    BENEF = x.TITUL,
                                    MONTENGAGEMENT = Data.Cipher.Decrypt(x.MONT, "Oppenheimer").ToString(),
                                    AGENTREJETE = isRejet != null ? await GetAgent(isRejet.IDUSER) : "",
                                    DATEREJETE = isRejet != null ? isRejet.DATEREJE : null,
                                    MOTIF = isRejet != null ? isRejet.MOTIF : "",
                                    COMMENTAIRE = isRejet != null ? isRejet.COMMENTAIRE : "",

                                    SOA = soa != null ? soa.SOA : "",
                                    PROJET = db.SI_PROJETS.Where(a => a.ID == crpt && a.DELETIONDATE == null).FirstOrDefault().PROJET
                                    //isLATE = isLate
                                });
                            }
                        }
                        if (db.SI_TRAITAVANCE.FirstOrDefault(a => a.IDPROJET == crpt && a.ETAT == 2 && a.DATEMANDAT >= DateDebut && a.DATEMANDAT <= DateFin) != null)
                        {
                            foreach (var x in db.SI_TRAITAVANCE.Where(a => a.IDPROJET == crpt && a.ETAT == 2 && a.DATEMANDAT >= DateDebut && a.DATEMANDAT <= DateFin).OrderBy(a => a.DATEMANDAT).OrderBy(a => a.DATECRE).ToList())
                            {
                                var soa = (from soas in db.SI_SOAS
                                           join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                           where prj.IDPROJET == crpt && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                           select new
                                           {
                                               soas.SOA
                                           }).FirstOrDefault();

                                var isRejet = (from user in db.SI_USERS
                                               join rejet in db.SI_TRAITANNUL on user.ID equals rejet.IDUSER
                                               where rejet.IDPROJET == crpt && rejet.No == x.No
                                               orderby rejet.DATEANNUL descending
                                               select new
                                               {
                                                   IDUSER = rejet.IDUSER,
                                                   DATEREJE = rejet.DATEANNUL,
                                                   MOTIF = rejet.MOTIF,
                                                   COMMENTAIRE = rejet.COMMENTAIRE
                                               }).FirstOrDefault();

                                list.Add(new TxLISTETRAIT
                                {
                                    No = x.No,
                                    REF = x.REF,
                                    BENEF = x.TITUL,
                                    MONTENGAGEMENT = Data.Cipher.Decrypt(x.MONT, "Oppenheimer").ToString(),
                                    AGENTREJETE = isRejet != null ? await GetAgent(isRejet.IDUSER) : "",
                                    DATEREJETE = isRejet != null ? isRejet.DATEREJE : null,
                                    MOTIF = isRejet != null ? isRejet.MOTIF : "",
                                    COMMENTAIRE = isRejet != null ? isRejet.COMMENTAIRE : "",

                                    SOA = soa != null ? soa.SOA : "",
                                    PROJET = db.SI_PROJETS.Where(a => a.ID == crpt && a.DELETIONDATE == null).FirstOrDefault().PROJET
                                    //isLATE = isLate
                                });
                            }
                        }
                    }
                }

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = list.ToList() }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        //Suivi des délais de traitement des engagements et avances//
        public ActionResult DelaisTraitementEngagements()
        {
            ViewBag.Controller = "Suvi des délais de traitement des engagements";

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GenereDelaisTraitementEngagements(SI_USERS suser, string listProjet, DateTime DateDebut, DateTime DateFin)
        {
            var user = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null);

            if (user == null)
            {
                return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));
            }

            string[] separators = { "," };

            var sProjectsId = listProjet.Split(separators, StringSplitOptions.RemoveEmptyEntries).ToList();

            var iProjectsId = new List<int>();

            for (int i = 0; i < sProjectsId.Count; i += 1)
            {
                int projectId = int.Parse(sProjectsId[i]);

                if ((await db.SI_MAPPAGES.FirstOrDefaultAsync(a => a.IDPROJET == projectId && a.DELETIONDATE == null)) == null)
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Le projet n'est pas mappé à une base de données TOM²PRO. " }, settings));
                }

                SOFTCONNECTOM.connex = new Data.Extension().GetCon(projectId);

                var tom = new SOFTCONNECTOM();

                var numCaEtapAPP = await db.SI_PARAMETAT.FirstOrDefaultAsync(a => a.IDPROJET == projectId && a.DELETIONDATE == null);

                if (numCaEtapAPP == null)
                {
                    return Json(JsonConvert.SerializeObject(new { type = "PEtat", msg = "Veuillez paramétrer la correspondance des états. " }, settings));
                }

                //TEST si les états dans les paramètres dans cohérents avec ceux de TOM²PRO//
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.DEF) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état DEF n'est pas paramétré sur TOM²PRO (Liquidation). " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.TEF) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état TEF n'est pas paramétré sur TOM²PRO (Liquidation). " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.BE) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état BE n'est pas paramétré sur TOM²PRO (Liquidation). " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT_AVANCE.FirstOrDefault(a => a.NUM == numCaEtapAPP.DEFA) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état DEF n'est pas paramétré sur TOM²PRO (Avance). " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT_AVANCE.FirstOrDefault(a => a.NUM == numCaEtapAPP.TEFA) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état TEF n'est pas paramétré sur TOM²PRO (Avance). " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT_AVANCE.FirstOrDefault(a => a.NUM == numCaEtapAPP.BEA) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état BE n'est pas paramétré sur TOM²PRO (Avance). " }, settings));

                iProjectsId.Add(projectId);
            }

            var result = new List<TraitementEngagement>();

            var lastIndex = -1;
            var lastIndexAVANCE = -1;

            for (int i = 0; i < iProjectsId.Count; i += 1)
            {
                int projectId = iProjectsId[i];

                var s = await (
                    from soa in db.SI_SOAS
                    join prosoa in db.SI_PROSOA on soa.ID equals prosoa.IDSOA
                    where prosoa.IDPROJET == projectId && prosoa.DELETIONDATE == null && soa.DELETIONDATE == null
                    select new
                    {
                        soa.SOA
                    }
                ).FirstOrDefaultAsync();

                if (s == null)
                {
                    continue;
                }

                //ENGAGEMENTS//
                var traitprojets = await db.SI_TRAITPROJET.Where(a => a.IDPROJET == projectId && a.ETAT != 2 && a.DATEMANDAT >= DateDebut && a.DATEMANDAT <= DateFin).OrderBy(a => a.DATEMANDAT).OrderBy(a => a.DATECRE).ToListAsync();

                if (traitprojets.Count == 0)
                {
                    continue;
                }

                lastIndex += 1;

                result.Add(new TraitementEngagement
                {
                    SOA = s != null ? s.SOA : "",
                    TraitementsEngagementsDetails = new List<TraitementEngagementDetails>()
                });

                for (int j = 0; j < traitprojets.Count; j += 1)
                {
                    result[lastIndex].TraitementsEngagementsDetails.Add(new TraitementEngagementDetails
                    {
                        PROJET = db.SI_PROJETS.FirstOrDefault(a => a.ID == projectId && a.DELETIONDATE == null).PROJET,
                        TYPE = "Engagement",
                        NUM_ENGAGEMENT = traitprojets[j].REF,
                        BENEFICIAIRE = traitprojets[j].TITUL,
                        MONTENGAGEMENT = Data.Cipher.Decrypt(traitprojets[j].MONT, "Oppenheimer").ToString(),
                        DATETRANSFERTRAF = traitprojets[j].DATECRE,
                        TRANSFERTRAFAGENT = await GetAgent(traitprojets[j].IDUSERCREATE),
                        DATEVALORDSEC = traitprojets[j].DATEVALIDATION,
                        VALORDSECAGENT = await GetAgent(traitprojets[j].IDUSERVALIDATE),
                        DATESENDSIIG = traitprojets[j].DATENVOISIIGFP,
                        SENDSIIGAGENT = await GetAgent(traitprojets[j].IDUSERENVOISIIGFP),
                        DATESIIGFP = traitprojets[j].DATESIIG,
                        SIIGFPAGENT = "",

                        DUREETRAITEMENTTRANSFERTRAF = Data.Date.GetDifference(traitprojets[j].DATECRE, traitprojets[j].DATEBE),
                        DUREETRAITEMENTVALORDSEC = Data.Date.GetDifference(traitprojets[j].DATEVALIDATION, traitprojets[j].DATECRE),
                        DUREETRAITEMENTSENDSIIG = Data.Date.GetDifference(traitprojets[j].DATENVOISIIGFP, traitprojets[j].DATEVALIDATION),
                        DUREETRAITEMENTSIIGFP = Data.Date.GetDifference(traitprojets[j].DATESIIG, traitprojets[j].DATENVOISIIGFP)
                    });
                }

                //AVANCES//
                var traitprojetsAVANCE = await db.SI_TRAITAVANCE.Where(a => a.IDPROJET == projectId && a.ETAT != 2 && a.DATEMANDAT >= DateDebut && a.DATEMANDAT <= DateFin).OrderBy(a => a.DATEMANDAT).OrderBy(a => a.DATECRE).ToListAsync();

                if (traitprojetsAVANCE.Count == 0)
                {
                    continue;
                }

                lastIndexAVANCE += 1;

                result.Add(new TraitementEngagement
                {
                    SOA = s != null ? s.SOA : "",
                    TraitementsEngagementsDetails = new List<TraitementEngagementDetails>()
                });

                for (int j = 0; j < traitprojetsAVANCE.Count; j += 1)
                {
                    result[lastIndexAVANCE].TraitementsEngagementsDetails.Add(new TraitementEngagementDetails
                    {
                        PROJET = db.SI_PROJETS.FirstOrDefault(a => a.ID == projectId && a.DELETIONDATE == null).PROJET,
                        TYPE = "Avance",
                        NUM_ENGAGEMENT = traitprojetsAVANCE[j].REF,
                        BENEFICIAIRE = traitprojetsAVANCE[j].TITUL,
                        MONTENGAGEMENT = Data.Cipher.Decrypt(traitprojetsAVANCE[j].MONT, "Oppenheimer").ToString(),
                        DATETRANSFERTRAF = traitprojetsAVANCE[j].DATECRE,
                        TRANSFERTRAFAGENT = await GetAgent(traitprojetsAVANCE[j].IDUSERCREATE),
                        DATEVALORDSEC = traitprojetsAVANCE[j].DATEVALIDATION,
                        VALORDSECAGENT = await GetAgent(traitprojetsAVANCE[j].IDUSERVALIDATE),
                        DATESENDSIIG = traitprojetsAVANCE[j].DATENVOISIIGFP,
                        SENDSIIGAGENT = await GetAgent(traitprojetsAVANCE[j].IDUSERENVOISIIGFP),
                        DATESIIGFP = traitprojetsAVANCE[j].DATESIIG,
                        SIIGFPAGENT = "",

                        DUREETRAITEMENTTRANSFERTRAF = Data.Date.GetDifference(traitprojetsAVANCE[j].DATECRE, traitprojetsAVANCE[j].DATEBE),
                        DUREETRAITEMENTVALORDSEC = Data.Date.GetDifference(traitprojetsAVANCE[j].DATEVALIDATION, traitprojetsAVANCE[j].DATECRE),
                        DUREETRAITEMENTSENDSIIG = Data.Date.GetDifference(traitprojetsAVANCE[j].DATENVOISIIGFP, traitprojetsAVANCE[j].DATEVALIDATION),
                        DUREETRAITEMENTSIIGFP = Data.Date.GetDifference(traitprojetsAVANCE[j].DATESIIG, traitprojetsAVANCE[j].DATENVOISIIGFP)
                    });
                }
            }

            return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = result }, settings));
        }

        //Liste des traitements en souffrance (par rapport au délai moyen)//
        public ActionResult SoufTraitement()
        {
            ViewBag.Controller = "Liste des traitements en souffrance (par rapport au délai moyen)";

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GenereSoufTraitement(SI_USERS suser, string listProjet, DateTime DateDebut, DateTime DateFin)
        {
            var user = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null);

            if (user == null)
            {
                return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));
            }

            string[] separators = { "," };

            var sProjectsId = listProjet.Split(separators, StringSplitOptions.RemoveEmptyEntries).ToList();

            var iProjectsId = new List<int>();

            for (int i = 0; i < sProjectsId.Count; i += 1)
            {
                int projectId = int.Parse(sProjectsId[i]);

                if ((await db.SI_MAPPAGES.FirstOrDefaultAsync(a => a.IDPROJET == projectId && a.DELETIONDATE == null)) == null)
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Le projet n'est pas mappé à une base de données TOM²PRO. " }, settings));
                }

                SOFTCONNECTOM.connex = new Data.Extension().GetCon(projectId);

                var tom = new SOFTCONNECTOM();

                var numCaEtapAPP = await db.SI_PARAMETAT.FirstOrDefaultAsync(a => a.IDPROJET == projectId && a.DELETIONDATE == null);

                if (numCaEtapAPP == null)
                {
                    return Json(JsonConvert.SerializeObject(new { type = "PEtat", msg = "Veuillez paramétrer la correspondance des états. " }, settings));
                }

                //TEST si les états dans les paramètres dans cohérents avec ceux de TOM²PRO//
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.DEF) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état DEF n'est pas paramétré sur TOM²PRO (Liquidation). " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.TEF) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état TEF n'est pas paramétré sur TOM²PRO (Liquidation). " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.BE) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état BE n'est pas paramétré sur TOM²PRO (Liquidation). " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT_AVANCE.FirstOrDefault(a => a.NUM == numCaEtapAPP.DEFA) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état DEF n'est pas paramétré sur TOM²PRO (Avance). " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT_AVANCE.FirstOrDefault(a => a.NUM == numCaEtapAPP.TEFA) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état TEF n'est pas paramétré sur TOM²PRO (Avance). " }, settings));
                if (tom.CPTADMIN_CHAINETRAITEMENT_AVANCE.FirstOrDefault(a => a.NUM == numCaEtapAPP.BEA) == null)
                    return Json(JsonConvert.SerializeObject(new { type = "Prese", msg = "L'état BE n'est pas paramétré sur TOM²PRO (Avance). " }, settings));

                iProjectsId.Add(projectId);
            }

            var result = new List<TraitementEngagement>();

            var lastIndex = -1;
            var lastIndexAVANCE = -1;

            try
            {
                for (int i = 0; i < iProjectsId.Count; i += 1)
                {
                    int projectId = iProjectsId[i];

                    var durPrevu = db.SI_DELAISTRAITEMENT.FirstOrDefault(a => a.IDPROJET == projectId && a.DELETIONDATE == null);
                    if (durPrevu == null || durPrevu.DELRAF == null || durPrevu.DELTV == null || durPrevu.DELENVOISIIGFP == null || durPrevu.DELSIIGFP == null)
                        return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Veuillez paramétrer le délais des traitements. " }, settings));

                    var durPrevuAVANCE = db.SI_DELAISTRAITEMENT.FirstOrDefault(a => a.IDPROJET == projectId && a.DELETIONDATE == null);
                    if (durPrevuAVANCE == null || durPrevuAVANCE.DELARAF == null || durPrevuAVANCE.DELAV == null || durPrevuAVANCE.DELAENVOISIIGFP == null || durPrevuAVANCE.DELASIIGFP == null)
                        return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Veuillez paramétrer le délais des traitements. " }, settings));

                    var s = await (
                        from soa in db.SI_SOAS
                        join prosoa in db.SI_PROSOA on soa.ID equals prosoa.IDSOA
                        where prosoa.IDPROJET == projectId && prosoa.DELETIONDATE == null && soa.DELETIONDATE == null
                        select new
                        {
                            soa.SOA
                        }
                    ).FirstOrDefaultAsync();

                    if (s == null)
                    {
                        continue;
                    }

                    //ENGAGEMENTS//
                    var traitprojets = await db.SI_TRAITPROJET.Where(a => a.IDPROJET == projectId && a.ETAT != 2 && a.DATEMANDAT >= DateDebut && a.DATEMANDAT <= DateFin).OrderBy(a => a.DATEMANDAT).OrderBy(a => a.DATECRE).ToListAsync();

                    if (traitprojets.Count == 0)
                    {
                        continue;
                    }

                    lastIndex += 1;

                    result.Add(new TraitementEngagement
                    {
                        SOA = s != null ? s.SOA : "",
                        TraitementsEngagementsDetails = new List<TraitementEngagementDetails>()
                    });

                    for (int j = 0; j < traitprojets.Count; j += 1)
                    {
                        result[lastIndex].TraitementsEngagementsDetails.Add(new TraitementEngagementDetails
                        {
                            PROJET = db.SI_PROJETS.FirstOrDefault(a => a.ID == projectId && a.DELETIONDATE == null).PROJET,
                            TYPE = "Engagement",
                            NUM_ENGAGEMENT = traitprojets[j].REF,
                            BENEFICIAIRE = traitprojets[j].TITUL,
                            MONTENGAGEMENT = Data.Cipher.Decrypt(traitprojets[j].MONT, "Oppenheimer").ToString(),

                            DATETRANSFERTRAF = traitprojets[j].DATECRE,
                            DATEVALORDSEC = traitprojets[j].DATEVALIDATION,
                            DATESENDSIIG = traitprojets[j].DATENVOISIIGFP,
                            DATESIIGFP = traitprojets[j].DATESIIG,

                            TRANSFERTRAFAGENT = await GetAgent(traitprojets[j].IDUSERCREATE),
                            VALORDSECAGENT = await GetAgent(traitprojets[j].IDUSERVALIDATE),
                            SENDSIIGAGENT = await GetAgent(traitprojets[j].IDUSERENVOISIIGFP),
                            SIIGFPAGENT = "",

                            DUREETRAITEMENTTRANSFERTRAF = Data.Date.GetDifference(traitprojets[j].DATECRE, traitprojets[j].DATEBE),
                            DUREETRAITEMENTVALORDSEC = Data.Date.GetDifference(traitprojets[j].DATEVALIDATION, traitprojets[j].DATECRE),
                            DUREETRAITEMENTSENDSIIG = Data.Date.GetDifference(traitprojets[j].DATENVOISIIGFP, traitprojets[j].DATEVALIDATION),
                            DUREETRAITEMENTSIIGFP = Data.Date.GetDifference(traitprojets[j].DATESIIG, traitprojets[j].DATENVOISIIGFP),

                            DURPREVUTRANSFERT = durPrevu != null ? durPrevu.DELRAF.Value : 0,
                            DURPREVUVALIDATION = durPrevu != null ? durPrevu.DELTV.Value : 0,
                            DURPREVUTRANSFSIIG = durPrevu != null ? durPrevu.DELENVOISIIGFP.Value : 0,
                            DURPREVUSIIG = durPrevu != null ? durPrevu.DELSIIGFP.Value : 0,

                            DEPASTRANSFERT = durPrevu != null ? Data.Date.GetDifference(traitprojets[j].DATECRE, traitprojets[j].DATEBE) - durPrevu.DELRAF.Value : 0,
                            DEPASVALIDATION = durPrevu != null ? Data.Date.GetDifference(traitprojets[j].DATEVALIDATION, traitprojets[j].DATECRE) - durPrevu.DELTV.Value : 0,
                            DEPASTRANSFSIIG = durPrevu != null ? Data.Date.GetDifference(traitprojets[j].DATENVOISIIGFP, traitprojets[j].DATEVALIDATION) - durPrevu.DELENVOISIIGFP.Value : 0,
                            DEPASSIIG = durPrevu != null ? Data.Date.GetDifference(traitprojets[j].DATESIIG, traitprojets[j].DATENVOISIIGFP) - durPrevu.DELSIIGFP.Value : 0
                        });
                    }

                    //AVANCES//
                    var traitprojetsAVANCE = await db.SI_TRAITAVANCE.Where(a => a.IDPROJET == projectId && a.ETAT != 2 && a.DATEMANDAT >= DateDebut && a.DATEMANDAT <= DateFin).OrderBy(a => a.DATEMANDAT).OrderBy(a => a.DATECRE).ToListAsync();

                    if (traitprojetsAVANCE.Count == 0)
                    {
                        continue;
                    }

                    lastIndexAVANCE += 1;

                    result.Add(new TraitementEngagement
                    {
                        SOA = s != null ? s.SOA : "",
                        TraitementsEngagementsDetails = new List<TraitementEngagementDetails>()
                    });

                    for (int j = 0; j < traitprojetsAVANCE.Count; j += 1)
                    {
                        result[lastIndexAVANCE].TraitementsEngagementsDetails.Add(new TraitementEngagementDetails
                        {
                            PROJET = db.SI_PROJETS.FirstOrDefault(a => a.ID == projectId && a.DELETIONDATE == null).PROJET,
                            TYPE = "Avance",
                            NUM_ENGAGEMENT = traitprojetsAVANCE[j].REF,
                            BENEFICIAIRE = traitprojetsAVANCE[j].TITUL,
                            MONTENGAGEMENT = Data.Cipher.Decrypt(traitprojetsAVANCE[j].MONT, "Oppenheimer").ToString(),

                            DATETRANSFERTRAF = traitprojetsAVANCE[j].DATECRE,
                            DATEVALORDSEC = traitprojetsAVANCE[j].DATEVALIDATION,
                            DATESENDSIIG = traitprojetsAVANCE[j].DATENVOISIIGFP,
                            DATESIIGFP = traitprojetsAVANCE[j].DATESIIG,

                            TRANSFERTRAFAGENT = await GetAgent(traitprojetsAVANCE[j].IDUSERCREATE),
                            VALORDSECAGENT = await GetAgent(traitprojetsAVANCE[j].IDUSERVALIDATE),
                            SENDSIIGAGENT = await GetAgent(traitprojetsAVANCE[j].IDUSERENVOISIIGFP),
                            SIIGFPAGENT = "",

                            DUREETRAITEMENTTRANSFERTRAF = Data.Date.GetDifference(traitprojetsAVANCE[j].DATECRE, traitprojetsAVANCE[j].DATEBE),
                            DUREETRAITEMENTVALORDSEC = Data.Date.GetDifference(traitprojetsAVANCE[j].DATEVALIDATION, traitprojetsAVANCE[j].DATECRE),
                            DUREETRAITEMENTSENDSIIG = Data.Date.GetDifference(traitprojetsAVANCE[j].DATENVOISIIGFP, traitprojetsAVANCE[j].DATEVALIDATION),
                            DUREETRAITEMENTSIIGFP = Data.Date.GetDifference(traitprojetsAVANCE[j].DATESIIG, traitprojetsAVANCE[j].DATENVOISIIGFP),

                            DURPREVUTRANSFERT = durPrevuAVANCE != null ? durPrevuAVANCE.DELRAF.Value : 0,
                            DURPREVUVALIDATION = durPrevuAVANCE != null ? durPrevuAVANCE.DELTV.Value : 0,
                            DURPREVUTRANSFSIIG = durPrevuAVANCE != null ? durPrevuAVANCE.DELENVOISIIGFP.Value : 0,
                            DURPREVUSIIG = durPrevuAVANCE != null ? durPrevuAVANCE.DELSIIGFP.Value : 0,

                            DEPASTRANSFERT = durPrevuAVANCE != null ? Data.Date.GetDifference(traitprojets[j].DATECRE, traitprojets[j].DATEBE) - durPrevuAVANCE.DELRAF.Value : 0,
                            DEPASVALIDATION = durPrevuAVANCE != null ? Data.Date.GetDifference(traitprojets[j].DATEVALIDATION, traitprojets[j].DATECRE) - durPrevuAVANCE.DELTV.Value : 0,
                            DEPASTRANSFSIIG = durPrevuAVANCE != null ? Data.Date.GetDifference(traitprojets[j].DATENVOISIIGFP, traitprojets[j].DATEVALIDATION) - durPrevuAVANCE.DELENVOISIIGFP.Value : 0,
                            DEPASSIIG = durPrevuAVANCE != null ? Data.Date.GetDifference(traitprojets[j].DATESIIG, traitprojets[j].DATENVOISIIGFP) - durPrevuAVANCE.DELSIIGFP.Value : 0
                        });
                    }
                }

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = result }, settings));
            }
            catch (Exception) { return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Problème de connexion. " }, settings)); }
        }

        public ActionResult StatutPaiement()
        {
            ViewBag.Controller = "Status des paiements";
            return View();
        }

        [HttpPost]
        public JsonResult GenereSTATPAIEMENT(SI_USERS suser, string listProjet, DateTime DateDebut, DateTime DateFin)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                List<TxtPAIEMENT> list = new List<TxtPAIEMENT>();
                string[] separators = { "," };
                var pro = listProjet;
                if (pro != null)
                {
                    string listUser = pro.ToString();
                    string[] lst = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var idP in lst)
                    {
                        int crpt = int.Parse(idP);
                        //Check si le projet est mappé à une base de données TOM²PRO//
                        if (db.SI_MAPPAGES.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null) == null)
                            return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Le projet n'est pas mappé à une base de données TOM²PRO. " }, settings));

                    }
                    foreach (var idP in lst)
                    {
                        int crpt = int.Parse(idP);

                        //var paielst = db.OPA_VALIDATIONS.Where(a => a.IDPROJET == crpt).Join().ToList();
                        var typeEcriture = db.SI_TYPECRITURE.Where(x => x.IDPROJET == crpt).FirstOrDefault().TYPE;
                        if (typeEcriture == 1)
                        {
                            var paielst = (
                               from r in db.OPA_REGLEMENTBR
                               join v in db.OPA_VALIDATIONS on r.IDSOCIETE equals v.IDPROJET
                               where r.NUM == v.IDREGLEMENT && r.IDSOCIETE == crpt
                               select new
                               {
                                   BENEFICIAIRE = r.BENEFICIAIRE,
                                   MONTANT = r.MONTANT != null ? r.MONTANT : null,
                                   NUM = r.NUM,
                                   DATECREA = v.DATECREA != null ? v.DATECREA : null,
                                   DATESEND = v.DATESEND != null ? v.DATESEND : null,
                                   DATEVAL = v.DATEVAL != null ? v.DATEVAL : null,
                               }
                           ).ToList();

                            foreach (var item in paielst)
                            {
                                var soa = (from soas in db.SI_SOAS
                                           join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                           where prj.IDPROJET == crpt && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                           select new
                                           {
                                               soas.SOA
                                           }).FirstOrDefault();


                                list.Add(new TxtPAIEMENT
                                {
                                    No = item.NUM,
                                    BENEF = item.BENEFICIAIRE,
                                    MONTANT = item.MONTANT.ToString(),
                                    DATEVALIDATIONOP = item.DATECREA,
                                    DATEVALIDATIONAC = item.DATESEND,
                                    DATEPAIEBANQUE = item.DATEVAL,
                                    SOA = soa.SOA != null ? soa.SOA : "",
                                    PROJET = db.SI_PROJETS.Where(a => a.ID == crpt && a.DELETIONDATE == null).FirstOrDefault().PROJET
                                });
                            }
                        }
                        else
                        {
                            var paielst = (
                               from r in db.OPA_REGLEMENT
                               join v in db.OPA_VALIDATIONS on r.IDSOCIETE equals v.IDPROJET
                               where r.NUM.ToString() == v.IDREGLEMENT && r.IDSOCIETE == crpt
                               select new
                               {
                                   BENEFICIAIRE = r.BENEFICIAIRE,
                                   MONTANT = r.MONTANT != null ? r.MONTANT : null,
                                   NUM = r.NUM,
                                   DATECREA = v.DATECREA != null ? v.DATECREA : null,
                                   DATESEND = v.DATESEND != null ? v.DATESEND : null,
                                   DATEVAL = v.DATEVAL != null ? v.DATEVAL : null,
                               }
                           ).ToList();

                            foreach (var item in paielst)
                            {
                                var soa = (from soas in db.SI_SOAS
                                           join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                           where prj.IDPROJET == crpt && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                           select new
                                           {
                                               soas.SOA
                                           }).FirstOrDefault();


                                list.Add(new TxtPAIEMENT
                                {
                                    No = item.NUM.ToString(),
                                    BENEF = item.BENEFICIAIRE,
                                    MONTANT = item.MONTANT.ToString(),
                                    DATEVALIDATIONOP = item.DATECREA,
                                    DATEVALIDATIONAC = item.DATESEND,
                                    DATEPAIEBANQUE = item.DATEVAL,
                                    SOA = soa.SOA != null ? soa.SOA : "",
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
        public ActionResult TraitementsPaiement()
        {
            ViewBag.Controller = "Traitements des paiements";
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GenereDelaisTraitementPaiement(SI_USERS suser, string listProjet, DateTime DateDebut, DateTime DateFin)
        {
            var user = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null);

            if (user == null)
            {
                return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));
            }

            string[] separators = { "," };

            var sProjectsId = listProjet.Split(separators, StringSplitOptions.RemoveEmptyEntries).ToList();

            var iProjectsId = new List<int>();

            for (int i = 0; i < sProjectsId.Count; i += 1)
            {
                int projectId = int.Parse(sProjectsId[i]);

                if ((await db.SI_MAPPAGES.FirstOrDefaultAsync(a => a.IDPROJET == projectId && a.DELETIONDATE == null)) == null)
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Le projet n'est pas mappé à une base de données TOM²PRO. " }, settings));
                }

                iProjectsId.Add(projectId);
            }

            var result = new List<TraitementPaiement>();

            var lastIndex = -1;

            for (int i = 0; i < iProjectsId.Count; i += 1)
            {
                int projectId = iProjectsId[i];

                var typeEcriture = db.SI_TYPECRITURE.Where(x => x.IDPROJET == projectId).FirstOrDefault().TYPE;

                if (typeEcriture == 1)
                {
                    var s = await (
                            from soa in db.SI_SOAS
                            join prosoa in db.SI_PROSOA on soa.ID equals prosoa.IDSOA
                            where prosoa.IDPROJET == projectId && prosoa.DELETIONDATE == null && soa.DELETIONDATE == null
                            select new
                            {
                                soa.SOA
                            }
                            ).FirstOrDefaultAsync();

                    var paielst = (
                                   from r in db.OPA_REGLEMENTBR
                                   join v in db.OPA_VALIDATIONS on r.IDSOCIETE equals v.IDPROJET
                                   where r.NUM == v.IDREGLEMENT && r.IDSOCIETE == projectId
                                   select new
                                   {
                                       BENEFICIAIRE = r.BENEFICIAIRE,
                                       MONTANT = r.MONTANT != null ? r.MONTANT : null,
                                       NUM = r.NUM,
                                       DATECREA = v.DATECREA != null ? v.DATECREA : null,
                                       DATESEND = v.DATESEND != null ? v.DATESEND : null,
                                       DATEVAL = v.DATEVAL != null ? v.DATEVAL : null,
                                       IDUSCREA = v.IDUSCREA != null ? v.IDUSCREA : null,
                                       IDUSSEND = v.IDUSSEND != null ? v.IDUSSEND : null,
                                       IDUSVAL = v.IDUSVAL != null ? v.IDUSVAL : null,
                                   }
                               ).ToList();

                    lastIndex += 1;

                    result.Add(new TraitementPaiement
                    {
                        SOA = s != null ? s.SOA : "",
                        TraitementPaiementDetails = new List<TraitementPaiementDetails>()
                    });

                    for (int j = 0; j < paielst.Count; j += 1)
                    {
                        result[lastIndex].TraitementPaiementDetails.Add(new TraitementPaiementDetails
                        {
                            NUM_ENGAGEMENT = paielst[j].NUM,
                            BENEFICIAIRE = paielst[j].BENEFICIAIRE,
                            MONTENGAGEMENT = paielst[j].MONTANT.ToString(),
                            DATETRANSFERTRAF = paielst[j].DATECREA,
                            TRANSFERTRAFAGENT = await GetAgent(paielst[j].IDUSCREA),
                            DATEVALORDSEC = paielst[j].DATEVAL,
                            VALORDSECAGENT = await GetAgent(paielst[j].IDUSVAL),
                            DATESENDSIIG = paielst[j].DATESEND,
                            SENDSIIGAGENT = await GetAgent(paielst[j].IDUSSEND),
                            DUREETRAITEMENTTRANSFERTRAF = Date.GetDifference(paielst[j].DATECREA, paielst[j].DATESEND),
                            DUREETRAITEMENTVALORDSEC = Date.GetDifference(paielst[j].DATESEND, paielst[j].DATEVAL)
                        });
                    }
                }
                else
                {
                    var s = await (
                            from soa in db.SI_SOAS
                            join prosoa in db.SI_PROSOA on soa.ID equals prosoa.IDSOA
                            where prosoa.IDPROJET == projectId && prosoa.DELETIONDATE == null && soa.DELETIONDATE == null
                            select new
                            {
                                soa.SOA
                            }
                        ).FirstOrDefaultAsync();

                    var paielst = (
                                   from r in db.OPA_REGLEMENT
                                   join v in db.OPA_VALIDATIONS on r.IDSOCIETE equals v.IDPROJET
                                   where r.NUM.ToString() == v.IDREGLEMENT && r.IDSOCIETE == projectId
                                   select new
                                   {
                                       BENEFICIAIRE = r.BENEFICIAIRE,
                                       MONTANT = r.MONTANT != null ? r.MONTANT : null,
                                       NUM = r.NUM,
                                       DATECREA = v.DATECREA != null ? v.DATECREA : null,
                                       DATESEND = v.DATESEND != null ? v.DATESEND : null,
                                       DATEVAL = v.DATEVAL != null ? v.DATEVAL : null,
                                       IDUSCREA = v.IDUSCREA != null ? v.IDUSCREA : null,
                                       IDUSSEND = v.IDUSSEND != null ? v.IDUSSEND : null,
                                       IDUSVAL = v.IDUSVAL != null ? v.IDUSVAL : null,
                                   }
                               ).ToList();

                    lastIndex += 1;

                    result.Add(new TraitementPaiement
                    {
                        SOA = s != null ? s.SOA : "",
                        TraitementPaiementDetails = new List<TraitementPaiementDetails>()
                    });

                    for (int j = 0; j < paielst.Count; j += 1)
                    {
                        result[lastIndex].TraitementPaiementDetails.Add(new TraitementPaiementDetails
                        {
                            NUM_ENGAGEMENT = paielst[j].NUM.ToString(),
                            BENEFICIAIRE = paielst[j].BENEFICIAIRE,
                            MONTENGAGEMENT = paielst[j].MONTANT.ToString(),
                            DATETRANSFERTRAF = paielst[j].DATECREA,
                            TRANSFERTRAFAGENT = await GetAgent(paielst[j].IDUSCREA),
                            DATEVALORDSEC = paielst[j].DATEVAL,
                            VALORDSECAGENT = await GetAgent(paielst[j].IDUSVAL),
                            DATESENDSIIG = paielst[j].DATESEND,
                            SENDSIIGAGENT = await GetAgent(paielst[j].IDUSSEND),
                            DUREETRAITEMENTTRANSFERTRAF = Date.GetDifference(paielst[j].DATECREA, paielst[j].DATESEND),
                            DUREETRAITEMENTVALORDSEC = Date.GetDifference(paielst[j].DATESEND, paielst[j].DATEVAL)
                        });
                    }
                }

            }

            return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = result }, settings));
        }
        public ActionResult TraitementPaiementSoufrance()
        {
            ViewBag.Controller = "Traitements des paiements en souffrance";
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GenereTraitementPaiementSouffrance(SI_USERS suser, string listProjet, DateTime DateDebut, DateTime DateFin)
        {
            var user = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null);

            if (user == null)
            {
                return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));
            }

            string[] separators = { "," };

            var sProjectsId = listProjet.Split(separators, StringSplitOptions.RemoveEmptyEntries).ToList();

            var iProjectsId = new List<int>();

            for (int i = 0; i < sProjectsId.Count; i += 1)
            {
                int projectId = int.Parse(sProjectsId[i]);

                if ((await db.SI_MAPPAGES.FirstOrDefaultAsync(a => a.IDPROJET == projectId && a.DELETIONDATE == null)) == null)
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Le projet n'est pas mappé à une base de données TOM²PRO. " }, settings));
                }

                iProjectsId.Add(projectId);
            }

            var result = new List<TraitementPaiement>();

            var lastIndex = -1;

            for (int i = 0; i < iProjectsId.Count; i += 1)
            {
                int projectId = iProjectsId[i];

                var typeEcriture = db.SI_TYPECRITURE.Where(x => x.IDPROJET == projectId).FirstOrDefault().TYPE;
                if (typeEcriture == 1)
                {
                    var s = await (
                    from soa in db.SI_SOAS
                    join prosoa in db.SI_PROSOA on soa.ID equals prosoa.IDSOA
                    where prosoa.IDPROJET == projectId && prosoa.DELETIONDATE == null && soa.DELETIONDATE == null
                    select new
                    {
                        soa.SOA
                    }
                ).FirstOrDefaultAsync();


                    var paielst = (
                                   from r in db.OPA_REGLEMENTBR
                                   join v in db.OPA_VALIDATIONS on r.IDSOCIETE equals v.IDPROJET
                                   where r.NUM == v.IDREGLEMENT && r.IDSOCIETE == projectId
                                   select new
                                   {
                                       BENEFICIAIRE = r.BENEFICIAIRE,
                                       MONTANT = r.MONTANT != null ? r.MONTANT : null,
                                       NUM = r.NUM,
                                       DATECREA = v.DATECREA != null ? v.DATECREA : null,
                                       DATESEND = v.DATESEND != null ? v.DATESEND : null,
                                       DATEVAL = v.DATEVAL != null ? v.DATEVAL : null,
                                       IDUSCREA = v.IDUSCREA != null ? v.IDUSCREA : null,
                                       IDUSSEND = v.IDUSSEND != null ? v.IDUSSEND : null,
                                       IDUSVAL = v.IDUSVAL != null ? v.IDUSVAL : null,
                                   }
                               ).ToList();

                    var durerTraite = db.SI_DELAISTRAITEMENT.Where(x => x.IDPROJET == projectId).Select(x => new {
                        DELAISOP = x.DELPP,
                        DELAISAC = x.DELPV,
                        DELAISBK = x.DELPB,
                    }).ToList();

                    lastIndex += 1;

                    result.Add(new TraitementPaiement
                    {
                        SOA = s != null ? s.SOA : "",
                        TraitementPaiementDetails = new List<TraitementPaiementDetails>()
                    });

                    for (int j = 0; j < paielst.Count; j += 1)
                    {
                        double dateOP = Date.GetDifference(paielst[j].DATESEND, paielst[j].DATECREA);
                        double dateAC = Date.GetDifference(paielst[j].DATEVAL, paielst[j].DATESEND);
                        double dateBK = Date.GetDifference(paielst[j].DATEVAL, paielst[j].DATESEND);
                        result[lastIndex].TraitementPaiementDetails.Add(new TraitementPaiementDetails
                        {
                            NUM_ENGAGEMENT = paielst[j].NUM,
                            BENEFICIAIRE = paielst[j].BENEFICIAIRE,
                            MONTENGAGEMENT = paielst[j].MONTANT.ToString(),
                            DATETRANSFERTRAF = paielst[j].DATECREA,
                            TRANSFERTRAFAGENT = await GetAgent(paielst[j].IDUSCREA),
                            DATEVALORDSEC = paielst[j].DATEVAL,
                            VALORDSECAGENT = await GetAgent(paielst[j].IDUSVAL),
                            DATESENDSIIG = paielst[j].DATESEND,
                            SENDSIIGAGENT = await GetAgent(paielst[j].IDUSSEND),
                            DUREETRAITEMENTTRANSFERTOP = dateOP,
                            DUREETRAITEMENTTRANSFERTAC = dateAC,
                            DUREETRAITEMENTTRANSFERTBK = dateBK,

                            DUREETRAITEMENTPREVUEOP = Convert.ToDouble(durerTraite.FirstOrDefault().DELAISOP),
                            DUREETRAITEMENTPREVUEAC = Convert.ToDouble(durerTraite.FirstOrDefault().DELAISAC),
                            DUREETRAITEMENTPREVUEBK = Convert.ToDouble(durerTraite.FirstOrDefault().DELAISBK),

                            DEPASSEMENTOP = durerTraite.FirstOrDefault().DELAISOP != null ? Convert.ToDouble(durerTraite.FirstOrDefault().DELAISOP) - dateOP : 0,
                            DEPASSEMENTAC = durerTraite.FirstOrDefault().DELAISAC != null ? Convert.ToDouble(durerTraite.FirstOrDefault().DELAISAC) - dateAC : 0,
                            DEPASSEMENTBK = durerTraite.FirstOrDefault().DELAISBK != null ? Convert.ToDouble(durerTraite.FirstOrDefault().DELAISBK) - dateBK : 0,
                        });
                    }
                }
                else
                {
                    var s = await (
                    from soa in db.SI_SOAS
                    join prosoa in db.SI_PROSOA on soa.ID equals prosoa.IDSOA
                    where prosoa.IDPROJET == projectId && prosoa.DELETIONDATE == null && soa.DELETIONDATE == null
                    select new
                    {
                        soa.SOA
                    }
                ).FirstOrDefaultAsync();


                    var paielst = (
                                   from r in db.OPA_REGLEMENT
                                   join v in db.OPA_VALIDATIONS on r.IDSOCIETE equals v.IDPROJET
                                   where r.NUM.ToString() == v.IDREGLEMENT && r.IDSOCIETE == projectId
                                   select new
                                   {
                                       BENEFICIAIRE = r.BENEFICIAIRE,
                                       MONTANT = r.MONTANT != null ? r.MONTANT : null,
                                       NUM = r.NUM,
                                       DATECREA = v.DATECREA != null ? v.DATECREA : null,
                                       DATESEND = v.DATESEND != null ? v.DATESEND : null,
                                       DATEVAL = v.DATEVAL != null ? v.DATEVAL : null,
                                       IDUSCREA = v.IDUSCREA != null ? v.IDUSCREA : null,
                                       IDUSSEND = v.IDUSSEND != null ? v.IDUSSEND : null,
                                       IDUSVAL = v.IDUSVAL != null ? v.IDUSVAL : null,
                                   }
                               ).ToList();
                    var durerTraite = db.SI_DELAISTRAITEMENT.Where(x => x.IDPROJET == projectId).Select(x => new {
                        DELAISOP = x.DELPP,
                        DELAISAC = x.DELPV,
                        DELAISBK = x.DELPB,
                    }).ToList();
                    lastIndex += 1;

                    result.Add(new TraitementPaiement
                    {
                        SOA = s != null ? s.SOA : "",
                        TraitementPaiementDetails = new List<TraitementPaiementDetails>()
                    });

                    for (int j = 0; j < paielst.Count; j += 1)
                    {
                        result[lastIndex].TraitementPaiementDetails.Add(new TraitementPaiementDetails
                        {
                            NUM_ENGAGEMENT = paielst[j].NUM.ToString(),
                            BENEFICIAIRE = paielst[j].BENEFICIAIRE,
                            MONTENGAGEMENT = paielst[j].MONTANT.ToString(),
                            DATETRANSFERTRAF = paielst[j].DATECREA,
                            TRANSFERTRAFAGENT = await GetAgent(paielst[j].IDUSCREA),
                            DATEVALORDSEC = paielst[j].DATEVAL,
                            VALORDSECAGENT = await GetAgent(paielst[j].IDUSVAL),
                            DATESENDSIIG = paielst[j].DATESEND,
                            SENDSIIGAGENT = await GetAgent(paielst[j].IDUSSEND),
                            DUREETRAITEMENTTRANSFERTOP = Date.GetDifference(paielst[j].DATECREA, paielst[j].DATESEND),
                            DUREETRAITEMENTTRANSFERTAC = Date.GetDifference(paielst[j].DATESEND, paielst[j].DATEVAL),
                            DUREETRAITEMENTTRANSFERTBK = Date.GetDifference(paielst[j].DATESEND, paielst[j].DATEVAL),

                            DUREETRAITEMENTPREVUEOP = Convert.ToDouble(durerTraite.FirstOrDefault().DELAISOP),
                            DUREETRAITEMENTPREVUEAC = Convert.ToDouble(durerTraite.FirstOrDefault().DELAISAC),
                            DUREETRAITEMENTPREVUEBK = Convert.ToDouble(durerTraite.FirstOrDefault().DELAISBK),

                            DEPASSEMENTOP = durerTraite.FirstOrDefault().DELAISOP != null ? Convert.ToDouble(durerTraite.FirstOrDefault().DELAISOP) - Convert.ToDouble(Date.GetDifference(paielst[j].DATECREA, paielst[j].DATESEND)) : 0,
                            DEPASSEMENTAC = durerTraite.FirstOrDefault().DELAISAC != null ? Convert.ToDouble(durerTraite.FirstOrDefault().DELAISAC) - Convert.ToDouble(Date.GetDifference(paielst[j].DATESEND, paielst[j].DATEVAL)) : 0,
                            DEPASSEMENTBK = durerTraite.FirstOrDefault().DELAISBK != null ? Convert.ToDouble(durerTraite.FirstOrDefault().DELAISBK) - Convert.ToDouble(Date.GetDifference(paielst[j].DATEVAL, paielst[j].DATEVAL)) : 0,
                        });
                    }
                }
            }

            return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = result }, settings));
        }
        public ActionResult TraitementPaiementRejet()
        {
            ViewBag.Controller = "Traitements des paiements réjéter";
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GenerePaiementREJETE(SI_USERS suser, string listProjet, DateTime DateDebut, DateTime DateFin)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                List<TxtPAIEMENT> list = new List<TxtPAIEMENT>();
                string[] separators = { "," };
                var pro = listProjet;
                if (pro != null)
                {
                    string listUser = pro.ToString();
                    string[] lst = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var idP in lst)
                    {
                        int crpt = int.Parse(idP);

                        //Check si le projet est mappé à une base de données TOM²PRO//
                        if (db.SI_MAPPAGES.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null) == null)
                            return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Le projet n'est pas mappé à une base de données TOM²PRO. " }, settings));
                    }

                    foreach (var idP in lst)
                    {
                        int crpt = int.Parse(idP);

                        var typeEcriture = db.SI_TYPECRITURE.Where(x => x.IDPROJET == crpt).FirstOrDefault().TYPE;
                        if (db.OPA_VALIDATIONS.FirstOrDefault(a => a.IDPROJET == crpt && a.ETAT == 2 && a.DateIn >= DateDebut && a.DateOut <= DateFin) != null)
                        {
                            foreach (var x in db.OPA_VALIDATIONS.Where(a => a.IDPROJET == crpt && a.ETAT == 2 && a.DateIn >= DateDebut && a.DateOut <= DateFin).OrderBy(a => a.DateIn).OrderBy(a => a.DATECREA).ToList())
                            {
                                var soa = (from soas in db.SI_SOAS
                                           join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                           where prj.IDPROJET == crpt && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                           select new
                                           {
                                               soas.SOA
                                           }).FirstOrDefault();

                                var cancel = db.OPA_VALIDATIONS.Where(a => a.IDPROJET == crpt && x.ETAT == 4).Join(db.SI_USERS,z => z.IDUSER,e => e.IDUSER, (z, e) => new
                                {
                                    AGENT = e.LOGIN,
                                    NUMENGAGEMENT = z.IDREGLEMENT,
                                    DATEREJETPAIEMENT = z.DATEANNULER,
                                    MOTIF = z.MOTIF,
                                    COMMENTAIRE = z.COMS,

                                }).Join(db.OPA_REGLEMENTBR, t =>t.NUMENGAGEMENT,p => p.NUM, (t, p) => new
                                {
                                    AGENT = t.AGENT,
                                    NUMENGAGEMENT = t.NUMENGAGEMENT,
                                    DATEREJETPAIEMENT = t.DATEREJETPAIEMENT,
                                    MOTIF = t.MOTIF,
                                    COMMENTAIRE = t.MOTIF,
                                    BENEFICIAIRE = p.BENEFICIAIRE,
                                    MONTANT = p.MONTANT,
                                }).ToList();

                                foreach (var item in cancel)
                                {
                                    list.Add(new TxtPAIEMENT
                                    {
                                        No = item.NUMENGAGEMENT,
                                        BENEF = item.BENEFICIAIRE,
                                        MONTANT = item.MONTANT.ToString(),
                                        DATEREJETAC = item.DATEREJETPAIEMENT,
                                        SOA = soa.SOA != null ? soa.SOA : "",
                                        PROJET = db.SI_PROJETS.Where(a => a.ID == crpt && a.DELETIONDATE == null).FirstOrDefault().PROJET
                                    });
                                }
                            }
                        }
                    }
                }

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = list.ToList() }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }
    }
}
