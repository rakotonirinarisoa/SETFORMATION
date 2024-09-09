namespace apptab.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FOP")]
    public partial class FOP
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string NUMEROOP { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(2)]
        public string SITE { get; set; }

        [StringLength(50)]
        public string CODE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANT { get; set; }

        [StringLength(200)]
        public string LIBELLE { get; set; }

        [StringLength(20)]
        public string COGE { get; set; }

        public DateTime? DATEOP { get; set; }

        [StringLength(10)]
        public string CONVENTION { get; set; }

        [StringLength(3)]
        public string CATEGORIE { get; set; }

        [StringLength(3)]
        public string SOUSCATEGORIE { get; set; }

        [StringLength(20)]
        public string ACTI { get; set; }

        [StringLength(20)]
        public string GEO { get; set; }

        [StringLength(20)]
        public string POSTE { get; set; }

        [StringLength(20)]
        public string PLAN6 { get; set; }

        [StringLength(20)]
        public string PLAN7 { get; set; }

        public DateTime? DATECRE { get; set; }

        public DateTime? DATEMAJ { get; set; }

        [StringLength(10)]
        public string USERCRE { get; set; }

        [StringLength(10)]
        public string USERMAJ { get; set; }

        [StringLength(20)]
        public string EXERCICE { get; set; }

        public int? NUMBUD { get; set; }

        [StringLength(20)]
        public string PLAN8 { get; set; }

        [StringLength(255)]
        public string OBSERVATION { get; set; }

        [StringLength(20)]
        public string COGEBENEF { get; set; }

        [StringLength(12)]
        public string AUXIBENEF { get; set; }

        [StringLength(100)]
        public string NOMBENEF { get; set; }

        public DateTime? DATETAPE1 { get; set; }

        public DateTime? DATETAPE2 { get; set; }

        public DateTime? DATETAPE3 { get; set; }

        public DateTime? DATETAPE4 { get; set; }

        public DateTime? DATETAPE5 { get; set; }

        public DateTime? DATETAPE6 { get; set; }

        public DateTime? DATETAPE7 { get; set; }

        public DateTime? DATETAPE8 { get; set; }

        public DateTime? DATETAPE9 { get; set; }

        public DateTime? DATETAPE10 { get; set; }

        [StringLength(50)]
        public string ETAPE1USER { get; set; }

        [StringLength(50)]
        public string ETAPE2USER { get; set; }

        [StringLength(50)]
        public string ETAPE3USER { get; set; }

        [StringLength(50)]
        public string ETAPE4USER { get; set; }

        [StringLength(50)]
        public string ETAPE5USER { get; set; }

        [StringLength(50)]
        public string ETAPE6USER { get; set; }

        [StringLength(50)]
        public string ETAPE7USER { get; set; }

        [StringLength(50)]
        public string ETAPE8USER { get; set; }

        [StringLength(50)]
        public string ETAPE9USER { get; set; }

        [StringLength(50)]
        public string ETAPE10USER { get; set; }

        [StringLength(10)]
        public string JOURNAL { get; set; }

        [StringLength(10)]
        public string TYPE_OPERATION { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTTOTALOPAVECOPENCOURSEXO { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTTOTALOPSANSOPENCOURSEXO { get; set; }

        public bool? FLAG { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTBUDGETDISPOEXO { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTBUDGETDISPOEXOAVANTOP { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTBUDGETDISPOPERIODE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTBUDGETDISPOPERIODEAVANTOP { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTBUDGETDISPOSURMARCHE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTMARCHEDISPOSUROP { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTMARCHEDISPOSUROPPERIODE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTCOMMISSION { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TAXECOMMISSION { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? COURSRAP { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? COURSDEV { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTRAP { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTDEV { get; set; }

        [StringLength(3)]
        public string DEVISE { get; set; }

        public bool? REJET { get; set; }

        public DateTime? REJETDATE { get; set; }

        [StringLength(50)]
        public string REJETUSER { get; set; }

        [StringLength(250)]
        public string REJETMOTIF { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTBUDGETEXO { get; set; }

        [StringLength(50)]
        public string TYPEENGAGEMENT { get; set; }

        [StringLength(50)]
        public string MODEPAIEMENT { get; set; }

        [StringLength(50)]
        public string NUMEROBORDEREAU { get; set; }

        public bool? ISGENERATE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ENGAGEMENTSANTERIEURS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTBUDGETPERIODE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTTOTALOPSANSOPENCOURSPERIODE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANTTOTALOPAVECOPENCOURSPERIODE { get; set; }

        [StringLength(10)]
        public string SOURCEFIN { get; set; }

        [StringLength(20)]
        public string ANNEEBUDGETAIRE { get; set; }

        [StringLength(10)]
        public string NUMEROBUDGETAIRE { get; set; }

        [StringLength(255)]
        public string LIGNEBUDGETAIRE { get; set; }

        public DateTime? DATE_CALCUL_BUDGET { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MARCHEMONTANTINITIAL { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MARCHEMONTANTOPANTERIEUR { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MARCHEMONTANTOPCUMULE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MARCHESOLDEAPAYER { get; set; }

        public string NUMEROLIQUIDATION { get; set; }
    }
}
