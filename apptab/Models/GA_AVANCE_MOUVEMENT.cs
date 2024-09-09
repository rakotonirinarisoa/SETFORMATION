namespace apptab.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class GA_AVANCE_MOUVEMENT
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(2)]
        public string SITE { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string NUMERO { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(36)]
        public string IDENTIFIANT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANT { get; set; }

        [StringLength(20)]
        public string COGE { get; set; }

        [StringLength(20)]
        public string AUXI { get; set; }

        [StringLength(20)]
        public string ACTI { get; set; }

        [StringLength(20)]
        public string POSTE { get; set; }

        [StringLength(20)]
        public string GEO { get; set; }

        [StringLength(10)]
        public string CONVENTION { get; set; }

        [StringLength(10)]
        public string CATEGORIE { get; set; }

        [StringLength(10)]
        public string SOUS_CATEGORIE { get; set; }

        [StringLength(20)]
        public string PLAN6 { get; set; }

        [StringLength(20)]
        public string PLAN7 { get; set; }

        [StringLength(20)]
        public string PLAN8 { get; set; }
    }
}
