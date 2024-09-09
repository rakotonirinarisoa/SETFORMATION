using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using apptab.Models;
using Newtonsoft.Json;

namespace apptab.Controllers
{
    public class CalendarController : Controller
    {
        private readonly SOFTCONNECTSIIG db = new SOFTCONNECTSIIG();

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        // GET: Calendar
        public ActionResult Index()
        {
            ViewBag.Controller = "Liste des événements";

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetAllEvents(SI_USERS suser)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                List<EVENEMENTS> list = new List<EVENEMENTS>();

                var test = db.SI_USERS.Where(x => x.LOGIN == exist.LOGIN && x.PWD == exist.PWD && x.DELETIONDATE == null).FirstOrDefault();

                var mandat = db.SI_TRAITPROJET.ToList();
                var ava = db.SI_TRAITAVANCE.ToList();
                var payement = db.OPA_VALIDATIONS.ToList();

                if (test.ROLE == (int)Role.SAdministrateur)
                {
                    //DATE CREATION//
                    foreach (var x in mandat.GroupBy(x => new { x.IDPROJET, x.DATECRE.Value.Date, x.IDUSERCREATE }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATECRE = x.FirstOrDefault().DATECRE.Value.Date, IDUSERCREATE = x.FirstOrDefault().IDUSERCREATE, COUNT = x.Count() }).ToList())
                    {
                        var isSoa = (from soas in db.SI_SOAS
                        join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                        where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                     select new
                                     {
                                         soas.SOA
                                     }).FirstOrDefault();

                        var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                        var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERCREATE && z.DELETIONDATE == null).FirstOrDefault();
                        var isCount = x.COUNT;
                        DateTime? isDate = x.DATECRE;
                        var isType = 0;
                        var isEtat = 0;

                        list.Add(new EVENEMENTS
                        {
                            SOA = isSoa != null ? isSoa.SOA : "",
                            PROJET = isProjet != null ? isProjet.PROJET : "",
                            TYPE = isType,
                            ETAT = isEtat,
                            USER = isUser != null ? isUser.LOGIN : "",
                            COUNT = isCount,
                            DATE = isDate.Value.Date,
                        });
                    }
                    foreach (var x in ava.GroupBy(x => new { x.IDPROJET, x.DATECRE.Value.Date, x.IDUSERCREATE }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATECRE = x.FirstOrDefault().DATECRE.Value.Date, IDUSERCREATE = x.FirstOrDefault().IDUSERCREATE, COUNT = x.Count() }).ToList())
                    {
                        var isSoa = (from soas in db.SI_SOAS
                                     join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                     where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                     select new
                                     {
                                         soas.SOA
                                     }).FirstOrDefault();

                        var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                        var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERCREATE && z.DELETIONDATE == null).FirstOrDefault();
                        var isCount = x.COUNT;
                        DateTime? isDate = x.DATECRE;
                        var isType = 3;
                        var isEtat = 0;

                        list.Add(new EVENEMENTS
                        {
                            SOA = isSoa != null ? isSoa.SOA : "",
                            PROJET = isProjet != null ? isProjet.PROJET : "",
                            TYPE = isType,
                            ETAT = isEtat,
                            USER = isUser != null ? isUser.LOGIN : "",
                            COUNT = isCount,
                            DATE = isDate.Value.Date,
                        });
                    }
                    //DATE VALIDATION//
                    foreach (var x in mandat.Where(x => x.DATEVALIDATION != null).GroupBy(x => new { x.IDPROJET, x.DATEVALIDATION.Value.Date, x.IDUSERVALIDATE }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATEVALIDATION = x.FirstOrDefault().DATEVALIDATION.Value.Date, IDUSERVALIDATE = x.FirstOrDefault().IDUSERVALIDATE, COUNT = x.Count() }).ToList())
                    {
                        var isSoa = (from soas in db.SI_SOAS
                                     join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                     where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                     select new
                                     {
                                         soas.SOA
                                     }).FirstOrDefault();

                        var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                        var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERVALIDATE && z.DELETIONDATE == null).FirstOrDefault();
                        var isCount = x.COUNT;
                        DateTime? isDate = x.DATEVALIDATION;
                        var isType = 0;
                        var isEtat = 1;

                        list.Add(new EVENEMENTS
                        {
                            SOA = isSoa != null ? isSoa.SOA : "",
                            PROJET = isProjet != null ? isProjet.PROJET : "",
                            TYPE = isType,
                            ETAT = isEtat,
                            USER = isUser != null ? isUser.LOGIN : "",
                            COUNT = isCount,
                            DATE = isDate.Value.Date,
                        });
                    }
                    foreach (var x in ava.Where(x => x.DATEVALIDATION != null).GroupBy(x => new { x.IDPROJET, x.DATEVALIDATION.Value.Date, x.IDUSERVALIDATE }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATEVALIDATION = x.FirstOrDefault().DATEVALIDATION.Value.Date, IDUSERVALIDATE = x.FirstOrDefault().IDUSERVALIDATE, COUNT = x.Count() }).ToList())
                    {
                        var isSoa = (from soas in db.SI_SOAS
                                     join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                     where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                     select new
                                     {
                                         soas.SOA
                                     }).FirstOrDefault();

                        var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                        var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERVALIDATE && z.DELETIONDATE == null).FirstOrDefault();
                        var isCount = x.COUNT;
                        DateTime? isDate = x.DATEVALIDATION;
                        var isType = 3;
                        var isEtat = 1;

                        list.Add(new EVENEMENTS
                        {
                            SOA = isSoa != null ? isSoa.SOA : "",
                            PROJET = isProjet != null ? isProjet.PROJET : "",
                            TYPE = isType,
                            ETAT = isEtat,
                            USER = isUser != null ? isUser.LOGIN : "",
                            COUNT = isCount,
                            DATE = isDate.Value.Date,
                        });
                    }
                    //DATE ENVOI SIIGFP//
                    foreach (var x in mandat.Where(x => x.DATENVOISIIGFP != null).GroupBy(x => new { x.IDPROJET, x.DATENVOISIIGFP.Value.Date, x.IDUSERENVOISIIGFP }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATENVOISIIGFP = x.FirstOrDefault().DATENVOISIIGFP.Value.Date, IDUSERENVOISIIGFP = x.FirstOrDefault().IDUSERENVOISIIGFP, COUNT = x.Count() }).ToList())
                    {
                        var isSoa = (from soas in db.SI_SOAS
                                     join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                     where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                     select new
                                     {
                                         soas.SOA
                                     }).FirstOrDefault();

                        var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                        var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERENVOISIIGFP && z.DELETIONDATE == null).FirstOrDefault();
                        var isCount = x.COUNT;
                        DateTime? isDate = x.DATENVOISIIGFP;
                        var isType = 0;
                        var isEtat = 3;

                        list.Add(new EVENEMENTS
                        {
                            SOA = isSoa != null ? isSoa.SOA : "",
                            PROJET = isProjet != null ? isProjet.PROJET : "",
                            TYPE = isType,
                            ETAT = isEtat,
                            USER = isUser != null ? isUser.LOGIN : "",
                            COUNT = isCount,
                            DATE = isDate.Value.Date,
                        });
                    }
                    foreach (var x in ava.Where(x => x.DATENVOISIIGFP != null).GroupBy(x => new { x.IDPROJET, x.DATENVOISIIGFP.Value.Date, x.IDUSERENVOISIIGFP }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATENVOISIIGFP = x.FirstOrDefault().DATENVOISIIGFP.Value.Date, IDUSERENVOISIIGFP = x.FirstOrDefault().IDUSERENVOISIIGFP, COUNT = x.Count() }).ToList())
                    {
                        var isSoa = (from soas in db.SI_SOAS
                                     join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                     where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                     select new
                                     {
                                         soas.SOA
                                     }).FirstOrDefault();

                        var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                        var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERENVOISIIGFP && z.DELETIONDATE == null).FirstOrDefault();
                        var isCount = x.COUNT;
                        DateTime? isDate = x.DATENVOISIIGFP;
                        var isType = 3;
                        var isEtat = 3;

                        list.Add(new EVENEMENTS
                        {
                            SOA = isSoa != null ? isSoa.SOA : "",
                            PROJET = isProjet != null ? isProjet.PROJET : "",
                            TYPE = isType,
                            ETAT = isEtat,
                            USER = isUser != null ? isUser.LOGIN : "",
                            COUNT = isCount,
                            DATE = isDate.Value.Date,
                        });
                    }
                    //DATE ANNULATION//
                    foreach (var x in mandat.Where(x => x.DATEANNUL != null).GroupBy(x => new { x.IDPROJET, x.DATEANNUL.Value.Date, x.IDUSERANNUL }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATEANNUL = x.FirstOrDefault().DATEANNUL.Value.Date, IDUSERANNUL = x.FirstOrDefault().IDUSERANNUL, COUNT = x.Count() }).ToList())
                    {
                        var isSoa = (from soas in db.SI_SOAS
                                     join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                     where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                     select new
                                     {
                                         soas.SOA
                                     }).FirstOrDefault();

                        var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                        var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERANNUL && z.DELETIONDATE == null).FirstOrDefault();
                        var isCount = x.COUNT;
                        DateTime? isDate = x.DATEANNUL;
                        var isType = 0;
                        var isEtat = 2;

                        list.Add(new EVENEMENTS
                        {
                            SOA = isSoa != null ? isSoa.SOA : "",
                            PROJET = isProjet != null ? isProjet.PROJET : "",
                            TYPE = isType,
                            ETAT = isEtat,
                            USER = isUser != null ? isUser.LOGIN : "",
                            COUNT = isCount,
                            DATE = isDate.Value.Date,
                        });
                    }
                    foreach (var x in ava.Where(x => x.DATEANNUL != null).GroupBy(x => new { x.IDPROJET, x.DATEANNUL.Value.Date, x.IDUSERANNUL }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATEANNUL = x.FirstOrDefault().DATEANNUL.Value.Date, IDUSERANNUL = x.FirstOrDefault().IDUSERANNUL, COUNT = x.Count() }).ToList())
                    {
                        var isSoa = (from soas in db.SI_SOAS
                                     join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                     where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                     select new
                                     {
                                         soas.SOA
                                     }).FirstOrDefault();

                        var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                        var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERANNUL && z.DELETIONDATE == null).FirstOrDefault();
                        var isCount = x.COUNT;
                        DateTime? isDate = x.DATEANNUL;
                        var isType = 3;
                        var isEtat = 2;

                        list.Add(new EVENEMENTS
                        {
                            SOA = isSoa != null ? isSoa.SOA : "",
                            PROJET = isProjet != null ? isProjet.PROJET : "",
                            TYPE = isType,
                            ETAT = isEtat,
                            USER = isUser != null ? isUser.LOGIN : "",
                            COUNT = isCount,
                            DATE = isDate.Value.Date,
                        });
                    }
                    //DATE TRAITE SIIG//
                    foreach (var x in mandat.Where(x => x.DATESIIG != null).GroupBy(x => new { x.IDPROJET, x.DATESIIG.Value.Date, x.USERSIIG }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATESIIG = x.FirstOrDefault().DATESIIG.Value.Date, USERSIIG = x.FirstOrDefault().USERSIIG, COUNT = x.Count() }).ToList())
                    {
                        var isSoa = (from soas in db.SI_SOAS
                                     join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                     where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                     select new
                                     {
                                         soas.SOA
                                     }).FirstOrDefault();

                        var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                        //var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERANNUL && z.DELETIONDATE == null).FirstOrDefault();
                        var isCount = x.COUNT;
                        DateTime? isDate = x.DATESIIG;
                        var isType = 0;
                        var isEtat = 4;

                        list.Add(new EVENEMENTS
                        {
                            SOA = isSoa != null ? isSoa.SOA : "",
                            PROJET = isProjet != null ? isProjet.PROJET : "",
                            TYPE = isType,
                            ETAT = isEtat,
                            USER = x.USERSIIG != "" ? x.USERSIIG : "",
                            COUNT = isCount,
                            DATE = isDate.Value.Date,
                        });
                    }
                    foreach (var x in ava.Where(x => x.DATESIIG != null).GroupBy(x => new { x.IDPROJET, x.DATESIIG.Value.Date, x.USERSIIG }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATESIIG = x.FirstOrDefault().DATESIIG.Value.Date, USERSIIG = x.FirstOrDefault().USERSIIG, COUNT = x.Count() }).ToList())
                    {
                        var isSoa = (from soas in db.SI_SOAS
                                     join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                     where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                     select new
                                     {
                                         soas.SOA
                                     }).FirstOrDefault();

                        var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                        //var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERANNUL && z.DELETIONDATE == null).FirstOrDefault();
                        var isCount = x.COUNT;
                        DateTime? isDate = x.DATESIIG;
                        var isType = 3;
                        var isEtat = 4;

                        list.Add(new EVENEMENTS
                        {
                            SOA = isSoa != null ? isSoa.SOA : "",
                            PROJET = isProjet != null ? isProjet.PROJET : "",
                            TYPE = isType,
                            ETAT = isEtat,
                            USER = x.USERSIIG != "" ? x.USERSIIG : "",
                            COUNT = isCount,
                            DATE = isDate.Value.Date,
                        });
                    }

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = list }, settings));
                }
                else
                {
                    if (test.IDPROJET != 0)
                    {
                        mandat = mandat.Where(a => a.IDPROJET == test.IDPROJET).ToList();
                        ava = ava.Where(a => a.IDPROJET == test.IDPROJET).ToList();
                        payement = payement.Where(a => a.IDPROJET == test.IDPROJET).ToList();

                        //DATE CREATION//
                        foreach (var x in mandat.GroupBy(x => new { x.IDPROJET, x.DATECRE.Value.Date, x.IDUSERCREATE }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATECRE = x.FirstOrDefault().DATECRE.Value.Date, IDUSERCREATE = x.FirstOrDefault().IDUSERCREATE, COUNT = x.Count() }).ToList())
                        {
                            var isSoa = (from soas in db.SI_SOAS
                                         join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                         where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                         select new
                                         {
                                             soas.SOA
                                         }).FirstOrDefault();

                            var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                            var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERCREATE && z.DELETIONDATE == null).FirstOrDefault();
                            var isCount = x.COUNT;
                            DateTime? isDate = x.DATECRE;
                            var isType = 0;
                            var isEtat = 0;

                            list.Add(new EVENEMENTS
                            {
                                SOA = isSoa != null ? isSoa.SOA : "",
                                PROJET = isProjet != null ? isProjet.PROJET : "",
                                TYPE = isType,
                                ETAT = isEtat,
                                USER = isUser != null ? isUser.LOGIN : "",
                                COUNT = isCount,
                                DATE = isDate.Value.Date,
                            });
                        }
                        foreach (var x in ava.GroupBy(x => new { x.IDPROJET, x.DATECRE.Value.Date, x.IDUSERCREATE }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATECRE = x.FirstOrDefault().DATECRE.Value.Date, IDUSERCREATE = x.FirstOrDefault().IDUSERCREATE, COUNT = x.Count() }).ToList())
                        {
                            var isSoa = (from soas in db.SI_SOAS
                                         join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                         where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                         select new
                                         {
                                             soas.SOA
                                         }).FirstOrDefault();

                            var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                            var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERCREATE && z.DELETIONDATE == null).FirstOrDefault();
                            var isCount = x.COUNT;
                            DateTime? isDate = x.DATECRE;
                            var isType = 3;
                            var isEtat = 0;

                            list.Add(new EVENEMENTS
                            {
                                SOA = isSoa != null ? isSoa.SOA : "",
                                PROJET = isProjet != null ? isProjet.PROJET : "",
                                TYPE = isType,
                                ETAT = isEtat,
                                USER = isUser != null ? isUser.LOGIN : "",
                                COUNT = isCount,
                                DATE = isDate.Value.Date,
                            });
                        }
                        //DATE VALIDATION//
                        foreach (var x in mandat.Where(x => x.DATEVALIDATION != null).GroupBy(x => new { x.IDPROJET, x.DATEVALIDATION.Value.Date, x.IDUSERVALIDATE }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATEVALIDATION = x.FirstOrDefault().DATEVALIDATION.Value.Date, IDUSERVALIDATE = x.FirstOrDefault().IDUSERVALIDATE, COUNT = x.Count() }).ToList())
                        {
                            var isSoa = (from soas in db.SI_SOAS
                                         join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                         where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                         select new
                                         {
                                             soas.SOA
                                         }).FirstOrDefault();

                            var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                            var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERVALIDATE && z.DELETIONDATE == null).FirstOrDefault();
                            var isCount = x.COUNT;
                            DateTime? isDate = x.DATEVALIDATION;
                            var isType = 0;
                            var isEtat = 1;

                            list.Add(new EVENEMENTS
                            {
                                SOA = isSoa != null ? isSoa.SOA : "",
                                PROJET = isProjet != null ? isProjet.PROJET : "",
                                TYPE = isType,
                                ETAT = isEtat,
                                USER = isUser != null ? isUser.LOGIN : "",
                                COUNT = isCount,
                                DATE = isDate.Value.Date,
                            });
                        }
                        foreach (var x in ava.Where(x => x.DATEVALIDATION != null).GroupBy(x => new { x.IDPROJET, x.DATEVALIDATION.Value.Date, x.IDUSERVALIDATE }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATEVALIDATION = x.FirstOrDefault().DATEVALIDATION.Value.Date, IDUSERVALIDATE = x.FirstOrDefault().IDUSERVALIDATE, COUNT = x.Count() }).ToList())
                        {
                            var isSoa = (from soas in db.SI_SOAS
                                         join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                         where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                         select new
                                         {
                                             soas.SOA
                                         }).FirstOrDefault();

                            var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                            var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERVALIDATE && z.DELETIONDATE == null).FirstOrDefault();
                            var isCount = x.COUNT;
                            DateTime? isDate = x.DATEVALIDATION;
                            var isType = 3;
                            var isEtat = 1;

                            list.Add(new EVENEMENTS
                            {
                                SOA = isSoa != null ? isSoa.SOA : "",
                                PROJET = isProjet != null ? isProjet.PROJET : "",
                                TYPE = isType,
                                ETAT = isEtat,
                                USER = isUser != null ? isUser.LOGIN : "",
                                COUNT = isCount,
                                DATE = isDate.Value.Date,
                            });
                        }
                        //DATE ENVOI SIIGFP//
                        foreach (var x in mandat.Where(x => x.DATENVOISIIGFP != null).GroupBy(x => new { x.IDPROJET, x.DATENVOISIIGFP.Value.Date, x.IDUSERENVOISIIGFP }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATENVOISIIGFP = x.FirstOrDefault().DATENVOISIIGFP.Value.Date, IDUSERENVOISIIGFP = x.FirstOrDefault().IDUSERENVOISIIGFP, COUNT = x.Count() }).ToList())
                        {
                            var isSoa = (from soas in db.SI_SOAS
                                         join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                         where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                         select new
                                         {
                                             soas.SOA
                                         }).FirstOrDefault();

                            var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                            var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERENVOISIIGFP && z.DELETIONDATE == null).FirstOrDefault();
                            var isCount = x.COUNT;
                            DateTime? isDate = x.DATENVOISIIGFP;
                            var isType = 0;
                            var isEtat = 3;

                            list.Add(new EVENEMENTS
                            {
                                SOA = isSoa != null ? isSoa.SOA : "",
                                PROJET = isProjet != null ? isProjet.PROJET : "",
                                TYPE = isType,
                                ETAT = isEtat,
                                USER = isUser != null ? isUser.LOGIN : "",
                                COUNT = isCount,
                                DATE = isDate.Value.Date,
                            });
                        }
                        foreach (var x in ava.Where(x => x.DATENVOISIIGFP != null).GroupBy(x => new { x.IDPROJET, x.DATENVOISIIGFP.Value.Date, x.IDUSERENVOISIIGFP }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATENVOISIIGFP = x.FirstOrDefault().DATENVOISIIGFP.Value.Date, IDUSERENVOISIIGFP = x.FirstOrDefault().IDUSERENVOISIIGFP, COUNT = x.Count() }).ToList())
                        {
                            var isSoa = (from soas in db.SI_SOAS
                                         join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                         where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                         select new
                                         {
                                             soas.SOA
                                         }).FirstOrDefault();

                            var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                            var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERENVOISIIGFP && z.DELETIONDATE == null).FirstOrDefault();
                            var isCount = x.COUNT;
                            DateTime? isDate = x.DATENVOISIIGFP;
                            var isType = 3;
                            var isEtat = 3;

                            list.Add(new EVENEMENTS
                            {
                                SOA = isSoa != null ? isSoa.SOA : "",
                                PROJET = isProjet != null ? isProjet.PROJET : "",
                                TYPE = isType,
                                ETAT = isEtat,
                                USER = isUser != null ? isUser.LOGIN : "",
                                COUNT = isCount,
                                DATE = isDate.Value.Date,
                            });
                        }
                        //DATE ANNULATION//
                        foreach (var x in mandat.Where(x => x.DATEANNUL != null).GroupBy(x => new { x.IDPROJET, x.DATEANNUL.Value.Date, x.IDUSERANNUL }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATEANNUL = x.FirstOrDefault().DATEANNUL.Value.Date, IDUSERANNUL = x.FirstOrDefault().IDUSERANNUL, COUNT = x.Count() }).ToList())
                        {
                            var isSoa = (from soas in db.SI_SOAS
                                         join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                         where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                         select new
                                         {
                                             soas.SOA
                                         }).FirstOrDefault();

                            var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                            var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERANNUL && z.DELETIONDATE == null).FirstOrDefault();
                            var isCount = x.COUNT;
                            DateTime? isDate = x.DATEANNUL;
                            var isType = 0;
                            var isEtat = 2;

                            list.Add(new EVENEMENTS
                            {
                                SOA = isSoa != null ? isSoa.SOA : "",
                                PROJET = isProjet != null ? isProjet.PROJET : "",
                                TYPE = isType,
                                ETAT = isEtat,
                                USER = isUser != null ? isUser.LOGIN : "",
                                COUNT = isCount,
                                DATE = isDate.Value.Date,
                            });
                        }
                        foreach (var x in ava.Where(x => x.DATEANNUL != null).GroupBy(x => new { x.IDPROJET, x.DATEANNUL.Value.Date, x.IDUSERANNUL }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATEANNUL = x.FirstOrDefault().DATEANNUL.Value.Date, IDUSERANNUL = x.FirstOrDefault().IDUSERANNUL, COUNT = x.Count() }).ToList())
                        {
                            var isSoa = (from soas in db.SI_SOAS
                                         join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                         where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                         select new
                                         {
                                             soas.SOA
                                         }).FirstOrDefault();

                            var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                            var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERANNUL && z.DELETIONDATE == null).FirstOrDefault();
                            var isCount = x.COUNT;
                            DateTime? isDate = x.DATEANNUL;
                            var isType = 3;
                            var isEtat = 2;

                            list.Add(new EVENEMENTS
                            {
                                SOA = isSoa != null ? isSoa.SOA : "",
                                PROJET = isProjet != null ? isProjet.PROJET : "",
                                TYPE = isType,
                                ETAT = isEtat,
                                USER = isUser != null ? isUser.LOGIN : "",
                                COUNT = isCount,
                                DATE = isDate.Value.Date,
                            });
                        }
                        //DATE TRAITE SIIG//
                        foreach (var x in mandat.Where(x => x.DATESIIG != null).GroupBy(x => new { x.IDPROJET, x.DATESIIG.Value.Date, x.USERSIIG }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATESIIG = x.FirstOrDefault().DATESIIG.Value.Date, USERSIIG = x.FirstOrDefault().USERSIIG, COUNT = x.Count() }).ToList())
                        {
                            var isSoa = (from soas in db.SI_SOAS
                                         join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                         where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                         select new
                                         {
                                             soas.SOA
                                         }).FirstOrDefault();

                            var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                            //var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERANNUL && z.DELETIONDATE == null).FirstOrDefault();
                            var isCount = x.COUNT;
                            DateTime? isDate = x.DATESIIG;
                            var isType = 0;
                            var isEtat = 4;

                            list.Add(new EVENEMENTS
                            {
                                SOA = isSoa != null ? isSoa.SOA : "",
                                PROJET = isProjet != null ? isProjet.PROJET : "",
                                TYPE = isType,
                                ETAT = isEtat,
                                USER = x.USERSIIG != "" ? x.USERSIIG : "",
                                COUNT = isCount,
                                DATE = isDate.Value.Date,
                            });
                        }
                        foreach (var x in ava.Where(x => x.DATESIIG != null).GroupBy(x => new { x.IDPROJET, x.DATESIIG.Value.Date, x.USERSIIG }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATESIIG = x.FirstOrDefault().DATESIIG.Value.Date, USERSIIG = x.FirstOrDefault().USERSIIG, COUNT = x.Count() }).ToList())
                        {
                            var isSoa = (from soas in db.SI_SOAS
                                         join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                         where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                         select new
                                         {
                                             soas.SOA
                                         }).FirstOrDefault();

                            var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                            //var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERANNUL && z.DELETIONDATE == null).FirstOrDefault();
                            var isCount = x.COUNT;
                            DateTime? isDate = x.DATESIIG;
                            var isType = 3;
                            var isEtat = 4;

                            list.Add(new EVENEMENTS
                            {
                                SOA = isSoa != null ? isSoa.SOA : "",
                                PROJET = isProjet != null ? isProjet.PROJET : "",
                                TYPE = isType,
                                ETAT = isEtat,
                                USER = x.USERSIIG != "" ? x.USERSIIG : "",
                                COUNT = isCount,
                                DATE = isDate.Value.Date,
                            });
                        }

                        return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = list }, settings));
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

                        foreach (var y in user)
                        {
                            mandat = mandat.Where(a => a.IDPROJET == y.ID).ToList();
                            ava = ava.Where(a => a.IDPROJET == y.ID).ToList();
                            payement = payement.Where(a => a.IDPROJET == y.ID).ToList();

                            //DATE CREATION//
                            foreach (var x in mandat.GroupBy(x => new { x.IDPROJET, x.DATECRE.Value.Date, x.IDUSERCREATE }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATECRE = x.FirstOrDefault().DATECRE.Value.Date, IDUSERCREATE = x.FirstOrDefault().IDUSERCREATE, COUNT = x.Count() }).ToList())
                            {
                                var isSoa = (from soas in db.SI_SOAS
                                             join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                             where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                             select new
                                             {
                                                 soas.SOA
                                             }).FirstOrDefault();

                                var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                                var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERCREATE && z.DELETIONDATE == null).FirstOrDefault();
                                var isCount = x.COUNT;
                                DateTime? isDate = x.DATECRE;
                                var isType = 0;
                                var isEtat = 0;

                                list.Add(new EVENEMENTS
                                {
                                    SOA = isSoa != null ? isSoa.SOA : "",
                                    PROJET = isProjet != null ? isProjet.PROJET : "",
                                    TYPE = isType,
                                    ETAT = isEtat,
                                    USER = isUser != null ? isUser.LOGIN : "",
                                    COUNT = isCount,
                                    DATE = isDate.Value.Date,
                                });
                            }
                            foreach (var x in ava.GroupBy(x => new { x.IDPROJET, x.DATECRE.Value.Date, x.IDUSERCREATE }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATECRE = x.FirstOrDefault().DATECRE.Value.Date, IDUSERCREATE = x.FirstOrDefault().IDUSERCREATE, COUNT = x.Count() }).ToList())
                            {
                                var isSoa = (from soas in db.SI_SOAS
                                             join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                             where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                             select new
                                             {
                                                 soas.SOA
                                             }).FirstOrDefault();

                                var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                                var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERCREATE && z.DELETIONDATE == null).FirstOrDefault();
                                var isCount = x.COUNT;
                                DateTime? isDate = x.DATECRE;
                                var isType = 3;
                                var isEtat = 0;

                                list.Add(new EVENEMENTS
                                {
                                    SOA = isSoa != null ? isSoa.SOA : "",
                                    PROJET = isProjet != null ? isProjet.PROJET : "",
                                    TYPE = isType,
                                    ETAT = isEtat,
                                    USER = isUser != null ? isUser.LOGIN : "",
                                    COUNT = isCount,
                                    DATE = isDate.Value.Date,
                                });
                            }
                            //DATE VALIDATION//
                            foreach (var x in mandat.Where(x => x.DATEVALIDATION != null).GroupBy(x => new { x.IDPROJET, x.DATEVALIDATION.Value.Date, x.IDUSERVALIDATE }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATEVALIDATION = x.FirstOrDefault().DATEVALIDATION.Value.Date, IDUSERVALIDATE = x.FirstOrDefault().IDUSERVALIDATE, COUNT = x.Count() }).ToList())
                            {
                                var isSoa = (from soas in db.SI_SOAS
                                             join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                             where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                             select new
                                             {
                                                 soas.SOA
                                             }).FirstOrDefault();

                                var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                                var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERVALIDATE && z.DELETIONDATE == null).FirstOrDefault();
                                var isCount = x.COUNT;
                                DateTime? isDate = x.DATEVALIDATION;
                                var isType = 0;
                                var isEtat = 1;

                                list.Add(new EVENEMENTS
                                {
                                    SOA = isSoa != null ? isSoa.SOA : "",
                                    PROJET = isProjet != null ? isProjet.PROJET : "",
                                    TYPE = isType,
                                    ETAT = isEtat,
                                    USER = isUser != null ? isUser.LOGIN : "",
                                    COUNT = isCount,
                                    DATE = isDate.Value.Date,
                                });
                            }
                            foreach (var x in ava.Where(x => x.DATEVALIDATION != null).GroupBy(x => new { x.IDPROJET, x.DATEVALIDATION.Value.Date, x.IDUSERVALIDATE }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATEVALIDATION = x.FirstOrDefault().DATEVALIDATION.Value.Date, IDUSERVALIDATE = x.FirstOrDefault().IDUSERVALIDATE, COUNT = x.Count() }).ToList())
                            {
                                var isSoa = (from soas in db.SI_SOAS
                                             join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                             where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                             select new
                                             {
                                                 soas.SOA
                                             }).FirstOrDefault();

                                var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                                var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERVALIDATE && z.DELETIONDATE == null).FirstOrDefault();
                                var isCount = x.COUNT;
                                DateTime? isDate = x.DATEVALIDATION;
                                var isType = 3;
                                var isEtat = 1;

                                list.Add(new EVENEMENTS
                                {
                                    SOA = isSoa != null ? isSoa.SOA : "",
                                    PROJET = isProjet != null ? isProjet.PROJET : "",
                                    TYPE = isType,
                                    ETAT = isEtat,
                                    USER = isUser != null ? isUser.LOGIN : "",
                                    COUNT = isCount,
                                    DATE = isDate.Value.Date,
                                });
                            }
                            //DATE ENVOI SIIGFP//
                            foreach (var x in mandat.Where(x => x.DATENVOISIIGFP != null).GroupBy(x => new { x.IDPROJET, x.DATENVOISIIGFP.Value.Date, x.IDUSERENVOISIIGFP }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATENVOISIIGFP = x.FirstOrDefault().DATENVOISIIGFP.Value.Date, IDUSERENVOISIIGFP = x.FirstOrDefault().IDUSERENVOISIIGFP, COUNT = x.Count() }).ToList())
                            {
                                var isSoa = (from soas in db.SI_SOAS
                                             join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                             where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                             select new
                                             {
                                                 soas.SOA
                                             }).FirstOrDefault();

                                var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                                var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERENVOISIIGFP && z.DELETIONDATE == null).FirstOrDefault();
                                var isCount = x.COUNT;
                                DateTime? isDate = x.DATENVOISIIGFP;
                                var isType = 0;
                                var isEtat = 3;

                                list.Add(new EVENEMENTS
                                {
                                    SOA = isSoa != null ? isSoa.SOA : "",
                                    PROJET = isProjet != null ? isProjet.PROJET : "",
                                    TYPE = isType,
                                    ETAT = isEtat,
                                    USER = isUser != null ? isUser.LOGIN : "",
                                    COUNT = isCount,
                                    DATE = isDate.Value.Date,
                                });
                            }
                            foreach (var x in ava.Where(x => x.DATENVOISIIGFP != null).GroupBy(x => new { x.IDPROJET, x.DATENVOISIIGFP.Value.Date, x.IDUSERENVOISIIGFP }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATENVOISIIGFP = x.FirstOrDefault().DATENVOISIIGFP.Value.Date, IDUSERENVOISIIGFP = x.FirstOrDefault().IDUSERENVOISIIGFP, COUNT = x.Count() }).ToList())
                            {
                                var isSoa = (from soas in db.SI_SOAS
                                             join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                             where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                             select new
                                             {
                                                 soas.SOA
                                             }).FirstOrDefault();

                                var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                                var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERENVOISIIGFP && z.DELETIONDATE == null).FirstOrDefault();
                                var isCount = x.COUNT;
                                DateTime? isDate = x.DATENVOISIIGFP;
                                var isType = 3;
                                var isEtat = 3;

                                list.Add(new EVENEMENTS
                                {
                                    SOA = isSoa != null ? isSoa.SOA : "",
                                    PROJET = isProjet != null ? isProjet.PROJET : "",
                                    TYPE = isType,
                                    ETAT = isEtat,
                                    USER = isUser != null ? isUser.LOGIN : "",
                                    COUNT = isCount,
                                    DATE = isDate.Value.Date,
                                });
                            }
                            //DATE ANNULATION//
                            foreach (var x in mandat.Where(x => x.DATEANNUL != null).GroupBy(x => new { x.IDPROJET, x.DATEANNUL.Value.Date, x.IDUSERANNUL }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATEANNUL = x.FirstOrDefault().DATEANNUL.Value.Date, IDUSERANNUL = x.FirstOrDefault().IDUSERANNUL, COUNT = x.Count() }).ToList())
                            {
                                var isSoa = (from soas in db.SI_SOAS
                                             join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                             where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                             select new
                                             {
                                                 soas.SOA
                                             }).FirstOrDefault();

                                var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                                var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERANNUL && z.DELETIONDATE == null).FirstOrDefault();
                                var isCount = x.COUNT;
                                DateTime? isDate = x.DATEANNUL;
                                var isType = 0;
                                var isEtat = 2;

                                list.Add(new EVENEMENTS
                                {
                                    SOA = isSoa != null ? isSoa.SOA : "",
                                    PROJET = isProjet != null ? isProjet.PROJET : "",
                                    TYPE = isType,
                                    ETAT = isEtat,
                                    USER = isUser != null ? isUser.LOGIN : "",
                                    COUNT = isCount,
                                    DATE = isDate.Value.Date,
                                });
                            }
                            foreach (var x in ava.Where(x => x.DATEANNUL != null).GroupBy(x => new { x.IDPROJET, x.DATEANNUL.Value.Date, x.IDUSERANNUL }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATEANNUL = x.FirstOrDefault().DATEANNUL.Value.Date, IDUSERANNUL = x.FirstOrDefault().IDUSERANNUL, COUNT = x.Count() }).ToList())
                            {
                                var isSoa = (from soas in db.SI_SOAS
                                             join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                             where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                             select new
                                             {
                                                 soas.SOA
                                             }).FirstOrDefault();

                                var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                                var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERANNUL && z.DELETIONDATE == null).FirstOrDefault();
                                var isCount = x.COUNT;
                                DateTime? isDate = x.DATEANNUL;
                                var isType = 3;
                                var isEtat = 2;

                                list.Add(new EVENEMENTS
                                {
                                    SOA = isSoa != null ? isSoa.SOA : "",
                                    PROJET = isProjet != null ? isProjet.PROJET : "",
                                    TYPE = isType,
                                    ETAT = isEtat,
                                    USER = isUser != null ? isUser.LOGIN : "",
                                    COUNT = isCount,
                                    DATE = isDate.Value.Date,
                                });
                            }
                            //DATE TRAITE SIIG//
                            foreach (var x in mandat.Where(x => x.DATESIIG != null).GroupBy(x => new { x.IDPROJET, x.DATESIIG.Value.Date, x.USERSIIG }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATESIIG = x.FirstOrDefault().DATESIIG.Value.Date, USERSIIG = x.FirstOrDefault().USERSIIG, COUNT = x.Count() }).ToList())
                            {
                                var isSoa = (from soas in db.SI_SOAS
                                             join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                             where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                             select new
                                             {
                                                 soas.SOA
                                             }).FirstOrDefault();

                                var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                                //var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERANNUL && z.DELETIONDATE == null).FirstOrDefault();
                                var isCount = x.COUNT;
                                DateTime? isDate = x.DATESIIG;
                                var isType = 0;
                                var isEtat = 4;

                                list.Add(new EVENEMENTS
                                {
                                    SOA = isSoa != null ? isSoa.SOA : "",
                                    PROJET = isProjet != null ? isProjet.PROJET : "",
                                    TYPE = isType,
                                    ETAT = isEtat,
                                    USER = x.USERSIIG != "" ? x.USERSIIG : "",
                                    COUNT = isCount,
                                    DATE = isDate.Value.Date,
                                });
                            }
                            foreach (var x in ava.Where(x => x.DATESIIG != null).GroupBy(x => new { x.IDPROJET, x.DATESIIG.Value.Date, x.USERSIIG }).Select(x => new { PROJET = x.FirstOrDefault().IDPROJET, DATESIIG = x.FirstOrDefault().DATESIIG.Value.Date, USERSIIG = x.FirstOrDefault().USERSIIG, COUNT = x.Count() }).ToList())
                            {
                                var isSoa = (from soas in db.SI_SOAS
                                             join prj in db.SI_PROSOA on soas.ID equals prj.IDSOA
                                             where prj.IDPROJET == x.PROJET && prj.DELETIONDATE == null && soas.DELETIONDATE == null
                                             select new
                                             {
                                                 soas.SOA
                                             }).FirstOrDefault();

                                var isProjet = db.SI_PROJETS.Where(a => a.ID == x.PROJET && a.DELETIONDATE == null).FirstOrDefault();
                                //var isUser = db.SI_USERS.Where(z => z.ID == x.IDUSERANNUL && z.DELETIONDATE == null).FirstOrDefault();
                                var isCount = x.COUNT;
                                DateTime? isDate = x.DATESIIG;
                                var isType = 3;
                                var isEtat = 4;

                                list.Add(new EVENEMENTS
                                {
                                    SOA = isSoa != null ? isSoa.SOA : "",
                                    PROJET = isProjet != null ? isProjet.PROJET : "",
                                    TYPE = isType,
                                    ETAT = isEtat,
                                    USER = x.USERSIIG != "" ? x.USERSIIG : "",
                                    COUNT = isCount,
                                    DATE = isDate.Value.Date,
                                });
                            }
                        }

                        return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = list }, settings));
                    }
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message, test = e.StackTrace }, settings));
            }
        }
    }
}
