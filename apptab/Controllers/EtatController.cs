using apptab.Data.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace apptab.Controllers
{
    public class EtatController : Controller
    {
        private readonly SOFTCONNECTSIIG db = new SOFTCONNECTSIIG();

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        //Financement//
        public ActionResult InfoProCreate()
        {
            ViewBag.Controller = "Informations PROJET";

            return View();
        }

        [HttpPost]
        //public async DetailsInfoPro(SI_USERS suser)
        public async Task<ActionResult> DetailsInfoPro(SI_USERS suser, int iProjet)
        {
            var exist = await db.SI_USERS.FirstOrDefaultAsync(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                //int crpt = exist.IDPROJET.Value;
                int crpt = iProjet;

                var fina = "";
                if (db.SI_FINANCEMENT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null) != null)
                {
                    fina = db.SI_FINANCEMENT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null).FINANCEMENT;
                }
                var convention = "";
                if (db.SI_CONVENTION.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null) != null)
                {
                    convention = db.SI_CONVENTION.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null).CONVENTION;
                }
                var catego = "";
                if (db.SI_CATEGORIE.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null) != null)
                {
                    catego = db.SI_CATEGORIE.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null).CATEGORIE;
                }
                var enga = "";
                if (db.SI_ENGAGEMENT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null) != null)
                {
                    enga = db.SI_ENGAGEMENT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null).ENGAGEMENT;
                }
                var proc = "";
                if (db.SI_PROCEDURE.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null) != null)
                {
                    proc = db.SI_PROCEDURE.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null).PROCEDURE;
                }
                var min = "";
                if (db.SI_MINISTERE.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null) != null)
                {
                    min = db.SI_MINISTERE.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null).MINISTERE;
                }
                var mis = "";
                if (db.SI_MISSION.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null) != null)
                {
                    mis = db.SI_MISSION.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null).MISSION;
                }
                var prog = "";
                if (db.SI_PROGRAMME.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null) != null)
                {
                    prog = db.SI_PROGRAMME.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null).PROGRAMME;
                }
                var act = "";
                if (db.SI_ACTIVITE.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null) != null)
                {
                    act = db.SI_ACTIVITE.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null).ACTIVITE;
                }
                int proj = 0;
                if (db.SI_PROJETS.FirstOrDefault(a => a.ID == crpt && a.DELETIONDATE == null) != null)
                {
                    proj = db.SI_PROJETS.FirstOrDefault(a => a.ID == crpt && a.DELETIONDATE == null).ID;
                }
                var soaA = "";
                if ((from soa in db.SI_SOAS
                     join pro in db.SI_PROSOA on soa.ID equals pro.IDSOA
                     where pro.IDPROJET == crpt && pro.DELETIONDATE == null && soa.DELETIONDATE == null
                     select soa.SOA).FirstOrDefault() != null)
                {
                    soaA = (from soa in db.SI_SOAS
                            join pro in db.SI_PROSOA on soa.ID equals pro.IDSOA
                            where pro.IDPROJET == crpt && pro.DELETIONDATE == null && soa.DELETIONDATE == null
                            select soa.SOA).FirstOrDefault();
                }

                return Json(JsonConvert.SerializeObject(new
                {
                    type = "success",
                    msg = "message",
                    data = new
                    {
                        FIN = fina,
                        CONV = convention,
                        CAT = catego,
                        ENG = enga,
                        PROC = proc,
                        MIN = min,
                        MIS = mis,
                        PROG = prog,
                        ACT = act,
                        PROJ = proj,
                        SOA = soaA
                    }
                }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        [HttpPost]
        public ActionResult DetailsInfoProMANDAT(SI_USERS suser)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = suser.IDPROJET.Value;

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

        //GET ALL PROJET//
        [HttpPost]
        public async Task<ActionResult> GetAllPROJET(SI_USERS suser)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                var test = db.SI_USERS.Where(x => x.LOGIN == exist.LOGIN && x.PWD == exist.PWD && x.DELETIONDATE == null).FirstOrDefault();
                if (test.ROLE == (int)Role.SAdministrateur)
                {
                    var user = await db.SI_PROJETS.Select(a => new
                    {
                        PROJET = a.PROJET,
                        ID = a.ID,
                        DELETIONDATE = a.DELETIONDATE,
                    }).Where(a => a.DELETIONDATE == null).ToListAsync();

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = user }, settings));
                }
                else
                {
                    if (test.IDPROJET != 0)
                    {
                        var user = await db.SI_PROJETS.Select(a => new
                        {
                            PROJET = a.PROJET,
                            ID = a.ID,
                            DELETIONDATE = a.DELETIONDATE,
                        }).Where(a => a.DELETIONDATE == null && a.ID == test.IDPROJET).ToListAsync();

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

        //ETAT MANDAT PROJET//
        public ActionResult EtatMandatP()
        {
            ViewBag.Controller = "Etat MANDATS";

            return View();
        }

        private async Task<List<DATATRPROJET>> GetM(Guid idLiquidation, int crpt)
        {
            var res = new List<DATATRPROJET>();

            SOFTCONNECTSIIG db = new SOFTCONNECTSIIG();

            SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
            SOFTCONNECTOM tom = new SOFTCONNECTOM();

            var ms = await tom.CPTADMIN_MLIQUIDATION.Where(a => a.IDLIQUIDATION == idLiquidation).ToListAsync();

            for (int i = 0; i < ms.Count; i += 1)
            {
                res.Add(new DATATRPROJET
                {
                    REF = ms[i].ID.ToString(),
                    OBJ = ms[i].LIBELLE,
                    TITUL = ms[i].LIBELLE,
                    MONT = Math.Round((double)ms[i].MONTANTLOCAL, 2).ToString(),
                    COMPTE = ms[i].COGE,
                    PCOP = ms[i].POSTE
                });
            }

            return res;
        }

        [HttpPost]
        public async Task<JsonResult> EtatMandatProjet(SI_USERS suser)
        {
            var exist = await db.SI_USERS.FirstOrDefaultAsync(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = exist.IDPROJET.Value;

                var db = new SOFTCONNECTSIIG();
                var connex = new Data.Extension().GetCon(crpt);

                var list = new List<DATATRPROJET>();

                if (await db.SI_TRAITPROJET.FirstOrDefaultAsync(a => a.IDPROJET == crpt) != null)
                {
                    foreach (var x in await db.SI_TRAITPROJET.Where(a => a.IDPROJET == crpt).ToListAsync())
                    {
                        var sta = "Attente validation";
                        if (x.ETAT == 1)
                            sta = "Validée";
                        else if (x.ETAT == 2)
                            sta = "Annulée";
                        else if (x.ETAT == 3)
                            sta = "Traitée SIIGFP";

                        list.Add(new X
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
                            STAT = sta,
                            M = await GetM((Guid)x.No, crpt)
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
        public JsonResult EtatMandatChange(SI_USERS suser, int IdPROJET)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = IdPROJET;

                SOFTCONNECTSIIG db = new SOFTCONNECTSIIG();
                SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
                SOFTCONNECTOM tom = new SOFTCONNECTOM();

                List<DATATRPROJET> list = new List<DATATRPROJET>();

                if (db.SI_TRAITPROJET.FirstOrDefault(a => a.IDPROJET == crpt && a.ETAT == 0) != null)
                {
                    foreach (var x in db.SI_TRAITPROJET.Where(a => a.IDPROJET == crpt && a.ETAT == 0).ToList())
                    {
                        var sta = "Attente validation";
                        if (x.ETAT == 1)
                            sta = "Validée";
                        else if (x.ETAT == 2)
                            sta = "Annulée";
                        else if (x.ETAT == 3)
                            sta = "Traitée SIIGFP";

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
                            STAT = sta
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
        public JsonResult EtatMandatProjetSEARCH(SI_USERS suser, DateTime DateDebut, DateTime DateFin, int STAT)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = 0;
                if (suser.IDPROJET == null)
                    crpt = exist.IDPROJET.Value;
                else
                    crpt = suser.IDPROJET.Value;

                SOFTCONNECTSIIG db = new SOFTCONNECTSIIG();
                SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
                SOFTCONNECTOM tom = new SOFTCONNECTOM();

                List<DATATRPROJET> list = new List<DATATRPROJET>();

                if (STAT == 0)
                {
                    if (db.SI_TRAITPROJET.FirstOrDefault(a => a.IDPROJET == crpt && a.DATEMANDAT >= DateDebut && a.DATEMANDAT <= DateFin) != null)
                    {
                        foreach (var x in db.SI_TRAITPROJET.Where(a => a.IDPROJET == crpt && a.DATEMANDAT >= DateDebut && a.DATEMANDAT <= DateFin).ToList())
                        {
                            var sta = "Attente validation";
                            if (x.ETAT == 1)
                                sta = "Validée";
                            else if (x.ETAT == 2)
                                sta = "Annulée";
                            else if (x.ETAT == 3)
                                sta = "Traitée SIIGFP";

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
                                STAT = sta
                            });
                        }
                    }
                }
                else
                {
                    var ett = 0;
                    /*if (STAT == 1) ett = 0;*/
                    if (STAT == 2) ett = 1;
                    if (STAT == 3) ett = 2;
                    if (STAT == 4) ett = 3;

                    if (db.SI_TRAITPROJET.FirstOrDefault(a => a.IDPROJET == crpt && a.DATEMANDAT >= DateDebut && a.DATEMANDAT <= DateFin && a.ETAT == ett) != null)
                    {
                        foreach (var x in db.SI_TRAITPROJET.Where(a => a.IDPROJET == crpt && a.DATEMANDAT >= DateDebut && a.DATEMANDAT <= DateFin && a.ETAT == ett).ToList())
                        {
                            var sta = "Attente validation";

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
                                STAT = sta
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
        public JsonResult DeleteUser(SI_USERS suser, string UserId)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDPROJET == suser.IDPROJET*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = 0;
                if (suser.IDPROJET == null)
                    crpt = exist.IDPROJET.Value;
                else
                    crpt = suser.IDPROJET.Value;

                Guid useID = Guid.Parse(UserId);
                var user = db.SI_TRAITPROJET.FirstOrDefault(a => a.No == useID && a.IDPROJET == crpt);
                if (user != null)
                {
                    user.ETAT = 2;
                    user.DATEANNUL = DateTime.Now;
                    user.IDUSERANNUL = exist.ID;
                    db.SaveChanges();

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Annulation avec succès. " }, settings));
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
    }
}
