namespace apptab.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class GA_AVANCE
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(2)]
        public string SITE { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string NUMERO { get; set; }

        [StringLength(100)]
        public string LIBELLE { get; set; }

        public DateTime? DATE { get; set; }

        public DateTime? ECHEANCE { get; set; }

        [StringLength(25)]
        public string NUMERO_PIECE { get; set; }

        [Required]
        [StringLength(20)]
        public string COGE { get; set; }

        [StringLength(20)]
        public string AUXI { get; set; }

        [Required]
        [StringLength(20)]
        public string TYPE { get; set; }

        [StringLength(20)]
        public string SOUS_TYPE { get; set; }

        public DateTime DEBUT_ECHEANCE { get; set; }

        [StringLength(50)]
        public string MARCHE { get; set; }

        [StringLength(10)]
        public string JOURNAL { get; set; }

        [StringLength(20)]
        public string EXCERCICE { get; set; }
    }
}
