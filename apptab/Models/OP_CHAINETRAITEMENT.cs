namespace apptab
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class OP_CHAINETRAITEMENT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NUM { get; set; }

        [StringLength(50)]
        public string NOM { get; set; }

        [StringLength(50)]
        public string ETAT { get; set; }

        [StringLength(50)]
        public string ACTION { get; set; }

        public string USERS { get; set; }

        public DateTime? DATECRE { get; set; }

        public DateTime? DATEMAJ { get; set; }

        [StringLength(10)]
        public string USERCRE { get; set; }

        [StringLength(10)]
        public string USERMAJ { get; set; }

        public bool? IMPRESSION { get; set; }

        public bool? SUPPRESSION { get; set; }

        public bool? MODIFICATION { get; set; }

        [StringLength(10)]
        public string COULEUR { get; set; }

        public bool? REJET { get; set; }

        [StringLength(100)]
        public string LABEL_CHAMP_ADDITIONNEL1 { get; set; }

        [StringLength(100)]
        public string LABEL_CHAMP_ADDITIONNEL2 { get; set; }

        [StringLength(100)]
        public string CODE_SIGNATAIRE { get; set; }
    }
}
