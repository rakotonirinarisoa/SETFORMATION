namespace apptab
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class TP_MPIECES_JUSTIFICATIVES
    {
        public int ID { get; set; }

        [StringLength(20)]
        public string MODULLE { get; set; }

        [StringLength(50)]
        public string CODE_SITE { get; set; }

        [StringLength(50)]
        public string NUMERO_FICHE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NOMBRE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? RANG { get; set; }

        [StringLength(255)]
        public string DESIGNATION { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONTANT { get; set; }

        public DateTime? DATECRE { get; set; }

        public DateTime? DATEMAJ { get; set; }

        [StringLength(10)]
        public string USERCRE { get; set; }

        [StringLength(10)]
        public string USERMAJ { get; set; }

        [StringLength(20)]
        public string TYPEPIECE { get; set; }

        public string LIEN { get; set; }
    }
}
