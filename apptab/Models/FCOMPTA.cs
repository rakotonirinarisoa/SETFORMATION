namespace apptab
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FCOMPTA")]
    public partial class FCOMPTA
    {
        [Key]
        [StringLength(14)]
        public string NORD { get; set; }

        [StringLength(1000)]
        public string DESCRIPTION { get; set; }

        public DateTime? DATECRE { get; set; }

        public DateTime? DATEMAJ { get; set; }

        [StringLength(10)]
        public string USERCRE { get; set; }

        [StringLength(10)]
        public string USERMAJ { get; set; }

        [StringLength(200)]
        public string PATHPC1 { get; set; }

        [StringLength(200)]
        public string PATHPC2 { get; set; }

        [StringLength(200)]
        public string PATHPC3 { get; set; }

        [StringLength(50)]
        public string REFPAIEMENT { get; set; }

        [StringLength(100)]
        public string BENEFPAIEMENT { get; set; }

        public DateTime? STATUT1DATE { get; set; }

        [StringLength(50)]
        public string STATUT1USER { get; set; }

        public DateTime? STATUT2DATE { get; set; }

        [StringLength(50)]
        public string STATUT2USER { get; set; }

        public DateTime? STATUT3DATE { get; set; }

        [StringLength(50)]
        public string STATUT3USER { get; set; }

        public DateTime? STATUT4DATE { get; set; }

        [StringLength(50)]
        public string STATUT4USER { get; set; }

        [StringLength(50)]
        public string REFFACTURE { get; set; }

        [StringLength(50)]
        public string REFPC1 { get; set; }

        [StringLength(50)]
        public string REFPC2 { get; set; }

        [StringLength(50)]
        public string REFPC3 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NUMEROCHEQUE { get; set; }
    }
}
