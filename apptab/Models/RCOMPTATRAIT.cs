namespace apptab
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCOMPTATRAIT")]
    public partial class RCOMPTATRAIT
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

        public DateTime? DATECRE { get; set; }

        public DateTime? DATEMAJ { get; set; }

        [StringLength(10)]
        public string USERCRE { get; set; }

        [StringLength(10)]
        public string USERMAJ { get; set; }

        public string USERS { get; set; }

        public bool? MODIFICATION { get; set; }

        public bool? SUPPRESSION { get; set; }
    }
}
