using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using apptab;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using apptab.Data.Entities;
using System.Data.Entity;

namespace SOFTCONNECT.Controllers
{
    public class UserController : Controller
    {
        private readonly SOFTCONNECTSIIG db = new SOFTCONNECTSIIG();

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        //// GET: User
        //[Route("UserList")]
        //[HttpGet]
        public ActionResult List()
        {
            ViewBag.Controller = "Liste des Utilisateurs";
            return View();
        }

        [HttpPost]
        public JsonResult FillTable(SI_USERS suser)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null);
            ViewBag.Role = exist.ROLE;
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                var test = db.SI_USERS.Where(x => x.ROLE == exist.ROLE && x.IDPROJET == exist.IDPROJET && x.DELETIONDATE == null).FirstOrDefault();
                //var test = db.SI_USERS.Where(x => x.ROLE == suser.ROLE && x.IDPROJET == suser.IDPROJET).FirstOrDefault();
                if (test.ROLE == (int)Role.SAdministrateur)
                {
                    var users = db.SI_USERS.Where(x => x.ROLE != Role.SAdministrateur).Select(a => new
                    {
                        a.LOGIN,
                        a.PWD,
                        ROLE = a.ROLE.ToString(),
                        ID = a.ID,
                        PROJET = a.IDPROJET == 0 ? "MULTIPLES" : db.SI_PROJETS.Where(z => z.ID == a.IDPROJET && z.DELETIONDATE == null).FirstOrDefault().PROJET,
                        DELETONDATE = a.DELETIONDATE,
                        //STAT = a.DELETIONDATE == null ? "ACTIF" : "INACTIF",
                        CREAT = a.CREATIONDATE
                    }).Where(a => a.PROJET != null && a.DELETONDATE == null).OrderBy(a => a.PROJET).OrderBy(a => a.CREAT).ToList();
                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = users }, settings));
                }
                else
                {
                    var users = db.SI_USERS.Where(x => x.ROLE != Role.SAdministrateur && x.ROLE != Role.Organe_de_Suivi && x.ROLE != Role.Agent_Comptable && x.IDPROJET == exist.IDPROJET && x.DELETIONDATE == null).Select(a => new
                    {
                        a.LOGIN,
                        a.PWD,
                        ROLE = a.ROLE.ToString(),
                        ID = a.ID,
                        PROJET = db.SI_PROJETS.Where(z => z.ID == exist.IDPROJET && z.DELETIONDATE == null).FirstOrDefault().PROJET,
                        DELETONDATE = a.DELETIONDATE,
                        //STAT = "ACTIF",
                        CREAT = a.CREATIONDATE
                    }).OrderBy(a => a.CREAT).Where(a => a.DELETONDATE == null).ToList();
                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = users }, settings));
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        [HttpPost]
        public async Task<JsonResult> Password(UserPassword userPassword)
        {
            var connectedUser = await db.SI_USERS.FirstOrDefaultAsync(
                a => a.LOGIN == userPassword.LoginName && a.PWD == userPassword.Password && a.DELETIONDATE == null && (a.ROLE == Role.SAdministrateur || a.ROLE == Role.Administrateur)
            );

            if (connectedUser == null)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = "" }, settings));
            }

            var res = await db.SI_USERS.FirstOrDefaultAsync(user => user.ID == userPassword.UserId);

            return Json(JsonConvert.SerializeObject(new
            {
                type = "success",
                msg = "Connexion avec succès. ",
                data = new
                {
                    login = res.LOGIN,
                    password = res.PWD
                }
            }, settings));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetAllRole(SI_USERS suser)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDPROJET == suser.IDPROJET*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            var enumlist = Enum.GetValues(typeof(Role));

            var roles = new Dictionary<int, string>();

            if (exist.ROLE != Role.SAdministrateur)
            {
                foreach (var item in enumlist)
                {
                    if (item.ToString() != "SAdministrateur" && item.ToString() != "Organe_de_Suivi" && item.ToString() != "Agent_Comptable")
                        roles.Add((int)item, Enum.GetName(typeof(Role), item));
                }
            }
            else
            {
                foreach (var item in enumlist)
                {
                    //roles.Add((int)item, Enum.GetName(typeof(Role), item));
                    if (item.ToString() != "SAdministrateur")
                        roles.Add((int)item, Enum.GetName(typeof(Role), item));
                }
            }
            return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = roles }, settings));
        }

        //GET ALL PROJET//
        [HttpPost]
        public ActionResult GetAllPROJET(SI_USERS suser)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDPROJET == suser.IDPROJET*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            var user = db.SI_PROJETS.Select(a => new
            {
                PROJET = a.PROJET,
                ID = a.ID,
                DELETIONDATE = a.DELETIONDATE,
            }).Where(a => a.DELETIONDATE == null).ToList();

            return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = user }, settings));
        }

        [HttpPost]
        public JsonResult AddUser(SI_USERS suser, SI_USERS user, string listProjet)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDPROJET == suser.IDPROJET*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                var userExist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == user.LOGIN && a.DELETIONDATE == null/* && a.IDPROJET == exist.IDPROJET*/);
                var test = db.SI_USERS.Where(x => x.ROLE == exist.ROLE && x.IDPROJET == exist.IDPROJET && x.DELETIONDATE == null).FirstOrDefault();
                if (userExist == null)
                {
                    if (test.ROLE == Role.SAdministrateur)
                    {
                        if (listProjet == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Veuillez sélectionner au moins un projet. " }, settings));

                        if (user.ROLE == Role.Administrateur || user.ROLE == Role.Autre)
                        {
                            int TestProjetRole = 0;
                            if (!int.TryParse(listProjet, out TestProjetRole))
                                return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Vous ne pouvez pas affecter plusieurs projets à ce type d'utilisateur. " }, settings));

                            var newUser = new SI_USERS()
                            {
                                LOGIN = user.LOGIN,
                                PWD = user.PWD,
                                IDPROJET = int.Parse(listProjet),
                                ROLE = user.ROLE,
                                CREATIONDATE = DateTime.Now,
                                IDUSER = exist.ID
                            };
                            db.SI_USERS.Add(newUser);
                            db.SaveChanges();
                        }
                        else
                        {
                            var newUser = new SI_USERS()
                            {
                                LOGIN = user.LOGIN,
                                PWD = user.PWD,
                                IDPROJET = 0,
                                ROLE = user.ROLE,
                                CREATIONDATE = DateTime.Now,
                                IDUSER = exist.ID
                            };
                            db.SI_USERS.Add(newUser);
                            db.SaveChanges();

                            var userExistTest = db.SI_USERS.FirstOrDefault(a => a.LOGIN == user.LOGIN && a.PWD == user.PWD && a.IDPROJET == 0 && a.ROLE == user.ROLE && a.DELETIONDATE == null/* && a.IDPROJET == exist.IDPROJET*/);

                            string[] separators = { "," };
                            var pro = listProjet;
                            if (pro != null)
                            {
                                string listUser = pro.ToString();
                                string[] lst = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                foreach (var a in lst)
                                {
                                    var newRel = new SI_MAPUSERPROJET()
                                    {
                                        IDUS = userExistTest.ID,
                                        IDPROJET = int.Parse(a),
                                        CREATIONDATE = DateTime.Now,
                                        IDUSER = exist.ID
                                    };
                                    db.SI_MAPUSERPROJET.Add(newRel);
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                    else
                    {
                        var newUser = new SI_USERS()
                        {
                            LOGIN = user.LOGIN,
                            PWD = user.PWD,
                            IDPROJET = exist.IDPROJET,
                            ROLE = user.ROLE,
                            CREATIONDATE = DateTime.Now,
                            IDUSER = exist.ID
                        };
                        db.SI_USERS.Add(newUser);

                        db.SaveChanges();
                    }

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = user }, settings));
                }
                else
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "L'utilisateur existe déjà (pour ce projet ou un autre). " }, settings));
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        [HttpPost]
        public JsonResult UpdateUser(SI_USERS suser, SI_USERS user, string oldPassword, string UserId, string listProjet)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDPROJET == suser.IDPROJET*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int userId = int.Parse(UserId);
                var userExist = db.SI_USERS.FirstOrDefault(a => a.ID == userId && a.DELETIONDATE == null);
                var test = db.SI_USERS.Where(x => x.ROLE == exist.ROLE && x.IDPROJET == exist.IDPROJET && x.DELETIONDATE == null).FirstOrDefault();
                if (userExist != null)
                {
                    if (userExist.PWD != oldPassword) return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Ancien mot de passe non valide. ", data = user }, settings));

                    if (test.ROLE == Role.SAdministrateur)
                    {
                        if (user.ROLE == Role.Administrateur || user.ROLE == Role.Autre)
                        {
                            int TestProjetRole = 0;
                            if (!int.TryParse(listProjet, out TestProjetRole))
                                return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Vous ne pouvez pas affecter plusieurs projets à ce type d'utilisateur. " }, settings));

                            userExist.LOGIN = user.LOGIN;
                            userExist.PWD = user.PWD;
                            userExist.IDPROJET = int.Parse(listProjet);
                            userExist.ROLE = user.ROLE;
                            userExist.IDUSER = exist.ID;

                            db.SaveChanges();
                        }
                        else
                        {
                            userExist.LOGIN = user.LOGIN;
                            userExist.PWD = user.PWD;
                            userExist.IDPROJET = 0;
                            userExist.ROLE = user.ROLE;
                            userExist.IDUSER = exist.ID;

                            db.SaveChanges();

                            if (db.SI_MAPUSERPROJET.Any(a => a.IDUS == userId))
                            {
                                foreach (var x in db.SI_MAPUSERPROJET.Where(a => a.IDUS == userId).ToList())
                                {
                                    db.SI_MAPUSERPROJET.Remove(x);
                                    db.SaveChanges();
                                }
                            }

                            string[] separators = { "," };
                            var pro = listProjet;
                            if (pro != null)
                            {
                                string listUser = pro.ToString();
                                string[] lst = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                foreach (var a in lst)
                                {
                                    var newRel = new SI_MAPUSERPROJET()
                                    {
                                        IDUS = userId,
                                        IDPROJET = int.Parse(a),
                                        CREATIONDATE = DateTime.Now,
                                        IDUSER = exist.ID
                                    };
                                    db.SI_MAPUSERPROJET.Add(newRel);
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                    else
                    {
                        userExist.LOGIN = user.LOGIN;
                        userExist.PWD = user.PWD;
                        userExist.IDPROJET = exist.IDPROJET;
                        userExist.ROLE = user.ROLE;
                        userExist.IDUSER = exist.ID;

                        db.SaveChanges();
                    }

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = user }, settings));
                }
                else
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "L'utilisateur existe déjà (pour ce projet ou un autre). " }, settings));
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }
        public ActionResult Param()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Param(SI_USERS suser)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDPROJET == suser.IDPROJET*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = exist.ID;
                var crpto = db.SI_USERS.FirstOrDefault(a => a.ID == crpt && a.DELETIONDATE == null);
                if (crpto != null)
                {
                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = crpto }, settings));
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
        public JsonResult UpdateMDP(SI_USERS suser, SI_USERS user/*, string MDPA*/)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDPROJET == suser.IDPROJET*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int crpt = exist.ID;
                var SExist = db.SI_USERS.FirstOrDefault(a => a.ID == crpt && a.DELETIONDATE == null);
                if (SExist != null)
                {
                    if (SExist.PWD != user.LOGIN)
                        return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Veuillez vérifier votre ancien mot de passe. ", data = user }, settings));

                    SExist.PWD = user.PWD;
                    db.SaveChanges();

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = user }, settings));
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
        public ActionResult DetailsUser(string UserId)
        {
            return View();
        }
        [HttpPost]
        public ActionResult DetailsUser(SI_USERS suser, string UserId)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDPROJET == suser.IDPROJET*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int useID = int.Parse(UserId);
                var user = db.SI_USERS.FirstOrDefault(a => a.ID == useID && a.DELETIONDATE == null);

                var proj = new List<int>();
                if (user.IDPROJET == 0)
                {
                    if (db.SI_MAPUSERPROJET.Any(a => a.IDUS == useID))
                    {
                        foreach (var x in db.SI_MAPUSERPROJET.Where(a => a.IDUS == useID).ToList())
                        {
                            proj.Add(x.IDPROJET.Value);
                        }
                    }
                }
                else
                {
                    proj.Add(user.IDPROJET.Value);
                }

                if (user != null)
                {
                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = new { LOGIN = user.LOGIN, ROLE = user.ROLE, PROJET = proj } }, settings));
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
        public ActionResult Login()
        {
            db.SI_USERS.Any();
            return View();
        }

        [HttpPost]
        public ActionResult Login(SI_USERS Users)
        {
            try
            {
                var test = db.SI_USERS.FirstOrDefault(x => x.LOGIN == Users.LOGIN && x.PWD == Users.PWD && x.DELETIONDATE == null);
                if (test == null) return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Vérifiez vos identifiants. " }, settings));

                if (test.ROLE != Role.SAdministrateur && test.ROLE != Role.Organe_de_Suivi && test.ROLE != Role.Agent_Comptable)
                {
                    if (test.IDPROJET != 0)
                    {
                        if (String.IsNullOrEmpty(test.IDPROJET.ToString()) || !db.SI_PROJETS.Any(a => a.ID == test.IDPROJET && a.DELETIONDATE == null))
                        {
                            return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Vous n'êtes pas rattaché à un projet actif. " }, settings));
                        }
                    }
                }

                Session["userSession"] = test;

                Session["UserName"] = test.LOGIN;

                Session["VERSIONCONNNECT"] = "1.0.0";
                Session["VERSION"] = "1.3.191";


                if (db.SI_MENU.Any())
                {
                    var isMenu = db.SI_MENU.FirstOrDefault();
                    Session["MTNON"] = isMenu.MTNON;
                    Session["MT0"] = isMenu.MT0;
                    Session["MT1"] = isMenu.MT1;
                    Session["MT2"] = isMenu.MT2;
                    Session["MP1"] = isMenu.MP1;
                    Session["MP2"] = isMenu.MP2;
                    Session["MP3"] = isMenu.MP3;
                    Session["MP4"] = isMenu.MP4;

                    Session["MD0"] = isMenu.MD0;
                    Session["MD1"] = isMenu.MD1;
                    Session["MD2"] = isMenu.MD2;
                    Session["MD3"] = isMenu.MD3;

                    Session["MOP0"] = isMenu.MOP0;
                    Session["MOP1"] = isMenu.MOP1;
                    Session["MOP2"] = isMenu.MOP2;

                    Session["TDB0"] = isMenu.TDB0;
                    Session["TDB1"] = isMenu.TDB1;
                    Session["TDB2"] = isMenu.TDB2;
                    Session["TDB3"] = isMenu.TDB3;
                    Session["TDB4"] = isMenu.TDB4;
                    Session["TDB5"] = isMenu.TDB5;
                    Session["TDB6"] = isMenu.TDB6;
                    Session["TDB7"] = isMenu.TDB7;
                    Session["TDB8"] = isMenu.TDB8;
                }

                if (db.SI_GEDLIEN.Any())
                {
                    var isMenu = db.SI_GEDLIEN.FirstOrDefault();
                    Session["GED"] = isMenu.LIEN;
                }

                test.LASTCONNEXTION = DateTime.Now;
                db.SaveChanges();

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", Data = new { test.ROLE, test.IDPROJET } }, settings));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult DeleteUser(SI_USERS suser, string UserId)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDPROJET == suser.IDPROJET*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                int useID = int.Parse(UserId);
                var user = db.SI_USERS.FirstOrDefault(a => a.ID == useID && a.DELETIONDATE == null);
                if (user != null)
                {
                    user.DELETIONDATE = DateTime.Now;
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

        [HttpPost]
        public JsonResult GetUR(SI_USERS suser)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDPROJET == suser.IDPROJET*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            return Json(JsonConvert.SerializeObject(new { type = "login", msg = "", data = exist.ROLE != (int)Role.SAdministrateur }, settings));
        }
    }
}
