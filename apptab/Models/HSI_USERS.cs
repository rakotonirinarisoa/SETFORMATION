namespace apptab
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HSI_USERS
    {
        public int ID { get; set; }

        public int IDPARENT { get; set; }

        [StringLength(50)]
        public string LOGIN { get; set; }

        [StringLength(50)]
        public string PWD { get; set; }

        public Role ROLE { get; set; }

        public int IDPROJET { get; set; }

        public int IDUSER { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime? CREATIONDATE { get; set; }
    }
}
