namespace apptab
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class OPA_REGLEMENTBR
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }

        [Required]
        [StringLength(100)]
        public string NUM { get; set; }

        public DateTime? DATE { get; set; }

        [StringLength(100)]
        public string NoPiece { get; set; }

        [StringLength(50)]
        public string BANQUE { get; set; }

        [StringLength(100)]
        public string BENEFICIAIRE { get; set; }

        [StringLength(5)]
        public string GUICHET { get; set; }

        [StringLength(11)]
        public string RIB { get; set; }

        public decimal? MONTANT { get; set; }

        [StringLength(100)]
        public string LIBELLE { get; set; }

        [StringLength(5)]
        public string NUM_ETABLISSEMENT { get; set; }

        [StringLength(10)]
        public string CODE_J { get; set; }

        [StringLength(200)]
        public string DOM1 { get; set; }

        [StringLength(200)]
        public string DOM2 { get; set; }

        [StringLength(50)]
        public string CATEGORIE { get; set; }

        public string ETAT { get; set; }

        [StringLength(50)]
        public string APPLICATION { get; set; }

        public int? IDSOCIETE { get; set; }
    }
}
