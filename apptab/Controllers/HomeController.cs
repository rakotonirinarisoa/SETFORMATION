using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Web.Mvc;
using apptab.Extension;
using Aspose.Zip.Saving;
using Aspose.Zip;
using Newtonsoft.Json;
using Renci.SshNet;
using System.Net;
using apptab.Models;
using Extensions.DateTime;
using System.Net.Mail;
using apptab.Data.Entities;
using apptab.Data;
using System.Collections;
using System.Web.WebPages;

namespace apptab.Controllers
{
    public class HomeController : Controller
    {
        private readonly SOFTCONNECTSIIG db = new SOFTCONNECTSIIG();
        private readonly SOFTCONNECTOM __db = new SOFTCONNECTOM();
        private static string Anarana;

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TdbAccueil()
        {
            return View();
        }
        public ActionResult PaiementValidation()
        {
            ViewBag.Controller = "Récupération des paiements";
            return View();
        }
        public ActionResult TeacherValidation()
        {
            ViewBag.Controller = "Envoi pour validation";
            return View();
        }
        public ActionResult FValidation()
        {
            ViewBag.Controller = "Génération fichier banque";
            return View();
        }
        public ActionResult BonPourPaiement()
        {
            ViewBag.Controller = "Validation comptable payeur";
            return View();
        }
        [HttpPost]
        public string GetTypeP(SI_USERS suser, string codeproject)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            int PROJECTid = int.Parse(codeproject);
            if (exist == null) return "";

            if (exist.IDPROJET != 0)
            {
                var basename = db.SI_TYPECRITURE.FirstOrDefault(a => a.IDPROJET == PROJECTid).TYPE;
                return basename.ToString();
            }
            else
            {
                //var mapuser = db.SI_MAPUSERPROJET.Where(a => a.IDUS == exist.ID).ToList();
                int PROJECTID = int.Parse(codeproject);
                var ii = db.SI_TYPECRITURE.FirstOrDefault(a => a.IDPROJET == PROJECTID);
                var basename = "";
                if (ii != null)
                {
                    basename = ii.TYPE.ToString();
                }


                return basename;
            }
        }
        [HttpPost]
        public JsonResult FillTable(SI_USERS suser)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD) != null;
            if (!exist) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                var droits = db.OPA_DROITS.Where(a => a.IDSOCIETE == suser.IDPROJET).Select(a => new
                {
                    USER = db.SI_USERS.FirstOrDefault(x => x.ID == a.IDUSER).LOGIN,
                    INSTANCE = db.SI_MAPPAGES.FirstOrDefault(x => x.ID == a.IDMAPPAGE).INSTANCE,
                    DBASE = db.SI_MAPPAGES.FirstOrDefault(x => x.ID == a.IDMAPPAGE).DBASE,
                    ID = a.ID
                }).ToList();

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = droits }, settings));
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }

        [HttpPost]
        public JsonResult CreateZipFile_old(SI_USERS suser, string list)
        {
            StreamWriter sw = null;
            var lists = list.Split(',');
            List<AnalY> analY = new List<AnalY>();
            //List<OPA_REGLEMENT> test = new List<OPA_REGLEMENT>();
            sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\OPAVI.txt");
            foreach (var item in lists)
            {
                var tt = Convert.ToDecimal(item);
                var xy = db.OPA_REGLEMENT.Where(x => x.NUM == tt).FirstOrDefault();
                sw.Write(xy.NUM + "\t" + xy.DATE + "\t" + xy.BENEFICIAIRE + "\t" + xy.BANQUE + "\t" + xy.GUICHET + "\t" + xy.RIB + "\t" + xy.MONTANT + "\t" + xy.LIBELLE + "\t" + xy.NUM_ETABLISSEMENT + "\t" + xy.CODE_J + "\t" + xy.DOM1 + "\t" + xy.DOM2 + "\t" + xy.CATEGORIE + "\t" + xy.APPLICATION + "\t" + xy.IDSOCIETE + Environment.NewLine);
            }
            sw.Close();

            return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = "" }, settings));
        }
        public string CreateAFBTXT(string pathchemin, string pathfiles)
        {
            try
            {
                string pth = AppDomain.CurrentDomain.BaseDirectory + "\\FILERESULT\\";
                if (!Directory.Exists(pth))
                {
                    Directory.CreateDirectory(pth);
                }
                StreamWriter sw = null;
                sw = new StreamWriter(pth + pathchemin + ".txt");
                sw.Write(pathfiles);
                sw.Close();
                string source = pth + pathchemin + ".txt";
                return source;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public FileResult CreateFileAFBtXt(string pathchemin, string pathfiles)
        {
            try
            {
                string pth = AppDomain.CurrentDomain.BaseDirectory + "\\FILERESULT\\";

                if (!Directory.Exists(pth))
                {
                    Directory.CreateDirectory(pth);
                }
                StreamWriter sw = null;

                sw = new StreamWriter(pth + pathchemin + ".txt");
                sw.Write(pathfiles);
                sw.Close();
                byte[] source = System.IO.File.ReadAllBytes(pth + pathchemin + ".txt");
                string s = "application/txt";
                return File(source, System.Net.Mime.MediaTypeNames.Application.Octet, pathfiles);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public FileResult CreateAFBTXTArch(string pathchemin, string pathfiles, string psw)
        {
            string pth = AppDomain.CurrentDomain.BaseDirectory + "\\FILERESULT\\";
            if (!Directory.Exists(pth))
            {
                Directory.CreateDirectory(pth);
            }
            StreamWriter sw = null;
            sw = new StreamWriter(pth + pathchemin + ".txt");
            //sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + pathchemin + ".txt");
            sw.Write(pathfiles);
            sw.Close();

            FileStream zipFile = System.IO.File.Open(pth + pathchemin + ".zip", FileMode.Create);

            var archive = new Archive(new ArchiveEntrySettings(encryptionSettings: new TraditionalEncryptionSettings(psw)));

            FileStream source = System.IO.File.Open(pth + pathchemin + ".txt", FileMode.Open, FileAccess.Read);

            archive.CreateEntry(pathchemin + ".txt", source);
            archive.Save(zipFile);

            zipFile.Dispose();
            // archive.Dispose();

            return File(source, System.Net.Mime.MediaTypeNames.Application.Octet);
        }
        [HttpPost]
        public ActionResult CreateZipFile(SI_USERS suser, string codeproject, int intbasetype, bool devise, string codeJ, string baseName)
        {
            AFB160 aFB160 = new AFB160();
            var send = "";
            int PROJECTID = int.Parse(codeproject);
            var ps = db.SI_USERS.Where(x => x.LOGIN == suser.LOGIN /*&& x.IDPROJET == PROJECTID*/ && x.PWD == suser.PWD).Select(x => x.PWD).FirstOrDefault();

            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            var pswftp = db.OPA_CRYPTO.Where(x => x.IDPROJET == PROJECTID && x.IDUSER == suser.ID && x.DELETIONDATE != null).Select(x => x.CRYPTOPWD).FirstOrDefault();
            var avalider = db.OPA_VALIDATIONS.Where(a => a.IDPROJET == PROJECTID && a.ETAT == 2).ToList();
            if (baseName == "2")
            {
                var pathfile = aFB160.CreateTOMPROAFB160(devise, codeJ, suser, codeproject);
                if (intbasetype == 0)
                {
                    Anarana = pathfile.Chemin;
                    return CreateFileAFBtXt(pathfile.Chemin, pathfile.Fichier);
                }
                else if (intbasetype == 1)
                {
                    Anarana = pathfile.Chemin;
                    return CreateAFBTXTArch(pathfile.Chemin, pathfile.Fichier, ps);
                }
                else if (intbasetype == 2)
                {
                    Anarana = pathfile.Chemin;
                    send = CreateAFBTXT(pathfile.Chemin, pathfile.Fichier);
                    var ftp = db.OPA_FTP.Where(x => x.IDPROJET == PROJECTID).FirstOrDefault();
                    SENDFTP(ftp.HOTE, ftp.PATH, ftp.IDENTIFIANT, ftp.FTPPWD, send);
                }
                else
                {
                    Anarana = pathfile.Chemin;
                    var ftp = db.OPA_FTP.Where(x => x.IDPROJET == PROJECTID).FirstOrDefault();
                    SENDFTP(ftp.HOTE, ftp.PATH, ftp.IDENTIFIANT, ftp.FTPPWD, send);
                    return CreateAFBTXTArch(pathfile.Chemin, pathfile.Fichier, ps);
                }

                if (avalider != null)
                {
                    foreach (var item in avalider)
                    {
                        try
                        {
                            item.DATEVAL = DateTime.Now;
                            item.IDUSVAL = exist.ID;
                            item.ETAT = 3;
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Erreur de connexion", data = ex.Message }, settings));
                            throw;
                        }
                    }
                }

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Archivage avec succès. ", data = send }, settings));
            }
            else
            {
                var pathfile = aFB160.CreateBRAFB160(devise, codeJ, suser, codeproject);
                if (avalider != null)
                {
                    foreach (var item in avalider)
                    {
                        try
                        {
                            item.DATEVAL = DateTime.Now;
                            item.IDUSVAL = exist.ID;
                            item.ETAT = 3;
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Erreur de connexion", data = ex.Message }, settings));
                            throw;
                        }
                    }
                }
                if (intbasetype == 0)
                {
                    Anarana = pathfile.Chemin;
                    return CreateFileAFBtXt(pathfile.Chemin, pathfile.Fichier);
                }
                else if (intbasetype == 1)
                {
                    Anarana = pathfile.Chemin;
                    return CreateAFBTXTArch(pathfile.Chemin, pathfile.Fichier, ps);
                }
                else if (intbasetype == 2)
                {
                    Anarana = pathfile.Chemin;
                    var ftp = db.OPA_FTP.Where(x => x.IDPROJET == PROJECTID).FirstOrDefault();
                    SENDFTP(ftp.HOTE, ftp.PATH, ftp.IDENTIFIANT, ftp.FTPPWD, send);
                    return CreateFileAFBtXt(pathfile.Chemin, pathfile.Fichier);
                }
                else
                {
                    Anarana = pathfile.Chemin;
                    var ftp = db.OPA_FTP.Where(x => x.IDPROJET == PROJECTID).FirstOrDefault();
                    SENDFTP(ftp.HOTE, ftp.PATH, ftp.IDENTIFIANT, ftp.FTPPWD, send);
                    return CreateAFBTXTArch(pathfile.Chemin, pathfile.Fichier, ps);
                }

            }
        }
        [HttpPost]
        public JsonResult FileName()
        {
            return Json(JsonConvert.SerializeObject(new { Filename = Anarana }, settings));
        }
        public ActionResult GetFile(string file)
        {
            try
            {
                if (file.Contains(".zip"))
                {
                    string path = file;
                    var fs = new FileStream(path, FileMode.Open);
                    return File(fs, "application/x-7z-compressed", file.Split('\\').Last().Split('.').First() + ".zip");
                }
                else
                {
                    string path = file;
                    var fs = new FileStream(path, FileMode.Open);
                    return File(fs, "application/txt", file.Split('\\').Last().Split('.').First() + ".txt");
                }

            }
            catch { return null; }
        }
        public class AnalY
        {
            public string Numero { get; set; }
            public string Datedordre { get; set; }
            public string NumPiece { get; set; }
            public string Compte { get; set; }
            public string Libelle { get; set; }
            public string debit { get; set; }
            public string credit { get; set; }
            public string Montadevise { get; set; }
            public string Mon { get; set; }
            public string Rang { get; set; }
            public string FinCat { get; set; }
            public string Comm { get; set; }
            public string Plan6 { get; set; }
            public string Journal { get; set; }
            public string Marche { get; set; }
        }
        //=========================================================================================================PaiementsValidations============================================================================
        [HttpPost]
        public JsonResult Getelementjs(int ChoixBase, string codeproject, string journal, DateTime datein, DateTime dateout, string comptaG, string auxi, string auxi1, DateTime dateP, SI_USERS suser)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            var basename = GetTypeP(suser, codeproject);
            int PROJECTID = int.Parse(codeproject);

            if (basename == "")
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Veuillez paramétrer le type d'ecriture avant toutes opérations. " }, settings));
            }
            if (basename == "")
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Veuillez paramétrer le type d'ecriture avant toutes opérations. " }, settings));
            }
            if (codeproject != "")
            {
                PROJECTID = int.Parse(codeproject);
            }
            else
            {
                PROJECTID = exist.IDPROJET.Value;
            }

            SOFTCONNECTOM.connex = new Data.Extension().GetCon(PROJECTID);
            SOFTCONNECTOM tom = new SOFTCONNECTOM();

            AFB160 afb160 = new AFB160();

            //var hst = db.OPA_HISTORIQUE.Select(x => x.NUMENREG.ToString()).ToArray();
            var hstSiig = db.OPA_VALIDATIONS.Where(x => x.ETAT != 4 && x.IDPROJET == PROJECTID).Select(x => x.IDREGLEMENT.ToString()).ToArray();
            var list = afb160.getListEcritureCompta(journal, PROJECTID, datein, dateout, comptaG, auxi, auxi1, dateP, suser).Where(x => !hstSiig.Contains(x.No.ToString())).ToList();
            return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = list }, settings));

        }
        [HttpPost]
        public JsonResult getelementjsBR(int ChoixBase, string codeproject, string journal, DateTime datein, bool devise, DateTime dateout, string comptaG, string auxi, string auxi1, string etat, DateTime dateP,/* int mois, int annee, string matr1, string matr2, DateTime datePaie,*/ SI_USERS suser)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            var basename = GetTypeP(suser, codeproject);
            int PROJECTID = int.Parse(codeproject);
            if (basename == "")
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Veuillez paramétrer le type d'ecriture avant toutes opérations. " }, settings));
            }
            if (codeproject != "")
            {
                PROJECTID = int.Parse(codeproject);
            }
            else
            {
                PROJECTID = exist.IDPROJET.Value;
            }

            SOFTCONNECTOM.connex = new Data.Extension().GetCon(PROJECTID);
            SOFTCONNECTOM tom = new SOFTCONNECTOM();

            AFB160 afb160 = new AFB160();

            RJL1 djournal = (from jrnl in tom.RJL1
                             where jrnl.CODE == journal
                             select jrnl).Single();
            if (djournal.RIB == null || djournal.RIB == "")
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Veuillez remplir les comptes RIB du " + journal + " journal. ", data = "" }, settings));
            }
            var hstSiig = db.OPA_VALIDATIONS.Where(x => x.ETAT != 4 && x.IDPROJET == PROJECTID && x.ComptaG == comptaG && x.Journal == journal).Select(x => x.IDREGLEMENT.ToString()).ToArray();

            var list = afb160.getListEcritureBR(journal, datein, dateout, devise, comptaG, auxi, etat, dateP, suser, PROJECTID).Where(x => !hstSiig.Contains(x.No.ToString())).ToList();
            return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = list }, settings));

        }
        [HttpPost]
        public JsonResult GetEtat(string codeproject, SI_USERS suser)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            //var basename = GetTypeP(suser, codeproject);

            int PROJECTID = int.Parse(codeproject);

            if (codeproject != "")
            {
                PROJECTID = int.Parse(codeproject);
            }
            else
            {
                PROJECTID = exist.IDPROJET.Value;
            }

            SOFTCONNECTOM.connex = new Data.Extension().GetCon(PROJECTID);
            SOFTCONNECTOM tom = new SOFTCONNECTOM();

            List<string> listEtat = __db.OP_CHAINETRAITEMENT.Select(x => x.ETAT).ToList();
            return Json(JsonConvert.SerializeObject(new { type = "sucess", msg = "", data = listEtat }));
        }
        [HttpPost]
        public JsonResult GetCODEJournal(string codeproject, SI_USERS suser)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));
            var basename = GetTypeP(suser, codeproject);
            if (basename == null)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Veuillez paramétrer le type d'ecriture avant toutes opérations. " }, settings));
            }
            int crpt = 0;
            if (codeproject != "")
            {
                crpt = int.Parse(codeproject);
            }
            else
            {
                crpt = exist.IDPROJET.Value;
            }

            //Check si le projet est mappé à une base de données TOM²PRO//
            if (db.SI_MAPPAGES.FirstOrDefault(a => a.IDPROJET == crpt) == null)
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Le projet n'est pas mappé à une base de données TOM²PRO. " }, settings));

            SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
            SOFTCONNECTOM tom = new SOFTCONNECTOM();

            var JournalVM = tom.RJL1.Select(x => new
            {
                CODE = x.CODE,
                LIBELLE = x.LIBELLE
            }).ToList();

            return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = JournalVM }, settings));
        }
        [HttpPost]
        public JsonResult GetCompteG(SI_USERS suser , string codeproject)
        {
            int PROJECTID = int.Parse(codeproject);
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));
            var basename = GetTypeP(suser, exist.IDPROJET.ToString());
            if (exist.IDPROJET == 0)
            {
                basename = db.SI_TYPECRITURE.Where(x => x.IDPROJET == PROJECTID).Select(x => x.TYPE).FirstOrDefault().ToString();
            }
            if (basename == null)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Veuillez paramétrer le type d'ecriture avant toutes opérations. " }, settings));
            }
            int crpt = 0;
            if (codeproject != "")
            {
                crpt = int.Parse(codeproject);
            }
            else
            {
                crpt = exist.IDPROJET.Value;
            }

            SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
            SOFTCONNECTOM tom = new SOFTCONNECTOM();

            if (basename == "1")
            {
                var CompteGBR = __db.MOP.Where(a => a.COGEFOURNISSEUR.StartsWith("4")).GroupBy(x => x.COGEFOURNISSEUR).Select(x => new
                {
                    COGE = x.Key,
                    AUXI = x.Select(y => y.AUXI/*new { AUXI = y.AUXI, NOM = y.NOM }*/).Distinct().ToList()
                }).ToList();
                var CompteAvance = __db.GA_AVANCE.Where(a => a.COGE.StartsWith("4")).GroupBy(x => x.COGE).Select(x => new
                {
                    COGE = x.Key,
                    AUXI = x.Select(y => y.AUXI).Distinct().ToList()
                }).ToList();

                if (CompteGBR.Count != 0)
                {
                    CompteGBR.AddRange(CompteAvance);
                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = CompteGBR }, settings));
                }
                else
                {
                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = CompteAvance }, settings));
                }
            }
            else
            {
                var CompteG = __db.MCOMPTA.Where(a => a.COGE.StartsWith("4")).GroupBy(x => x.COGE).Select(x => new
                {
                    COGE = x.Key,
                    AUXI = x.Select(y => y.AUXI).Distinct().ToList()
                }).ToList();
                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = CompteG }, settings));
            }

        }

        [HttpPost]
        public JsonResult GetCheckedCompte(string baseName, string codeproject, DateTime datein, DateTime dateout, string comptaG, string auxi, DateTime dateP, string listCompte, string journal, string etat, bool devise, SI_USERS suser)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));
            var basename = GetTypeP(suser, codeproject);
            int PROJECTID = int.Parse(codeproject);

            if (basename == "")
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Veuillez paramétrer le type d'ecriture avant toutes opérations. " }, settings));
            }

            if (string.IsNullOrEmpty(listCompte))
                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès.", data = "" }, settings));

            //List<string> list = listCompte.Split(',').ToList();
            var list = JsonConvert.DeserializeObject<List<AvanceDetails>>(listCompte);

            #region CommOpavi

            #endregion
            int countTraitement = 0;
            var lien = "http://srvapp.softwell.cloud/softconnectsiig/";

            var ProjetIntitule = db.SI_PROJETS.Where(a => a.ID == PROJECTID).FirstOrDefault().PROJET;

            OPA_VALIDATIONS avalider = new OPA_VALIDATIONS();
            if (basename == "2")
            {
                string auxi1 = auxi;
                AFB160 afb160 = new AFB160();
                var hst = db.OPA_HISTORIQUE.Select(x => x.NUMENREG.ToString()).ToArray();

                foreach (var h in list)
                {
                    var listA = afb160.getListEcritureCompta(journal, PROJECTID, datein, dateout, comptaG, auxi, auxi1, dateP, suser).Where(x => x.No.ToString() == h.Id).ToList();

                    foreach (var item in listA)
                    {
                        avalider.IDREGLEMENT = item.No.ToString();
                        avalider.ETAT = 0;
                        avalider.IDPROJET = PROJECTID;
                        avalider.DateIn = datein;
                        avalider.DateOut = dateout;
                        avalider.ComptaG = comptaG;
                        avalider.auxi = auxi;
                        avalider.DateP = dateP;
                        avalider.Journal = journal;
                        avalider.dateOrdre = item.dateOrdre;
                        avalider.NoPiece = item.NoPiece;
                        avalider.Compte = item.Compte;
                        avalider.Libelle = item.Libelle;
                        avalider.Debit = item.Debit;
                        avalider.Credit = item.Credit;
                        avalider.MontantDevise = item.MontantDevise;
                        avalider.Mon = item.Mon;
                        avalider.Rang = item.Rang;
                        avalider.Poste = item.Poste;
                        avalider.FinancementCategorie = item.FinancementCategorie;
                        avalider.Commune = item.Commune;
                        avalider.Plan6 = item.Plan6;
                        avalider.Marche = item.Marche;
                        avalider.Statut = item.Statut;
                        avalider.DATECREA = DateTime.Now;
                        avalider.IDUSCREA = exist.ID;
                        avalider.AVANCE = false;

                        try
                        {
                            db.OPA_VALIDATIONS.Add(avalider);
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Erreur de connexion", data = ex.Message }, settings));
                        }
                    }

                    countTraitement++;
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
                    if (db.SI_MAIL.FirstOrDefault(a => a.IDPROJET == PROJECTID && a.DELETIONDATE == null).MAILPP != null)
                    {
                        string[] separators = { ";" };

                        var Tomail = mail;
                        if (Tomail != null)
                        {
                            string listUser = db.SI_MAIL.FirstOrDefault(a => a.IDPROJET == PROJECTID && a.DELETIONDATE == null).MAILPP;
                            string[] mailListe = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var mailto in mailListe)
                            {
                                mail.To.Add(mailto);
                            }
                        }
                    }

                    mail.Subject = "Attente validation paiements du projet " + ProjetIntitule;
                    mail.IsBodyHtml = true;
                    mail.Body = "Madame, Monsieur,<br/><br>" + "Nous vous informons que vous avez " + countTraitement + " paiements en attente validation pour le compte du projet " + ProjetIntitule + ".<br/><br>" +
                        "Nous vous remercions de cliquer <a href='" + lien + "'>(ici)</a> pour accéder à la plate-forme SOFT-SIIG CONNECT.<br/><br>" + "Cordialement";

                    smtp.Port = 587;
                    smtp.Credentials = new System.Net.NetworkCredential(MailAdresse, mdpMail);
                    smtp.EnableSsl = true;

                    try { smtp.Send(mail); }
                    catch (Exception) { }
                }
            }
            else
            {
                string auxi1 = auxi;
                AFB160 afb160 = new AFB160();
                var hst = db.OPA_HISTORIQUE.Select(x => x.NUMENREG.ToString()).ToArray();
                foreach (var h in list)
                {
                    var listA = afb160.getListEcritureBR(journal, datein, dateout, devise, comptaG, auxi, etat, dateP, suser, PROJECTID).Where(x => x.No.ToString() == h.Id).ToList();
                    foreach (var item in listA)
                    {
                        avalider.IDREGLEMENT = item.No;
                        avalider.ETAT = 0;
                        avalider.IDPROJET = PROJECTID;
                        avalider.DateIn = datein;
                        avalider.DateOut = dateout;
                        avalider.ComptaG = comptaG;
                        avalider.auxi = auxi;
                        avalider.DateP = dateP;
                        avalider.Journal = journal;
                        avalider.dateOrdre = item.Date;
                        avalider.NoPiece = item.NoPiece;
                        avalider.Compte = item.Compte;
                        avalider.Libelle = item.Libelle;
                        avalider.MONTANT = item.Montant;
                        avalider.MontantDevise = item.MontantDevise;
                        avalider.Mon = item.Mon;
                        avalider.Rang = item.Rang;
                        avalider.Poste = item.Poste;
                        avalider.FinancementCategorie = item.FinancementCategorie;
                        avalider.Commune = item.Commune;
                        avalider.Plan6 = item.Plan6;
                        avalider.Marche = item.Marche;
                        avalider.Statut = item.Status;
                        avalider.DATECREA = DateTime.Now;
                        avalider.IDUSCREA = exist.ID;
                        avalider.AVANCE = item.Avance;
                        avalider.NUMEROLIQUIDATION = item.Mandat;
                        try
                        {
                            db.OPA_VALIDATIONS.Add(avalider);
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Erreur de connexion", data = ex.Message }, settings));
                            throw;
                        }
                    }
                    countTraitement++;
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
                    if (db.SI_MAIL.FirstOrDefault(a => a.IDPROJET == PROJECTID && a.DELETIONDATE == null).MAILPE != null)
                    {
                        string[] separators = { ";" };

                        var Tomail = mail;
                        if (Tomail != null)
                        {
                            string listUser = db.SI_MAIL.FirstOrDefault(a => a.IDPROJET == PROJECTID && a.DELETIONDATE == null).MAILPE;
                            string[] mailListe = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var mailto in mailListe)
                            {
                                mail.To.Add(mailto);
                            }
                        }
                    }

                    mail.Subject = "Attente validation paiements du projet " + ProjetIntitule;
                    mail.IsBodyHtml = true;
                    mail.Body = "Madame, Monsieur,<br/><br>" + "Nous vous informons que vous avez " + countTraitement + " paiements en attente validation pour le compte du projet " + ProjetIntitule + ".<br/><br>" +
                        "Nous vous remercions de cliquer <a href='" + lien + "'>(ici)</a> pour accéder à la plate-forme SOFT-SIIG CONNECT.<br/><br>" + "Cordialement";

                    smtp.Port = 587;
                    smtp.Credentials = new System.Net.NetworkCredential(MailAdresse, mdpMail);
                    smtp.EnableSsl = true;

                    try { smtp.Send(mail); }
                    catch (Exception) { }
                }
            }
            return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = "" }, settings));
        }
        //=========================================================================================TeacherValidation======================================================================
        [HttpPost]
        public JsonResult GetElementAvalider(string ChoixBase, string codeproject, DateTime datein, DateTime dateout, string comptaG, string auxi, string auxi1, DateTime dateP, string journal, string etat, bool devise, SI_USERS suser)
        {
            AFB160 aFB160 = new AFB160();
            int PROJECTID = int.Parse(codeproject);
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            int retarDate = 0;
            if (db.SI_DELAISTRAITEMENT.Any(a => a.IDPROJET == PROJECTID && a.DELETIONDATE == null))
                retarDate = db.SI_DELAISTRAITEMENT.FirstOrDefault(a => a.IDPROJET == PROJECTID && a.DELETIONDATE == null).DELPE.Value;

            List<OPA_VALIDATIONS> list = new List<OPA_VALIDATIONS>();
            if (ChoixBase == "2")
            {
                var HistoAFB = db.OPA_HISTORIQUEBR.Where(a => a.IDSOCIETE == PROJECTID).Select(x => x.NUMENREG).ToArray();
                var avalider = db.OPA_VALIDATIONS.Where(ecriture => ecriture.IDPROJET == PROJECTID && ecriture.ETAT == 0 && ecriture.dateOrdre >= datein && ecriture.dateOrdre <= dateout.Date && ecriture.ComptaG == comptaG && ecriture.Journal == journal && !HistoAFB.Contains(ecriture.IDREGLEMENT.ToString())).ToList();
                foreach (var item in avalider)
                {
                    bool isLate = false;
                    if (item.DATEVAL.Value.AddBusinessDays(retarDate).Date < DateTime.Now/* && ((int)DateTime.Now.DayOfWeek) != 6 && ((int)DateTime.Now.DayOfWeek) != 0*/)
                        isLate = true;
                    list.Add(new OPA_VALIDATIONS
                    {
                        IDREGLEMENT = item.IDREGLEMENT,
                        dateOrdre = item.dateOrdre,
                        NoPiece = item.NoPiece,
                        Compte = item.Compte,
                        Journal = item.Journal,
                        Credit = item.Credit,
                        Debit = item.Debit,
                        FinancementCategorie = item.FinancementCategorie,
                        Mon = item.Mon,
                        MontantDevise = item.MontantDevise,
                        Rang = item.Rang,
                        Plan6 = item.Plan6,
                        Commune = item.Commune,
                        Marche = item.Marche,
                        isLATE = isLate,
                        AVANCE = item.AVANCE,
                        NUMEROLIQUIDATION = item.NUMEROLIQUIDATION,
                    });
                }
                //var list = aFB160.getListEcritureCompta(journal, datein, dateout, comptaG, auxi, auxi1, dateP, suser).Where(x => avalider.Contains((int)x.No)).ToList();
                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succés. ", data = list }, settings));
            }
            else
            {
                var HistoAFB = db.OPA_HISTORIQUEBR.Where(a => a.IDSOCIETE == PROJECTID).Select(x => x.NUMENREG).ToArray();
                var avalider = db.OPA_VALIDATIONS.Where(ecriture => ecriture.IDPROJET == PROJECTID && ecriture.ETAT == 0 && ecriture.ComptaG == comptaG && ecriture.Journal == journal && ecriture.dateOrdre >= datein && ecriture.dateOrdre <= dateout && !HistoAFB.Contains(ecriture.IDREGLEMENT.ToString())).ToList();
                foreach (var item in avalider)
                {
                    bool isLate = false;
                    if (item.DATECREA.Value.AddBusinessDays(retarDate).Date < DateTime.Now.Date/* && ((int)DateTime.Now.DayOfWeek) != 6 && ((int)DateTime.Now.DayOfWeek) != 0*/)
                        isLate = true;
                    list.Add(new OPA_VALIDATIONS
                    {
                        IDREGLEMENT = item.IDREGLEMENT,
                        dateOrdre = item.dateOrdre,
                        NoPiece = item.NoPiece,
                        Compte = item.Compte,
                        Journal = item.Journal,
                        Credit = item.Credit,
                        Debit = item.Debit,
                        FinancementCategorie = item.FinancementCategorie,
                        MONTANT = item.MONTANT,
                        Mon = item.Mon,
                        MontantDevise = item.MontantDevise,
                        Rang = item.Rang,
                        Plan6 = item.Plan6,
                        Commune = item.Commune,
                        Marche = item.Marche,
                        isLATE = isLate,
                        AVANCE = item.AVANCE,
                        NUMEROLIQUIDATION = item.NUMEROLIQUIDATION,
                    });
                }
                //var list = aFB160.getListEcritureBR(journal, datein, dateout, devise, comptaG, auxi, etat, dateP, suser).Where(x => avalider.ToString().Contains(x.No)).ToList();
                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succés. ", data = list }, settings));
            }

        }
    
        [HttpPost]
        public JsonResult GetElementAvaliderLoad(SI_USERS suser, string codeproject)
        {
            AFB160 aFB160 = new AFB160();
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            var basename = GetTypeP(suser, codeproject);
            int PROJECTID = int.Parse(codeproject);

            int retarDate = 0;
            if (db.SI_DELAISTRAITEMENT.Any(a => a.IDPROJET == PROJECTID && a.DELETIONDATE == null))
                retarDate = db.SI_DELAISTRAITEMENT.FirstOrDefault(a => a.IDPROJET == PROJECTID && a.DELETIONDATE == null).DELPE.Value;

            List<OPA_VALIDATIONS> list = new List<OPA_VALIDATIONS>();
            if (basename == "")
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Veuillez paramétrer le type d'ecriture avant toutes opérations. " }, settings));
            }
            if (basename == "2")
            {
                var avalider = db.OPA_VALIDATIONS.Where(ecriture => ecriture.IDPROJET == PROJECTID && ecriture.ETAT == 0).ToList();
                foreach (var item in avalider)
                {
                    bool isLate = false;
                    if (item.DATECREA.Value.AddBusinessDays(retarDate).Date < DateTime.Now/* && ((int)DateTime.Now.DayOfWeek) != 6 && ((int)DateTime.Now.DayOfWeek) != 0*/)
                        isLate = true;

                    list.Add(new OPA_VALIDATIONS
                    {
                        IDREGLEMENT = item.IDREGLEMENT,
                        dateOrdre = item.dateOrdre,
                        NoPiece = item.NoPiece,
                        Compte = item.Compte,
                        Journal = item.Journal,
                        Credit = item.Credit,
                        Debit = item.Debit,
                        FinancementCategorie = item.FinancementCategorie,
                        MONTANT = item.MONTANT,
                        Mon = item.Mon,
                        MontantDevise = item.MontantDevise,
                        Rang = item.Rang,
                        Plan6 = item.Plan6,
                        Commune = item.Commune,
                        Marche = item.Marche,
                        isLATE = isLate,
                        AVANCE = item.AVANCE,
                        NUMEROLIQUIDATION = item.NUMEROLIQUIDATION,
                    });
                }
                //var avalider = db.OPA_VALIDATIONS.Where(ecriture => ecriture.IDPROJET == suser.IDPROJET && ecriture.ETAT == 0).ToList();
                //var list = aFB160.getListEcritureCompta(journal, datein, dateout, comptaG, auxi, auxi1, dateP, suser).Where(x => avalider.Contains((int)x.No)).ToList();
                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succés.", data = list }, settings));
            }
            else
            {
                var avalider = db.OPA_VALIDATIONS.Where(ecriture => ecriture.IDPROJET == PROJECTID && ecriture.ETAT == 0).ToList();

                foreach (var item in avalider)
                {
                    bool isLate = false;
                    if (item.DATECREA.Value.AddBusinessDays(retarDate).Date < DateTime.Now/* && ((int)DateTime.Now.DayOfWeek) != 6 && ((int)DateTime.Now.DayOfWeek) != 0*/)
                        isLate = true;
                    if (item.AVANCE == true)
                    {
                        list.Add(new OPA_VALIDATIONS
                        {
                            IDREGLEMENT = item.IDREGLEMENT,
                            dateOrdre = item.dateOrdre,
                            NoPiece = item.NoPiece,
                            Compte = item.Compte,
                            Journal = item.Journal,
                            Credit = item.Credit,
                            Debit = item.Debit,
                            FinancementCategorie = item.FinancementCategorie,
                            MONTANT = item.MONTANT,
                            Mon = item.Mon,
                            MontantDevise = item.MontantDevise,
                            Rang = item.Rang,
                            Plan6 = item.Plan6,
                            Commune = item.Commune,
                            Marche = item.Marche,
                            isLATE = isLate,
                            AVANCE = item.AVANCE,
                            NUMEROLIQUIDATION = item.NUMEROLIQUIDATION,
                        });
                    }
                    else
                    {
                        list.Add(new OPA_VALIDATIONS
                        {
                            IDREGLEMENT = item.IDREGLEMENT,
                            dateOrdre = item.dateOrdre,
                            NoPiece = item.NoPiece,
                            Compte = item.Compte,
                            Journal = item.Journal,
                            Credit = item.Credit,
                            Debit = item.Debit,
                            FinancementCategorie = item.FinancementCategorie,
                            MONTANT = item.MONTANT,
                            Mon = item.Mon,
                            MontantDevise = item.MontantDevise,
                            Rang = item.Rang,
                            Plan6 = item.Plan6,
                            Commune = item.Commune,
                            Marche = item.Marche,
                            isLATE = isLate,
                            AVANCE = item.AVANCE,
                            NUMEROLIQUIDATION = item.NUMEROLIQUIDATION,
                        });
                    }
                }
                //var list = aFB160.getListEcritureBR(journal, datein, dateout, devise, comptaG, auxi, etat, dateP, suser).Where(x => avalider.ToString().Contains(x.No)).ToList();
                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succés.", data = list }, settings));
            }

        }
        //=========================================================================================TeacherValidation======================================================================

        [HttpPost]
        public JsonResult GetElementValiderF(string listCompte, SI_USERS suser, string codeproject)
        {
            AFB160 aFB160 = new AFB160();
            if (codeproject == "")
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Veuillez choisir un projet avant toutes actions. " }, settings));
            }
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));
            bool devise = false;
            int PROJECTID = int.Parse(codeproject);
            var basename = GetTypeP(suser, codeproject);
            if (basename == null)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Veuillez paramétrer le type d'ecriture avant toutes opérations. " }, settings));
            }
            //List<string> list = listCompte.Split(',').ToList();
            //List<string> numBR = listCompte.Split(',').ToList();
            var list = JsonConvert.DeserializeObject<List<AvanceDetails>>(listCompte);
            var numBR = JsonConvert.DeserializeObject<List<AvanceDetails>>(listCompte);

            var AvaliderList = new List<OPA_VALIDATIONS>();

            foreach (var item in list)
            {
                AvaliderList.Add(db.OPA_VALIDATIONS.Where(a => a.IDREGLEMENT == item.Id && a.ETAT == 0).FirstOrDefault());
            }

            int countTraitement = 0;
            var lien = "http://srvapp.softwell.cloud/softconnectsiig/";

            var ProjetIntitule = db.SI_PROJETS.Where(a => a.ID == PROJECTID).FirstOrDefault().PROJET;

            OPA_VALIDATIONS avalider = new OPA_VALIDATIONS();

            if (basename == "2")
            {
                aFB160.SaveValideSelectEcriture(list, true, suser, codeproject);
            }
            else
            {
                foreach (var item in AvaliderList)
                {

                    aFB160.SaveValideSelectEcritureBR(numBR, item.Journal, item.ETAT.ToString(), devise, suser, PROJECTID, (bool)item.AVANCE);
                }

            }
            if (basename == "2")
            {
                foreach (var item in list)
                {
                    int b = int.Parse(item.Id);
                    avalider = db.OPA_VALIDATIONS.Where(a => a.IDREGLEMENT == b.ToString() && a.ETAT == 0).FirstOrDefault();
                    if (avalider != null)
                    {
                        try
                        {
                            avalider.DATEACCEPT = DateTime.Now;
                            avalider.IDUSSEND = exist.ID;
                            avalider.ETAT = 1;
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Erreur de connexion", data = ex.Message }, settings));
                            throw;
                        }
                    }
                    countTraitement++;
                }
            }
            else
            {
                foreach (var item in numBR)
                {
                    //int b = int.Parse(item);
                    avalider = db.OPA_VALIDATIONS.Where(a => a.IDREGLEMENT == item.Id && a.ETAT == 0).FirstOrDefault();
                    if (avalider != null)
                    {
                        try
                        {
                            avalider.DATEACCEPT = DateTime.Now;
                            avalider.IDUSSEND = exist.ID;
                            avalider.ETAT = 1;
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Erreur de connexion", data = ex.Message }, settings));
                            throw;
                        }
                    }
                    countTraitement++;
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
                if (db.SI_MAIL.FirstOrDefault(a => a.IDPROJET == PROJECTID && a.DELETIONDATE == null).MAILTE != null)
                {
                    string[] separators = { ";" };

                    var Tomail = mail;
                    if (Tomail != null)
                    {
                        string listUser = db.SI_MAIL.FirstOrDefault(a => a.IDPROJET == PROJECTID && a.DELETIONDATE == null).MAILTE;
                        string[] mailListe = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var mailto in mailListe)
                        {
                            mail.To.Add(mailto);
                        }
                    }
                }

                mail.Subject = "Attente validation paiements du projet " + ProjetIntitule;
                mail.IsBodyHtml = true;
                mail.Body = "Madame, Monsieur,<br/><br>" + "Nous vous informons que vous avez " + countTraitement + " paiements en attente validation pour le compte du projet " + ProjetIntitule + ".<br/><br>" +
                    "Nous vous remercions de cliquer <a href='" + lien + "'>(ici)</a> pour accéder à la plate-forme SOFT-SIIG CONNECT.<br/><br>" + "Cordialement";

                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential(MailAdresse, mdpMail);
                smtp.EnableSsl = true;

                try { smtp.Send(mail); }
                catch (Exception) { }
            }
            return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succés. ", data = "" }, settings));
        }
        //ETAT = 1
        //=========================================================================================TeacherValidation======================================================================

        [HttpPost]
        public JsonResult GetAcceptecriture(string ChoixBase, string codeproject, DateTime datein, DateTime dateout, string comptaG, string auxi, string auxi1, DateTime dateP, string journal, string etat, bool devise, SI_USERS suser)
        {
            AFB160 aFB160 = new AFB160();
            int PROJECTID = int.Parse(codeproject);
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            int retarDate = 0;
            if (db.SI_DELAISTRAITEMENT.Any(a => a.IDPROJET == PROJECTID && a.DELETIONDATE == null))
                retarDate = db.SI_DELAISTRAITEMENT.FirstOrDefault(a => a.IDPROJET == PROJECTID && a.DELETIONDATE == null).DELPV.Value;
            List<OPA_VALIDATIONS> list = new List<OPA_VALIDATIONS>();

            var basename = GetTypeP(suser, codeproject);
            if (basename == "2")
            {
                var avalider = db.OPA_VALIDATIONS.Where(ecriture => ecriture.IDPROJET == PROJECTID && ecriture.ETAT == 1 && ecriture.ComptaG == comptaG && ecriture.auxi == auxi && ecriture.Journal == journal).ToList();
                foreach (var item in avalider)
                {
                    bool isLate = false;
                    if (item.DATEACCEPT.Value.AddBusinessDays(retarDate).Date < DateTime.Now/* && ((int)DateTime.Now.DayOfWeek) != 6 && ((int)DateTime.Now.DayOfWeek) != 0*/)
                        isLate = true;

                    list.Add(new OPA_VALIDATIONS
                    {
                        IDREGLEMENT = item.IDREGLEMENT,
                        dateOrdre = item.dateOrdre,
                        NoPiece = item.NoPiece,
                        Compte = item.Compte,
                        Libelle = item.Libelle,
                        Journal = item.Journal,
                        Credit = item.Credit,
                        Debit = item.Debit,
                        MONTANT = item.MONTANT,
                        FinancementCategorie = item.FinancementCategorie,
                        Mon = item.Mon,
                        MontantDevise = item.MontantDevise,
                        Rang = item.Rang,
                        Plan6 = item.Plan6,
                        Commune = item.Commune,
                        Marche = item.Marche,
                        isLATE = isLate,
                        AVANCE = item.AVANCE,
                        NUMEROLIQUIDATION = item.NUMEROLIQUIDATION,
                    });
                }
                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succés. ", data = list }, settings));
            }
            else
            {
                var avalider = db.OPA_VALIDATIONS.Where(ecriture => ecriture.IDPROJET == PROJECTID && ecriture.ETAT == 1 && ecriture.ComptaG == comptaG && ecriture.Journal == journal).ToList();
                foreach (var item in avalider)
                {
                    bool isLate = false;
                    if (item.DATEACCEPT.Value.AddBusinessDays(retarDate).Date < DateTime.Now/* && ((int)DateTime.Now.DayOfWeek) != 6 && ((int)DateTime.Now.DayOfWeek) != 0*/)
                        isLate = true;

                    list.Add(new OPA_VALIDATIONS
                    {
                        IDREGLEMENT = item.IDREGLEMENT,
                        dateOrdre = item.dateOrdre,
                        NoPiece = item.NoPiece,
                        Compte = item.Compte,
                        Libelle = item.Libelle,
                        Journal = item.Journal,
                        Credit = item.Credit,
                        Debit = item.Debit,
                        MONTANT = item.MONTANT,
                        FinancementCategorie = item.FinancementCategorie,
                        Mon = item.Mon,
                        MontantDevise = item.MontantDevise,
                        Rang = item.Rang,
                        Plan6 = item.Plan6,
                        Commune = item.Commune,
                        Marche = item.Marche,
                        isLATE = isLate,
                        AVANCE = item.AVANCE,
                        NUMEROLIQUIDATION = item.NUMEROLIQUIDATION,
                    });
                }

                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succés. ", data = list }, settings));
            }
        }
        [HttpPost]
        public JsonResult GetAcceptecritureLoad(SI_USERS suser, string codeproject)
        {
            AFB160 aFB160 = new AFB160();
            int PROJECTID = int.Parse(codeproject);
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            var basename = GetTypeP(suser, codeproject);
            if (basename == null)
            {
                return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Veuillez paramétrer le type d'ecriture avant toutes opérations. " }, settings));
            }

            int retarDate = 0;
            if (db.SI_DELAISTRAITEMENT.Any(a => a.IDPROJET == PROJECTID && a.DELETIONDATE == null))
                retarDate = db.SI_DELAISTRAITEMENT.FirstOrDefault(a => a.IDPROJET == PROJECTID && a.DELETIONDATE == null).DELPV.Value;
            List<OPA_VALIDATIONS> list = new List<OPA_VALIDATIONS>();

            if (basename == "2")
            {
                var avalider = db.OPA_VALIDATIONS.Where(ecriture => ecriture.IDPROJET == PROJECTID && ecriture.ETAT == 1).ToList();
                foreach (var item in avalider)
                {
                    bool isLate = false;
                    if (item.DATEACCEPT.Value.AddBusinessDays(retarDate).Date < DateTime.Now/* && ((int)DateTime.Now.DayOfWeek) != 6 && ((int)DateTime.Now.DayOfWeek) != 0*/)
                        isLate = true;

                    list.Add(new OPA_VALIDATIONS
                    {
                        IDREGLEMENT = item.IDREGLEMENT,
                        dateOrdre = item.dateOrdre,
                        NoPiece = item.NoPiece,
                        Compte = item.Compte,
                        Journal = item.Journal,
                        Credit = item.Credit,
                        Debit = item.Debit,
                        Libelle = item.Libelle,
                        FinancementCategorie = item.FinancementCategorie,
                        MONTANT = item.MONTANT,
                        Mon = item.Mon,
                        MontantDevise = item.MontantDevise,
                        Rang = item.Rang,
                        Plan6 = item.Plan6,
                        Commune = item.Commune,
                        Marche = item.Marche,
                        isLATE = isLate,
                        AVANCE = item.AVANCE,
                        NUMEROLIQUIDATION = item.NUMEROLIQUIDATION,
                    });
                }
                //var list = aFB160.getListEcritureCompta(journal, datein, dateout, comptaG, auxi, auxi1, dateP, suser).Where(x => avalider.Contains((int)x.No)).ToList();
                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succés. ", data = list }, settings));
            }
            else
            {
                var avalider = db.OPA_VALIDATIONS.Where(ecriture => ecriture.IDPROJET == PROJECTID && ecriture.ETAT == 1).ToList();
                foreach (var item in avalider)
                {
                    bool isLate = false;
                    if (item.DATECREA.Value.AddBusinessDays(retarDate).Date < DateTime.Now/* && ((int)DateTime.Now.DayOfWeek) != 6 && ((int)DateTime.Now.DayOfWeek) != 0*/)
                        isLate = true;

                    list.Add(new OPA_VALIDATIONS
                    {
                        IDREGLEMENT = item.IDREGLEMENT,
                        dateOrdre = item.dateOrdre,
                        NoPiece = item.NoPiece,
                        Compte = item.Compte,
                        Journal = item.Journal,
                        Credit = item.Credit,
                        Debit = item.Debit,
                        Libelle= item.Libelle,
                        FinancementCategorie = item.FinancementCategorie,
                        Mon = item.Mon,
                        MONTANT = item.MONTANT,
                        MontantDevise = item.MontantDevise,
                        Rang = item.Rang,
                        Plan6 = item.Plan6,
                        Commune = item.Commune,
                        Marche = item.Marche,
                        isLATE = isLate,
                        AVANCE = item.AVANCE,
                        NUMEROLIQUIDATION = item.NUMEROLIQUIDATION,
                    });
                }
                //var list = aFB160.getListEcritureBR(journal, datein, dateout, devise, comptaG, auxi, etat, dateP, suser).Where(x => avalider.ToString().Contains(x.No)).ToList();
                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succés. ", data = list }, settings));
            }
        }

        [HttpPost]
        public JsonResult GetAcceptecritureF(string listCompte, SI_USERS suser, string codeproject)
        {//validations
            AFB160 aFB160 = new AFB160();
            List<string> list = listCompte.Split(',').ToList();
            List<string> numBR = listCompte.Split(',').ToList();
            OPA_VALIDATIONS avalider = new OPA_VALIDATIONS();
            int PROJECTID = int.Parse(codeproject);
            int countTraitement = 0;
            var lien = "http://srvapp.softwell.cloud/softconnectsiig/";

            foreach (var item in list)
            {
                int b = int.Parse(item);
                avalider = db.OPA_VALIDATIONS.Where(a => a.IDREGLEMENT == b.ToString()).FirstOrDefault();
                if (avalider != null)
                {
                    try
                    {
                        avalider.IDREGLEMENT = item;
                        avalider.ETAT = 2;
                        avalider.DATESEND = DateTime.Now;
                        avalider.IDPROJET = suser.IDPROJET;
                        avalider.DateIn = avalider.DateIn;
                        avalider.DateOut = avalider.DateOut;
                        avalider.ComptaG = avalider.ComptaG;
                        avalider.auxi = avalider.auxi;
                        avalider.DateP = avalider.DateP;
                        avalider.Journal = avalider.Journal;
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Erreur de connexion. ", data = ex.Message }, settings));
                        throw;
                    }
                }
            }
            return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succés. ", data = "" }, settings));
        }
        //======================================================================================================EnvoyerValidations===============================================================

        [HttpPost]
        public JsonResult EnvoyeValidatioF(string ChoixBase, string codeproject, DateTime datein, DateTime dateout, string comptaG, string auxi, string auxi1, DateTime dateP, string journal, string etat, bool devise, SI_USERS suser)
        {
            AFB160 aFB160 = new AFB160();
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            // var basename = GetTypeP(suser, exist.IDPROJET.ToString());

            int PROJECTID = int.Parse(codeproject);

            int retarDate = 0;
            if (db.SI_DELAISTRAITEMENT.Any(a => a.IDPROJET == PROJECTID && a.DELETIONDATE == null))
                retarDate = db.SI_DELAISTRAITEMENT.FirstOrDefault(a => a.IDPROJET == PROJECTID && a.DELETIONDATE == null).DELPP.Value;
            List<OPA_VALIDATIONS> list = new List<OPA_VALIDATIONS>();

            if (ChoixBase == "2")
            {
                var HistoAFB = db.OPA_HISTORIQUEBR.Where(a => a.IDSOCIETE == PROJECTID).Select(x => x.NUMENREG).ToArray();
                var avalider = db.OPA_VALIDATIONS.Where(ecriture => ecriture.IDPROJET == PROJECTID && ecriture.ETAT == 2 && ecriture.ComptaG == comptaG  && ecriture.Journal == journal && !HistoAFB.Contains(ecriture.IDREGLEMENT.ToString())).ToList();
                foreach (var item in avalider)
                {
                    bool isLate = false;
                    if (item.DATESEND.Value.AddBusinessDays(retarDate).Date < DateTime.Now/* && ((int)DateTime.Now.DayOfWeek) != 6 && ((int)DateTime.Now.DayOfWeek) != 0*/)
                        isLate = true;

                    list.Add(new OPA_VALIDATIONS
                    {
                        IDREGLEMENT = item.IDREGLEMENT,
                        dateOrdre = item.dateOrdre,
                        NoPiece = item.NoPiece,
                        Compte = item.Compte,
                        Journal = item.Journal,
                        Credit = item.Credit,
                        Debit = item.Debit,
                        MONTANT = item.MONTANT,
                        FinancementCategorie = item.FinancementCategorie,
                        Mon = item.Mon,
                        MontantDevise = item.MontantDevise,
                        Rang = item.Rang,
                        Plan6 = item.Plan6,
                        Commune = item.Commune,
                        Marche = item.Marche,
                        isLATE = isLate,
                    });
                }
                //var list = aFB160.getListEcritureCompta(journal, datein, dateout, comptaG, auxi, auxi1, dateP, suser).Where(x => avalider.Contains((int)x.No)).ToList();
                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succés. ", data = list }, settings));
            }
            else
            {
                var HistoAFB = db.OPA_HISTORIQUEBR.Where(a => a.IDSOCIETE == PROJECTID).Select(x => x.NUMENREG.ToString()).ToArray();
                var avalider = db.OPA_VALIDATIONS.Where(ecriture => ecriture.IDPROJET == PROJECTID && ecriture.ETAT == 2 && ecriture.ComptaG == comptaG  && ecriture.Journal == journal && !HistoAFB.Contains(ecriture.IDREGLEMENT.ToString())).ToList();
                //var list = aFB160.getListEcritureBR(journal, datein, dateout, devise, comptaG, auxi, etat, dateP, suser).Where(x => avalider.ToString().Contains(x.No)).ToList();
                foreach (var item in avalider)
                {
                    bool isLate = false;
                    if (item.DATESEND.Value.AddBusinessDays(retarDate - 1).Date < DateTime.Now/* && ((int)DateTime.Now.DayOfWeek) != 6 && ((int)DateTime.Now.DayOfWeek) != 0*/)
                        isLate = true;

                    list.Add(new OPA_VALIDATIONS
                    {
                        IDREGLEMENT = item.IDREGLEMENT,
                        dateOrdre = item.dateOrdre,
                        NoPiece = item.NoPiece,
                        Compte = item.Compte,
                        Journal = item.Journal,
                        Credit = item.Credit,
                        Debit = item.Debit,
                        MONTANT = item.MONTANT,
                        FinancementCategorie = item.FinancementCategorie,
                        Mon = item.Mon,
                        MontantDevise = item.MontantDevise,
                        Rang = item.Rang,
                        Plan6 = item.Plan6,
                        Commune = item.Commune,
                        Marche = item.Marche,
                        isLATE = isLate,
                    });
                }
                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succés.  ", data = list }, settings));
            }
        }
        //======================================================================================================ValidationsEcrituresF===============================================================

        [HttpPost]
        public JsonResult ValidationsEcrituresF(string baseName, string codeproject, string listCompte, SI_USERS suser)
        {
            List<DataListTompro> listReg = new List<DataListTompro>();
            List<DataListTompro> listReg__ = new List<DataListTompro>();
            List<DataListTomOP> listRegBR = new List<DataListTomOP>();
            List<DataListTomOP> listRegBR__ = new List<DataListTomOP>();
            AFB160 aFB160 = new AFB160();
            int PROJECTID = int.Parse(codeproject);

            int countTraitement = 0;
            var lien = "http://srvapp.softwell.cloud/softconnectsiig/";
            var ProjetIntitule = db.SI_PROJETS.Where(a => a.ID == PROJECTID && a.DELETIONDATE == null).FirstOrDefault().PROJET;

            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));
            baseName = GetTypeP(suser, codeproject);
            if (baseName == "")
            {
                return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Veuillez paramétrer le type d'ecriture avant toutes opérations. " }, settings));
            }

            //List<string> list = listCompte.Split(',').ToList();
            var list = JsonConvert.DeserializeObject<List<AvanceDetails>>(listCompte);

            if (baseName == "2")
            {
                listReg = aFB160.getREGLEMENT(suser, PROJECTID);
            }
            else
            {
                listRegBR = aFB160.getREGLEMENTBR(suser, PROJECTID);
            }

            OPA_VALIDATIONS avalider = new OPA_VALIDATIONS();
            if (baseName == "2")
            {
                foreach (var item in list)
                {
                    int b = int.Parse(item.Id);
                    avalider = db.OPA_VALIDATIONS.Where(a => a.IDREGLEMENT == b.ToString()).FirstOrDefault();
                    if (avalider != null)
                    {
                        try
                        {
                            avalider.IDREGLEMENT = b.ToString();
                            avalider.ETAT = 2;
                            avalider.DATESEND = DateTime.Now.Date;
                            avalider.IDPROJET = PROJECTID;
                            avalider.DateIn = avalider.DateIn;
                            avalider.DateOut = avalider.DateOut;
                            avalider.ComptaG = avalider.ComptaG;
                            avalider.auxi = avalider.auxi;
                            avalider.DateP = avalider.DateP;
                            avalider.Journal = avalider.Journal;
                            avalider.DATEVAL = DateTime.Now;
                            avalider.IDUSVAL = exist.ID;

                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Erreur de connexion", data = ex.Message }, settings));
                            throw;
                        }
                        if (baseName == "2")
                        {
                            listReg__.Add(listReg.Where(a => (int)a.No == int.Parse(item.Id)).FirstOrDefault());
                        }
                        else
                        {

                            listRegBR__.Add(listRegBR.Where(a => a.No == item.Id).FirstOrDefault());
                        }
                    }
                    countTraitement++;
                }
            }
            else
            {
                foreach (var item in list)
                {
                    //int b = int.Parse(item);
                    avalider = db.OPA_VALIDATIONS.Where(a => a.IDREGLEMENT == item.Id).FirstOrDefault();
                    if (avalider != null)
                    {
                        try
                        {
                            avalider.IDREGLEMENT = item.Id;
                            avalider.ETAT = 2;
                            avalider.DATESEND = DateTime.Now.Date;
                            avalider.IDPROJET = PROJECTID;
                            avalider.DateIn = avalider.DateIn;
                            avalider.DateOut = avalider.DateOut;
                            avalider.ComptaG = avalider.ComptaG;
                            avalider.auxi = avalider.auxi;
                            avalider.DateP = avalider.DateP;
                            avalider.Journal = avalider.Journal;
                            avalider.DATEVAL = DateTime.Now;
                            avalider.IDUSVAL = exist.ID;

                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Erreur de connexion", data = ex.Message }, settings));
                            throw;
                        }
                        if (baseName == "2")
                        {
                            listReg__.Add(listReg.Where(a => (int)a.No == int.Parse(item.Id)).FirstOrDefault());
                        }
                        else
                        {

                            listRegBR__.Add(listRegBR.Where(a => a.No == item.Id).FirstOrDefault());
                        }
                    }
                    countTraitement++;
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
                if (db.SI_MAIL.FirstOrDefault(a => a.IDPROJET == PROJECTID && a.DELETIONDATE == null).MAILPV != null)
                {
                    string[] separators = { ";" };

                    var Tomail = mail;
                    if (Tomail != null)
                    {
                        string listUser = db.SI_MAIL.FirstOrDefault(a => a.IDPROJET == PROJECTID && a.DELETIONDATE == null).MAILPV;
                        string[] mailListe = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var mailto in mailListe)
                        {
                            mail.To.Add(mailto);
                        }
                    }
                }

                mail.Subject = "Validation paiement du projet " + ProjetIntitule;
                mail.IsBodyHtml = true;
                mail.Body = "Madame, Monsieur,<br/><br>" + "Nous vous informons que vous avez " + countTraitement + " paiement valider pour le compte du projet " + ProjetIntitule + ".<br/><br>" +
                    "Nous vous remercions de cliquer <a href='" + lien + "'>(ici)</a> pour accéder à la plate-forme SOFT-SIIG CONNECT.<br/><br>" + "Cordialement";

                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential(MailAdresse, mdpMail);
                smtp.EnableSsl = true;

                try { smtp.Send(mail); }
                catch (Exception) { }
            }

            if (baseName == "2")
            {
                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succés. ", data = listReg__ }, settings));
            }
            else
            {
                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succés. ", data = listRegBR__ }, settings));
            }

        }
        //END ETAT = 1
        //======================================================================================================Fvalidations===============================================================
        [HttpPost]
        public JsonResult LoadValidateEcriture(SI_USERS suser, string codeproject)
        {
            AFB160 aFB160 = new AFB160();
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));
            int PROJECTID = int.Parse(codeproject);

            int retarDate = 0;
            if (db.SI_DELAISTRAITEMENT.Any(a => a.IDPROJET == PROJECTID && a.DELETIONDATE == null))
                retarDate = db.SI_DELAISTRAITEMENT.FirstOrDefault(a => a.IDPROJET == PROJECTID && a.DELETIONDATE == null).DELPP.Value;
            List<OPA_VALIDATIONS> list = new List<OPA_VALIDATIONS>();

            var typeEcriture = db.SI_TYPECRITURE.Where(x => x.IDPROJET == PROJECTID).FirstOrDefault().TYPE;

            if (typeEcriture == 1)
            {
                //var HistoAFB = db.OPA_HISTORIQUEBR.Where(a => a.IDSOCIETE == PROJECTID).Select(x => x.NUMENREG).ToArray();
                //var val = db.OPA_VALIDATIONS.Where(a => a.DATESEND != null && a.IDPROJET == PROJECTID && a.ETAT == 2 && !HistoAFB.Contains(a.IDREGLEMENT.ToString())).ToList();
                var val = db.OPA_VALIDATIONS.Where(a => a.DATESEND != null && a.IDPROJET == PROJECTID && a.ETAT == 2).ToList();
                foreach (var item in val)
                {
                    bool isLate = false;
                    if (item.DATEVAL.Value.AddBusinessDays(retarDate).Date < DateTime.Now/* && ((int)DateTime.Now.DayOfWeek) != 6 && ((int)DateTime.Now.DayOfWeek) != 0*/)
                        isLate = true;

                    list.Add(new OPA_VALIDATIONS
                    {
                        IDREGLEMENT = item.IDREGLEMENT,
                        dateOrdre = item.dateOrdre,
                        NoPiece = item.NoPiece,
                        Compte = item.Compte,
                        Libelle = item.Libelle,
                        Journal = item.Journal,
                        Credit = item.Credit,
                        Debit = item.Debit,
                        MONTANT = item.MONTANT,
                        FinancementCategorie = item.FinancementCategorie,
                        Mon = item.Mon,
                        MontantDevise = item.MontantDevise,
                        Rang = item.Rang,
                        Plan6 = item.Plan6,
                        Commune = item.Commune,
                        Marche = item.Marche,
                        isLATE = isLate,
                    });
                }
            }
            else
            {
                //var HistoAFB = db.OPA_HISTORIQUE.Where(a => a.IDSOCIETE == PROJECTID).Select(x => x.NUMENREG.ToString()).ToArray();
                //var val = db.OPA_VALIDATIONS.Where(a => a.DATESEND != null && a.IDPROJET == PROJECTID && a.ETAT == 2 && !HistoAFB.Contains(a.IDREGLEMENT.ToString())).ToList();
                var val = db.OPA_VALIDATIONS.Where(a => a.DATESEND != null && a.IDPROJET == PROJECTID && a.ETAT == 2 ).ToList();
                foreach (var item in val)
                {
                    bool isLate = false;
                    if (item.DATEVAL.Value.AddBusinessDays(retarDate).Date < DateTime.Now/* && ((int)DateTime.Now.DayOfWeek) != 6 && ((int)DateTime.Now.DayOfWeek) != 0*/)
                        isLate = true;

                    list.Add(new OPA_VALIDATIONS
                    {
                        IDREGLEMENT = item.IDREGLEMENT,
                        dateOrdre = item.dateOrdre,
                        NoPiece = item.NoPiece,
                        Compte = item.Compte,
                        Libelle = item.Libelle,
                        Journal = item.Journal,
                        Credit = item.Credit,
                        Debit = item.Debit,
                        MONTANT = item.MONTANT,
                        FinancementCategorie = item.FinancementCategorie,
                        Mon = item.Mon,
                        MontantDevise = item.MontantDevise,
                        Rang = item.Rang,
                        Plan6 = item.Plan6,
                        Commune = item.Commune,
                        Marche = item.Marche,
                        isLATE = isLate,
                    });
                }
            }
            return Json(JsonConvert.SerializeObject(new { type = "Success", msg = "Connexion avec success. ", data = list }, settings));
        }
        //======================================================================================================Cancel===============================================================
        [HttpPost]
        public JsonResult CancelEcriture(string id, string motif, string commentaire, SI_USERS suser, string codeproject)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            int PROJECTID = int.Parse(codeproject);
            var cancel = db.OPA_VALIDATIONS.Where(x => x.IDREGLEMENT == id.ToString() && x.IDPROJET == PROJECTID).FirstOrDefault();
            //OPA_HCANCEL Hcancel = new OPA_HCANCEL();
            if (cancel != null)
            {
                if (motif != "")
                {
                    cancel.ETAT = 4;
                    cancel.MOTIF = motif;
                    cancel.COMS = commentaire;
                    cancel.DATEANNULER = DateTime.Now;
                    cancel.IDUSER = exist.ID;
                    //historique
                    //Hcancel.IDREGLEMENT = id;
                    //Hcancel.COMS = commentaire;
                    //Hcancel.MOTIF = motif;
                    //Hcancel.DATECANCEL = DateTime.Now;
                    //Hcancel.IDUSER = exist.ID;
                    //Hcancel.IDPROJETS = suser.IDPROJET;
                    //Hcancel.ETAT = 4;

                    //db.OPA_HCANCEL.Add(Hcancel);
                    db.SaveChanges();

                    var lien = "http://srvapp.softwell.cloud/softconnectsiig/";
                    var ProjetIntitule = db.SI_PROJETS.Where(a => a.ID == PROJECTID && a.DELETIONDATE == null).FirstOrDefault().PROJET;
                    //SEND MAIL ALERT et NOTIFICATION//
                    string MailAdresse = "serviceinfo@softwell.mg";
                    string mdpMail = "09eYpçç0601";

                    using (System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage())
                    {
                        SmtpClient smtp = new SmtpClient("smtpauth.moov.mg");
                        smtp.UseDefaultCredentials = true;

                        mail.From = new MailAddress(MailAdresse);

                        mail.To.Add(MailAdresse);
                        if (db.SI_MAIL.FirstOrDefault(a => a.IDPROJET == PROJECTID && a.DELETIONDATE == null).MAILREJETPAIE != null)
                        {
                            string[] separators = { ";" };

                            var Tomail = mail;
                            if (Tomail != null)
                            {
                                string listUser = db.SI_MAIL.FirstOrDefault(a => a.IDPROJET == PROJECTID && a.DELETIONDATE == null).MAILREJETPAIE;
                                string[] mailListe = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                foreach (var mailto in mailListe)
                                {
                                    mail.To.Add(mailto);
                                }
                            }
                        }

                        mail.Subject = "Rejet paiement du projet " + ProjetIntitule;
                        mail.IsBodyHtml = true;
                        mail.Body = "Madame, Monsieur,<br/><br>" + "Nous vous informons que vous avez un paiement rejeté pour le compte du projet " + ProjetIntitule + ".<br/><br>" +
                            "Nous vous remercions de cliquer <a href='" + lien + "'>(ici)</a> pour accéder à la plate-forme SOFT-SIIG CONNECT.<br/><br>" + "Cordialement";

                        smtp.Port = 587;
                        smtp.Credentials = new System.Net.NetworkCredential(MailAdresse, mdpMail);
                        smtp.EnableSsl = true;

                        try { smtp.Send(mail); }
                        catch (Exception) { }
                    }

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succés. ", data = "" }, settings));
                }
                else
                {
                    return Json(JsonConvert.SerializeObject(new { type = "error", msg = "Motif obligatoire. ", data = "" }, settings));
                }
            }

            return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succés .", data = "" }, settings));
        }
        public JsonResult GetCheckedComptePaie(string baseName, int mois, int annee, string listCompte, string matriculeD, string matriculeF, bool devise, DateTime dateP, string journal, SI_USERS suser)
        {
            if (string.IsNullOrEmpty(listCompte))
                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = "" }, settings));

            List<OPA_REGLEMENTBR> brResult = new List<OPA_REGLEMENTBR>();
            List<string> listReg = new List<string>();
            listReg = listCompte.Split(',').ToList();
            AFB160 aFB160 = new AFB160();
            try
            {

                aFB160.SaveValideSelectEcriturePaie(listReg, journal, devise, suser);
                //var zz = aFB160.getListEcriturePaie(journal, mois, annee, matriculeD, matriculeF, dateP, suser);
                var listePaie = aFB160.getREGLEMENTPaie(suser);
                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = listePaie }, settings));
            }
            catch (Exception ex)
            {

                return Json(JsonConvert.SerializeObject(new { type = "error", msg = ex.Message, data = ex.Message }, settings));
            }

        }
        //======================================================================================================GetAnomalieBack===============================================================
        public JsonResult GetAnomalieBack(SI_USERS suser, string baseName)
        {
            AFB160 Afb = new AFB160();
            if (baseName == "3")
            {
                //var anom = db.OPA_ANOMALIEBR.ToList();
                var resultAnomalies = Afb.getListAnomalieBR(suser);
                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = resultAnomalies }, settings));
            }
            else
            {
                //var anom = db.OPA_ANOMALIE.ToList();
                var resultAnomalies = Afb.getListAnomalie(suser);
                return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = resultAnomalies }, settings));
            }

        }
        //======================================================================================================GetAllProjectUser===============================================================
        public JsonResult GetAllProjectUser(SI_USERS suser)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null/* && a.IDSOCIETE == suser.IDSOCIETE*/);
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
        //======================================================================================================FTP===============================================================

        public void SENDFTP(string HOTE, string PATH, string USERFTP, string PWDFTP, string SOURCE)
        {
            DateTime now = DateTime.Now;
            FileStream fs = null;
            Stream rs = null;

            try
            {
                GC.Collect(0);
                int Size = 2048;/*8092*/

                Task T = Task.Run(() =>
                {
                    string file = SOURCE;
                    string uploadFileName = new FileInfo(file).Name;
                    string uploadUrl = String.Format("{0}/{1}/", "ftp://" + HOTE, PATH);
                    fs = new FileStream(file, FileMode.Open, FileAccess.Read);

                    string ftpUrl = string.Format("{0}/{1}", uploadUrl, uploadFileName);
                    FtpWebRequest requestObj = FtpWebRequest.Create(ftpUrl) as FtpWebRequest;
                    requestObj.Method = WebRequestMethods.Ftp.UploadFile;
                    requestObj.Credentials = new NetworkCredential(USERFTP, PWDFTP);

                    //ADD 17h Zoma
                    requestObj.UseBinary = true;
                    requestObj.UsePassive = true;
                    requestObj.KeepAlive = true;
                    //

                    rs = requestObj.GetRequestStream();

                    byte[] buffer = new byte[Size];
                    int read = 0;
                    while ((read = fs.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        rs.Write(buffer, 0, read);
                    }
                    rs.Flush();

                });

                if (T.IsCompleted)
                    return;

                T.Wait();

                GC.Collect(0);

                throw new OutOfMemoryException();
            }
            catch (Exception)
            {
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }

                if (rs != null)
                {
                    rs.Close();
                    rs.Dispose();
                }
            }
        }
        [HttpPost]
        public JsonResult GetHistoriques(SI_USERS suser)
        {

            var usr = db.SI_USERS.Where(x => x.LOGIN == suser.LOGIN && x.IDPROJET == suser.IDPROJET).FirstOrDefault();

            var query = db.OPA_HISTORIQUE
                .Where(x => x.IDSOCIETE == suser.IDPROJET && x.IDUSER == usr.ID)
                .Join(db.OPA_REGLEMENT, histo => histo.NUMENREG, reglement => reglement.NUM, (histo, reglement) => new
                {
                    NUMENREG = histo.NUMENREG,
                    DATEAFB = histo.DATEAFB,
                    IDUSER = histo.IDUSER,
                    IDSOCIETE = histo.IDSOCIETE,
                    CODE_J = reglement.CODE_J,
                    RIB = reglement.RIB,
                    MONTANT = reglement.MONTANT,
                    DATE = reglement.DATE,
                    LIBELLE = reglement.LIBELLE,
                    BANQUE = reglement.BANQUE,
                    GUICHET = reglement.GUICHET,
                }).Join(db.SI_USERS, x => x.IDUSER, user => user.ID, (x, user) => new
                {
                    NUMENREG = x.NUMENREG,
                    DATEAFB = x.DATEAFB,
                    IDUSER = x.IDUSER,
                    IDSOCIETE = x.IDSOCIETE,
                    CODE_J = x.CODE_J,
                    RIB = x.RIB,
                    MONTANT = x.MONTANT,
                    DATE = x.DATE,
                    LIBELLE = x.LIBELLE,
                    BANQUE = x.BANQUE,
                    GUICHET = x.GUICHET,
                    LOGIN = user.LOGIN
                })
                .OrderBy(x => x.DATE).ToList();
            var queryBr = db.OPA_HISTORIQUEBR
                .Where(x => x.IDSOCIETE == suser.IDPROJET && x.IDUSER == usr.ID)
                .Join(db.OPA_REGLEMENTBR, histo => histo.NUMENREG, reglement => reglement.NUM, (histo, reglement) => new
                {
                    NUMENREG = histo.NUMENREG,
                    DATEAFB = histo.DATEAFB,
                    IDUSER = histo.IDUSER,
                    IDSOCIETE = histo.IDSOCIETE,
                    CODE_J = reglement.CODE_J,
                    RIB = reglement.RIB,
                    MONTANT = reglement.MONTANT,
                    DATE = reglement.DATE,
                    LIBELLE = reglement.LIBELLE,
                    BANQUE = reglement.BANQUE,
                    GUICHET = reglement.GUICHET,
                }).Join(db.SI_USERS, x => x.IDUSER, user => user.ID, (x, user) => new
                {
                    NUMENREG = x.NUMENREG,
                    DATEAFB = x.DATEAFB,
                    IDUSER = x.IDUSER,
                    IDSOCIETE = x.IDSOCIETE,
                    CODE_J = x.CODE_J,
                    RIB = x.RIB,
                    MONTANT = x.MONTANT,
                    DATE = x.DATE,
                    LIBELLE = x.LIBELLE,
                    BANQUE = x.BANQUE,
                    GUICHET = x.GUICHET,
                    LOGIN = user.LOGIN
                })
                .OrderBy(x => x.DATE).ToList();

            return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = query, databr = queryBr }, settings));
        }
        public JsonResult GetCancel(SI_USERS suser, string listCompte)
        {
            var list = listCompte.Split(',');
            List<OPA_HISTORIQUE> result = new List<OPA_HISTORIQUE>();
            List<OPA_HISTORIQUEBR> resultBR = new List<OPA_HISTORIQUEBR>();
            var user = db.SI_USERS.Where(x => x.LOGIN == suser.LOGIN && x.PWD == suser.PWD).FirstOrDefault();
            foreach (var item in list)
            {
                if (item.Contains("BR"))
                {
                    resultBR = db.OPA_HISTORIQUEBR.Where(y => y.NUMENREG == item && y.IDUSER == user.ID && y.IDSOCIETE == suser.IDPROJET).ToList();
                }
                else
                {
                    decimal ii;
                    ii = Convert.ToDecimal(item);
                    result = db.OPA_HISTORIQUE.Where(x => x.NUMENREG == ii && x.IDUSER == user.ID && x.IDSOCIETE == suser.IDPROJET).ToList();

                }


                foreach (var pc in result)
                {
                    db.OPA_HISTORIQUE.Remove(pc);
                }
                foreach (var br in resultBR)
                {
                    db.OPA_HISTORIQUEBR.Remove(br);
                }
            }
            db.SaveChanges();
            return Json(JsonConvert.SerializeObject(new { msg = "success", data = result, datebr = resultBR }));
        }
        public JsonResult SFTP(string HOTE, string PATH, string USERFTP, string PWDFTP, string SOURCE)
        {
            //Console.WriteLine("Create client Object");
            using (SftpClient sftpClient = new SftpClient(getSftpConnection(HOTE, USERFTP, 22, SOURCE)))
            {
                //Console.WriteLine("Connect to server");
                sftpClient.Connect();
                //Console.WriteLine("Creating FileStream object to stream a file");
                using (FileStream fs = new FileStream("filePath", FileMode.Open))
                {
                    sftpClient.BufferSize = 1024;
                    sftpClient.UploadFile(fs, Path.GetFileName("filePath"));
                }
                sftpClient.Dispose();
            }
            return Json(JsonConvert.SerializeObject(new { type = "success", msg = "Connexion avec succès. ", data = "" }, settings));
        }

        private ConnectionInfo getSftpConnection(string hOTE, string uSERFTP, int v, string sOURCE)
        {
            throw new NotImplementedException();
        }
        private static AuthenticationMethod[] privateKeyObject(string username, string publicKeyPath)
        {
            PrivateKeyFile privateKeyFile = new PrivateKeyFile(publicKeyPath);
            PrivateKeyAuthenticationMethod privateKeyAuthenticationMethod =
               new PrivateKeyAuthenticationMethod(username, privateKeyFile);
            return new AuthenticationMethod[]
             {
        privateKeyAuthenticationMethod
             };
        }
    }
}
