namespace apptab
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class SI_USERS
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string LOGIN { get; set; }

        [StringLength(50)]
        public string PWD { get; set; }

        public Role ROLE { get; set; }

        public int? IDPROJET { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DELETIONDATE { get; set; }

        public int? IDUSER { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CREATIONDATE { get; set; }
        public DateTime? LASTCONNEXTION { get; set; }

    }
}
