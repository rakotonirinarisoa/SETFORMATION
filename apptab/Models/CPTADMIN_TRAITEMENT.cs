namespace apptab
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class CPTADMIN_TRAITEMENT
    {
        [Key]
        public int NUMENREG { get; set; }

        public int? NUMCAETAPE { get; set; }

        [StringLength(50)]
        public string NUMEROCA { get; set; }

        public DateTime? DATECA { get; set; }

        [StringLength(250)]
        public string COMMENTAIRE { get; set; }

        [StringLength(250)]
        public string USERRESPONSABLE { get; set; }

        [StringLength(20)]
        public string CODE_SITE { get; set; }

        public DateTime? DATECRE { get; set; }

        public DateTime? DATEMAJ { get; set; }

        [StringLength(10)]
        public string USERCRE { get; set; }

        [StringLength(10)]
        public string USERMAJ { get; set; }
    }
}
