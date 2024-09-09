namespace apptab
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RPROJET")]
    public partial class RPROJET
    {
        [Key]
        public decimal IDPROJET { get; set; }

        [Required]
        [StringLength(30)]
        public string SIGLE { get; set; }

        [StringLength(200)]
        public string NOM { get; set; }

        [StringLength(50)]
        public string ADRESSE1 { get; set; }

        [StringLength(50)]
        public string ADRESSE2 { get; set; }

        [StringLength(50)]
        public string VILLE { get; set; }

        [StringLength(50)]
        public string PAYS { get; set; }

        [StringLength(50)]
        public string TEL { get; set; }

        [StringLength(50)]
        public string FAX { get; set; }

        [StringLength(50)]
        public string EMAIL { get; set; }

        [StringLength(3)]
        public string LIB1 { get; set; }

        [StringLength(3)]
        public string LIB2 { get; set; }

        public bool? LIB1ACTIF { get; set; }

        public bool SUIVIACTI { get; set; }

        public bool SUIVIBUD { get; set; }

        public bool SUIVIFIN { get; set; }

        public bool SUIVIGEO { get; set; }

        public bool SUIVIPLAN6 { get; set; }

        public bool SOUSCATEGORIE { get; set; }

        public bool GESTIONBR { get; set; }

        public bool GESTIONNOPIECE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NOIMMO { get; set; }

        public bool GESTIONECHEANCE { get; set; }

        public bool INCREMENTDP { get; set; }

        [StringLength(30)]
        public string MONNAIELOC { get; set; }

        [StringLength(10)]
        public string COURSDEV { get; set; }

        [StringLength(3)]
        public string MONNAIEREP { get; set; }

        public decimal? COURSREP { get; set; }

        [StringLength(3)]
        public string MONNAIERAPP { get; set; }

        [StringLength(3)]
        public string MONNAIEBUD { get; set; }

        [StringLength(20)]
        public string FRAISBQ1 { get; set; }

        [StringLength(20)]
        public string FRAISBQ2 { get; set; }

        [StringLength(20)]
        public string FRAISBQ3 { get; set; }

        public bool RESEAU { get; set; }

        public bool MULTIBASE { get; set; }

        [StringLength(3)]
        public string LANGUE { get; set; }

        public bool MULTISITE { get; set; }

        public bool DRFIDA { get; set; }

        public bool DRFLACI { get; set; }

        public bool DRFFED { get; set; }

        public bool DRFBOAD { get; set; }

        public bool DRFBAD { get; set; }

        public bool DRFFIDA { get; set; }

        public bool DRFSTAND { get; set; }

        public bool SYSCOA { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NBJOURMIN1 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NBJOURMAX1 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NBJOURMIN2 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NBJOURMAX2 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NBJOURMIN3 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NBJOURMAX3 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NBJOURMIN4 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NBJOURMAX4 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NBJOURMIN5 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NBJOURMAX5 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NIVACTI { get; set; }

        [StringLength(250)]
        public string NOM2 { get; set; }

        [StringLength(10)]
        public string VERSION { get; set; }

        [StringLength(50)]
        public string OS { get; set; }

        [Column(TypeName = "image")]
        public byte[] LOGO1 { get; set; }

        [StringLength(50)]
        public string TITRE1 { get; set; }

        [Column(TypeName = "image")]
        public byte[] LOGO2 { get; set; }

        [StringLength(50)]
        public string TITRE2 { get; set; }

        public int? TIMER { get; set; }

        [StringLength(2)]
        public string SECTIONS { get; set; }

        [StringLength(10)]
        public string AXECATALOGUE { get; set; }

        public bool? ELABORATIONBUDGET { get; set; }

        public int? NBREARBITRAGE { get; set; }

        public bool? COMPTAACTI { get; set; }

        public bool? SUIVISOURCEFIN { get; set; }

        public bool? MODPARAMETRE { get; set; }

        public bool? MODCOMPTAGEN { get; set; }

        public bool? MODCOMPTAANAL { get; set; }

        public bool? MODSUIVIBUD { get; set; }

        public bool? MODSUIVICONV { get; set; }

        public bool? MODIMMO { get; set; }

        public bool? MODETATFIN { get; set; }

        public bool? MODMARCHE { get; set; }

        public bool? MODDECAISSEMENT { get; set; }

        public bool? MODUTILITAIRE { get; set; }

        public bool? MODPASSMARCHE { get; set; }

        public bool? MODSTOCK { get; set; }

        public DateTime? DATECRE { get; set; }

        public DateTime? DATEMAJ { get; set; }

        [StringLength(10)]
        public string USERCRE { get; set; }

        [StringLength(10)]
        public string USERMAJ { get; set; }

        public bool? ALERTEECHEANCIER { get; set; }

        public bool? APPROVISIONNEMENT { get; set; }

        public bool? GESTIONTRESORERIE { get; set; }

        [StringLength(10)]
        public string MODDEPRECIATION { get; set; }

        public bool? GESTIONCLOTURERAPPRO { get; set; }

        public bool? GESTIONTAXEMARCHE { get; set; }

        public bool? GESTVEREXPORT { get; set; }

        public bool? ACCEPTNULLCOMPTA { get; set; }

        public bool? DRFFID { get; set; }

        public bool? CMOD { get; set; }

        public bool? COMPTABUDGETAIRE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? INCREMENTAUTOMARCHE { get; set; }

        [StringLength(100)]
        public string CLE { get; set; }

        public bool? DRFBID { get; set; }

        public bool? FED { get; set; }

        public bool? BAD { get; set; }

        public bool? FIDA { get; set; }

        public bool? IDA { get; set; }

        public bool? FID { get; set; }

        [StringLength(3)]
        public string code_pays { get; set; }

        [StringLength(2)]
        public string code_site { get; set; }

        [StringLength(4)]
        public string annee { get; set; }

        [StringLength(255)]
        public string siteweb { get; set; }

        [StringLength(255)]
        public string commentaire { get; set; }

        public decimal? TAUX_TVA { get; set; }

        public decimal? TAUX_AIR { get; set; }

        [StringLength(255)]
        public string ARCHIVE { get; set; }

        [StringLength(255)]
        public string MYXML { get; set; }

        [StringLength(255)]
        public string MYREPX { get; set; }

        [StringLength(255)]
        public string MYRTF { get; set; }

        public bool IMPRESSIONBORD { get; set; }

        public bool IMPRESSIONCHK { get; set; }

        public bool GESTBASEMAITRE { get; set; }

        public bool GESTRATESOURCEFIN { get; set; }

        public bool EPIC { get; set; }

        public bool FACE { get; set; }

        public bool KPI { get; set; }

        [StringLength(30)]
        public string AXESF { get; set; }

        [StringLength(100)]
        public string AXESFLIB { get; set; }

        [StringLength(1)]
        public string TFRSTATUT { get; set; }

        [StringLength(255)]
        public string ADRESSESERVEUR { get; set; }

        [StringLength(50)]
        public string NOMBASE { get; set; }

        [StringLength(50)]
        public string PORTNUMBER { get; set; }

        [StringLength(50)]
        public string NOMUTILISATEUR { get; set; }

        [StringLength(50)]
        public string MOTDEPASSE { get; set; }

        public bool? SUIVIPLAN7 { get; set; }

        public bool? LIGNEBUD { get; set; }

        public bool? ParamPaie_estIndice { get; set; }

        public bool? KPICONSO { get; set; }

        [StringLength(10)]
        public string FORMATRPT { get; set; }

        public bool? DRFAFD { get; set; }

        public bool? DRFBIDS { get; set; }

        public bool? GESTMINUSCULE { get; set; }

        [StringLength(50)]
        public string PWSUPPBORD { get; set; }

        [StringLength(50)]
        public string PWDELETTRAGE { get; set; }

        [StringLength(50)]
        public string PWANOUVEAU { get; set; }

        [StringLength(50)]
        public string PWEXPORT { get; set; }

        [StringLength(50)]
        public string PWCLORAPPRO { get; set; }

        [StringLength(50)]
        public string PWCLOBUDGET { get; set; }

        public bool? DRFGF { get; set; }

        [StringLength(50)]
        public string SIGNATAIREEFSTD { get; set; }

        [StringLength(50)]
        public string SIGNATAIREEFIFR { get; set; }

        [StringLength(50)]
        public string PWANNULATIONDRF { get; set; }

        [StringLength(200)]
        public string PIECESCOMPTABLES { get; set; }

        public bool? CNPS { get; set; }

        public bool? CAA { get; set; }

        public bool? TOMON { get; set; }

        public bool? KPIMARCHE { get; set; }

        [StringLength(50)]
        public string CODECAA { get; set; }

        [StringLength(20)]
        public string TYPELIGNEBUD { get; set; }

        public bool? COGEBUD { get; set; }

        public bool? COGEACTI { get; set; }

        public bool? ACTIGEO { get; set; }

        public bool? ACTIPLAN6 { get; set; }

        [StringLength(30)]
        public string LIBCATEGORIE { get; set; }

        [StringLength(30)]
        public string LIBSOUSCATEGORIE { get; set; }

        public bool? DRFMCA { get; set; }

        public bool? GESTBUDNEG { get; set; }

        public bool? DPOBLIGATOIRE { get; set; }

        [StringLength(20)]
        public string PREFIXEMARCHE { get; set; }

        [StringLength(20)]
        public string PREFIXEBC { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? INCREMENTAUTOBC { get; set; }

        public bool? GESTSUIVIQTE { get; set; }

        public bool? DRFDPP { get; set; }

        public bool? DRFKFW { get; set; }

        public bool? DRFASIANDB { get; set; }

        [StringLength(30)]
        public string COMPTAADD1 { get; set; }

        public bool? GESTIONCAISSE { get; set; }

        [StringLength(20)]
        public string COMPTAADD1AXE { get; set; }

        public bool? GESTIONREFPAIEMENT { get; set; }

        public bool? GESTIONFCEFCL { get; set; }

        public bool? SYSTNATIONAL { get; set; }

        [StringLength(50)]
        public string GED { get; set; }

        [StringLength(50)]
        public string DOMAINE { get; set; }

        public bool? DRFSITE { get; set; }

        public bool? COURSSITE { get; set; }

        [StringLength(50)]
        public string PWSUPPIMPORT { get; set; }

        public bool? DMDACHAT { get; set; }

        public bool? FLAGFUSIONBASE { get; set; }

        public bool? GESTIONOP { get; set; }

        [StringLength(1)]
        public string MONTOPBUD { get; set; }

        [StringLength(1)]
        public string MONTOPMARCHE { get; set; }

        [StringLength(1)]
        public string MONTMARCHEBUD { get; set; }

        public bool? SUIVIPLAN8 { get; set; }

        public bool? EF2 { get; set; }

        [StringLength(20)]
        public string AXELIAISON { get; set; }

        [StringLength(20)]
        public string COMPTETVA { get; set; }

        public bool? GESTIONREFRELEVE { get; set; }

        public bool? ENGAGEMENTMULTIPLE { get; set; }

        public bool? USEEXTERNALPLAN { get; set; }

        [StringLength(250)]
        public string ADRESSETOMGED { get; set; }

        [StringLength(50)]
        public string NOMBASETOMGED { get; set; }

        public bool? USE_D_DP1_SUIVIBUD { get; set; }

        [StringLength(5)]
        public string PRINTINGOPTION { get; set; }

        public bool? IMMOLOCKDEPRECIATIONRATE { get; set; }

        public bool? DRFGAVI { get; set; }

        [StringLength(1)]
        public string MONTCOMPTABUD { get; set; }

        [StringLength(50)]
        public string NIF { get; set; }

        public bool? ISVALIDATIONPARLOT { get; set; }

        public bool? ETAT { get; set; }

        [StringLength(50)]
        public string NUMPROJET { get; set; }

        [StringLength(250)]
        public string CONTACT { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DATEDEBUT { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DATEFIN { get; set; }

        public bool? RAPPROCOMPTENONFIN { get; set; }

        [StringLength(50)]
        public string CODE_POSTALE { get; set; }

        [StringLength(50)]
        public string BOITE_POSTALE { get; set; }

        [StringLength(20)]
        public string FraisBq4 { get; set; }

        [StringLength(20)]
        public string FraisBq5 { get; set; }

        [StringLength(20)]
        public string FraisBq6 { get; set; }

        [StringLength(10)]
        public string AMORTMOIS { get; set; }

        public bool? IMPRESSIONBORDCT { get; set; }
    }
}
