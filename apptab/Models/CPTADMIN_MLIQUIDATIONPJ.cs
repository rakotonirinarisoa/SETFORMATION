namespace apptab
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class CPTADMIN_MLIQUIDATIONPJ
    {
        public Guid ID { get; set; }

        public Guid IDLIQUIDATION { get; set; }

        [Required]
        [StringLength(36)]
        public string IDDOC { get; set; }

        [StringLength(255)]
        public string NOMDOC { get; set; }

        [StringLength(20)]
        public string FORMAT { get; set; }

        public DateTime? DATECRE { get; set; }

        public DateTime? DATEMAJ { get; set; }

        [StringLength(10)]
        public string USERCRE { get; set; }

        [StringLength(10)]
        public string USERMAJ { get; set; }

        public virtual CPTADMIN_FLIQUIDATION CPTADMIN_FLIQUIDATION { get; set; }
    }
}
