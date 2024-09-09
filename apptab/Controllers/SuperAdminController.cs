using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Data.Entity;
using apptab.Models;

namespace apptab.Controllers
{
    public class SuperAdminController : Controller
    {
        private readonly SOFTCONNECTSIIG db = new SOFTCONNECTSIIG();

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        //PROJETS LISTE//
        public ActionResult ProjetList()
        {
            ViewBag.Controller = "Liste des PROJETS";

            return View();
        }

        public async Task<JsonResult> FillTable(SI_USERS suser)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                var societe = await db.SI_PROJETS.Where(x => x.DELETIONDATE == null).ToListAsync();

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = societe }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        //PROJETS CREATE//
        public ActionResult SuperAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> AddSociete(SI_USERS suser, SI_PROJETS societe, SI_USERS user)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            var societeExist = await db.SI_PROJETS.FirstOrDefaultAsync(a => a.PROJET == societe.PROJET && a.DELETIONDATE == null);

            if (societeExist == null)
            {
                var newSociete = new SI_PROJETS()
                {
                    PROJET = societe.PROJET
                };
                db.SI_PROJETS.Add(newSociete);
                //var eeee = db.GetValidationErrors();
                db.SaveChanges();

                //First ADMIN//
                int IDSOC = db.SI_PROJETS.FirstOrDefault(a => a.PROJET == societe.PROJET && a.DELETIONDATE == null).ID;
                var newFirstAdmin = new SI_USERS()
                {
                    LOGIN = user.LOGIN,
                    PWD = user.PWD,
                    ROLE = Role.Administrateur,// db.OPA_ROLES.Where(a => a.INTITULES == "Administrateur").FirstOrDefault().ID,
                    IDPROJET = IDSOC
                };
                db.SI_USERS.Add(newFirstAdmin);
                //var eeee = db.GetValidationErrors();
                db.SaveChanges();

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = societe }, settings));
            }
            else
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Projet déjà existant. " }, settings));
            }
        }

        //SOA LISTE//
        public ActionResult SOAList()
        {
            ViewBag.Controller = "Liste des PROJETS";
            return View();
        }

        public JsonResult FillTableSOA(SI_USERS suser)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                var societe = db.SI_SOAS.Select(a => new
                {
                    SOA = a.SOA,
                    ID = a.ID,
                    DELETIONDATE = a.DELETIONDATE
                }).Where(a => a.DELETIONDATE == null).ToList();

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = societe }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        //SOA CREATE//
        public ActionResult SuperAdminSOA()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddSocieteSOA(SI_USERS suser, SI_SOAS societe)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            var societeExist = db.SI_SOAS.FirstOrDefault(a => a.SOA == societe.SOA && a.DELETIONDATE == null);
            if (societeExist == null)
            {
                var newSociete = new SI_SOAS()
                {
                    SOA = societe.SOA
                };
                db.SI_SOAS.Add(newSociete);
                //var eeee = db.GetValidationErrors();
                db.SaveChanges();
                var Hsoas = db.SI_SOAS.Where(a => a.SOA == societe.SOA).FirstOrDefault();
                var HnewSociete = new HSI_SOAS()
                {
                    IDPARENT = Hsoas.ID,
                    IDUSER = exist.ID,
                    SOA = Hsoas.SOA,
                    DATECREA = DateTime.Now,

                };
                db.HSI_SOAS.Add(HnewSociete);
                db.SaveChanges();
                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = societe }, settings));
            }
            else
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = "SOA déjà existant. " }, settings));
            }
        }

        //CORRESPONDANCE PROJET - SOA LISTE//
        public ActionResult PROSOAList()
        {
            ViewBag.Controller = "Correspondance entre PROJETS - SOA";
            return View();
        }

        public JsonResult FillTablePROSOA(SI_USERS suser)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                var societe = db.SI_PROSOA.Select(a => new
                {
                    PROJET = db.SI_PROJETS.FirstOrDefault(x => x.ID == a.IDPROJET && a.DELETIONDATE == null).PROJET,
                    SOA = db.SI_SOAS.FirstOrDefault(x => x.ID == a.IDSOA).SOA,
                    ID = a.ID,
                    DELETIONDATE = a.DELETIONDATE
                }).Where(a => a.DELETIONDATE == null).ToList();

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = societe }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        //CORRESPONDANCE PROJET - SOA CREATE//
        public ActionResult SuperAdminPROSOA()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddSocietePROSOA(SI_USERS suser, SI_PROSOA societe)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            var Projet = db.SI_PROJETS.FirstOrDefault(a => a.ID == societe.IDPROJET && a.DELETIONDATE == null).ID;
            var Soa = db.SI_SOAS.FirstOrDefault(a => a.ID == societe.IDSOA && a.DELETIONDATE == null).ID;

            var societeExist = db.SI_PROSOA.FirstOrDefault(a => a.IDPROJET == Projet && a.DELETIONDATE == null/* || a.IDSOA == Soa*/);

            if (societeExist == null)
            {
                var newSociete = new SI_PROSOA()
                {
                    IDPROJET = Projet,
                    IDSOA = Soa,
                    CREATIONDATE = DateTime.Now,
                    IDUSER = exist.ID
                };
                db.SI_PROSOA.Add(newSociete);
                db.SaveChanges();

                var isElemH = db.SI_PROSOA.FirstOrDefault(a => a.IDPROJET == Projet && a.IDSOA == Soa && a.DELETIONDATE == null);
                var newSocieteH = new HSI_PROSOA()
                {
                    IDPROJET = isElemH.IDPROJET,
                    IDSOA = isElemH.IDSOA,
                    CREATIONDATE = isElemH.CREATIONDATE,
                    IDUSER = isElemH.IDUSER,
                    IDPARENT = isElemH.ID
                };
                db.HSI_PROSOA.Add(newSocieteH);
                db.SaveChanges();

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = societe }, settings));
            }
            else
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Correspondance déjà existante. " }, settings));
            }
        }

        //GET ALL PROJET//
        [HttpPost]
        public ActionResult GetAllPROJET(SI_USERS suser, string IDPROSOA)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            if (IDPROSOA != null)
            {
                int? PROSOAID = int.Parse(IDPROSOA);
                var idPro = db.SI_PROSOA.Where(a => a.ID == PROSOAID && a.DELETIONDATE == null).Select(a => a.IDPROJET).FirstOrDefault();
                var FProfet = db.SI_PROJETS.Where(a => a.ID != idPro && a.DELETIONDATE == null).Select(a => new
                {
                    PROJET = a.PROJET,
                    ID = a.ID

                }).ToList();
                var FprojetFirst = db.SI_PROJETS.Where(a => a.ID == idPro && a.DELETIONDATE == null).Select(a => new
                {
                    PROJET = a.PROJET,
                    ID = a.ID
                }).ToList();
                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = FprojetFirst, datas = FProfet }, settings));
            }
            else
            {
                var user = db.SI_PROJETS
                    .Where(a => a.DELETIONDATE == null).Select(a => new
                    {
                        PROJET = a.PROJET,
                        ID = a.ID
                    }).ToList();

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = user, datas = "" }, settings));
            }

        }

        //GET ALL SOA//
        [HttpPost]
        public ActionResult GetAllSOA(SI_USERS suser, string IDPROSOA)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            if (IDPROSOA != null)
            {
                int? PROSOAID = int.Parse(IDPROSOA);
                var idsoa = db.SI_PROSOA.Where(a => a.ID == PROSOAID && a.DELETIONDATE == null).Select(a => a.IDSOA).FirstOrDefault();
                var SOA = db.SI_SOAS.Where(x => x.ID != idsoa && x.DELETIONDATE == null).Select(a => new
                {
                    SOA = a.SOA,
                    ID = a.ID,
                    DELETIONDATE = a.DELETIONDATE
                }).ToList();

                var soa1 = db.SI_SOAS.Where(x => x.ID == idsoa && x.DELETIONDATE == null).Select(x => new
                {
                    SOA = x.SOA,
                    ID = x.ID,
                    DELETIONDATE = x.DELETIONDATE
                }).ToList();

                List<SI_SOAS> SOAf = new List<SI_SOAS>();
                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = soa1, datas = SOA }, settings));
            }
            else
            {
                var SOA = db.SI_SOAS.Where(x => x.DELETIONDATE == null).Select(a => new
                {
                    SOA = a.SOA,
                    ID = a.ID
                }).ToList();
                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = SOA }, settings));
            }

        }

        //MAPPAGE LISTE//
        public ActionResult SuperAdminMaPList()
        {
            return View();
        }

        [HttpPost]
        public JsonResult FillTableMAPP(SI_USERS suser)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                var mapp = db.SI_MAPPAGES.Select(a => new
                {
                    db.SI_PROJETS.FirstOrDefault(x => x.ID == a.IDPROJET && a.DELETIONDATE == null).PROJET,
                    a.INSTANCE,
                    a.DBASE,
                    a.ID
                }).ToList();

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = mapp }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        //MAPPAGE DELETE//
        [HttpPost]
        public JsonResult DeleteMAPP(SI_USERS suser, string MAPPId)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int useID = int.Parse(MAPPId);
                var user = db.SI_MAPPAGES.FirstOrDefault(a => a.ID == useID);
                if (user != null)
                {
                    db.SI_MAPPAGES.Remove(user);
                    db.SaveChanges();
                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Suppression avec succès. " }, settings));
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

        //MAPPAGE DETAILS//
        public ActionResult DetailsMAPP(string UserId)
        {
            return View();
        }
        [HttpPost]
        public ActionResult DetailsMAPP(SI_USERS suser, string UserId)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int useID = int.Parse(UserId);
                var map = db.SI_MAPPAGES.FirstOrDefault(a => a.ID == useID);

                if (map != null)
                {
                    var mapp = new
                    {
                        soc = db.SI_PROJETS.FirstOrDefault(a => a.ID == map.IDPROJET).ID,
                        inst = map.INSTANCE,
                        auth = map.AUTH,
                        conn = map.CONNEXION,
                        mdp = map.CONNEXPWD,
                        baseD = map.DBASE,
                        id = map.ID
                    };

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = new { PROJET = mapp.soc, INSTANCE = mapp.inst, AUTH = mapp.auth, CONNEXION = mapp.conn, MDP = mapp.mdp, BASED = mapp.baseD, mapp.id } }, settings));
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

        //MAPPAGE CREATE//
        public ActionResult SuperAdminMaPCreate()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SuperAdminMaPCreate(SI_USERS suser, SI_MAPPAGES user)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                var userExist = db.SI_MAPPAGES.FirstOrDefault(a => a.IDPROJET == user.IDPROJET && a.INSTANCE == user.INSTANCE && a.DBASE == user.DBASE);
                if (userExist == null)
                {
                    var newUser = new SI_MAPPAGES()
                    {
                        INSTANCE = user.INSTANCE,
                        AUTH = user.AUTH,
                        CONNEXION = user.CONNEXION,
                        CONNEXPWD = user.CONNEXPWD,
                        DBASE = user.DBASE,
                        IDPROJET = user.IDPROJET,
                        CREATIONDATE = DateTime.Now,
                        IDUSER = exist.ID
                    };
                    db.SI_MAPPAGES.Add(newUser);
                    //var eeee = db.GetValidationErrors();
                    db.SaveChanges();

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = user }, settings));
                }
                else
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Le mappage existe déjà. " }, settings));
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        //GET ALL INSTANCE//
        [HttpPost]
        public ActionResult GetNewInstance(SI_USERS suser, SI_MAPPAGES map)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            //Get all bases with the instance
            var BaseList = new List<string>();

            try
            {
                SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder();
                connection.DataSource = map.INSTANCE;
                if (map.CONNEXION != null) connection.UserID = map.CONNEXION;
                if (map.CONNEXPWD != null) connection.Password = map.CONNEXPWD;

                if (map.AUTH != 1)
                    connection.IntegratedSecurity = true;
                else
                    connection.TrustServerCertificate = true;

                String strConn = connection.ToString();
                SqlConnection sqlConn = new SqlConnection(strConn);
                try
                {
                    sqlConn.Open();
                    DataTable tblDatabases = sqlConn.GetSchema("Databases");
                    sqlConn.Close();
                    foreach (DataRow row in tblDatabases.Rows)
                    {
                        String strDatabaseName = row["database_name"].ToString();
                        BaseList.Add(strDatabaseName);
                    }
                    BaseList.Sort();

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = BaseList }, settings));
                }
                catch (Exception)
                {
                    if (map.AUTH == 1) return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Échec de l'ouverture de session de l'utilisateur 'sa'. Vérifiez vos identifiants pour la connexion à SQL Server. " }, settings));
                    else return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Une erreur liée au réseau ou spécifique à l'instance s'est produite lors de l'établissement d'une connexion à SQL Server. Le serveur est introuvable ou n'est pas accessible. Vérifiez que le nom de l'instance est correct et que SQL Server est configuré pour autoriser les connexions distantes. " }, settings));
                }
            }
            catch (Exception)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Une erreur liée au réseau ou spécifique à l'instance s'est produite lors de l'établissement d'une connexion à SQL Server. Le serveur est introuvable ou n'est pas accessible. Vérifiez que le nom de l'instance est correct et que SQL Server est configuré pour autoriser les connexions distantes. " }, settings));
            }
        }

        //MAPPAGE UPDATE//
        [HttpPost]
        public JsonResult SuperAdminMaPUpdate(SI_USERS suser, SI_MAPPAGES user, string UserId)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int userId = int.Parse(UserId);
                var userExist = db.SI_MAPPAGES.FirstOrDefault(a => a.IDPROJET == user.IDPROJET && a.INSTANCE == user.INSTANCE && a.DBASE == user.DBASE);
                var userupdate = db.SI_MAPPAGES.FirstOrDefault(a => a.ID == userId);
                if (userExist == null)
                {
                    userupdate.INSTANCE = user.INSTANCE;
                    userupdate.AUTH = user.AUTH;
                    userupdate.CONNEXION = user.CONNEXION;
                    userupdate.CONNEXPWD = user.CONNEXPWD;
                    userupdate.DBASE = user.DBASE;
                    userupdate.IDPROJET = user.IDPROJET;
                    userupdate.IDUSER = suser.ID;

                    //var eeee = db.GetValidationErrors();
                    db.SaveChanges();

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = user }, settings));
                }
                else
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Le mappage existe déjà. " }, settings));
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }
        //DELETE SOA
        [HttpPost]
        public JsonResult DeleteFSOA(SI_USERS suser, string SOAid)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int IDSOA = int.Parse(SOAid);
                var SOA = db.SI_SOAS.FirstOrDefault(a => a.ID == IDSOA && a.DELETIONDATE == null);
                var ProjSoa = db.SI_PROSOA.Where(F_ProjetSoa => F_ProjetSoa.IDSOA == IDSOA && F_ProjetSoa.DELETIONDATE == null).Select(F_ProjetSoa => F_ProjetSoa.IDSOA).ToList();
                if (SOA != null)
                {
                    SOA.DELETIONDATE = DateTime.Now;
                    if (ProjSoa != null)
                    {
                        foreach (var p in ProjSoa)
                        {
                            var F_del = db.SI_PROSOA.Where(F_remSoa => F_remSoa.IDSOA == p && F_remSoa.DELETIONDATE == null).FirstOrDefault();
                            F_del.DELETIONDATE = DateTime.Now;
                            db.SaveChanges();
                        }
                    }

                    var Hsoas = new HSI_SOAS()
                    {
                        IDPARENT = SOA.ID,
                        IDUSER = exist.ID,
                        DELETIONDATE = DateTime.Now,
                        SOA = SOA.SOA,

                    };
                    db.HSI_SOAS.Add(Hsoas);
                    db.SaveChanges();
                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Suppression avec succès." }, settings));
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
        [HttpGet]
        public ActionResult SuperAdminDetailFSOA(SI_USERS suser, string SOAid)
        {
            return View();
        }
        [HttpPost]
        public ActionResult DetailsFSOA(SI_USERS suser, string SOAID)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int IDSOA = int.Parse(SOAID);
                var soa = db.SI_SOAS.FirstOrDefault(a => a.ID == IDSOA && a.DELETIONDATE == null);

                if (soa != null)
                {
                    var soas = new
                    {
                        soa = soa.SOA
                    };

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Liste des SOA", data = soas.soa }, settings));
                }
                else
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Erreur de connexion" }, settings));
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }
        public ActionResult UpdatFSOA(SI_USERS suser, string SOAID, string SOAID_2)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int IDSOA = int.Parse(SOAID);
                var SOAEXIST = db.SI_SOAS.Where(soaid => soaid.ID == IDSOA && soaid.DELETIONDATE == null).FirstOrDefault();
                //var SOAupdate = db.SI_SOAS.FirstOrDefault(soaid => soaid.ID == IDSOA);
                DateTime dt = DateTime.Now;

                if (SOAEXIST != null)
                {
                    if (SOAEXIST.SOA != SOAID_2)
                    {
                        SOAEXIST.DELETIONDATE = dt;
                        db.SI_SOAS.Add(new SI_SOAS
                        { // SOAupdate.SOA = SOAID_2;
                            SOA = SOAID_2
                        });
                        var tt = db.HSI_SOAS.Where(a => a.IDPARENT == SOAEXIST.ID && a.DELETIONDATE == null).FirstOrDefault();
                        if (tt != null)
                        {
                            tt.DELETIONDATE = dt;
                            var HSoas = new HSI_SOAS()
                            {
                                IDUSER = exist.ID,
                                SOA = SOAID_2,
                                IDPARENT = SOAEXIST.ID,
                            };
                            db.HSI_SOAS.Add(HSoas);
                        }
                        else
                        {
                            var HSoas = new HSI_SOAS()
                            {
                                IDUSER = exist.ID,
                                SOA = SOAID_2,
                                DATECREA = dt,
                                IDPARENT = SOAEXIST.ID,
                            };
                            db.HSI_SOAS.Add(HSoas);
                        }

                        db.SaveChanges();
                        return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = SOAID_2 }, settings));
                    }
                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = SOAID_2 }, settings));
                }
                else
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "La correspondance existe déjà. " }, settings));
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }
        public ActionResult DeleteFPROSOA(SI_USERS suser, string PROSOAID)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int IDPROSOA = int.Parse(PROSOAID);
                var PROSOA = db.SI_PROSOA.FirstOrDefault(a => a.ID == IDPROSOA && a.DELETIONDATE == null);

                var elemH = db.HSI_PROSOA.FirstOrDefault(a => a.IDPARENT == IDPROSOA && a.DELETIONDATE == null);

                if (PROSOA != null)
                {
                    PROSOA.DELETIONDATE = DateTime.Now;
                    PROSOA.IDUSERDEL = exist.ID;

                    if (elemH != null)
                    {
                        elemH.DELETIONDATE = DateTime.Now;
                    }

                    db.SaveChanges();

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Suppression avec succès. " }, settings));
                }
                else
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Erreur de Supression" }, settings));
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }
        public ActionResult SuperAdminDetailFPROSOA(SI_USERS suser, string PROSOAID)
        {
            return View();
        }
        public JsonResult UpdateFPROSOA(SI_USERS suser, SI_PROSOA societe, string idprosoaUp)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            int idUp = int.Parse(idprosoaUp);
            var Projet = db.SI_PROJETS.FirstOrDefault(a => a.ID == societe.IDPROJET && a.DELETIONDATE == null).ID;
            var Soa = db.SI_SOAS.FirstOrDefault(a => a.ID == societe.IDSOA && a.DELETIONDATE == null).ID;

            var CorrespondanceExist = db.SI_PROSOA.FirstOrDefault(a => a.ID == idUp && a.DELETIONDATE == null);

            var CorrespondanceExistH = db.HSI_PROSOA.FirstOrDefault(a => a.IDPARENT == idUp && a.DELETIONDATE == null);

            if (CorrespondanceExist != null)
            {
                if (CorrespondanceExist.IDPROJET != Projet || CorrespondanceExist.IDSOA != Soa)
                {
                    CorrespondanceExist.IDPROJET = Projet;
                    CorrespondanceExist.IDSOA = Soa;
                    db.SaveChanges();
                }

                if (CorrespondanceExistH != null)
                {
                    CorrespondanceExistH.DELETIONDATE = DateTime.Now;
                    db.SaveChanges();
                }

                var isElemH = db.SI_PROSOA.FirstOrDefault(a => a.IDPROJET == Projet && a.IDSOA == Soa && a.DELETIONDATE == null);
                var newSocieteH = new HSI_PROSOA()
                {
                    IDPROJET = isElemH.IDPROJET,
                    IDSOA = isElemH.IDSOA,
                    CREATIONDATE = DateTime.Now,
                    IDUSER = exist.ID,
                    IDPARENT = isElemH.ID
                };
                db.HSI_PROSOA.Add(newSocieteH);
                db.SaveChanges();

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = societe }, settings));
            }
            else
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Correspondance déjà existante. " }, settings));
            }
        }

        //Mailing//
        public ActionResult MailCreate()
        {
            ViewBag.Controller = "Paramétrage des mails (Notifications et Alertes)";

            return View();
        }

        [HttpPost]
        public ActionResult DetailsMail(SI_USERS suser, int iProjet)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = iProjet;

                var crpto = db.SI_MAIL.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null);
                if (crpto != null)
                {
                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = crpto }, settings));
                }
                else
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Veuillez créer une nouvelle liste de mail. " }, settings));
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        [HttpPost]
        public JsonResult UpdateMail(SI_USERS suser, SI_MAIL param, int iProjet)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            //Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            //var canCreate = true;
            //string[] separators = { ";" };

            bool MAILTE = new Data.Extension().TestMail(param.MAILTE);
            bool MAILTV = new Data.Extension().TestMail(param.MAILTV);
            bool MAILSIIG = new Data.Extension().TestMail(param.MAILSIIG);
            bool MAILPI = new Data.Extension().TestMail(param.MAILPI);
            bool MAILPE = new Data.Extension().TestMail(param.MAILPE);
            bool MAILPV = new Data.Extension().TestMail(param.MAILPV);
            bool MAILPP = new Data.Extension().TestMail(param.MAILPP);
            bool MAILREJET = new Data.Extension().TestMail(param.MAILREJET);
            bool MAILREJETPAIE = new Data.Extension().TestMail(param.MAILREJETPAIE);

            bool MAILTEA = new Data.Extension().TestMail(param.MAILTEA);
            bool MAILTVA = new Data.Extension().TestMail(param.MAILTVA);
            bool MAILSIIGA = new Data.Extension().TestMail(param.MAILSIIGA); 
            bool MAILREJETA = new Data.Extension().TestMail(param.MAILREJETA);

            //bool MAILPB = new Extension().TestMail(param.MAILPB);

            if (MAILTE == false || MAILTV == false || MAILSIIG == false || MAILTEA == false || MAILTVA == false || MAILSIIGA == false
                || MAILREJET == false || MAILREJETPAIE == false || MAILREJETA == false
                || MAILPI == false || MAILPE == false || MAILPV == false || MAILPP == false/* || MAILPB == false*/)
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = "L'une des adresses mail renseignée n'est pas valide. " }, settings));

            try
            {
                int IdS = iProjet;
                var SExist = db.SI_MAIL.FirstOrDefault(a => a.IDPROJET == IdS && a.DELETIONDATE == null);

                if (SExist != null)
                {
                    if (SExist.MAILTE != param.MAILTE || SExist.MAILTV != param.MAILTV || SExist.MAILSIIG != param.MAILSIIG || SExist.MAILTEA != param.MAILTEA || SExist.MAILTVA != param.MAILTVA || SExist.MAILSIIGA != param.MAILSIIGA
                        || SExist.MAILREJET != param.MAILREJET || SExist.MAILREJETA != param.MAILREJETA
                        || SExist.MAILPI != param.MAILPI || SExist.MAILPE != param.MAILPE || SExist.MAILPV != param.MAILPV || SExist.MAILPP != param.MAILPP || SExist.MAILPB != param.MAILPB)
                    {
                        SExist.MAILTE = param.MAILTE;
                        SExist.MAILTV = param.MAILTV;
                        SExist.MAILSIIG = param.MAILSIIG;
                        SExist.MAILPI = param.MAILPI;
                        SExist.MAILPE = param.MAILPE;
                        SExist.MAILPV = param.MAILPV;
                        SExist.MAILPP = param.MAILPP;
                        SExist.MAILREJET = param.MAILREJET;
                        SExist.MAILREJETPAIE = param.MAILREJETPAIE;

                        SExist.MAILTEA = param.MAILTEA;
                        SExist.MAILTVA = param.MAILTVA;
                        SExist.MAILSIIGA = param.MAILSIIGA;
                        SExist.MAILREJETA = param.MAILREJETA;

                        db.SaveChanges();

                        var H = db.HSI_MAIL.FirstOrDefault(a => a.IDPARENT == SExist.ID && a.DELETIONDATE == null);
                        if (H != null)
                        {
                            H.DELETIONDATE = DateTime.Now;
                            db.SaveChanges();
                        }

                        var newElemH = new HSI_MAIL()
                        {
                            MAILTE = param.MAILTE,
                            MAILTV = param.MAILTV,
                            MAILSIIG = param.MAILSIIG,
                            MAILPI = param.MAILPI,
                            MAILPE = param.MAILPE,
                            MAILPV = param.MAILPV,
                            MAILPP = param.MAILPP,
                            MAILREJET = param.MAILREJET,
                            MAILREJETPAIE = param.MAILREJETPAIE,
                            IDPROJET = IdS,
                            CREATIONDATE = DateTime.Now,
                            IDUSER = exist.ID,
                            IDPARENT = SExist.ID,

                            MAILTEA = param.MAILTEA,
                            MAILTVA = param.MAILTVA,
                            MAILSIIGA = param.MAILSIIGA,
                            MAILREJETA = param.MAILREJETA,
                        };
                        db.HSI_MAIL.Add(newElemH);
                        db.SaveChanges();
                    }

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = param }, settings));
                }
                else
                {
                    var newPara = new SI_MAIL()
                    {
                        MAILTE = param.MAILTE,
                        MAILTV = param.MAILTV,
                        MAILSIIG = param.MAILSIIG,
                        MAILPI = param.MAILPI,
                        MAILPE = param.MAILPE,
                        MAILPV = param.MAILPV,
                        MAILPP = param.MAILPP,
                        MAILREJET = param.MAILREJET,
                        MAILREJETPAIE = param.MAILREJETPAIE,
                        IDPROJET = IdS,
                        CREATIONDATE = DateTime.Now,
                        IDUSER = exist.ID,

                        MAILTEA = param.MAILTEA,
                        MAILTVA = param.MAILTVA,
                        MAILSIIGA = param.MAILSIIGA,
                        MAILREJETA = param.MAILREJETA,
                    };

                    db.SI_MAIL.Add(newPara);
                    db.SaveChanges();

                    var isElemH = db.SI_MAIL.FirstOrDefault(a => a.IDPROJET == IdS && a.MAILTE == param.MAILTE && a.MAILTV == param.MAILTV  && a.MAILSIIG == param.MAILSIIG && a.MAILREJET == param.MAILREJET
                    && a.MAILTEA == param.MAILTEA && a.MAILTVA == param.MAILTVA && a.MAILSIIGA == param.MAILSIIGA && a.MAILREJETA == param.MAILREJETA
                    && a.MAILPE == param.MAILPE && a.MAILPV == param.MAILPV && a.MAILPP == param.MAILPP && a.MAILPI == param.MAILPI && a.DELETIONDATE == null);
                    var newElemH = new HSI_MAIL()
                    {
                        MAILTE = isElemH.MAILTE,
                        MAILTV = isElemH.MAILTV,
                        MAILSIIG = isElemH.MAILSIIG,
                        MAILPI = isElemH.MAILPI,
                        MAILPE = isElemH.MAILPE,
                        MAILPV = isElemH.MAILPV,
                        MAILPP = isElemH.MAILPP,
                        MAILREJET = param.MAILREJET,
                        MAILREJETPAIE = param.MAILREJETPAIE,
                        IDPROJET = IdS,
                        CREATIONDATE = isElemH.CREATIONDATE,
                        IDUSER = isElemH.IDUSER,
                        IDPARENT = isElemH.ID,

                        MAILTEA = param.MAILTEA,
                        MAILTVA = param.MAILTVA,
                        MAILSIIGA = param.MAILSIIGA,
                        MAILREJETA = param.MAILREJETA,
                    };
                    db.HSI_MAIL.Add(newElemH);
                    db.SaveChanges();

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = param }, settings));
                }
            }
            catch (Exception)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Erreur d'enregistrement de l'information. " }, settings));
            }
        }

        //Délais de traitement//
        public ActionResult DelaisCreate()
        {
            ViewBag.Controller = "Paramétrage des délais de traitement";

            return View();
        }

        [HttpPost]
        public ActionResult DetailsDelais(SI_USERS suser, int iProjet)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = iProjet;

                var crpto = db.SI_DELAISTRAITEMENT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null);
                if (crpto != null)
                {
                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = crpto }, settings));
                }
                else
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Veuillez créer les délais de traitement. " }, settings));
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        [HttpPost]
        public JsonResult UpdateDelais(SI_USERS suser, SI_DELAISTRAITEMENT param, int iProjet)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int IdS = iProjet;
                var SExist = db.SI_DELAISTRAITEMENT.FirstOrDefault(a => a.IDPROJET == IdS && a.DELETIONDATE == null);

                if (SExist != null)
                {
                    if (SExist.DELTV != param.DELTV || SExist.DELSIIGFP != param.DELSIIGFP || SExist.DELENVOISIIGFP != param.DELENVOISIIGFP || SExist.DELRAF != param.DELRAF
                        || SExist.DELAV != param.DELAV || SExist.DELASIIGFP != param.DELASIIGFP || SExist.DELAENVOISIIGFP != param.DELAENVOISIIGFP || SExist.DELARAF != param.DELARAF
                        || SExist.DELPE != param.DELPE || SExist.DELPV != param.DELPV || SExist.DELPP != param.DELPP || SExist.DELPB != param.DELPB)
                    {
                        SExist.DELRAF = param.DELRAF;//Tris par le RAF
                        SExist.DELTV = param.DELTV;//Validation mandat
                        SExist.DELENVOISIIGFP = param.DELENVOISIIGFP;//Transfert SIIGFP
                        SExist.DELSIIGFP = param.DELSIIGFP;//Traitement SIIGFP
                        SExist.DELPE = param.DELPE;//ENVOI POUR VALIDATION PAIEMENT
                        SExist.DELPV = param.DELPV;//VALIDATION PAIEMENT
                        SExist.DELPP = param.DELPP;//PAIEMENT
                        SExist.DELPB = param.DELPB;//TRAITEMENT BANQUE

                        SExist.DELARAF = param.DELARAF;//Tris par le RAF AVANCE
                        SExist.DELAV = param.DELAV;//Validation mandat AVANCE
                        SExist.DELAENVOISIIGFP = param.DELAENVOISIIGFP;//Transfert SIIGFP AVANCE
                        SExist.DELASIIGFP = param.DELASIIGFP;//Traitement SIIGFP AVANCE

                        db.SaveChanges();

                        var H = db.HSI_DELAISTRAITEMENT.FirstOrDefault(a => a.IDPARENT == SExist.ID && a.DELETIONDATE == null);
                        if (H != null)
                        {
                            H.DELETIONDATE = DateTime.Now;
                            db.SaveChanges();
                        }

                        var newElemH = new HSI_DELAISTRAITEMENT()
                        {
                            DELRAF = param.DELRAF,
                            DELTV = param.DELTV,
                            DELENVOISIIGFP = param.DELENVOISIIGFP,
                            DELSIIGFP = param.DELSIIGFP,
                            DELPE = param.DELPE,
                            DELPV = param.DELPV,
                            DELPP = param.DELPP,
                            DELPB = param.DELPB,
                            IDPROJET = IdS,
                            CREATIONDATE = DateTime.Now,
                            IDUSER = exist.ID,
                            IDPARENT = SExist.ID,

                            DELARAF = param.DELARAF,
                            DELAV = param.DELAV,
                            DELAENVOISIIGFP = param.DELAENVOISIIGFP,
                            DELASIIGFP = param.DELASIIGFP,
                        };
                        db.HSI_DELAISTRAITEMENT.Add(newElemH);
                        db.SaveChanges();
                    }

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = param }, settings));
                }
                else
                {
                    var newPara = new SI_DELAISTRAITEMENT()
                    {
                        DELRAF = param.DELRAF,
                        DELTV = param.DELTV,
                        DELENVOISIIGFP = param.DELENVOISIIGFP,
                        DELSIIGFP = param.DELSIIGFP,
                        DELPE = param.DELPE,
                        DELPV = param.DELPV,
                        DELPP = param.DELPP,
                        DELPB = param.DELPB,
                        IDPROJET = IdS,
                        CREATIONDATE = DateTime.Now,
                        IDUSER = exist.ID,

                        DELARAF = param.DELARAF,
                        DELAV = param.DELAV,
                        DELAENVOISIIGFP = param.DELAENVOISIIGFP,
                        DELASIIGFP = param.DELASIIGFP,
                    };

                    db.SI_DELAISTRAITEMENT.Add(newPara);
                    db.SaveChanges();

                    var isElemH = db.SI_DELAISTRAITEMENT.FirstOrDefault(a => a.IDPROJET == IdS && a.DELTV == param.DELTV && a.DELSIIGFP == param.DELSIIGFP && a.DELPE == param.DELPE
                    && a.DELPV == param.DELPV && a.DELPP == param.DELPP && a.DELPB == param.DELPB && a.DELETIONDATE == null);
                    var newElemH = new HSI_DELAISTRAITEMENT()
                    {
                        DELRAF = param.DELRAF,
                        DELTV = isElemH.DELTV,
                        DELENVOISIIGFP = isElemH.DELENVOISIIGFP,
                        DELSIIGFP = isElemH.DELSIIGFP,
                        DELPE = isElemH.DELPE,
                        DELPV = isElemH.DELPV,
                        DELPP = isElemH.DELPP,
                        DELPB = isElemH.DELPB,
                        IDPROJET = IdS,
                        CREATIONDATE = isElemH.CREATIONDATE,
                        IDUSER = isElemH.IDUSER,
                        IDPARENT = isElemH.ID,

                        DELARAF = param.DELARAF,
                        DELAV = param.DELAV,
                        DELAENVOISIIGFP = param.DELAENVOISIIGFP,
                        DELASIIGFP = param.DELASIIGFP,
                    };
                    db.HSI_DELAISTRAITEMENT.Add(newElemH);
                    db.SaveChanges();

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = param }, settings));
                }
            }
            catch (Exception)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Erreur d'enregistrement de l'information. " }, settings));
            }
        }

        //Menu Dynamique//
        public ActionResult MenuCreate()
        {
            ViewBag.Controller = "Paramétrage des intitulés des menus";

            return View();
        }

        [HttpPost]
        public ActionResult DetailsMenu(SI_USERS suser)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = exist.IDPROJET.Value;
                var crpto = db.SI_MENU.FirstOrDefault();
                if (crpto != null)
                {
                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = crpto }, settings));
                }
                else
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Veuillez créer les intitulés des menus. " }, settings));
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        [HttpPost]
        public JsonResult UpdateMenu(SI_USERS suser, SI_MENU param)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                var SExist = db.SI_MENU.FirstOrDefault();

                if (SExist != null)
                {
                    if (SExist.MTNON != param.MTNON || SExist.MT0 != param.MT0 || SExist.MT1 != param.MT1 || SExist.MT2 != param.MT2
                        || SExist.MD0 != param.MD0 || SExist.MD1 != param.MD1 || SExist.MD2 != param.MD2 || SExist.MD3 != param.MD3
                        || SExist.MOP0 != param.MOP0 || SExist.MOP1 != param.MOP1 || SExist.MOP2 != param.MOP2
                        || SExist.MP1 != param.MP1 || SExist.MP2 != param.MP2 || SExist.MP3 != param.MP3 || SExist.MP4 != param.MP4
                        || SExist.TDB0 != param.TDB0 || SExist.TDB1 != param.TDB1 || SExist.TDB2 != param.TDB2 || SExist.TDB3 != param.TDB3 || SExist.TDB4 != param.TDB4
                        || SExist.TDB5 != param.TDB5 || SExist.TDB6 != param.TDB6 || SExist.TDB7 != param.TDB7 || SExist.TDB8 != param.TDB8)
                    {
                        SExist.MTNON = param.MTNON;
                        SExist.MT0 = param.MT0;
                        SExist.MT1 = param.MT1;
                        SExist.MT2 = param.MT2;
                        SExist.MT2 = param.MT2;
                        SExist.MP1 = param.MP1;
                        SExist.MP2 = param.MP2;
                        SExist.MP3 = param.MP3;
                        SExist.MP4 = param.MP4;

                        SExist.MD0 = param.MD0;
                        SExist.MD1 = param.MD1;
                        SExist.MD2 = param.MD2;
                        SExist.MD3 = param.MD3;

                        SExist.MOP0 = param.MOP0;
                        SExist.MOP1 = param.MOP1;
                        SExist.MOP2 = param.MOP2;

                        SExist.TDB0 = param.TDB0;
                        SExist.TDB1 = param.TDB1;
                        SExist.TDB2 = param.TDB2;
                        SExist.TDB3 = param.TDB3;
                        SExist.TDB4 = param.TDB4;
                        SExist.TDB5 = param.TDB5;
                        SExist.TDB6 = param.TDB6;
                        SExist.TDB7 = param.TDB7;
                        SExist.TDB8 = param.TDB8;

                        db.SaveChanges();
                    }

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = param }, settings));
                }
                else
                {
                    var newPara = new SI_MENU()
                    {
                        MTNON = param.MTNON,
                        MT0 = param.MT0,
                        MT1 = param.MT1,
                        MT2 = param.MT2,
                        MP1 = param.MP1,
                        MP2 = param.MP2,
                        MP3 = param.MP3,
                        MP4 = param.MP4,

                        MD0 = param.MD0,
                        MD1 = param.MD1,
                        MD2 = param.MD2,
                        MD3 = param.MD3,

                        MOP0 = param.MOP0,
                        MOP1 = param.MOP1,
                        MOP2 = param.MOP2,

                        TDB0 = param.TDB0,
                        TDB1 = param.TDB1,
                        TDB2 = param.TDB2,
                        TDB3 = param.TDB3,
                        TDB4 = param.TDB4,
                        TDB5 = param.TDB5,
                        TDB6 = param.TDB6,
                        TDB7 = param.TDB7,
                        TDB8 = param.TDB8,

                        CREATIONDATE = DateTime.Now
                    };

                    db.SI_MENU.Add(newPara);
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
