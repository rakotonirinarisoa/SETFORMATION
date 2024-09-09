using System.Web.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using apptab.Data.Entities;
using System.Data.Entity;
using System;

namespace apptab.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly SOFTCONNECTSIIG _db;
        private readonly JsonSerializerSettings _settings;

        public ProjectsController()
        {
            _db = new SOFTCONNECTSIIG();
            _settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        public ActionResult AllProjects()
        {
            ViewBag.Controller = "Liste des PROJETS";

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetAllProjects(SI_USERS suser)
        {
            var currentUser = await _db.SI_USERS.FirstOrDefaultAsync(u => u.LOGIN == suser.LOGIN && u.PWD == suser.PWD && u.DELETIONDATE == null);

            if (currentUser == null)
            {
                return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, _settings));
            }

            var projects = await _db.SI_PROJETS.Where(p => p.DELETIONDATE == null).ToListAsync();

            return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = projects }, _settings));
        }

        public ActionResult NewProject()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> AddProject(SI_USERS suser, SI_PROJETS societe, SI_USERS user)
        {
            var currentUser = await _db.SI_USERS.FirstOrDefaultAsync(u => u.LOGIN == suser.LOGIN && u.PWD == suser.PWD && u.DELETIONDATE == null);

            if (currentUser == null)
            {
                return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, _settings));
            }

            var project = await _db.SI_PROJETS.FirstOrDefaultAsync(p => p.PROJET == societe.PROJET && p.DELETIONDATE == null);

            if (project == null)
            {
                var now = DateTime.Now;

                _db.SI_PROJETS.Add(new SI_PROJETS()
                {
                    PROJET = societe.PROJET,
                    IDUSER = suser.ID,
                    CREATIONDATE = now
                });

                await _db.SaveChangesAsync();

                int projectId = (await _db.SI_PROJETS.FirstOrDefaultAsync(p => p.PROJET == societe.PROJET && p.DELETIONDATE == null)).ID;

                _db.HSI_PROJETS.Add(new HSI_PROJETS
                {
                    IDPARENT = projectId,
                    PROJET = societe.PROJET,
                    IDUSER = currentUser.ID,
                    CREATIONDATE = now
                });

                _db.SI_USERS.Add(new SI_USERS()
                {
                    LOGIN = user.LOGIN,
                    PWD = user.PWD,
                    ROLE = Role.Administrateur,
                    IDPROJET = projectId
                });

                await _db.SaveChangesAsync();

                int adminId = (await _db.SI_USERS.FirstOrDefaultAsync(u => u.IDPROJET == projectId && u.DELETIONDATE == null)).ID;

                _db.HSI_USERS.Add(new HSI_USERS
                {
                    IDPARENT = adminId,
                    LOGIN = user.LOGIN,
                    PWD = user.PWD,
                    IDPROJET = projectId,
                    IDUSER = currentUser.ID,
                    CREATIONDATE = now
                });

                await _db.SaveChangesAsync();

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès. ", data = societe }, _settings));
            }

            return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Projet déjà existant. " }, _settings));
        }

        public async Task<ActionResult> ProjectDetails(int id)
        {
            var project = await _db.SI_PROJETS.Where(p => p.ID == id && p.DELETIONDATE == null).FirstOrDefaultAsync();

            if (project == null)
            {
                throw new Exception("404");
            }

            ViewData["Id"] = id;
            ViewData["Title"] = project.PROJET;

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> UpdateProject(ProjectToUpdate projectToUpdate)
        {
            var currentUser = await _db.SI_USERS.FirstOrDefaultAsync(u => u.LOGIN == projectToUpdate.Login && u.PWD == projectToUpdate.Password && u.DELETIONDATE == null);

            if (currentUser == null)
            {
                return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion!" }, _settings));
            }

            var project = await _db.SI_PROJETS.FirstOrDefaultAsync(p => p.ID == projectToUpdate.Id && p.DELETIONDATE == null);

            if (project != null)
            {
                var now = DateTime.Now;

                project.PROJET = projectToUpdate.Title;

                _db.HSI_PROJETS.Add(new HSI_PROJETS
                {
                    IDPARENT = projectToUpdate.Id,
                    PROJET = projectToUpdate.Title,
                    IDUSER = currentUser.ID,
                    CREATIONDATE = now
                });

                await _db.SaveChangesAsync();

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Enregistrement avec succès." }, _settings));
            }

            return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Projet non trouvé!" }, _settings));
        }

        [HttpPost]
        public async Task<JsonResult> DeleteProject(ProjectToDelete projectToDelete)
        {
            var currentUser = await _db.SI_USERS.FirstOrDefaultAsync(u => u.LOGIN == projectToDelete.Login && u.PWD == projectToDelete.Password && u.DELETIONDATE == null);

            if (currentUser == null)
            {
                return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion!" }, _settings));
            }

            var project = await _db.SI_PROJETS.FirstOrDefaultAsync(p => p.ID == projectToDelete.Id && p.DELETIONDATE == null);

            if (project != null)
            {
                var prosoa = await _db.SI_PROSOA.FirstOrDefaultAsync(ps => ps.IDPROJET == projectToDelete.Id && ps.DELETIONDATE == null);

                var now = DateTime.Now;

                if (prosoa != null)
                {
                    prosoa.DELETIONDATE = now;
                }

                project.DELETIONDATE = now;

                await _db.SaveChangesAsync();

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Suppression avec succès." }, _settings));
            }

            return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Projet non trouvé!" }, _settings));
        }
    }
}
