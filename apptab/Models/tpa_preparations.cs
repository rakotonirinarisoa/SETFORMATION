namespace apptab
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class tpa_preparations
    {
        [Key]
        public int numeroautomatique { get; set; }

        [Required]
        [StringLength(10)]
        public string code_etablissement { get; set; }

        [Required]
        [StringLength(10)]
        public string matricule { get; set; }

        public int mois { get; set; }

        public int annee { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? numerodordre { get; set; }

        [StringLength(50)]
        public string code_rubriques { get; set; }

        public int? code_typerubriques { get; set; }

        [StringLength(15)]
        public string code_constante { get; set; }

        [StringLength(2)]
        public string code_nature { get; set; }

        [StringLength(2)]
        public string typeretenue { get; set; }

        [Column("base")]
        public decimal? _base { get; set; }

        public decimal? taux { get; set; }

        public decimal? valeur { get; set; }

        [StringLength(255)]
        public string valeurstring { get; set; }

        public DateTime? valeurdate { get; set; }

        [StringLength(50)]
        public string typevaleur { get; set; }

        public DateTime? dateTraitement { get; set; }

        public DateTime? DATECRE { get; set; }

        public DateTime? DATEMAJ { get; set; }

        [StringLength(10)]
        public string USERCRE { get; set; }

        [StringLength(10)]
        public string USERMAJ { get; set; }

        [StringLength(15)]
        public string code_constante1 { get; set; }

        public decimal? base1 { get; set; }

        public decimal? taux1 { get; set; }

        public decimal? valeur1 { get; set; }
    }
}
