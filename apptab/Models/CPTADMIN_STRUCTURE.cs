namespace apptab
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class CPTADMIN_STRUCTURE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NUM { get; set; }

        [StringLength(50)]
        public string MINISTERE { get; set; }

        [StringLength(50)]
        public string MISSION { get; set; }

        [StringLength(50)]
        public string PROGRAMME { get; set; }

        public string SOA { get; set; }

        public bool? CHAINETRAITEMENT { get; set; }

        public bool? LIAISONSIGFP { get; set; }

        public DateTime? DATECRE { get; set; }

        public DateTime? DATEMAJ { get; set; }

        [StringLength(10)]
        public string USERCRE { get; set; }

        [StringLength(10)]
        public string USERMAJ { get; set; }
    }
}
