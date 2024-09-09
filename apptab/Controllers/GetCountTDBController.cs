using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using apptab.Models;
using Newtonsoft.Json;

namespace apptab.Controllers
{
    public class GetCountTDBController : Controller
    {
        private readonly SOFTCONNECTSIIG db = new SOFTCONNECTSIIG();

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        [HttpPost]
        public ActionResult GetCountTDB(SI_USERS suser)
        {
            var exist = db.SI_USERS.FirstOrDefault(a => a.LOGIN == suser.LOGIN && a.PWD == suser.PWD && a.DELETIONDATE == null);
            if (exist == null) return Json(JsonConvert.SerializeObject(new { type = "login", msg = "Problème de connexion. " }, settings));

            try
            {
                var proj = new List<int>();

                var newElemH = new COUNTTDB()
                {
                    MandatI = 0,
                    MandatTR = 0,
                    MandatT = 0,
                    MandatV = 0,
                    MandatA = 0,
                    PaieR = 0,
                    PaieT = 0,
                    PaieV = 0,
                    PaieF = 0,
                    PaieA = 0,
                    MandatTRA = 0,
                    MandatTA = 0,
                    MandatVA = 0,
                    MandatAA = 0,
                };

                var test = db.SI_USERS.Where(x => x.LOGIN == exist.LOGIN && x.PWD == exist.PWD && x.DELETIONDATE == null).FirstOrDefault();

                var mandat = db.SI_TRAITPROJET.ToList();
                var ava = db.SI_TRAITAVANCE.ToList();

                var typedecriture = db.SI_TYPECRITURE.Where(a => a.IDUSER == test.ID).ToList();
                //List<ListPaimentV> payement = new List<ListPaimentV>();
                //foreach (var type in typedecriture)
                //{
                //    if (type.TYPE == 1)
                //    {
                //        payement = db.OPA_VALIDATIONS.Join(db.OPA_REGLEMENT, va => va.IDPROJET, re => re.IDSOCIETE, (va, re) => new ListPaimentV
                //        {
                //            ETAT = va.ETAT,
                //            IDPROJET = va.IDPROJET,
                //        }).Where(a => a.IDPROJET == type.IDPROJET).ToList();
                //    }
                //    else
                //    {
                //        payement = db.OPA_VALIDATIONS.Join(db.OPA_REGLEMENTBR, va => va.IDPROJET, re => re.IDSOCIETE, (va, re) => new ListPaimentV
                //        {
                //            ETAT = va.ETAT,
                //            IDPROJET = va.IDPROJET,
                //        }).Where(a => a.IDPROJET == type.IDPROJET).ToList();
                //    }
                //}
                List<OPA_VALIDATIONS> paiement = new List<OPA_VALIDATIONS>();
                if (test.ROLE == (int)Role.SAdministrateur)
                {
                    int mandatI = 0;
                    int mandatIA = 0;
                    int mandatRAFI = 0;
                    int mandatRAFA = 0;
                    var PRJ = db.SI_PROJETS.Select(a => new
                    {
                        PROJET = a.PROJET,
                        ID = a.ID,
                        DELETIONDATE = a.DELETIONDATE,
                    }).Where(a => a.DELETIONDATE == null).ToList();

                    foreach (var item in PRJ)
                    {
                        int crpt = item.ID;
                        //Check si le projet est mappé à une base de données TOM²PRO//
                        if (db.SI_MAPPAGES.FirstOrDefault(a => a.IDPROJET == crpt) == null)
                        {
                            continue;
                        }
                        paiement.AddRange(db.OPA_VALIDATIONS.Where(a => a.IDPROJET == item.ID).ToList());
                        SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
                        SOFTCONNECTOM tom = new SOFTCONNECTOM();

                        List<DATATRPROJET> list = new List<DATATRPROJET>();

                        //Check si la correspondance des états est OK//
                        var numCaEtapAPP = db.SI_PARAMETAT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null);
                        if (numCaEtapAPP == null)
                        {
                            continue;
                        }
                        //TEST si les états dans les paramètres dans cohérents avec ceux de TOM²PRO//
                        if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.DEF) == null)
                        {
                            continue;
                        }
                        if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.TEF) == null)
                        {
                            continue;
                        }
                        if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.BE) == null)
                        {
                            continue;
                        }

                        if (tom.CPTADMIN_CHAINETRAITEMENT_AVANCE.FirstOrDefault(a => a.NUM == numCaEtapAPP.DEF) == null)
                        {
                            continue;
                        }
                        if (tom.CPTADMIN_CHAINETRAITEMENT_AVANCE.FirstOrDefault(a => a.NUM == numCaEtapAPP.TEF) == null)
                        {
                            continue;
                        }
                        if (tom.CPTADMIN_CHAINETRAITEMENT_AVANCE.FirstOrDefault(a => a.NUM == numCaEtapAPP.BE) == null)
                        {
                            continue;
                        }

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
                                        mandatI++;
                                    }

                                    if (canBeDEF && canBeTEF && canBeBE)
                                    {
                                        if (!db.SI_TRAITPROJET.Any(a => a.No == x.ID && a.IDPROJET == crpt) || db.SI_TRAITPROJET.Any(a => a.No == x.ID && a.ETAT == 2 && a.IDPROJET == crpt))
                                            mandatRAFI++;
                                    }
                                }
                            }
                        }

                        if (tom.CPTADMIN_FAVANCE.Any())
                        {
                            foreach (var x in tom.CPTADMIN_FAVANCE.OrderBy(a => a.DATEAVANCE).ToList())
                            {
                                decimal MTN = 0;
                                decimal MTNPJ = 0;
                                var PCOP = "";

                                //Get total MTN dans CPTADMIN_MLIQUIDATION pour vérification du SOMMES MTN M = SOMMES MTN MPJ//
                                if (tom.CPTADMIN_MAVANCE.Any(a => a.IDAVANCE == x.ID))
                                {
                                    foreach (var y in tom.CPTADMIN_MAVANCE.Where(a => a.IDAVANCE == x.ID).ToList())
                                    {
                                        MTN += y.MONTANTLOCAL.Value;

                                        if (String.IsNullOrEmpty(PCOP))
                                            PCOP = y.POSTE;
                                    }
                                }

                                //TEST SI SOMMES MTN M = SOMMES MTN MPJ//
                                var IDString = x.ID.ToString();
                                if (tom.TP_MPIECES_JUSTIFICATIVES.Any(a => a.NUMERO_FICHE == IDString && a.MODULLE == "CPTADMINAVANCE"))
                                {
                                    foreach (var y in tom.TP_MPIECES_JUSTIFICATIVES.Where(a => a.NUMERO_FICHE == IDString && a.MODULLE == "CPTADMINAVANCE").ToList())
                                    {
                                        MTNPJ += y.MONTANT.Value;
                                    }
                                }

                                //MathRound 3 satria kely kokoa ny marge d'erreur no le 2//
                                if (Math.Round(MTN, 3) == Math.Round(MTNPJ, 3))
                                {
                                    //Check si F a déjà passé les 3 étapes (DEF, TEF et BE) pour avoir les dates => BE étape finale//
                                    var canBeDEFA = true;
                                    var canBeTEFA = true;
                                    var canBeBEA = true;
                                    if (tom.CPTADMIN_TRAITEMENT_AVANCE.FirstOrDefault(a => a.NUMEROAVANCE == x.NUMEROAVANCE && a.NUMEROETAPE == numCaEtapAPP.DEF) == null)
                                        canBeDEFA = false;
                                    if (tom.CPTADMIN_TRAITEMENT_AVANCE.FirstOrDefault(a => a.NUMEROAVANCE == x.NUMEROAVANCE && a.NUMEROETAPE == numCaEtapAPP.TEF) == null)
                                        canBeTEFA = false;
                                    if (tom.CPTADMIN_TRAITEMENT_AVANCE.FirstOrDefault(a => a.NUMEROAVANCE == x.NUMEROAVANCE && a.NUMEROETAPE == numCaEtapAPP.BE) == null)
                                        canBeBEA = false;

                                    //TEST que F n'est pas encore traité ou F a été annulé// ETAT annulé = 2//
                                    if (canBeDEFA == false || canBeTEFA == false || canBeBEA == false)
                                    {
                                        mandatIA++;
                                    }

                                    if (canBeDEFA && canBeTEFA && canBeBEA)
                                    {
                                        if (!db.SI_TRAITAVANCE.Any(a => a.No == x.ID && a.IDPROJET == crpt) || db.SI_TRAITPROJET.Any(a => a.No == x.ID && a.ETAT == 2 && a.IDPROJET == crpt))
                                            mandatRAFA++;
                                    }
                                }
                            }
                        }
                    }

                    newElemH = new COUNTTDB()
                    {
                        MandatT = mandatI,
                        MandatV = mandat.Where(a => a.ETAT == 0).Count(),//ok
                        MandatA = mandatRAFI,
                        MandatTR = mandat.Where(a => a.ETAT == 1).Count(),//ok
                        PaieR = paiement.Where(a => a.ETAT == 0).Count(),//ok
                        PaieT = paiement.Where(a => a.ETAT == 1).Count(),//ok
                        PaieV = paiement.Where(a => a.ETAT == 2).Count(),//ok
                        PaieF = paiement.Where(a => a.ETAT == 3).Count(),//ok
                        //PaieA = paiement.Where(a => a.ETAT == 4).Count(),
                        MandatTA = mandatIA,
                        MandatVA = ava.Where(a => a.ETAT == 0).Count(),//ok
                        MandatAA = mandatRAFA,
                        MandatTRA = ava.Where(a => a.ETAT == 1).Count(),//ok
                    };

                    return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = newElemH }, settings));
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

                        int mandatI = 0;
                        int mandatIA = 0;
                        int mandatRAFI = 0;
                        int mandatRAFA = 0;

                        paiement.AddRange(db.OPA_VALIDATIONS.Where(a => a.IDPROJET == test.IDPROJET).ToList());
                        foreach (var x in user)
                        {
                            proj.Add(x.ID);
                        }

                        foreach (var item in proj)
                        {
                            int crpt = item;
                            //Check si le projet est mappé à une base de données TOM²PRO//
                            if (db.SI_MAPPAGES.FirstOrDefault(a => a.IDPROJET == crpt) == null)
                            {
                                continue;
                            }

                            SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
                            SOFTCONNECTOM tom = new SOFTCONNECTOM();

                            List<DATATRPROJET> list = new List<DATATRPROJET>();

                            //Check si la correspondance des états est OK//
                            var numCaEtapAPP = db.SI_PARAMETAT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null);
                            if (numCaEtapAPP == null)
                            {
                                continue;
                            }
                            //TEST si les états dans les paramètres dans cohérents avec ceux de TOM²PRO//
                            if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.DEF) == null)
                            {
                                continue;
                            }
                            if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.TEF) == null)
                            {
                                continue;
                            }
                            if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.BE) == null)
                            {
                                continue;
                            }

                            if (tom.CPTADMIN_CHAINETRAITEMENT_AVANCE.FirstOrDefault(a => a.NUM == numCaEtapAPP.DEF) == null)
                            {
                                continue;
                            }
                            if (tom.CPTADMIN_CHAINETRAITEMENT_AVANCE.FirstOrDefault(a => a.NUM == numCaEtapAPP.TEF) == null)
                            {
                                continue;
                            }
                            if (tom.CPTADMIN_CHAINETRAITEMENT_AVANCE.FirstOrDefault(a => a.NUM == numCaEtapAPP.BE) == null)
                            {
                                continue;
                            }

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
                                            mandatI++;
                                        }

                                        if (canBeDEF && canBeTEF && canBeBE)
                                        {
                                            if (!db.SI_TRAITPROJET.Any(a => a.No == x.ID && a.IDPROJET == crpt) || db.SI_TRAITPROJET.Any(a => a.No == x.ID && a.ETAT == 2 && a.IDPROJET == crpt))
                                                mandatRAFI++;
                                        }
                                    }
                                }
                            }

                            if (tom.CPTADMIN_FAVANCE.Any())
                            {
                                foreach (var x in tom.CPTADMIN_FAVANCE.OrderBy(a => a.DATEAVANCE).ToList())
                                {
                                    decimal MTN = 0;
                                    decimal MTNPJ = 0;
                                    var PCOP = "";

                                    //Get total MTN dans CPTADMIN_MLIQUIDATION pour vérification du SOMMES MTN M = SOMMES MTN MPJ//
                                    if (tom.CPTADMIN_MAVANCE.Any(a => a.IDAVANCE == x.ID))
                                    {
                                        foreach (var y in tom.CPTADMIN_MAVANCE.Where(a => a.IDAVANCE == x.ID).ToList())
                                        {
                                            MTN += y.MONTANTLOCAL.Value;

                                            if (String.IsNullOrEmpty(PCOP))
                                                PCOP = y.POSTE;
                                        }
                                    }

                                    //TEST SI SOMMES MTN M = SOMMES MTN MPJ//
                                    var IDString = x.ID.ToString();
                                    if (tom.TP_MPIECES_JUSTIFICATIVES.Any(a => a.NUMERO_FICHE == IDString && a.MODULLE == "CPTADMINAVANCE"))
                                    {
                                        foreach (var y in tom.TP_MPIECES_JUSTIFICATIVES.Where(a => a.NUMERO_FICHE == IDString && a.MODULLE == "CPTADMINAVANCE").ToList())
                                        {
                                            MTNPJ += y.MONTANT.Value;
                                        }
                                    }

                                    //MathRound 3 satria kely kokoa ny marge d'erreur no le 2//
                                    if (Math.Round(MTN, 3) == Math.Round(MTNPJ, 3))
                                    {
                                        //Check si F a déjà passé les 3 étapes (DEF, TEF et BE) pour avoir les dates => BE étape finale//
                                        var canBeDEFA = true;
                                        var canBeTEFA = true;
                                        var canBeBEA = true;
                                        if (tom.CPTADMIN_TRAITEMENT_AVANCE.FirstOrDefault(a => a.NUMEROAVANCE == x.NUMEROAVANCE && a.NUMEROETAPE == numCaEtapAPP.DEF) == null)
                                            canBeDEFA = false;
                                        if (tom.CPTADMIN_TRAITEMENT_AVANCE.FirstOrDefault(a => a.NUMEROAVANCE == x.NUMEROAVANCE && a.NUMEROETAPE == numCaEtapAPP.TEF) == null)
                                            canBeTEFA = false;
                                        if (tom.CPTADMIN_TRAITEMENT_AVANCE.FirstOrDefault(a => a.NUMEROAVANCE == x.NUMEROAVANCE && a.NUMEROETAPE == numCaEtapAPP.BE) == null)
                                            canBeBEA = false;

                                        //TEST que F n'est pas encore traité ou F a été annulé// ETAT annulé = 2//
                                        if (canBeDEFA == false || canBeTEFA == false || canBeBEA == false)
                                        {
                                            mandatIA++;
                                        }

                                        if (canBeDEFA && canBeTEFA && canBeBEA)
                                        {
                                            if (!db.SI_TRAITAVANCE.Any(a => a.No == x.ID && a.IDPROJET == crpt) || db.SI_TRAITPROJET.Any(a => a.No == x.ID && a.ETAT == 2 && a.IDPROJET == crpt))
                                                mandatRAFA++;
                                        }
                                    }
                                }
                            }
                        }

                        mandat = mandat.Where(a => a.IDPROJET == test.IDPROJET).ToList();
                        ava = ava.Where(a => a.IDPROJET == test.IDPROJET).ToList();
                        //paiement = paiement.Where(a => a.IDPROJET == test.IDPROJET).ToList();
                        newElemH = new COUNTTDB()
                        {
                            MandatT = mandatI,
                            MandatV = mandat.Where(a => a.ETAT == 0).Count(),//ok
                            MandatA = mandatRAFI,
                            MandatTR = mandat.Where(a => a.ETAT == 1).Count(),//ok
                            PaieR = paiement.Where(a => a.ETAT == 0).Count(),//ok
                            PaieT = paiement.Where(a => a.ETAT == 1).Count(),//ok
                            PaieV = paiement.Where(a => a.ETAT == 2).Count(),//ok
                            PaieF = paiement.Where(a => a.ETAT == 3).Count(),//ok
                            //PaieA = paiement.Where(a => a.ETAT == 4).Count(),
                            MandatTA = mandatIA,
                            MandatVA = ava.Where(a => a.ETAT == 0).Count(),//ok
                            MandatAA = mandatRAFA,
                            MandatTRA = ava.Where(a => a.ETAT == 1).Count(),//ok
                        };

                        return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = newElemH }, settings));
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

                        int mandatI = 0;
                        int mandatIA = 0;
                        int mandatRAFI = 0;
                        int mandatRAFA = 0;

                        foreach (var x in user)
                        {
                            proj.Add(x.ID);
                        }

                        foreach (var z in proj)
                        {
                            int crpt = z;
                            //Check si le projet est mappé à une base de données TOM²PRO//
                            if (db.SI_MAPPAGES.FirstOrDefault(a => a.IDPROJET == crpt) == null)
                            {
                                continue;
                            }

                            SOFTCONNECTOM.connex = new Data.Extension().GetCon(crpt);
                            SOFTCONNECTOM tom = new SOFTCONNECTOM();

                            List<DATATRPROJET> list = new List<DATATRPROJET>();

                            //Check si la correspondance des états est OK//
                            var numCaEtapAPP = db.SI_PARAMETAT.FirstOrDefault(a => a.IDPROJET == crpt && a.DELETIONDATE == null);
                            if (numCaEtapAPP == null)
                            {
                                continue;
                            }
                            //TEST si les états dans les paramètres dans cohérents avec ceux de TOM²PRO//
                            if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.DEF) == null)
                            {
                                continue;
                            }
                            if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.TEF) == null)
                            {
                                continue;
                            }
                            if (tom.CPTADMIN_CHAINETRAITEMENT.FirstOrDefault(a => a.NUM == numCaEtapAPP.BE) == null)
                            {
                                continue;
                            }

                            if (tom.CPTADMIN_CHAINETRAITEMENT_AVANCE.FirstOrDefault(a => a.NUM == numCaEtapAPP.DEF) == null)
                            {
                                continue;
                            }
                            if (tom.CPTADMIN_CHAINETRAITEMENT_AVANCE.FirstOrDefault(a => a.NUM == numCaEtapAPP.TEF) == null)
                            {
                                continue;
                            }
                            if (tom.CPTADMIN_CHAINETRAITEMENT_AVANCE.FirstOrDefault(a => a.NUM == numCaEtapAPP.BE) == null)
                            {
                                continue;
                            }

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
                                            mandatI++;
                                        }

                                        if (canBeDEF && canBeTEF && canBeBE)
                                        {
                                            if (!db.SI_TRAITPROJET.Any(a => a.No == x.ID && a.IDPROJET == crpt) || db.SI_TRAITPROJET.Any(a => a.No == x.ID && a.ETAT == 2 && a.IDPROJET == crpt))
                                                mandatRAFI++;
                                        }
                                    }
                                }
                            }

                            if (tom.CPTADMIN_FAVANCE.Any())
                            {
                                foreach (var x in tom.CPTADMIN_FAVANCE.OrderBy(a => a.DATEAVANCE).ToList())
                                {
                                    decimal MTN = 0;
                                    decimal MTNPJ = 0;
                                    var PCOP = "";

                                    //Get total MTN dans CPTADMIN_MLIQUIDATION pour vérification du SOMMES MTN M = SOMMES MTN MPJ//
                                    if (tom.CPTADMIN_MAVANCE.Any(a => a.IDAVANCE == x.ID))
                                    {
                                        foreach (var y in tom.CPTADMIN_MAVANCE.Where(a => a.IDAVANCE == x.ID).ToList())
                                        {
                                            MTN += y.MONTANTLOCAL.Value;

                                            if (String.IsNullOrEmpty(PCOP))
                                                PCOP = y.POSTE;
                                        }
                                    }

                                    //TEST SI SOMMES MTN M = SOMMES MTN MPJ//
                                    var IDString = x.ID.ToString();
                                    if (tom.TP_MPIECES_JUSTIFICATIVES.Any(a => a.NUMERO_FICHE == IDString && a.MODULLE == "CPTADMINAVANCE"))
                                    {
                                        foreach (var y in tom.TP_MPIECES_JUSTIFICATIVES.Where(a => a.NUMERO_FICHE == IDString && a.MODULLE == "CPTADMINAVANCE").ToList())
                                        {
                                            MTNPJ += y.MONTANT.Value;
                                        }
                                    }

                                    //MathRound 3 satria kely kokoa ny marge d'erreur no le 2//
                                    if (Math.Round(MTN, 3) == Math.Round(MTNPJ, 3))
                                    {
                                        //Check si F a déjà passé les 3 étapes (DEF, TEF et BE) pour avoir les dates => BE étape finale//
                                        var canBeDEFA = true;
                                        var canBeTEFA = true;
                                        var canBeBEA = true;
                                        if (tom.CPTADMIN_TRAITEMENT_AVANCE.FirstOrDefault(a => a.NUMEROAVANCE == x.NUMEROAVANCE && a.NUMEROETAPE == numCaEtapAPP.DEF) == null)
                                            canBeDEFA = false;
                                        if (tom.CPTADMIN_TRAITEMENT_AVANCE.FirstOrDefault(a => a.NUMEROAVANCE == x.NUMEROAVANCE && a.NUMEROETAPE == numCaEtapAPP.TEF) == null)
                                            canBeTEFA = false;
                                        if (tom.CPTADMIN_TRAITEMENT_AVANCE.FirstOrDefault(a => a.NUMEROAVANCE == x.NUMEROAVANCE && a.NUMEROETAPE == numCaEtapAPP.BE) == null)
                                            canBeBEA = false;

                                        //TEST que F n'est pas encore traité ou F a été annulé// ETAT annulé = 2//
                                        if (canBeDEFA == false || canBeTEFA == false || canBeBEA == false)
                                        {
                                            mandatIA++;
                                        }

                                        if (canBeDEFA && canBeTEFA && canBeBEA)
                                        {
                                            if (!db.SI_TRAITPROJET.Any(a => a.No == x.ID && a.IDPROJET == crpt) || db.SI_TRAITPROJET.Any(a => a.No == x.ID && a.ETAT == 2 && a.IDPROJET == crpt))
                                                mandatRAFA++;
                                        }
                                    }
                                }
                            }

                            mandat = mandat.Where(a => a.IDPROJET == z).ToList();
                            ava = ava.Where(a => a.IDPROJET == z).ToList();
                            paiement = paiement.Where(a => a.IDPROJET == z).ToList();
                            newElemH = new COUNTTDB()
                            {
                                MandatT = mandatI,
                                MandatV = mandat.Where(a => a.ETAT == 0).Count(),//ok
                                MandatA = mandatRAFI,
                                MandatTR = mandat.Where(a => a.ETAT == 1).Count(),//ok
                                PaieR = paiement.Where(a => a.ETAT == 0).Count(),//ok
                                PaieT = paiement.Where(a => a.ETAT == 1).Count(),//ok
                                PaieV = paiement.Where(a => a.ETAT == 2).Count(),//ok
                                PaieF = paiement.Where(a => a.ETAT == 3).Count(),//ok
                                PaieA = paiement.Where(a => a.ETAT == 4).Count(),
                                MandatTA = mandatIA,
                                MandatVA = ava.Where(a => a.ETAT == 0).Count(),//ok
                                MandatAA = mandatRAFA,
                                MandatTRA = ava.Where(a => a.ETAT == 1).Count(),//ok
                            };
                        }

                        return Json(JsonConvert.SerializeObject(new { type = "success", msg = "message", data = newElemH }, settings));
                    }
                }
            }
            catch (Exception e)
            {
                return Json(JsonConvert.SerializeObject(new { type = "error", msg = e.Message }, settings));
            }
        }
    }
}
