namespace apptab
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class tmp_bulletin
    {
        [Required]
        [StringLength(255)]
        public string id_GUID { get; set; }

        [Key]
        public int num { get; set; }

        [Required]
        [StringLength(50)]
        public string code_etablissement { get; set; }

        [Required]
        [StringLength(50)]
        public string matricule { get; set; }

        [StringLength(255)]
        public string code_rubrique { get; set; }

        [StringLength(100)]
        public string libelle_rubrique { get; set; }

        [Column("base")]
        public decimal? _base { get; set; }

        public decimal? taux { get; set; }

        public decimal? gain { get; set; }

        public decimal? retenue { get; set; }

        public decimal? montant { get; set; }

        [StringLength(20)]
        public string typeligne { get; set; }

        public int? mois { get; set; }

        public int? annee { get; set; }

        public DateTime? datetraitement { get; set; }

        public DateTime? datedebut { get; set; }

        public DateTime? datefin { get; set; }

        public decimal? base1 { get; set; }

        public decimal? taux1 { get; set; }

        public decimal? gain1 { get; set; }

        public decimal? retenue1 { get; set; }

        public DateTime? datepaiement { get; set; }
    }
}
