namespace apptab
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class CPTADMIN_TRAITEMENT_AVANCE
    {
        [Key]
        public int NUMENREG { get; set; }

        public int? NUMEROETAPE { get; set; }

        [StringLength(50)]
        public string NUMEROAVANCE { get; set; }

        public DateTime? DATETRAITEMENT { get; set; }

        [StringLength(250)]
        public string USERRESPONSABLE { get; set; }

        [StringLength(20)]
        public string CODE_SITE { get; set; }

        [StringLength(250)]
        public string COMMENTAIRE { get; set; }

        [StringLength(255)]
        public string CHAMP_ADDITIONNEL { get; set; }

        public DateTime? DATECRE { get; set; }

        public DateTime? DATEMAJ { get; set; }

        [StringLength(10)]
        public string USERCRE { get; set; }

        [StringLength(10)]
        public string USERMAJ { get; set; }
    }
}
